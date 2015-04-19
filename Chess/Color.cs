using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public static class Extensions
    {
        public static Color GetOpposite(this Color currentColor)
        {
            if (currentColor == Color.White)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }
    }

    public enum Color
    {
        Black,
        White
    }
}
