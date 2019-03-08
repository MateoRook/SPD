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
            int iloscZadan,iloscMaszyn;
            string line;
            try
            {
                Console.Write("PathToFile: ");
                string path = Console.ReadLine();

                using (StreamReader sr = new StreamReader(path))
                {

                    line = sr.ReadLine();
                    string[] znaki = line.Split(new char[] {' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    iloscZadan = int.Parse(znaki[0]);
                    iloscMaszyn = int.Parse(znaki[1]);
                    int ilosc = iloscMaszyn * iloscZadan;
                    int[] zadania = new int[ilosc];
                    for (int i = 0; i < ilosc; i++)
                    {
                        znaki = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        zadania[i]= int.Parse(znaki[0]);
                        iloscMaszyn = int.Parse(znaki[1]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message); Console.ReadKey();
            }
        }
    }
}
