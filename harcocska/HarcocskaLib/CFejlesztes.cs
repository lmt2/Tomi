using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
	/// <summary>
	/// A fejlesztési fát tároló osztály
	/// </summary>
	[Serializable()]
	public class CFejlesztes
	{
		//ez a fa
		public TreeNode<CFejlesztesiElem> Root { get; set; }

		#region constructors
		public CFejlesztes() {
			//készítünk egy fejlesztési elemet
			CFejlesztesiElem alap = new CFejlesztesiElem("alap");
			alap.feloldott = true;
			//ez lesz a gyökér
			Root = new TreeNode<CFejlesztesiElem>(alap);

			//készítünk egy fejlesztési elemet
			CFejlesztesiElem katonasag = new CFejlesztesiElem("katonasag");
			//hozzáadjuk a gyökérhez
			TreeNode<CFejlesztesiElem> katonasagnode =Root.AddChild(katonasag);

			//készítünk egy fejlesztési elemet
			CFejlesztesiElem barakk = new CFejlesztesiElem("barakk");
			//hozzáadjuk a katonasag node-hoz
			TreeNode<CFejlesztesiElem> barakknode = katonasagnode.AddChild(barakk);

			//készítünk egy fejlesztési elemet
			CFejlesztesiElem bunker = new CFejlesztesiElem("bunker");
			//hozzáadjuk a katonasag node-hoz
			katonasagnode.AddChild(bunker);

			//készítünk egy fejlesztési elemet
			CFejlesztesiElem barakk2 = new CFejlesztesiElem("barakk2");
			//hozzáadjuk a barakk node-hoz
			TreeNode<CFejlesztesiElem> barakknode2 = barakknode.AddChild(barakk2);

			//készítünk egy fejlesztési elemet
			CFejlesztesiElem gazdasag = new CFejlesztesiElem("gazdasag");
			//hozzáadjuk a fához
			TreeNode<CFejlesztesiElem> gazdasagnode = Root.AddChild(gazdasag);

			//készítünk egy fejlesztési elemet
			CFejlesztesiElem olajkut= new CFejlesztesiElem("olajkút");
			//hozzáadjuk a barakk node-hoz
			gazdasagnode.AddChild(olajkut);
		}
		#endregion



	}
}

