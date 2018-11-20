using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;

namespace network
{
    //public delegate bool HalozatiKliensEvent(object sender, string action);

    public class CHalozatiKliens
    {
        //public event HalozatiKliensEvent HalozatiEsemeny;

        private CNetworkMessage _networkMessage;

        public CHalozatiKliensID _kliensID = new CHalozatiKliensID();

        //az automatikus felcsatlakozás miatt
        private System.Timers.Timer _connecttimer = new System.Timers.Timer();

        public int remotePort { get; set; }
        public string remoteHost { get; set; }

        private TcpClient _client = null;

        private IPHostEntry _ipEntry = null;

        //private Thread _clientThread = null;
        BackgroundWorker bw;

        private IPEndPoint _serverEndPoint = null;

        private bool _autoconnect;

        public bool autoconnect
        {
            get
            {
                return _autoconnect;
            }
            set
            {
                autoconnectSet = true;
                _autoconnect = value;
            }
        }
        private bool autoconnectSet = false;

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

        public CHalozatiKliens(string s, int p, string l)
        {
            remotePort = p;
            remoteHost = s;
            Trace.WriteLine(string.Format("----HalozatiKliens up---- {0}:{1}",remoteHost,remotePort));
            getRemoteHost();
            _kliensID.getMAC();
            _kliensID.gepNev = Environment.MachineName;
            _kliensID.location = l;
            autoconnect = false;

            _client = new TcpClient();

            _connecttimer.Enabled = true;
            _connecttimer.Interval = 2000;
            _connecttimer.Elapsed += new System.Timers.ElapsedEventHandler(this.connecttimer_Elapsed);

            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

        }
        ~CHalozatiKliens()
        {
            try
            {
                Trace.WriteLine(string.Format("----HalozatiKliens down---- {0}:{1}", remoteHost, remotePort));
            }
            catch (System.Exception ex)
            {
            	
            }
            
        }
        /// <summary>
        /// az automatikus csatlakozás
        /// </summary>
        private void connecttimer_Elapsed(object sender, EventArgs e)
        {
            if (autoconnect)
            {
                if ((_client != null) && (_client.Connected)) { return; }
                clientConnectToServer();
            }
            else
            {
                if (autoconnectSet)
                {
                    Trace.WriteLine(string.Format("Autoconnect to server: host:port {0}:{1} disabled!", remoteHost, remotePort));
                    autoconnectSet = !autoconnectSet;
                }
            }

        }

        /// <summary>
        /// a hálózatról jövő adatok figyelése, szálban (végtelen ciklus)
        /// </summary>
        /// <param name="client">ezt a TcpClient-t figyeljük</param>
        private void handleComm(object client)
        {

            TcpClient tcpClient = (TcpClient)client;
            Thread.Sleep(200);
            NetworkStream clientStream;

            try
            {
                clientStream = tcpClient.GetStream();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);

                return;
            }

            while (true)
            {
                try
                {
                    IFormatter formatter = new BinaryFormatter();

                    _networkMessage = (CNetworkMessage)formatter.Deserialize(clientStream);

                    erkezettadatfeldolgozas(_networkMessage);
                }
                catch
                {
                    //Trace.WriteLine(e);
                    disconnect();
                    //Trace.WriteLine("Disconnected");
                    //_client.Close();
                    //_client = null;
                    break;

                }



            }
        }

        /// <summary>
        /// a bejövő adatok kezelése
        /// </summary>
        /// <param name="nm">ezt jött hálózaton</param>
        private void erkezettadatfeldolgozas(CNetworkMessage nm)
        {
            MemoryStream ms = new MemoryStream(nm.objData);
            BinaryFormatter bf = new BinaryFormatter();
            object o = (object)bf.Deserialize(ms);
            ms.Close();

            Trace.WriteLine("incoming object: " + nm.objTipus);

            if (nm.objTipus == typeof(String))
            {
                Trace.WriteLine((string)o);

                //if ((string)o == "auth_failed")
                //{
                //    disconnect();
                //    return;
                //}

            }
            if (nm.objTipus == typeof(Ping))
            {
                sendObject(new Ping());
            }
            if (nm.objTipus == typeof(CHalozatiKliensID))
            {
                _kliensID.id = ((CHalozatiKliensID)o).id;
                //Log.logToFile("ID a szerveren: " + _kliensID.id, true);
                Trace.WriteLine(string.Format("ID a szerveren: {0}", _kliensID.id.wsname));
                //return;
            }

            //HalozatiEsemeny(this, "process");

        }

        /// <summary>
        /// a távoli végpont felderítése
        /// </summary>
        private bool setServerEndPoint()
        {

            //serverEndPoint = null;
            try
            {
                IPAddress[] addr = _ipEntry.AddressList;


                foreach (IPAddress ad in addr)
                {
                    if (ad.AddressFamily == AddressFamily.InterNetwork)
                    {
                        _serverEndPoint = new IPEndPoint(ad, remotePort);
                        //Log.logToFile("Kapcsolódás végpontja: " + ad.ToString() + ":" + Properties.Settings.Default.remoteport.ToString(), false);
                        return true;
                    }
                }
            }
            catch
            {
                Trace.WriteLine(string.Format("Kapcsolódás végpontja még nem meghatározott: {0}:{1}", remoteHost, remotePort));
            }
            return false;
        }

        /// <summary>
        /// külön szálban indítjuk a dns lekérdezést, mert időigényes lehet
        /// </summary>
        public void getRemoteHost()
        {
            //remoteHost = CGame.host;
            Thread clientinitThread = new Thread(setipEntry);
            clientinitThread.IsBackground = false;
            clientinitThread.Start();
        }

        /// <summary>
        /// a dns lekérdezés
        /// </summary>
        private void setipEntry()
        {
            try
            {
                _ipEntry = null;
                _ipEntry = System.Net.Dns.GetHostEntry(remoteHost);
                //Trace.WriteLine("Távoli gép azonosítva: {0}", Properties.Settings.Default.remotehost);
            }
            catch
            {
                Trace.WriteLine(string.Format("Nem lehet a távoli gép IPv4 címét lekérni! (Dns.GetHostEntry error.) Connect to remote host:port {0}:{1}", remoteHost, remotePort));
            }

        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            //HalozatiEsemeny(this, "kapcsolodva");
            handleComm(_client);
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            disconnect();
            Trace.WriteLine(string.Format("network thread stop {0}", remotePort));
            bw.Dispose();
        }

        public bool connected()
        {
            if (_client == null)
                return false;
            return _client.Connected;
        }

        /// <summary>
        /// a tényleges csatlakozás
        /// </summary>
        public void clientConnectToServer()
        {

            if ((_client != null) && (_client.Connected)) { Trace.WriteLine("már van kapcsolat"); return; }

            if (!setServerEndPoint())
            {
                Trace.WriteLine("Nem sikerult a távoli csatlakozási pont azonosítása.");
                //HalozatiEsemeny(this, "connecting");
                return;
            }

            try
            {
                if (_client == null) _client = new TcpClient();
                _client.Connect(_serverEndPoint);
            }
            catch(Exception ex)
            {
                //_client = null;
                //_kliensID.id = -1;
                //Console.Clear();
                //Console.CursorVisible = false;
                //HalozatiEsemeny(this, "connecting");
                //Trace.WriteLine("connecting to WSadmin server...");

                return;
            }
            //sikerult konnektalni


            bw.RunWorkerAsync();
            //_clientThread = new Thread(new ParameterizedThreadStart(handleComm));
            //_clientThread.Name = "listenThread";
            //_clientThread.IsBackground = false;
            //_clientThread.Start(_client);

            //eloszor kuldunk egy PCid objektumot (logon)
            _kliensID.ip = _client.Client.LocalEndPoint.ToString();
            _kliensID.gepNev = Environment.MachineName;
            sendObject(_kliensID);

        }

        /// <summary>
        /// Hálózaton adatot küldünk
        /// </summary>
        /// <param name="o">ezt az adatot küldjük</param>
        public bool sendObject(object o)
        {
            //Trace.WriteLine("outging object: " + o.GetType().ToString());
            bool ret = false;
            NetworkStream nstream = null;
            try
            {
                nstream = _client.GetStream();
            }
            catch
            {
                //Trace.WriteLine("nincs network stream...");
                return ret;
            }

            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            //MemoryStream előállítás
            try
            {
                bf.Serialize(ms, o);
            }
            catch(Exception e)
            {

                Trace.WriteLine("MemoryStream error");
                Debug.Assert(false, e.Message);
            }
            byte[] data = ms.GetBuffer();
            ms.Close();

            CNetworkMessage üzenet = new CNetworkMessage(o.GetType(), data);

            IFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(nstream, üzenet);
                ret = true;
            }
            catch(Exception ex)
            {
                Trace.WriteLine("NetworkMessage send failed");
            }
            return ret;
        }

        /// <summary>
        /// hálózat lecsatlakozás
        /// </summary>
        public void disconnect()
        {  
            try
            {
                //HalozatiEsemeny(this, "disconnect");
                Trace.WriteLine("disconnecting...");

                if ((_client != null))
                {
                    _client.Close();
                    _client = null;
                }
            }
            catch
            {
                Trace.WriteLine("disconnect error...");
                return;
            }
        }

    }


}

