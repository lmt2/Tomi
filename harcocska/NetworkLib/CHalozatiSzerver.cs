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
    /// <summary>
    /// TCP szerver, amely figyel a kliensekre, és ha elfogadja a socket-et egy külön kezelő osztályban kezeli őket.
    /// </summary>
    public class CHalozatiSzerver
    {
        public static readonly Object _lock = new Object();

        //public event KliensKezeloHalozatiEvent KliensekEsemenyei;

        /// <summary>
        /// a klienseket itt tárolja
        /// </summary>
        private Dictionary<CPC, CHalozatiKliensKezelo> _kliensek = new Dictionary<CPC, CHalozatiKliensKezelo>();

        public int port{get; set;}

        private int _kliensSzamlalo = 0;

        //private int _kliensekDictionaryID = 0;

        private TcpListener _tcpListener = null;

        //private Thread _listenThread = null;

        BackgroundWorker bw;

        bool _listening;

        /// <summary>
        /// konstruktor
        /// </summary>
        public CHalozatiSzerver(int port)
        {
            _listening = false;
            this.port = port;
            //Trace.WriteLine("----HalozatiSzerver up----");
            Trace.WriteLine(string.Format("----HalozatiSzerver up---- ({0})", port));
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }
        /// <summary>
        /// destruktor
        /// </summary>
        ~CHalozatiSzerver()
        {
            try { Trace.WriteLine("----HalozatiSzerver down----"); }
            catch { }
            
        }

        public Dictionary<CPC, CHalozatiKliensKezelo> kliensek
        {
            get
            {
                return _kliensek;
            }
        }
        public bool listening
        {
            get { return _listening; }
            set { _listening = value; }
        }

        public int kliensSzamlalo
        {
            get { return _kliensSzamlalo; }
            set { _kliensSzamlalo = value; }
        }

        //public int kliensekDictionaryID
        //{
        //    get { return _kliensekDictionaryID; }
        //    set { _kliensekDictionaryID = value; }
        //}


        public void serverListenStart()
        {
            
            if (_tcpListener != null) return;
            _tcpListener = new TcpListener(IPAddress.Any, port);
            bw.RunWorkerAsync();


            //_listenThread = new Thread(new ThreadStart(listenForClients));
            //_listenThread.Name = "listenThread";
            //_listenThread.IsBackground = true;
            //_listenThread.Start();

        }

        public CHalozatiKliensKezelo getKezeloFromWSname(string s){
            CHalozatiKliensKezelo kk = null;
            foreach (KeyValuePair<CPC, CHalozatiKliensKezelo> kk1 in kliensek)
            {
                if (kk1.Key.wsname == s)
                {
                    kk = kk1.Value;
                    break;
                }
            }
            return kk;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            listenForClients();
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Trace.WriteLine(string.Format("listening stop {0}", port));
            bw.Dispose();
        }

        public void serverListenStop(){
            if (_tcpListener!=null)
                _tcpListener.Stop();
            _tcpListener = null;
            _listening = false;
            bw.CancelAsync();
        }

        public bool logonelbiralas(CHalozatiKliensKezelo kk)
        {
            lock (_lock)
            {
                foreach (KeyValuePair<CPC, CHalozatiKliensKezelo> kvp in kliensek)
                {
                    if (kvp.Value.kliensID.mac == (kk.kliensID.mac))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void reg(CHalozatiKliensKezelo kk)
        {
            lock (_lock)
            {
                kliensSzamlalo++;
                //kliensekDictionaryID++;

                //kk.kliensID.id = kliensekDictionaryID;

                CPC p = new CPC();

                p.wsname = kk.kliensID.gepNev;

                kk.kliensID.id = p;
                kliensek.Add(p, kk);

            }
            //KliensekEsemenyei(kk, "reg");
        }

        public void remove(CHalozatiKliensKezelo kk)
        {
            kliensek.Remove(kk.kliensID.id);
            kliensSzamlalo--;
            Debug.Assert(kliensSzamlalo >= 0, "A kliensszámlálóval baj van!", "Az érték nulla alá ment");
            //KliensekEsemenyei(kk, "remove");
        }
        public void process(CHalozatiKliensKezelo kk)
        {
            //KliensekEsemenyei(kk, "process");
        }
        public void listenForClients()
        {
            try
            {
                _tcpListener.Start();
                _listening = true;
                string IP = ((IPEndPoint)_tcpListener.LocalEndpoint).Address.ToString();
                Trace.WriteLine(string.Format("TCP listening start {0}:{1}", IP,port));
            }
            catch
            {
                Trace.WriteLine("A porttal valami baj van, (foglalt?)");
                return;
            }
            CHalozatiKliensKezelo kliensKezelo;
            TcpClient tcpKliens;
            int counter = 0;
            while (true)
            {
                try
                {
                    if (!_listening)
                    {
                        break;
                    }
                    if (!_tcpListener.Pending())
                    {
                        Thread.Sleep(200); // choose a number (in milliseconds) that makes sense
                        continue; // skip to next iteration of loop
                    }
                    else {
                        //Thread.Sleep(200); //legyen idő feldolgozni a klienst...
                        tcpKliens = null;
                        kliensKezelo = null;

                        //Log.logToFile("----waiting for new TcpClient----", false);

                        tcpKliens = _tcpListener.AcceptTcpClient();



                        //Log.logToFile("connecting from: " + Sclient.Client.RemoteEndPoint.ToString(), false);
                        //és kezeljük a klienst
                        string IP = ((IPEndPoint)tcpKliens.Client.RemoteEndPoint).Address.ToString();
                        bool ret = true;


                        //lock (Program._lock)
                        //{
                        //    foreach (KeyValuePair<pc, KliensKezelo> kvp in _kliensek)
                        //    {
                        //        if (kvp.Value.kliensID.ip == IP)
                        //        {
                        //            try
                        //            {
                        //                Trace.WriteLine("Existing client IP [{0}] on server!", IP);
                        //                MemoryStream ms = new MemoryStream();
                        //                BinaryFormatter bf = new BinaryFormatter();

                        //                bf.Serialize(ms, "existing");
                        //                byte[] data = ms.GetBuffer();
                        //                ms.Close();

                        //                NetworkMessage uzenet = new NetworkMessage(typeof(string), data);
                        //                IFormatter formatter = new BinaryFormatter();
                        //                //feladjuk a Stream-re
                        //                formatter.Serialize(tcpKliens.GetStream(), uzenet);
                        //                formatter = null;
                        //                uzenet = null;
                        //                data = null;
                        //                ms = null;
                        //                bf = null;

                        //            }
                        //            catch 
                        //            {

                        //            }


                        //            ret = false;
                        //            break;
                        //        }
                        //    }
                        //}

                        if (ret)
                        {
                            counter++;

                            kliensKezelo = new CHalozatiKliensKezelo(this, IP);

                            kliensKezelo.AddSocket(tcpKliens);

                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("Error... listening off ({0})",port));
                    Trace.WriteLine(ex);
                    //_tcpListener.Stop();
                    //_tcpListener = null;
                    //_listenThread.Abort();
                    serverListenStop();
                    break;
                }
            }
        }


    }
}
