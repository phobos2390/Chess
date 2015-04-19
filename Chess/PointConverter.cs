using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class PointConverter
    {
        public static System.Drawing.Point ToSystemPoint(BoardLocation location)
        {
            int x = (int)location.File - 'a';
            int y = location.Rank - 1;
            return new System.Drawing.Point(x, y);
        }

        public static System.Drawing.Point ToSystemPoint(BoardLocation location, int constant)
        {
            return ToSystemPoint(location, constant, constant);
        }

        public static System.Drawing.Point ToSystemPoint(BoardLocation location, int constantX, int constantY)
        {
            System.Drawing.Point returnPoint = ToSystemPoint(location);
            return new System.Drawing.Point(returnPoint.X * constantX, returnPoint.Y * constantY);
        }
    }
}
