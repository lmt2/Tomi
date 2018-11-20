using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.IO;

using System.Xml.Serialization;

namespace harcocska
{
	[Serializable()]
	public class CTerkep
	{
		public int szelesseg { get; set; }
		public int magassag { get; set; }
		private CGame parent = null;
		
		public ETerkepAllapot terkepAllapot { get; set; }
		
		public List<List<CTerkepiCella>> sorok = new List<List<CTerkepiCella>>();

		public List<List<int>> szomdossagiMatrix = new List<List<int>>();
		public List<List<int>> latogatottVertex = new List<List<int>>();
		public List<List<int>> tavolsagTabla = new List<List<int>>();
		public List<csucs> prev = new List<csucs>();
		//List<csucs> Q = new List<csucs>();
		public CTerkep(CGame g)
		{
			parent = g;
			terkepAllapot = ETerkepAllapot.szabad;
			szelesseg = 20;
			magassag = 20;
			//int szamlalo = 0;
			for (int j = 0; j < magassag; j++)
			{
				List<CTerkepiCella> sor = new List<CTerkepiCella>();
				for (int i = 0; i < szelesseg; i++)
				{
					CTerkepiCella c = new CTerkepiCella(j, i);
					c.cellaTipus = ECellaTipus.viz;
					c.tulaj = parent.jatekosok[1];
					sor.Add(c);

					//szamlalo++;
					//if (szamlalo>0 && szamlalo < 133) { c.tulaj = App.jatek.jatekosok[0]; }
					//else if (szamlalo >= 133 && szamlalo < 266) { c.tulaj = App.jatek.jatekosok[1]; }
					//else { c.tulaj = App.jatek.jatekosok[2]; }


				}
				sorok.Add(sor);
			}
		}
		
		

	
		

		


		public void Dijkstra(CTerkepiCella startCella)
		{

			szomdossagiMatrix.Clear();
			tavolsagTabla.Clear();
			prev.Clear();

			for (int j = 0; j < magassag * szelesseg; j++)
			{
				List<int> sor = new List<int>();
				for (int i = 0; i < magassag * szelesseg; i++)
				{
					sor.Add(0);
				}
				szomdossagiMatrix.Add(sor);
			}

			for (int j = 0; j < magassag; j++)
			{
				for (int i = 0; i < szelesseg; i++)
				{
					CTerkepiCella tc = null;
					if (sorok[j][i].tulaj != null)
					{
						tc = Fel(sorok[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = JobbraFel(sorok[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = JobbraLe(sorok[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = Le(sorok[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = BalraLe(sorok[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						tc = BalraFel(sorok[j][i]);
						if (tc != null && tc.tulaj != null)
						{
							szomdossagiMatrix[j * szelesseg + i][tc.Sor * szelesseg + tc.Oszlop] = 1;
							szomdossagiMatrix[tc.Sor * szelesseg + tc.Oszlop][j * szelesseg + i] = 1;
						}
						
						if (sorok[j][i].extraSzomszed != null){ 
							foreach (CTerkepiCella extra in sorok[j][i].extraSzomszed)
							{
								if (extra != null && extra.tulaj != null)
								{
									szomdossagiMatrix[j * szelesseg + i][extra.Sor * szelesseg + extra.Oszlop] = 3;
									szomdossagiMatrix[extra.Sor * szelesseg + extra.Oszlop][j * szelesseg + i] = 3;
								}
							}
						
						}

					}
				}
			}

			for (int j = 0; j < magassag; j++)
			{
				List<int> sor = new List<int>();
				for (int i = 0; i < szelesseg; i++)
				{
					sor.Add(50000);
				}
				tavolsagTabla.Add(sor);
			}


			tavolsagTabla[startCella.Sor][startCella.Oszlop] = 0;



			List<csucs> Qv = new List<csucs>();

			csucs kezdopont;
			kezdopont.Sor = startCella.Sor;
			kezdopont.Oszlop = startCella.Oszlop;
			Qv.Add(kezdopont);
			csucs v = new csucs();

			while (Qv.Count > 0)
			{

				csucs u = Qv.Last<csucs>();
				Qv.Remove(Qv.Last<csucs>());

				int ujUt = 0;
				for (int i = 0; i < magassag * szelesseg; i++)
				{
					if (szomdossagiMatrix[u.Sor * szelesseg + u.Oszlop][i] !=0 )
					{
						v.Sor = i / magassag;
						v.Oszlop = i % szelesseg;
						
						int alt = tavolsagTabla[u.Sor][u.Oszlop] + szomdossagiMatrix[u.Sor * szelesseg + u.Oszlop][i];
						if (alt < tavolsagTabla[v.Sor][v.Oszlop])
						{
							tavolsagTabla[v.Sor][v.Oszlop] = alt;
							prev.Add(u);
							Qv.Add(v);
							ujUt++;
						}
						
					}
				}
			}
			
		}
		[Serializable()]
		public struct csucs
		{
			public int Sor;
			public int Oszlop;
		}

		public CTerkepiCella BalraFel(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if ((tc.Oszlop % 2) == 0)
			{
				if ((tc.Sor - 1 < 0) || (tc.Oszlop - 1 < 0))
					return ret;
				ret = sorok[tc.Sor - 1][tc.Oszlop - 1];
			}
			else
			{
				if (tc.Oszlop - 1 < 0)
					return ret;
				ret = sorok[tc.Sor][tc.Oszlop - 1];
			}
			return ret;
		}

		public CTerkepiCella BalraLe(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if (tc.Oszlop - 1 < 0)
				return ret;
			if ((tc.Oszlop % 2) == 0)
			{
				ret = sorok[tc.Sor][tc.Oszlop - 1];
			}
			else
			{
				if (tc.Sor + 1 > magassag - 1)
					return ret;
				ret = sorok[tc.Sor + 1][tc.Oszlop - 1];
			}
			return ret;
		}

		public CTerkepiCella JobbraFel(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;

			if ((tc.Oszlop % 2) == 0)
			{
				if ((tc.Sor - 1 < 0) || (tc.Oszlop + 1 > szelesseg - 1))
					return ret;
				ret = sorok[tc.Sor - 1][tc.Oszlop + 1];
			}
			else
			{
				if (tc.Oszlop + 1 > szelesseg - 1)
					return ret;
				ret = sorok[tc.Sor][tc.Oszlop + 1];
			}
			return ret;
		}
		public CTerkepiCella JobbraLe(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;

			if ((tc.Oszlop % 2) == 0)
			{
				if ((tc.Oszlop + 1) > (szelesseg - 1))
					return ret;
				ret = sorok[tc.Sor][tc.Oszlop + 1];
			}
			else
			{
				if (((tc.Sor + 1) > (magassag - 1) || (tc.Oszlop + 1) > (szelesseg - 1)))
					return ret;
				ret = sorok[tc.Sor + 1][tc.Oszlop + 1];
			}
			return ret;
		}
		public CTerkepiCella Le(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if (tc.Sor + 1 > magassag - 1)
				return ret;
			ret = sorok[tc.Sor + 1][tc.Oszlop];
			return ret;
		}
		public CTerkepiCella Fel(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			if (tc.Sor - 1 < 0)
				return ret;
			ret = sorok[tc.Sor - 1][tc.Oszlop];
			return ret;
		}

		public CTerkepiCella Balra(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			ret = sorok[tc.Sor][tc.Oszlop - 1];
			return ret;
		}
		public CTerkepiCella Jobbra(CTerkepiCella tc)
		{
			CTerkepiCella ret = null;
			ret = sorok[tc.Sor][tc.Oszlop + 1];
			return ret;
		}

		public bool EgySorbanVannak(CTerkepiCella tc1, CTerkepiCella tc2)
		{
			bool ret = false;
			if (tc1.Sor == tc2.Sor)
				ret = true;
			return ret;
		}

		public bool EgyOszlopbanVannak(CTerkepiCella tc1, CTerkepiCella tc2)
		{
			bool ret = false;
			if (tc1.Oszlop == tc2.Oszlop)
				ret = true;
			return ret;
		}

		



		





		
	}


}