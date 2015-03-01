using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    abstract class Piece
    {
        private Color pieceColor;
        private BoardLocation location;

        protected string getRoot()
        {
            string root;
            if (this.pieceColor == Color.White)
            {
                root = "White";
            }
            else
            {
                root = "Black";
            }
            return root;
        }

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
    }
}
