using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
	public class CFejlesztesiElem
	{
		public string name;
		public bool feloldott { get; set; }

		#region constructors
		public CFejlesztesiElem() {

		}
		public CFejlesztesiElem(string n) {
			name = n;
			feloldott = false;
		}
		#endregion
	}
}
