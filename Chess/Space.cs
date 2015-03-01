using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Chess
{
    class Space
    {
        private BoardLocation location;
        private Color spaceColor;

        public Space(BoardLocation location, Color spaceColor)
        {
            this.location = location;
            this.spaceColor = spaceColor;
        }

        public BoardLocation Location
        {
            get
            {
                return this.location;
            }
        }

        public Color SpaceColor
        {
            get
            {
                return this.spaceColor;
            }
        }
    }
}
