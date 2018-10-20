using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sorb
{
    class Program
    {
        static void Main(string[] args)
        {

			ConsoleKeyInfo info;
			Console.WriteLine("sorbarendezes program, nyomjon egy gombot a folytatáshoz...");

			info = Console.ReadKey();

            //végtelen ciklus amíg X-et nem üt
			while (info.Key != ConsoleKey.X) {

				Console.Clear();

				//a tömb
				List<int> tomb = new List<int>();
				//egy véletlen generátor
				Random rnd = new Random();

				int lepesszam = 0;
				int tombhossz = 6;

				for (int j = 0; j < tombhossz; j++)
				{
					//tömbhöz adjuk a véletlen számot (1-100)
					tomb.Add(rnd.Next(1, 100));
				}

                //kiírjuk a rendezetlen tömböt
				Console.WriteLine("Rendezetlen:");
				for (int j = 0; j < tombhossz; j++)
				{
					Console.WriteLine(tomb[j]);
				}


				Console.WriteLine("1: buborék\n2: min-max\n3:beszúrásos\nx: exit");
                //a felhasználó választása
				info = Console.ReadKey();

                //buborék,csökkenő
				if (info.Key == ConsoleKey.D1)
				{
					for (int j = 0; j < tombhossz - 1; j++)
					{
						for (int i = 0; i < tombhossz - 1; i++)
						{
                            //itt döntünk, hogy csökkenő-e vagy növekvő
							if (tomb[i] < tomb[i + 1])
							{
                                //az i-edik és az i+1-edik elemet megcseréljük
								int temp = tomb[i];
								tomb[i] = tomb[i + 1];
								tomb[i + 1] = temp;
							}
							lepesszam++;
						}
					}
				}
                //min-max,csökkenő
				if (info.Key == ConsoleKey.D2)
				{
					//A tömb 1..N eleme közül kiválasztjuk a legkisebbet, majd azt a legelső elem helyére tesszük.
					//A tömb 2..N eleme közül kiválasztjuk a legkisebbet, majd azt a második elem helyére tesszük.
					//...
					for (int i = 0; i < tombhossz - 1; i++)
					{
						int max = -1;
						int maxpozicio = -1;
						for (int j = i; j < tombhossz; j++)
						{
							if (tomb[j] > max)
							{
								max = tomb[j];
								maxpozicio = j;
							}
							lepesszam++;
						}
						tomb[maxpozicio] = tomb[i];
						tomb[i] = max;
					}
				}
                //beszúrásos, növekvő
				if (info.Key == ConsoleKey.D3)
				{
					for (int i = 1; i < tombhossz; i++)
					{
						int ureshely = i;
						int kivalasztott = tomb[i];
						for (int j = i - 1; j > -1; j--)
						{
							if (kivalasztott < tomb[j])
							{
								ureshely = j;
								tomb[j + 1] = tomb[j];
								lepesszam++;
							}
						}
						tomb[ureshely] = kivalasztott;
					}
				}

				if (info.Key != ConsoleKey.X)
				{
					Console.WriteLine("Rendezett:");
					for (int j = 0; j < tombhossz; j++)
					{
						Console.WriteLine(tomb[j]);

					}
					Console.WriteLine("Lépésszám:{0}", lepesszam);
					Console.WriteLine("press any key...");
					info = Console.ReadKey();
				}
				

			}
		}
    }
}
