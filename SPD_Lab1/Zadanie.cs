using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD_Lab1
{
    public class Zadanie
    {
        public int[] CzasNaMaszynie { get; }

        public Zadanie(int iloscMaszyn, params int[] czasNaMaszynie)
        {
            CzasNaMaszynie = new int[iloscMaszyn];

            czasNaMaszynie.CopyTo(CzasNaMaszynie, 0);
        }
        public void Wypisz()
        {
            foreach(var x in CzasNaMaszynie)
            {
                Console.Write($"{x} ");
            }
            Console.WriteLine();
        }
    }
}
