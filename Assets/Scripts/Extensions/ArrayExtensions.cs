using UnityEngine;

namespace Extensions
{
    public static class ArrayExtensions
    {
        public static void Swap<T>(this T[,] array, Vector2Int first, Vector2Int second)
        {
            (array[first.x, first.y], array[second.x, second.y]) = (array[second.x, second.y], array[first.x, first.y]);
        }

        public static T At<T>(this T[,] array, Vector2Int position)
        {
            return array[position.x, position.y];
        }

    }

}