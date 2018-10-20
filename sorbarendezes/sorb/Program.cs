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

            //a tömb
            List<int> tomb = new List<int>();
            //egy véletlen generátor
            Random rnd = new Random();

            int lepesszam = 0;
            int tombhossz = 6;

            for (int j = 0; j < tombhossz; j++)
            {
                //tömbhöz adjuk a véletlen számot (1-10)
                tomb.Add(rnd.Next(1, 10));
            }

            Console.WriteLine("Rendezetlen:");
            for (int j = 0; j < tombhossz; j++)
            {
                Console.WriteLine(tomb[j]);
            }


            Console.WriteLine("1: buborék\n2: min-max\n3:beszúrásos");
            ConsoleKeyInfo info = Console.ReadKey();
            if (info.Key == ConsoleKey.D1)
            {
                for (int j = 0; j < tombhossz - 1; j++)
                {
                    for (int i = 0; i < tombhossz - 1; i++)
                    {
                        if (tomb[i] < tomb[i + 1])
                        {
                            int temp = tomb[i];
                            tomb[i] = tomb[i + 1];
                            tomb[i + 1] = temp;
                        }
                        lepesszam++;
                    }
                }
            }
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

            if (info.Key == ConsoleKey.D3)
            {
                for (int i = 1; i < tombhossz; i++)
                {
                    int urshely = i;
                    int kivalasztott = tomb[i];
                    for (int j = i - 1; j > -1; j--)
                    {
                        if (kivalasztott < tomb[j])
                        {
                            urshely = j;
                            tomb[j + 1] = tomb[j];
                            lepesszam++;
                        }
                    }
                    tomb[urshely] = kivalasztott;
                }
            }


            Console.WriteLine("Rendezett:");
            for (int j = 0; j < tombhossz; j++)
            {
                Console.WriteLine(tomb[j]);
                
            }
            Console.WriteLine("Lépésszám:{0}",lepesszam);
            info = Console.ReadKey();
        }
    }
}
