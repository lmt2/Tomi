#define TODO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Data;
using System.Net.NetworkInformation;



namespace network
{
    //public delegate bool KliensKezeloHalozatiEvent(object sender,string command);

    /// <summary>
    /// Egy szerverre csatlakozott hálózati klienst ilyennel kezelünk
    /// 
    /// </summary>
    public sealed class CHalozatiKliensKezelo
    {
        //public event KliensKezeloHalozatiEvent kliensKezeloHalozatiEvent;

        #region Variables
        private string _id;
        private CHalozatiSzerver _parentTCPszerver;
        private string _hostName = "";
        private DateTime _lastMsgTime = DateTime.Now;
        private System.Timers.Timer _socketCheckTimer = new System.Timers.Timer();
        public bool _vanJateka = false;
        public bool _kellControlsFrissites = true;
        
        // ez azonosítja a klienst
        private CHalozatiKliensID _kliensID;

        private TcpClient _tcpClient;

        BackgroundWorker _bw;

        // a halozatrol ilyen adatot várunk:
        CNetworkMessage _networkMessage;

        #endregion

        #region Constructors


        public CHalozatiKliensKezelo(CHalozatiSzerver szerver,string id)
        {
            this._parentTCPszerver = szerver;
            this.ID = id;
            //Trace.WriteLine(string.Format("----KliensKezelo up---- {0}", ID));
            //majd később az IPcím/hostname párost is kiírjuk, hogy könnyebb legyen a logot olvasni.
            Thread clientinitThread = new Thread(IpEntry);
            clientinitThread.IsBackground = false;
            clientinitThread.Start();
            _socketCheckTimer.Interval = 3000;
            _socketCheckTimer.Elapsed += new System.Timers.ElapsedEventHandler(SocketCheckTimer_Tick);
            _socketCheckTimer.Enabled = false;
            //_socketCheckTimer.Start();
        }


        ~CHalozatiKliensKezelo()
        {
            try
            {
                //Trace.WriteLine(string.Format("----KliensKezelo down---- {0}", ID));
                Trace.WriteLine(string.Format("----KliensKezelo down---- {0} {1}", ID, _hostName));
            }
            catch (System.Exception ex)
            {

            }
            
        }

        #endregion

        #region Properties
        /// <summary>
        /// Kell tudnunk, melyik klienst kezeli ez a KliensKezelo
        /// </summary>
        public CHalozatiKliensID kliensID
        {
            get
            {
                return _kliensID;
            }
            set
            {
                _kliensID=value;
            }
        }
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public CNetworkMessage networkMessage
        {
            get
            {
                return _networkMessage;
            }
            set
            {
                _networkMessage = value;
            }
        }
        #endregion

        #region Methods

        private void SocketCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!SendObject(new Ping()))
            {
                Trace.WriteLine(string.Format("socket closed {0}", _hostName));
                CloseConnection();
            }

            TimeSpan interval = DateTime.Now - _lastMsgTime;
            if (interval.TotalSeconds>10)
            {
                //Trace.WriteLine(string.Format("socket closed {0}", _hostName));
                //closeConnection();
            }
        }

        public void AddSocket(TcpClient tcpCon){
            _tcpClient = tcpCon;

            _bw = new BackgroundWorker();
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(Bw_DoWork);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bw_RunWorkerCompleted);

            _bw.RunWorkerAsync();
        }
        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            HandleComm();
        }
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Trace.WriteLine(string.Format("socket closed {0}",_hostName));
            CloseConnection();
            _tcpClient = null;
            _parentTCPszerver = null;
            _bw.Dispose();
        }
        public bool IsConnected()
        {
            if (_tcpClient == null)
                return false;
            return _tcpClient.Connected;
        }
        /// <summary>
        /// külön szálban indítjuk a dns lekérdezést, mert időigényes lehet
        /// </summary>
        private void GetRemoteHost()
        {
            Thread clientinitThread = new Thread(IpEntry);
            clientinitThread.IsBackground = false;
            clientinitThread.Start();
        }

        /// <summary>
        /// a dns lekérdezés
        /// </summary>
        private void IpEntry()
        {
            try
            {
                // get the hostname
                IPHostEntry hostEntry = Dns.GetHostEntry(ID);
                _hostName = hostEntry.HostName;
                Trace.WriteLine(string.Format("----KliensKezelo up---- {0} {1}", ID, _hostName));
            }
            catch
            {
                Trace.WriteLine(string.Format("Nem lehet a távoli gép hostnevét lekérni! (Dns.GetHostEntry error.) {0}", ID));
            }

        }


        bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        /// <summary>
        /// a hálózatot itt kezeljuk, szálban indítva (vegtelen ciklus)
        /// </summary>
        private void HandleComm()
        {

            MemoryStream ms;
            BinaryFormatter bf;
            NetworkStream clientStream = _tcpClient.GetStream();
            //az első kontakt időtúllépése nem sok
            clientStream.ReadTimeout= 10000;
            IFormatter formatter = new BinaryFormatter();

            

            try
            {
                //megprobaljuk kiolvasni
                _networkMessage = (CNetworkMessage)formatter.Deserialize(clientStream);
            }
            catch(Exception e)
            {
                Trace.WriteLine(string.Format("Nem Common.NetworkMessage üzenet jött innen: {0}", _hostName));
                //Debug.Assert(false, e.Message);
                CloseConnection();
                return;
            }
            
            clientStream.ReadTimeout = -1;
            //Trace.WriteLine("incoming object: " + networkmessage.objtipus);

            ms = new MemoryStream(_networkMessage.objData);
            bf = new BinaryFormatter();

            //elso uzenet: KliensID object
            //ebbol elbiraljuk, van-e joga a kliensnek belepni

            try
            {
                _kliensID = (CHalozatiKliensID)bf.Deserialize(ms);
            }
            catch
            {
                Trace.WriteLine("Az első üzenet az új klienstől nem logon, hanem valami más...");
                ms.Close();
                CloseConnection();
                return;
            }

            ms.Close();

            //Thread aaa = new Thread(new ThreadStart(abc));
            //aaa.Name = "aaa";
            //aaa.IsBackground = true;
            //aaa.Start();

            if (!(_parentTCPszerver.logonelbiralas(this)))
            {
                Trace.WriteLine(string.Format("login failed... {0} {1}", _kliensID.gepNev, _kliensID.mac));
                this.SendObject("auth_failed");
                Thread.Sleep(200);
                return;
            }
            else
            {
                this.SendObject("auth_ok");
            }

            //remote IP
            _kliensID.ip = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString();
            //remote port
            _kliensID.port = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Port.ToString();
            //if (_kliensID.location=="")
            //    _kliensID.location = Properties.Settings.Default.Location;
            //if (_kliensID.location == null)
            //    _kliensID.location = Properties.Settings.Default.Location;

            _parentTCPszerver.reg(this);
            

            //es az ID-t is elküldjük
            SendObject(_kliensID);

            Trace.WriteLine("logon rendben: " + _kliensID.gepNev);

            //figyeljük a socketet
            while (true)
            {
                try
                {
                    _networkMessage = (CNetworkMessage)formatter.Deserialize(clientStream);
                    _lastMsgTime = DateTime.Now;
                    //ami jött, azt átadjuk feldolgozásra
                    //kliensKezeloHalozatiEvent(this,"process");
                    _parentTCPszerver.process(this);
                }
                catch(Exception ex)
                {
                    Trace.WriteLine("Disconnected: " + _kliensID.gepNev);
                    CloseConnection();
                    break;
                }
            }
        }

        /// <summary>
        /// feladunk hálózatra adatot
        /// </summary>
        /// <param name="o">az adat</param>
        public bool SendObject(object o)
        {
            bool ret = false;
            //Log.logToFile("outging object: " + o.GetType().ToString(),false);
            try
            {
                NetworkStream clientStream = _tcpClient.GetStream();
                if (clientStream == null)
                {
                    Trace.WriteLine("nincs írható network stream...");
                    return ret;
                }

                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(ms, o);
                byte[] data = ms.GetBuffer();
                ms.Close();

                CNetworkMessage uzenet = new CNetworkMessage(o.GetType(), data);
                IFormatter formatter = new BinaryFormatter();
                //feladjuk a Stream-re
                formatter.Serialize(clientStream, uzenet);
                ret = true;
                if (o.GetType() == typeof(string))
                {
                    Trace.WriteLine(string.Format("send to client {0} : {1}", _kliensID.gepNev, (string)o));
                }

                
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(string.Format("send object ({1}) to client failed: ({0}) !", _kliensID.gepNev, o.GetType()));
            }

            return ret;
            
        }

        /// <summary>
        /// lezárjuk a hálózati kapcsolatot
        /// </summary>
        public void CloseConnection()
        {
            // Close KliensKapcsolat
            try
            {
                _socketCheckTimer.Enabled = false;
                _socketCheckTimer.Stop();
                _tcpClient.ReceiveTimeout = 1;
                _tcpClient.SendTimeout = 1;
                _parentTCPszerver.remove(this);
                _tcpClient.Close();
                
                _bw.CancelAsync();
                
                              
            }
            catch(Exception e)
            {
                //Trace.WriteLine("Close KliensKapcsolat error...");
            }
        }
        #endregion
    }
}
