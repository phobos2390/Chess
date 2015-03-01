using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Pawn : Piece
    {
        private char outputChar;

        public Pawn(Color pieceColor, BoardLocation location)
            :base(pieceColor,location)
        {
            this.outputChar = 'p';
        }

        public override char OutputChar()
        {
            return this.outputChar;
        }
    }
}
