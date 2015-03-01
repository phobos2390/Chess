using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class BoardLocation
    {
        private int rank;

        private char file;

        public BoardLocation(char file, int rank)
        {
            if (1 <= rank && rank <= 8)
            {
                this.file = file;
            }
            if ('a' <= file && file <= 'h')
            {
                this.rank = rank;
            }
            if ( 'A' <= file && file <= 'H')
            {
                this.rank = (char)(rank - ((int)'A' - (int)'a'));
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
    }
}
