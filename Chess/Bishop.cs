using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Pawn : Piece
    {
        public Pawn(bool white)
            : base(white)
        {
            this.texture.Image = System.Drawing.Bitmap.FromFile(getRoot() + "Pawn.png");
            this.texture.Size = this.texture.Image.Size;
            this.texture.Visible = true;
        }

        public override void texture_Click(object sender, EventArgs e)
        {
            //TODO
        }
    }
}
