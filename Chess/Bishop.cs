using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Bishop : Piece
    {
        private char outputChar;

        public Bishop(Color pieceColor, BoardLocation location)
            : base(pieceColor, location)
        {
            if (pieceColor == Color.Black)
            {
                this.outputChar = 'b';
            }
            else
            {
                this.outputChar = 'B';
            }
        }

        public override char OutputChar()
        {
            return this.outputChar;
        }

        public override IList<BoardLocation> AvailableSpaces(BoardLocation currentLocation)
        {
            return BoardLocation.DiagonalSpaces(currentLocation);
        }

        public override void MoveToLocation(BoardLocation nextLocation)
        {
            this.Location = nextLocation;
        }

        public override PieceType GetPieceType()
        {
            return PieceType.Bishop;
        }
    }
}
