using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A kliens programok hálózati azonosításához szükséges dolgai
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Serializable]
    public class CHalozatiKliensID
    {
        #region Variables

        #endregion

        #region Constructors
        /// <summary>
        /// def. konstruktor
        /// </summary>
        public CHalozatiKliensID()
        {

        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public CPC id { get; set; }
        public String gepNev { get; set; }
        public String location { get; set; }
        public String ip { get; set; }
        public String port { get; set; }
        public String mac { get; set; }
        public String username { get; set; }

        #endregion

        #region Methods
        public void getMAC()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            mac = nics[0].GetPhysicalAddress().ToString();

            username = Environment.UserName;
        }

        #endregion
    }
}
