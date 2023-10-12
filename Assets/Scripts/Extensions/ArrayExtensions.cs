using UnityEngine;

namespace Extensions
{
    public static class ArrayExtensions
    {
        public static void Swap<T>(this T[,] array, Vector3Int first, Vector3Int second)
        {
            (array[first.x, first.y], array[second.x, second.y]) = (array[second.x, second.y], array[first.x, first.y]);
        }

        public static T At<T>(this T[,] array, Vector3Int position)
        {
            return array[position.x, position.y];
        }

    }

    public static class Vector3IntExtensions
    {
        public static Vector3Int WithValueAtAxis(this Vector3Int position, int value, int axis)
        {
            position[axis] = value;
            return position;
        }
    }
}