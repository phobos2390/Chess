using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class BoardLocation
    {
        private int rank;

        private char file;

        public BoardLocation(char file, int rank)
        {
            if (1 <= rank && rank <= 8)
            {
                this.rank = rank;
            }
            if ('a' <= file && file <= 'h')
            {
                this.file = file;
            }
            if ( 'A' <= file && file <= 'H')
            {
                this.file = (char)(file - ((int)'A' - (int)'a'));
            }
        }

        public override string ToString()
        {
            return file.ToString() + rank.ToString();
        }

        public int Rank
        {
            get
            {
                return this.rank;
            }
        }

        public char File
        {
            get
            {
                return this.file;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is BoardLocation)
            {
                BoardLocation other = obj as BoardLocation;
                bool rankEqual = (this.rank == other.rank);
                bool fileEqual = (this.file == other.file);
                return rankEqual && fileEqual;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.File * 71 + this.Rank * 29;
        }

        public static bool operator ==(BoardLocation rightOperand, BoardLocation leftOperand)
        {
            if (!object.Equals(rightOperand, null))
            {
                return rightOperand.Equals(leftOperand);
            }
            else
            {
                return object.Equals(leftOperand, null);
            }
        }

        public static bool operator !=(BoardLocation rightOperand, BoardLocation leftOperand)
        {
            return !(rightOperand == leftOperand);
        }

        public static bool IsValid(int rank, char file)
        {
            bool validRank = (1 <= rank && rank <= 8);
            bool validFile = ('a' <= file && file <= 'h'
                || 'A' <= file && file <= 'H');
            return validFile && validRank;
        }

        public static IList<BoardLocation> SpacesInRank(int rank)
        {
            IList<BoardLocation> returnLocations = new List<BoardLocation>();
            for (int i = 0; i < 8; i++)
            {
                char file = (char)(i+'a');
                returnLocations.Add(new BoardLocation(file, rank));
            }
            return returnLocations;
        }

        public static IList<BoardLocation> SpacesInFile(char file)
        {
            IList<BoardLocation> returnLocations = new List<BoardLocation>();
            for (int i = 0; i < 8; i++)
            {
                int rank = i+1;
                returnLocations.Add(new BoardLocation(file, rank));
            }
            return returnLocations;
        }

        public static bool OnSameRank(BoardLocation first, BoardLocation second)
        {
            return first.Rank == second.Rank;
        }

        public static bool OnSameFile(BoardLocation first, BoardLocation second)
        {
            return first.File == second.File;
        }

        public static bool OnSameDiagonal(BoardLocation first, BoardLocation second, BoardLocation third)
        {
            return BoardLocation.OnSameDiagonal(first, second) 
                && BoardLocation.OnSameDiagonal(first, third) 
                && BoardLocation.OnSameDiagonal(second, third);
        }

        public static bool OnSameDiagonal(BoardLocation first, BoardLocation second)
        {
            return Math.Abs(first.file - second.file) == Math.Abs(first.Rank - second.Rank);
        }

        public static IList<BoardLocation> DiagonalSpaces(BoardLocation location)
        {
            IList<BoardLocation> returnLocations = new List<BoardLocation>();
            for (int i = 0; i < 8; i++)
            {
                int r1 = location.Rank - i;
                int r2 = location.Rank + i;
                char f1 = (char)(location.File - i);
                char f2 = (char)(location.File + i);
                if (BoardLocation.IsValid(r1, f1))
                {
                    returnLocations.Add(new BoardLocation(f1, r1));
                }
                if (BoardLocation.IsValid(r1, f2))
                {
                    returnLocations.Add(new BoardLocation(f2, r1));
                }
                if (BoardLocation.IsValid(r2, f1))
                {
                    returnLocations.Add(new BoardLocation(f1, r2));
                }
                if (BoardLocation.IsValid(r2, f2))
                {
                    returnLocations.Add(new BoardLocation(f2, r2));
                }
            }
            return returnLocations;
        }

        public static IList<BoardLocation> SurroundingSpaces(BoardLocation location)
        {
            IList<BoardLocation> returnLocations = new List<BoardLocation>();
            char initialFile = (char)(location.File - 1);
            int initialRank = location.Rank - 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    char currentFile = (char)(initialFile + i);
                    int currentRank = initialRank + j;
                    bool fileSame = (currentFile == location.File);
                    bool rankSame = (currentRank == location.Rank);
                    bool validLocation = BoardLocation.IsValid(currentRank, currentFile);
                    if (!(fileSame && rankSame) && validLocation)
                    {
                        returnLocations.Add(new BoardLocation(currentFile, currentRank));
                    }
                }
            }
            return returnLocations;
        }

        public static IList<BoardLocation> LocationsBetweenTwoLocation(BoardLocation firstLocation, BoardLocation secondLocation)
        {
            IList<BoardLocation> returnLocations = new List<BoardLocation>();
            bool inSameRank = firstLocation.Rank == secondLocation.Rank;
            bool inSameFile = firstLocation.File == secondLocation.File;
            int rankDifference = Math.Abs(firstLocation.Rank - secondLocation.Rank);
            int fileDifference = Math.Abs(firstLocation.File - secondLocation.File);
            int minFile = Math.Min(firstLocation.File, secondLocation.File);
            int maxFile = Math.Max(firstLocation.File, secondLocation.File);
            int minRank = Math.Min(firstLocation.Rank, secondLocation.Rank);
            int maxRank = Math.Max(firstLocation.Rank, secondLocation.Rank);
            bool diagonal = rankDifference == fileDifference;
            if (inSameRank)
            {
                foreach (BoardLocation loc in BoardLocation.SpacesInRank(minRank))
                {
                    bool valid = minFile < loc.File && loc.File < maxFile;
                    if (valid)
                    {
                        returnLocations.Add(loc);
                    }
                }
            }
            else if (inSameFile)
            {
                foreach (BoardLocation loc in BoardLocation.SpacesInFile((char)minFile))
                {
                    bool valid = minRank < loc.Rank && loc.Rank < maxRank;
                    if (valid)
                    {
                        returnLocations.Add(loc);
                    }
                }
            }
            else if (diagonal)
            {
                foreach (BoardLocation loc in BoardLocation.DiagonalSpaces(firstLocation))
                {
                    bool validRank = minRank < loc.Rank && loc.Rank < maxRank;
                    bool validFile = minFile < loc.File && loc.File < maxFile;
                    bool valid = validFile && validRank;
                    if (valid)
                    {
                        returnLocations.Add(loc);
                    }
                }
            }
            return returnLocations;
        }
    }
}
