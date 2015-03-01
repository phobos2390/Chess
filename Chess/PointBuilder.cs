using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class PointBuilder
    {
        private bool xSets;
        private bool ySets;

        private int x;
        private int y;

        public PointBuilder setX(int x)
        {
            this.x = x;
            xSets = true;
            return this;
        }

        public PointBuilder setY(int y)
        {
            this.y = y;
            ySets = true;
            return this;
        }

        public System.Drawing.Point CreateStandardPoint()
        {
            if (xSets && ySets)
            {
                return new System.Drawing.Point(x, y);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public System.Drawing.Point CreateConstantMultipliedPoint(int constant)
        {
            if (xSets && ySets)
            {
                return new System.Drawing.Point(x * constant, y * constant);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public System.Drawing.Point CreateTwoConstantMultipliedPoint(int constantX, int constantY)
        {
            if (xSets && ySets)
            {
                return new System.Drawing.Point(x * constantX, y * constantY);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
