using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Model
    {
        private IList<Piece> pieces;

        public Model()
        {
            this.pieces = new List<Piece>();
        }

        public Model(IList<Piece> pieces)
        {
            this.pieces = pieces;
        }

        public IList<Piece> Pieces
        {
            get
            {
                return this.pieces;
            }
        }
    }
}
