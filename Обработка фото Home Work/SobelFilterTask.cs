using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var sxWidth = sx.GetLength(0);
            var sxHeight = sx.GetLength(1);
            int shift = sx.GetLength(0) / 2;
            var result = new double[width, height];
            for (int x = shift; x < width - shift; x++)
            {
                for (int y = shift; y < height - shift; y++)
                {
                    double gradX = 0;
                    double gradY = 0;
                    for (int i = -shift, z = 0; z < sxWidth; i++, z++)
                        for (int j = -shift, c = 0; c < sxHeight; j++, c++)
                        {
                            gradX += g[x + i, y + j] * sx[z, c];
                            gradY += g[x + i, y + j] * sx[c, z];
                            result[x, y] = Math.Sqrt(gradX * gradX + gradY * gradY);
                        }
                }
            }
            return result;
        }
    }
}