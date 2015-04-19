using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public abstract class Piece
    {
        private Color pieceColor;
        private BoardLocation location;

        public Piece(Color pieceColor, BoardLocation location)
        {
            this.pieceColor = pieceColor;
            this.location = location;
        }

        public BoardLocation Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        public Color PieceColor
        {
            get
            {
                return this.pieceColor;
            }
            protected set
            {
                this.pieceColor = value;
            }
        }

        public abstract char OutputChar();

        public abstract IList<BoardLocation> AvailableSpaces(BoardLocation currentLocation);

        public abstract void MoveToLocation(BoardLocation nextLocation);

        public abstract PieceType GetPieceType();
    }
}
