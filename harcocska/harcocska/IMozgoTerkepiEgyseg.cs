using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harcocska
{
	public interface IMozgoTerkepiEgyseg
	{
		void MozgasJobbra();
		void mozgasIde(CTerkepiCella tc);
	}

	public interface IHarcoloTerkepiEgyseg
	{
		void Tamadas();
	}

}
