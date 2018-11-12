using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
	public interface IMozgoTerkepiEgyseg
	{
		void mozgasCellara(CTerkep t, CTerkepiCella to);
	}

	public interface IHarcoloTerkepiEgyseg
	{
		void Tamadas(CTerkepiEgyseg te);
	}

}
