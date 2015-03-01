using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Chess
{
    class Board : Control
    {
        private Space[,] spaces = new Space[8, 8];
        private IDictionary<Space, System.Drawing.Point> board;
        private Image backGround;

        public Board()
        {
            this.backGround = System.Drawing.Bitmap.FromFile("ChessBackGround.png");
            this.ClientSize = new Size(400,400);
            this.Visible = true;

            board = new Dictionary<Space, System.Drawing.Point>();

            Color spaceColor = Color.White;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int rank = (i + 1);
                    char file = (char)(j + (int)'a');
                    BoardLocation spaceLocation = new BoardLocation(file, rank);

                    this.spaces[i, j] = new Space(spaceLocation, spaceColor);
                    if (spaceColor == Color.White)
                    {
                        spaceColor = Color.Black;
                    }
                    else
                    {
                        spaceColor = Color.White;
                    }

                    this.board.Keys.Add(this.spaces[i, j]);
                    this.board[this.spaces[i, j]] = PointConverter.ToSystemPoint(spaceLocation);
                }
            }
        }

        

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            for(int i = 0; i < 
        }
    }
}
