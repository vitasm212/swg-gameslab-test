using System.Collections.Generic;
using UnityEngine;

namespace Control.Core
{
    public static class Combinations
    {
        public static List<int[,]> pointsCombinations30 = new List<int[,]>()
        {
        new int[,]{
                { 1, 1, 1, 1, 1 },
                { 0, 0, 1, 0, 0 },
                { 0, 0, 1, 0, 0 },},
        new int[,]{
                { 0, 0, 1, 0, 0 },
                { 0, 0, 1, 0, 0 },
                { 1, 1, 1, 1, 1 },},
        new int[,]{
                { 1, 0, 0 },
                { 1, 0, 0 },
                { 1, 1, 1 },
                { 1, 0, 0 },
                { 1, 0, 0 },},
        new int[,]{
                { 0, 0, 1 },
                { 0, 0, 1 },
                { 1, 1, 1 },
                { 0, 0, 1 },
                { 0, 0, 1 },}
        };
        public static List<int[,]> pointsCombinations20 = new List<int[,]>()
        {
        new int[,]{
                { 1, 1, 1 },
                { 0, 0, 1 },
                { 0, 0, 1 },},
        new int[,]{
                { 0, 0, 1 },
                { 0, 0, 1 },
                { 1, 1, 1 },},
        new int[,]{
                { 1, 0, 0 },
                { 1, 0, 0 },
                { 1, 1, 1 },},
        new int[,]{
                { 1, 1, 1 },
                { 1, 0, 0 },
                { 1, 0, 0 },},
        new int[,]{
                { 1, 1, 1, 1, 1 },},
        new int[,]{
                { 1 },
                { 1 },
                { 1 },
                { 1 },
                { 1 },}
        };
        public static List<int[,]> pointsCombinations15 = new List<int[,]>()
        {
        new int[,]{
                { 1, 1, 1, 1 },},
        new int[,]{
                { 1 },
                { 1 },
                { 1 },
                { 1 },}
};
        public static List<int[,]> pointsCombinations10 = new List<int[,]>()
        {
        new int[,]{
                { 1, 1, 1 },},
        new int[,]{
                { 1 },
                { 1 },
                { 1 },}
        };

        public static Color GetColor(int index)
        {
            switch (index)
            {
                case 1:
                    return Color.red;
                case 2:
                    return Color.green;
                case 3:
                    return Color.yellow;
                case 4:
                    return Color.blue;
                case 5:
                    return Color.magenta;
                case 6:
                    return Color.cyan;
                case 7:
                    return Color.black;
                default:
                    return Color.white;
            }
        }
    }
}