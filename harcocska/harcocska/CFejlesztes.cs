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
	public class CFejlesztes
	{
		//ez a fa
		public TreeNode<CFejlsztesiElem> Root { get; set; }

		#region constructors
		public CFejlesztes() {
			//készítünk egy fejlesztési elemet
			CFejlsztesiElem alap = new CFejlsztesiElem("alap");
			//ez lesz a gyökér
			Root = new TreeNode<CFejlsztesiElem>(alap);

			//készítünk egy fejlesztési elemet
			CFejlsztesiElem katonasag = new CFejlsztesiElem("katonasag");
			//hozzáadjuk a gyökérhez
			TreeNode<CFejlsztesiElem> katonasagnode =Root.AddChild(katonasag);

			//készítünk egy fejlesztési elemet
			CFejlsztesiElem barakk = new CFejlsztesiElem("barakk");
			//hozzáadjuk a katonasag node-hoz
			TreeNode<CFejlsztesiElem> barakknode = katonasagnode.AddChild(barakk);

			//készítünk egy fejlesztési elemet
			CFejlsztesiElem bunker = new CFejlsztesiElem("bunker");
			//hozzáadjuk a katonasag node-hoz
			katonasagnode.AddChild(bunker);

			//készítünk egy fejlesztési elemet
			CFejlsztesiElem barakk2 = new CFejlsztesiElem("barakk2");
			//hozzáadjuk a barakk node-hoz
			TreeNode<CFejlsztesiElem> barakknode2 = barakknode.AddChild(barakk2);

			//készítünk egy fejlesztési elemet
			CFejlsztesiElem gazdasag = new CFejlsztesiElem("gazdasag");
			//hozzáadjuk a fához
			TreeNode<CFejlsztesiElem> gazdasagnode = Root.AddChild(gazdasag);

			//készítünk egy fejlesztési elemet
			CFejlsztesiElem olajkut= new CFejlsztesiElem("olajkút");
			//hozzáadjuk a barakk node-hoz
			gazdasagnode.AddChild(olajkut);
		}
		#endregion



	}
}

