using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public partial class Pawn : Piece
    {
        private Model model;
        private char outputChar;
        private IMoveOrientationBehavior directionBehavior;
        private IPawnState currentState;
        private IPawnState initialState;
        private IPawnState normalState;

        public Pawn(Color pieceColor, BoardLocation location, Model model)
            : base(pieceColor, location)
        {
            this.model = model;
            this.initialState = new PawnSecondRankState(this);
            this.normalState = new PawnNormalState(this);
            if (pieceColor == Color.Black)
            {
                this.outputChar = 'p';
                this.directionBehavior = new BlackOrientationBehavior();
                if (location.Rank == 7)
                {
                    this.currentState = this.initialState;
                }
                else 
                {
                    this.currentState = this.normalState;
                }
            }
            else
            {
                this.outputChar = 'P';
                this.directionBehavior = new WhiteOrientationBehavior();
                if (location.Rank == 2)
                {
                    this.currentState = this.initialState;
                }
                else 
                {
                    this.currentState = this.normalState;
                }
            }
        }

        public override char OutputChar()
        {
            return this.outputChar;
        }

        public IList<BoardLocation> CaptureSpaces(BoardLocation currentLocation)
        {
            return this.currentState.AvailableCaptureSpaces(currentLocation);
        }

        public override IList<BoardLocation> AvailableSpaces(BoardLocation currentLocation)
        {
            List<BoardLocation> returnLocations = new List<BoardLocation>();
            IList<BoardLocation> forward = this.currentState.AvailableForwardSpaces(currentLocation);
            returnLocations.AddRange(forward);
            IList<BoardLocation> captures = this.currentState.AvailableCaptureSpaces(currentLocation);
            returnLocations.AddRange(captures);
            return returnLocations;
        }

        public override void MoveToLocation(BoardLocation nextLocation)
        {
            this.currentState.MovePieceToSpace(nextLocation);
        }

        public override PieceType GetPieceType()
        {
            return PieceType.Pawn;
        }
    }
}
