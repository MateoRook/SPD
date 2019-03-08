using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            int iloscZadan, iloscMaszyn;
            string line;
            Zadanie[] zadania;
            int minSpan = int.MaxValue;

            try
            {
                Console.Write("PathToFile: ");
                string path = Console.ReadLine();

                using (StreamReader sr = new StreamReader(path))
                {

                    line = sr.ReadLine();
                    string[] znaki = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    iloscZadan = int.Parse(znaki[0]);
                    iloscMaszyn = int.Parse(znaki[1]);

                    zadania = new Zadanie[iloscZadan];

                    for (int i = 0; i < iloscZadan; i++)
                    {
                        line = sr.ReadLine();
                        znaki = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        int[] liczby = new int[znaki.Length];

                        for (int j = 0; j < znaki.Length; j++)
                        {
                            liczby[j] = int.Parse(znaki[j]);
                        }
                        zadania[i] = new Zadanie(iloscMaszyn, liczby);
                    }
                }
                foreach (var p in Util<Zadanie>.Permute(zadania))
                {
                    //int[] czasPracyMaszyny = new int[iloscMaszyn];
                    //for (int i = 0; i < iloscZadan; i++)
                    //{
                    //    czasPracyMaszyny[0] += p[i].CzasNaMaszynie[0];
                    //    for (int j = 1; j < iloscMaszyn; j++)
                    //    {
                    //        czasPracyMaszyny[j] = Math.Max(czasPracyMaszyny[j - 1], czasPracyMaszyny[j]) + p[i].CzasNaMaszynie[j];
                    //    }
                    //}
                    int czasWykonania = Util<Zadanie>.PoliczSpanC(iloscZadan, iloscMaszyn, p);
                    Console.WriteLine($"Czas wykonania {czasWykonania}");
                    minSpan = Math.Min(minSpan, czasWykonania);
                }
                Console.WriteLine($"Minimalny czas pracy to :{minSpan}");
            }
            catch (Exception e)
            {
                Console.Write(e.Message); Console.ReadKey();
            }
        }   
    }
}
