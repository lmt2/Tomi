using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    [Serializable]
    public class CPC : IEquatable<CPC>
    {
        public string wsname { get; set; }
        //public int tcpID { get; set; }

        #region IEquatable<pc> Members

        public bool Equals(CPC other)
        {
            return this.wsname.Equals(other.wsname);
        }

        #endregion

        public override int GetHashCode()
        {
            return this.wsname.Length;
        }
    }
    [Serializable]
    public class Ping { }
}
