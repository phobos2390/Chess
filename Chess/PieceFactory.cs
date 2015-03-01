using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class PieceFactory
    {
        public Piece fromChar(char input, BoardLocation location)
        {
            Color pieceColor;
            Piece returnPiece;
            if (!('a' <= input && input <= 'z'))
            {
                pieceColor = Color.White;
            }
            else
            {
                pieceColor = Color.Black;
            }
            switch (input)
            {
                case 'p':
                case 'P':
                    returnPiece = new Pawn(pieceColor, location);
                    break;
                case 'n':
                case 'N':
                    returnPiece = null;//new Knight();
                    break;
                case 'b':
                case 'B':
                    returnPiece = null;//new Bishop();
                    break;
                case 'r':
                case 'R':
                    returnPiece = null;//new Rook();
                    break;
                case 'q':
                case 'Q':
                    returnPiece = null;//new Queen();
                    break;
                case 'k':
                case 'K':
                    returnPiece = null;//new Knight();
                    break;
                case ' ':
                    returnPiece = null;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return returnPiece;
        }
    }
}
