using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// ilyen üzenetek mennek át a hálózaton
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Serializable]
    public class CNetworkMessage
    {
        #region Variables
        //az objektum tipusát tartalmazza
        private Type _objTipus;
        //és itt az adat
        private byte[] _objData;
        #endregion

        #region Properties
        /// <summary>
        /// eltároljuk benne az objektum típusát
        /// </summary>
        public Type objTipus
        {
            get { return _objTipus; }
            set { _objTipus = value; }
        }
        /// <summary>
        /// és itt az adat maga
        /// </summary>
        public byte[] objData
        {
            get { return _objData; }
            set { _objData = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="objtipus">meg kell adni a továbbítandó adat típusát</param>
        /// <param name="data">és az adatot</param>
        public CNetworkMessage(Type objtipus, byte[] data)
        {
            _objTipus = objtipus;
            _objData = data;
        }
        #endregion
    }
}
