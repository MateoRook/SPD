using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD_Lab1
{
    public static class Util<T>
    {
        public static IEnumerable<T[]> Permute(T[] zad, int low = 0)
        {
            T temp;
            if (low + 1 >= zad.Length)
                yield return zad;
            else
            {
                foreach (var p in Permute(zad, low + 1))
                    yield return p;

                for (int i = low + 1; i < zad.Length; i++)
                {
                    temp = zad[low];
                    zad[low] = zad[i];
                    zad[i] = temp;

                    foreach (var p in Permute(zad, low + 1))
                        yield return p;

                    temp = zad[low];
                    zad[low] = zad[i];
                    zad[i] = temp;
                }
            }
        }
        public static int PoliczSpanC (int iloscZadan, int iloscMaszyn, Zadanie[] zadanie)
        {
            int[] czasPracyMaszyny = new int[iloscMaszyn];
            for (int i = 0; i < iloscZadan; i++)
            {
                czasPracyMaszyny[0] += zadanie[i].CzasNaMaszynie[0];
                for (int j = 1; j < iloscMaszyn; j++)
                {
                    czasPracyMaszyny[j] = Math.Max(czasPracyMaszyny[j - 1], czasPracyMaszyny[j]) + zadanie[i].CzasNaMaszynie[j];
                }
            }
            return czasPracyMaszyny[iloscMaszyn - 1];
        }
    }
}
