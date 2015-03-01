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
            int y = location.Rank;
            return new System.Drawing.Point(x, y);
        }
    }
}
