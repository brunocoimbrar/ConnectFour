using System.Collections.Generic;

namespace ConnectFour
{
    public static class ReadOnlyListExtensions
    {
        public static int IndexOf<T>(this IReadOnlyList<T> list, T element)
            where T : class
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == element)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
