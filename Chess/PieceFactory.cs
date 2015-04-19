using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    class PieceFactory
    {
        private Model model;

        private Image whitePawnImage;
        private Image whiteBishopImage;
        private Image whiteKnightImage;
        private Image whiteRookImage;
        private Image whiteQueenImage;
        private Image whiteKingImage;

        private Image blackPawnImage;
        private Image blackBishopImage;
        private Image blackKnightImage;
        private Image blackRookImage;
        private Image blackQueenImage;
        private Image blackKingImage;

        private string getRoot(Color pieceColor)
        {
            string root;
            if (pieceColor == Color.White)
            {
                root = "White";
            }
            else
            {
                root = "Black";
            }
            return root;
        }

        private void initializeImages()
        {
            string root = "White";
            this.whitePawnImage = Image.FromFile(root + "Pawn.png");
            this.whiteBishopImage = Image.FromFile(root + "Bishop.png");
            this.whiteKnightImage = Image.FromFile(root + "Knight.png");
            this.whiteRookImage = Image.FromFile(root + "Rook.png");
            this.whiteQueenImage = Image.FromFile(root + "Queen.png");
            this.whiteKingImage = Image.FromFile(root + "King.png");

            root = "Black";
            this.blackPawnImage = Image.FromFile(root + "Pawn.png");
            this.blackBishopImage = Image.FromFile(root + "Bishop.png");
            this.blackKnightImage = Image.FromFile(root + "Knight.png");
            this.blackRookImage = Image.FromFile(root + "Rook.png");
            this.blackQueenImage = Image.FromFile(root + "Queen.png");
            this.blackKingImage = Image.FromFile(root + "King.png");
        }

        public PieceFactory(Model model)
        {
            this.initializeImages();
            this.model = model;
        }

        public IPieceMove getCheckMoveFromType(PieceType type, Color pieceColor, BoardLocation location)
        {
            switch (type)
            {
                case PieceType.Pawn:
                    return new PawnMove(new Pawn(pieceColor, location, this.model));
                case PieceType.Knight:
                    return new KnightMove();
                case PieceType.Bishop:
                    return new BishopMove();
                case PieceType.Rook:
                    return new RookMove();
                case PieceType.Queen:
                    return new QueenMove();
                case PieceType.King:
                    return new KingMove();
                default:
                    return null;
            }
        }

        public Piece fromType(PieceType type, Color pieceColor, BoardLocation location)
        {
            switch (type)
            {
                case PieceType.Pawn:
                    return new Pawn(pieceColor, location, this.model);
                case PieceType.Knight:
                    return new Knight(pieceColor, location);
                case PieceType.Bishop:
                    return new Bishop(pieceColor, location);
                case PieceType.Rook:
                    return new Rook(pieceColor, location);
                case PieceType.Queen:
                    return new Queen(pieceColor, location);
                case PieceType.King:
                    return new King(pieceColor, location, this.model);
                default:
                    return null;
            }
        }

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
                    returnPiece = new Pawn(pieceColor, location, this.model);
                    break;
                case 'n':
                case 'N':
                    returnPiece = new Knight(pieceColor, location);
                    break;
                case 'b':
                case 'B':
                    returnPiece = new Bishop(pieceColor, location);
                    break;
                case 'r':
                case 'R':
                    returnPiece = new Rook(pieceColor, location);
                    break;
                case 'q':
                case 'Q':
                    returnPiece = new Queen(pieceColor, location);
                    break;
                case 'k':
                case 'K':
                    returnPiece = new King(pieceColor, location, this.model);
                    break;
                case ' ':
                    returnPiece = null;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return returnPiece;
        }

        public Tuple<Image,Point> pieceImage(char input, BoardLocation location)
        {
            Color pieceColor;
            if (!('a' <= input && input <= 'z'))
            {
                pieceColor = Color.White;
            }
            else
            {
                pieceColor = Color.Black;
            }
            string root = this.getRoot(pieceColor);
            Image returnImage;
            Point imageLocation;
            switch (input)
            {
                case 'p':
                    returnImage = this.blackPawnImage;
                    break;
                case 'P':
                    returnImage = this.whitePawnImage;
                    break;
                case 'n':
                    returnImage = this.blackKnightImage;
                    break;
                case 'N':
                    returnImage = this.whiteKnightImage;
                    break;
                case 'b':
                    returnImage = this.blackBishopImage;
                    break;
                case 'B':
                    returnImage = this.whiteBishopImage;
                    break;
                case 'r':
                    returnImage = this.blackRookImage;
                    break;
                case 'R':
                    returnImage = this.whiteRookImage;
                    break;
                case 'q':
                    returnImage = this.blackQueenImage;
                    break;
                case 'Q':
                    returnImage = this.whiteQueenImage;
                    break;
                case 'k':
                    returnImage = this.blackKingImage;
                    break;
                case 'K':
                    returnImage = this.whiteKingImage;
                    break;
                case ' ':
                    returnImage = Image.FromFile("WhiteSpace.png");
                    break;
                default:
                    throw new NotImplementedException();
            }
            imageLocation = PointConverter.ToSystemPoint(location,returnImage.Width,returnImage.Height);
            return new Tuple<Image,Point>(returnImage,imageLocation);
        }
    }
}
