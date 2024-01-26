using System.Collections.Generic;
namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            int x = original.GetLength(0);
            int y = original.GetLength(1);
            double[,] array = new double[x, y];
            List<double> list = new List<double>();
            int amount = (int)(original.Length * whitePixelsFraction);
            double treshHold = 0;
            foreach (var number in original)
                list.Add(number);
            list.Sort();
            if (amount > 0) treshHold = list[list.Count - amount];
            else treshHold = 300;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (original[i, j] >= treshHold) array[i, j] = 1.0;
                    else array[i, j] = 0.0;
                }
            }
            return array;
        }
    }
}