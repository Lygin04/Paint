namespace Paint
{
    internal class Utils
    {
        public static T[,] Copy<T>(T[,] array)
        {
            var width = array.GetLength(0);
            var height = array.GetLength(1);
            var copy = new T[width, height];

            for (var w = 0; w < width; w++)
            {
                for (var h = 0; h < height; h++)
                {
                    copy[w, h] = array[w, h];
                }
            }

            return copy;
        }
    }
}
