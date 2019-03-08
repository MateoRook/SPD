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
    }
}
