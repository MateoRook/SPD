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
            int minSpan;

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
                   // TODO: policz min C span
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message); Console.ReadKey();
            }
        }   
    }
}
