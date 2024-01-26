using System.Collections.Generic;

namespace Recognizer
{
	internal static class MedianFilterTask
	{
        /* 
		 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
		 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
		 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
		 * https://en.wikipedia.org/wiki/Median_filter
		 * 
		 * Используйте окно размером 3х3 для не граничных пикселей,
		 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
		 */
        public static double[,] MedianFilter(double[,] original)
        {
            int x = original.GetLength(0);
            int y = original.GetLength(1);
            double[,] array = new double[x, y];
            int k = 1;
            List<double> list = new List<double>();

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    int vi = i;
                    int bj = j;
                    int xi = i;
                    int cj = j;
                    int ii = i;
                    int jj = j;
                    if (x > 1 && y > 1)
                    {
                        if (i == 0 && j == 0)
                        {
                            Calculate2X2(list, i, j, vi + 1, bj + 1, array, original);
                        }
                        if (i == 0 && j == y - 1)
                        {
                            Calculate2X2(list, i, j, vi + 1, bj - 1, array, original);
                        }
                        if (i == x - 1 && j == 0)
                        {
                            Calculate2X2(list, i, j, vi - 1, bj + 1, array, original);
                        }
                        if (i == x - 1 && j == y - 1)
                        {
                            Calculate2X2(list, i, j, vi - 1, bj - 1, array, original);
                        }
                        if (i == 0 && j != 0 && j != y - 1)
                        {
                            Calculate2X3(list,i,j,vi + 1,bj - 1,xi + 1,cj + 1,ii,jj - 1,array,original);
                        }
                        if (i != 0 && j == 0 && i != x - 1)
                        {
                            Calculate2X3(list, i, j, vi + 1, bj + 1, xi - 1, cj + 1, ii + 1, jj, array, original);
                        }
                        if (i != 0 && j == y - 1 && i != x - 1)
                        {
                            Calculate2X3(list, i, j, vi + 1, bj - 1, xi - 1, cj - 1, ii + 1, jj, array, original);
                        }
                        if (i == x - 1 && j != 0 && j != y - 1)
                        {
                            Calculate2X3(list, i, j, vi - 1, bj - 1, xi - 1, cj + 1, ii, jj - 1, array, original);
                        }
                        if (i != 0 && i != x - 1 && j != y - 1 && j != 0)
                        {
                            list.Add(original[i, j]);
                            list.Add(original[i, j + 1]);
                            list.Add(original[i, j - 1]);
                            list.Add(original[i - 1, j - 1]);
                            list.Add(original[i - 1, j]);
                            list.Add(original[i - 1, j + 1]);
                            list.Add(original[i + 1, j - 1]);
                            list.Add(original[i + 1, j]);
                            list.Add(original[i + 1, j + 1]);
                            list.Sort();
                            array[i, j] = list[4];
                            list.Clear();
                        }
                    }
                    else if (x == 1 && y == 1)
                    {
                        array[i, j] = original[i, j];
                    }
                    else if (x == 1 && y > 1)
                    {
                        if (k == y)
                        {
                            array[i, j] = (original[i, j] + original[i, j - 1]) / 2;
                            break;
                        }
                        if ( i == 0 && j == 0)
                        {
                            array[i, j] = (original[i, j] + original[i, j + 1]) / 2;
                            k++;
                            continue;
                        }
                        if (k != y)
                        {
                            list.Add(original[i, j]);
                            list.Add(original[i, j + 1]);
                            list.Add(original[i, j - 1]);
                            list.Sort();
                            array[i, j] = list[1];
                            k++;
                            list.Clear();
                        }  
                    }
                }
            }
            return array;
        }
        static double Calculate2X2 (List<double> list,int i, int j,int vi, int bj, double[,] array, double[,] original)
        {
            list.Add(original[i, j]);
            list.Add(original[vi, j]);
            list.Add(original[i, bj]);
            list.Add(original[vi, bj]);
            list.Sort();
            array[i, j] = (list[1] + list[2]) / 2;
            list.Clear();
            return array[i, j];
        }
        static double Calculate2X3(List<double> list, int i, int j, int vi, int bj, int xi, int cj, int ii,int jj, double[,] array, double[,] original)
        {
            list.Add(original[i, j]);
            list.Add(original[i, bj]);
            list.Add(original[ii, cj]);
            list.Add(original[xi, j]);
            list.Add(original[vi, jj]);
            list.Add(original[xi, cj]);
            list.Sort();
            array[i, j] = (list[2] + list[3]) / 2;
            list.Clear();
            return array[i, j];
        }
    }
}