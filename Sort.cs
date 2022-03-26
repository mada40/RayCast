using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    static class Sort<T>
    {

        public static void QSort(ref List<T> list, int first, int last, Func<T, T, bool> C)
        {

            if (list.Count < 2)
            {
                return;
            }
            int n = (first + last) / 2;
            T m = list[n];
            int b = first;
            int e = last;

            do
            {
                while (C(list[b], m) && b != n && b < last)
                {
                    b++;
                }
                while (!C(list[e], m) && e != n && e > first)
                {
                    e--;
                }
                if (b <= e)
                {
                    (list[b], list[e]) = (list[e], list[b]);
                    b++;
                    e--;
                }
            }
            while (b <= e);


            if (e > first)
            {
                QSort(ref list, first, e, C);
            }
            if (b < last)
            {
                QSort(ref list, b, last, C);
            }


        }

        
    }
}
