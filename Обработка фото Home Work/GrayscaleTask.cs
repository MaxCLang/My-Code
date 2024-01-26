namespace Recognizer
{
	public static class GrayscaleTask
	{
		/* 
		 * Переведите изображение в серую гамму.
		 * 
		 * original[x, y] - массив пикселей с координатами x, y. 
		 * Каждый канал R,G,B лежит в диапазоне от 0 до 255.
		 * 
		 * Получившийся массив должен иметь те же размеры, 
		 * grayscale[x, y] - яркость пикселя (x,y) в диапазоне от 0.0 до 1.0
		 *
		 * Используйте формулу:
		 * Яркость = (0.299*R + 0.587*G + 0.114*B) / 255
		 * 
		 * Почему формула именно такая — читайте в википедии 
		 * http://ru.wikipedia.org/wiki/Оттенки_серого
		 */

		public static double[,] ToGrayscale(Pixel[,] original)
		{
			int length = original.GetLength(0);
			int width = original.GetLength(1);
			double[,] array = new double[length, width];
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < width; j++)
				{
					array[i, j] = (original[i, j].R * 0.299 + original[i, j].G * 0.587
						+ original[i, j].B * 0.114) / 255;
                }
			}
			return array;
		}
	}
}