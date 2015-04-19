using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ModelUnsubscriber : IDisposable
    {
        private IList<IObserver<Model>> _observers;
        private IObserver<Model> _observer;

        public ModelUnsubscriber(IList<IObserver<Model>> observers, IObserver<Model> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }

    public class Model : System.IObservable<Model>
    {
        private IList<IObserver<Model>> observers;
        private IDictionary<Color, BoardLocation> kingLocations;
        private Piece[,] board;
        private IList<Tuple<Piece,BoardLocation>> pieces;
        private IList<Piece> capturedPieces;
        private Stack<Tuple<Move, Piece>> moves;
        private Stack<Tuple<Move, Piece>> undoneMoves;
        private Color currentPlayer;
        private BoardLocation promotionLocation;

        private bool check;

        private bool promote;

        public Model()
            :this(new List<Tuple<Piece,BoardLocation>>())
        {}

        public Model(IList<Tuple<Piece,BoardLocation>> pieces)
        {
            this.pieces = pieces;
            this.kingLocations = new Dictionary<Color, BoardLocation>();
            this.kingLocations[Color.White] = null;
            this.kingLocations[Color.Black] = null;
            this.capturedPieces = new List<Piece>();
            this.moves = new Stack<Tuple<Move, Piece>>();
            this.board = new Piece[8, 8];
            this.undoneMoves = new Stack<Tuple<Move, Piece>>();
            this.observers = new List<IObserver<Model>>();
            this.currentPlayer = Color.White;
        }

        private void setLocationOfKing(BoardLocation location, Color playerColor)
        {
            this.kingLocations[playerColor] = location;
        }

        private BoardLocation getLocationOfKing(Color playerColor)
        {
            return this.kingLocations[playerColor];
        }

        public bool Promoting
        {
            get
            {
                return this.promote;
            }
        }

        private Tuple<int, int> boardLocationToIndexes(BoardLocation location)
        {
            return new Tuple<int, int>(location.File - 'a', location.Rank - 1);
        }

        public void AddPiece(Piece addPiece, BoardLocation locationOfPiece)
        {
            if (addPiece.GetPieceType() == PieceType.King)
            {
                this.kingLocations[addPiece.PieceColor] = locationOfPiece;
            }
            Tuple<int, int> locationIndexes = this.boardLocationToIndexes(locationOfPiece);
            this.board[locationIndexes.Item1, locationIndexes.Item2] = addPiece;
            //this.pieces.Add(new Tuple<Piece,BoardLocation>(addPiece,locationOfPiece));
        }

        public void RemovePieceAtLocation(BoardLocation location)
        {
            Tuple<int, int> locationIndexes = this.boardLocationToIndexes(location);
            this.board[locationIndexes.Item1, locationIndexes.Item2] = null;
            //for (int i = 0; i < this.pieces.Count; i++)
            //{
            //    if (location == this.pieces[i].Item2)
            //    {
            //        this.pieces.RemoveAt(i);
            //    }
            //}
        }

        public void Update()
        {
            foreach (IObserver<Model> observer in this.observers)
            {
                observer.OnNext(this);
            }
        }

        public void PieceCaptured(Piece capturedPiece)
        {
            this.capturedPieces.Add(capturedPiece); ;
        }

        public bool IsPieceAtLocation(BoardLocation location)
        {
            Tuple<int, int> locationIndexes = this.boardLocationToIndexes(location);
            return this.board[locationIndexes.Item1, locationIndexes.Item2] != null;
            //IEnumerable<Piece> piece = from tup in this.pieces
            //            where tup.Item2 == location
            //            select tup.Item1;
            //return piece.Any();
        }

        public Piece GetPieceAtLocation(BoardLocation location)
        {
            Tuple<int, int> locationIndexes = this.boardLocationToIndexes(location);
            return this.board[locationIndexes.Item1, locationIndexes.Item2];
            //IEnumerable<Piece> piece = from tup in this.pieces
            //                           where tup.Item2 == location
            //                           select tup.Item1;
            //return piece.First<Piece>();
        }

        public IList<Tuple<Piece, BoardLocation>> Pieces
        {
            get
            {
                return this.pieces;
            }
        }

        private void nextTurn()
        {
            this.currentPlayer = this.currentPlayer.GetOpposite();
        }

        public IDisposable Subscribe(IObserver<Model> observer)
        {
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);
                observer.OnNext(this);
            }
            return new ModelUnsubscriber(this.observers, observer);
        }

        public void Promote(BoardLocation location)
        {
            this.promote = true;
            this.promotionLocation = location;
            this.Update();
            this.promote = false;
        }

        public void PromotePawn(Piece newPiece)
        {
            this.RemovePieceAtLocation(this.promotionLocation);
            this.AddPiece(newPiece, this.promotionLocation);
        }

        private bool isValidDestination(BoardLocation destination, Color pieceColor)
        {
            bool pieceAtLocation = this.IsPieceAtLocation(destination);
            bool colorNotSame = true;
            if (pieceAtLocation)
            {
                Piece destPiece = this.GetPieceAtLocation(destination);
                colorNotSame = (destPiece.PieceColor != pieceColor);
            }
            return colorNotSame;
        }

        public bool CanExecuteMove(Move move)
        {
            Piece currentPiece = move.MovePiece;
            BoardLocation source = move.Source;
            BoardLocation destination = move.Destination;
            bool correctTurn = currentPiece.PieceColor == this.currentPlayer;
            bool newLocation = (source != destination);
            bool validSource = this.IsPieceAtLocation(source);
            bool pieceAtLocation = (this.GetPieceAtLocation(source).OutputChar() 
                == currentPiece.OutputChar());
            bool putsInCheck = this.movePutsInCheck(source, destination);
            bool pieceBetweenLocations = this.isPieceBetweenLocations(source, destination);
            bool validDestination = isValidDestination(destination, currentPiece.PieceColor);
            bool withinAvailableLocations = currentPiece.AvailableSpaces(source).Contains(destination);
            return newLocation && validSource && pieceAtLocation 
                && validDestination && !pieceBetweenLocations 
                && withinAvailableLocations && correctTurn
                && !putsInCheck;                    
        }

        public void ExecuteMove(Move move)
        {
            this.undoneMoves.Clear();
            performMove(move);
        }

        private void performMove(Move move)
        {
            Piece currentPiece = move.MovePiece;
            BoardLocation source = move.Source;
            BoardLocation destination = move.Destination;
            this.RemovePieceAtLocation(source);
            bool pieceAtLocation = this.IsPieceAtLocation(destination);
            Piece capturePiece = null;
            if (pieceAtLocation)
            {
                capturePiece = this.GetPieceAtLocation(destination);
                this.PieceCaptured(capturePiece);
                this.RemovePieceAtLocation(destination);
            }
            this.AddPiece(currentPiece, destination);
            currentPiece.MoveToLocation(destination);
            this.moves.Push(new Tuple<Move, Piece>(move, capturePiece));
            this.nextTurn();
            this.Update();
        }

        public void UndoMove()
        {
            Tuple<Move, Piece> tup = this.moves.Peek();
            char outPiece = tup.Item1.MovePiece.OutputChar();
            bool king = tup.Item1.MovePiece.GetPieceType() == PieceType.King;
            bool leftCastle = tup.Item1.Source.File - tup.Item1.Destination.File == 2;
            bool rightCastle = tup.Item1.Destination.File - tup.Item1.Source.File == 2;
            int rookLocation = (tup.Item1.Source.File + tup.Item1.Destination.File)/2;
            if (king)
            {
                BoardLocation currentPlaceOfRook = new BoardLocation((char)rookLocation, tup.Item1.Source.Rank);
                Piece rook = this.GetPieceAtLocation(currentPlaceOfRook);
                if (rightCastle)
                {
                    BoardLocation previousPlaceOfRook = new BoardLocation('H', tup.Item1.Source.Rank);
                    this.RemovePieceAtLocation(currentPlaceOfRook);
                    this.AddPiece(rook, previousPlaceOfRook);
                }
                else if (leftCastle)
                {
                    BoardLocation previousPlaceOfRook = new BoardLocation('A', tup.Item1.Source.Rank);
                    this.RemovePieceAtLocation(currentPlaceOfRook);
                    this.AddPiece(rook, previousPlaceOfRook);
                }
            }
            this.RemovePieceAtLocation(tup.Item1.Destination);
            this.AddPiece(tup.Item1.MovePiece, tup.Item1.Source);
            this.undoneMoves.Push(tup);
            this.moves.Pop();
            if (tup.Item2 != null)
            {
                this.AddPiece(tup.Item2, tup.Item1.Destination);
            }
            this.nextTurn();
        }

        public void RedoMove()
        {
            if (this.undoneMoves.Count > 0)
            {
                Tuple<Move, Piece> tup = this.undoneMoves.Peek();
                this.performMove(tup.Item1);
                this.undoneMoves.Pop();
            }
        }

        private IList<BoardLocation> getPiecesBetweenLocations(BoardLocation source, BoardLocation destination)
        {
            IList<BoardLocation> returnLocations = new List<BoardLocation>();
            IList<BoardLocation> locations = BoardLocation.LocationsBetweenTwoLocation(source, destination);
            foreach (BoardLocation loc in locations)
            {
                if (this.IsPieceAtLocation(loc))
                {
                    returnLocations.Add(loc);
                }
            }
            return returnLocations;
        }

        private bool isPieceBetweenLocations(BoardLocation source, BoardLocation destination)
        {
            return getPiecesBetweenLocations(source, destination).Count > 0;
            //IList<BoardLocation> locations = BoardLocation.LocationsBetweenTwoLocation(source, destination);
            //foreach (BoardLocation loc in locations)
            //{
            //    if(this.IsPieceAtLocation(loc))
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        public void PerformCastle(Move castleMove,BoardLocation rookLocation)
        {
            int rookPlacement = (castleMove.Source.File + castleMove.Destination.File) / 2;
            Piece rook = this.GetPieceAtLocation(rookLocation);
            this.RemovePieceAtLocation(rookLocation);
            this.AddPiece(rook, new BoardLocation((char)rookPlacement, castleMove.Destination.Rank));
        }

        private bool movePutsInCheck(BoardLocation src, BoardLocation dst)
        {
            Piece movingPiece = this.GetPieceAtLocation(src);
            Move testMove = new Move(movingPiece, src, dst);
            bool currentlyInCheck = inCheck(movingPiece.PieceColor);
            BoardLocation kingLocation = movingPiece.Location;
            IList<BoardLocation> loc = BoardLocation.LocationsBetweenTwoLocation(src, dst);
            bool movingIntoCheck = piecesAreAttackingSpot(movingPiece.PieceColor,dst);
            bool castling = loc.Count > 0;
            bool movingOutOfCheck = currentlyInCheck;
            bool movingThroughCheck = castling && piecesAreAttackingSpot(movingPiece.PieceColor,loc.First());
            if (movingPiece.GetPieceType() != PieceType.King)
            {
                currentlyInCheck = this.moveRevealsCheck(testMove);
            }
            else if(castling)
            {
                if(!movingIntoCheck
                    && !movingOutOfCheck
                    && !movingThroughCheck)
                {
                    currentlyInCheck = false;
                }
            }
            else if (!movingIntoCheck)
            {
                currentlyInCheck = false;
            }
            return currentlyInCheck;
        }

        public IList<BoardLocation> AvailableMoves(BoardLocation location)
        {
            IList<BoardLocation> returnLocations = new List<BoardLocation>();
            Piece piece = this.GetPieceAtLocation(location);
            if (piece != null)
            {
                IList<BoardLocation> availableSpaces = piece.AvailableSpaces(location);
                foreach (BoardLocation loc in availableSpaces)
                {
                    bool obstructingPiece = this.isPieceBetweenLocations(location, loc);
                    bool validDestination = this.isValidDestination(loc, piece.PieceColor);
                    bool putsInCheck = this.movePutsInCheck(location, loc);
                    bool validMove = !obstructingPiece && validDestination && !putsInCheck;
                    if (validMove)
                    {
                        returnLocations.Add(loc);
                    }
                }
            }
            return returnLocations;
        }

        private IList<BoardLocation> piecesAttackingSpot(Color playerColor, BoardLocation spotBeingAttacked)
        {
            IList<BoardLocation> returnLocations = new List<BoardLocation>();
            PieceFactory factory = new PieceFactory(this);
            Color opponentColor = playerColor.GetOpposite();
            foreach(PieceType type in Enum.GetValues(PieceType.Queen.GetType()))
            {
                IPieceMove move = factory.getCheckMoveFromType(type, playerColor, spotBeingAttacked);
                foreach(BoardLocation loc in move.AvailableLocations(spotBeingAttacked))
                {
                    if (this.IsPieceAtLocation(loc) 
                        && !this.isPieceBetweenLocations(loc,spotBeingAttacked))
                    {
                        Piece pieceAtLocation = this.GetPieceAtLocation(loc);
                        if (pieceAtLocation.GetPieceType() == type
                            && pieceAtLocation.PieceColor == opponentColor)
                        {
                            returnLocations.Add(loc);
                        }
                    }
                }
            }
            return returnLocations;
        }

        //private BoardLocation getLocationOfKing(Color playerColor)
        //{
        //    BoardLocation locationOfKing = null;
        //    for(int i = 0; i < this.board.GetUpperBound(0) + 1; i++)
        //    {
        //        for(int j = 0; j < this.board.GetUpperBound(1) + 1; j++)
        //        {
        //            Piece tup = this.board[i, j];
        //            if (tup != null
        //                && tup.PieceColor == playerColor)
        //            {
        //                if (tup.GetPieceType() == PieceType.King)
        //                {
        //                    locationOfKing = new BoardLocation((char)(i + 'a'), j + 1);
        //                }
        //            }
        //        }
        //    }
        //    return locationOfKing;
        //}

        private IList<BoardLocation> piecesGivingCheck(Color playerColor)
        {
            IList<BoardLocation> returnLocations = new List<BoardLocation>();
            BoardLocation locationOfKing = getLocationOfKing(playerColor);
            if (locationOfKing != null)
            {
                return this.piecesAttackingSpot(playerColor, locationOfKing);
            }
            else
            {
                return returnLocations;
            }
        }

        private bool hasKingMoved(Color playerColor)
        {
            bool kingMoved = false;
            foreach (Tuple<Move, Piece> tup in this.moves)
            {
                bool colorSame = tup.Item1.MovePiece.PieceColor == playerColor;
                char pieceChar = tup.Item1.MovePiece.OutputChar();
                bool king = tup.Item1.MovePiece.GetPieceType() == PieceType.King;
                if (king && colorSame)
                {
                    kingMoved = true;
                }
            }
            return kingMoved;
        }

        private bool hasCastleMoved(char initialFile, Color playerColor)
        {
            bool rookMoved = false;
            foreach (Tuple<Move, Piece> tup in this.moves)
            {
                bool colorSame = tup.Item1.MovePiece.PieceColor == playerColor;
                char pieceChar = tup.Item1.MovePiece.OutputChar();
                bool rook = tup.Item1.MovePiece.GetPieceType() == PieceType.Rook;
                bool left = tup.Item1.Source == new BoardLocation(initialFile, 1)
                    || tup.Item1.Source == new BoardLocation(initialFile, 8);
                if (rook && colorSame)
                {
                    if (left)
                    {
                        rookMoved = true;
                    }
                }
            }
            return rookMoved;
        }

        public bool CanQueenCastle(Color playerColor)
        {
            return !this.hasKingMoved(playerColor) 
                && !this.hasCastleMoved('A',playerColor);
        }

        public bool CanKingCastle(Color playerColor)
        {
            return !this.hasKingMoved(playerColor) 
                && !this.hasCastleMoved('H', playerColor);
        }

        public bool CanCastle(Color playerColor)
        {
            return !this.hasKingMoved(playerColor) 
                && !this.hasCastleMoved('A', playerColor) 
                && !this.hasCastleMoved('H', playerColor);
        }

        public Move LastMove()
        {
            return this.moves.Peek().Item1;
        }

        private bool moveRevealsCheck(Move checkMove)
        {
            bool revealsCheck = false;
            this.RemovePieceAtLocation(checkMove.Source);
            Piece capturedPiece = null;
            if (this.IsPieceAtLocation(checkMove.Destination))
            {
                capturedPiece = this.GetPieceAtLocation(checkMove.Destination);
                this.RemovePieceAtLocation(checkMove.Destination);
            }
            this.AddPiece(checkMove.MovePiece, checkMove.Destination);
            if (inCheck(checkMove.MovePiece.PieceColor))
            {
                revealsCheck = true;
            }
            this.RemovePieceAtLocation(checkMove.Destination);
            this.AddPiece(checkMove.MovePiece, checkMove.Source);
            if (capturedPiece != null)
            {
                this.AddPiece(capturedPiece, checkMove.Destination);
            }
            return revealsCheck;
        }

        private bool piecesAreAttackingSpot(Color playerColor, BoardLocation loc)
        {
            return this.piecesAttackingSpot(playerColor, loc).Count > 0;
        }

        private bool inCheck(Color playerColor)
        {
            return this.piecesGivingCheck(playerColor).Count > 0;
        }

        private IList<Move> possibleMoves(Color playerColor)
        {
            List<Move> returnList = new List<Move>();
            foreach(Tuple<Piece,BoardLocation> tup in this.pieces)
            {
                if (tup.Item1.PieceColor == playerColor)
                {
                    IList<BoardLocation> availableLocations = this.AvailableMoves(tup.Item2);
                    foreach (BoardLocation location in availableLocations)
                    {
                        Move addMove = new Move(tup.Item1, tup.Item2, location);
                        returnList.Add(addMove);
                    }
                }
            }
            return returnList;
        }
    }
}
