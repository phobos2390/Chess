using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    class Client : Form
    {
        private BoardComponent boardComponent;
        private BoardView boardView;
        private BoardController boardController;
        private Model model;
        private PieceFactory factory;

        public Client()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BoardBuilder builder = new BoardBuilder();
            builder.CreateInitialArrangement();
            this.model = builder.CreateModel();
            this.boardView = new BoardView();
            this.boardController = new BoardController(this.model);
            this.boardComponent = new BoardComponent();
            this.boardComponent.View = this.boardView;
            this.boardView.Component = this.boardComponent;
            this.boardView.Controller = this.boardController;
            this.boardController.View = this.boardView;
            this.model.Subscribe(this.boardController);
            this.factory = new PieceFactory(this.model);
            this.ClientSize = this.boardComponent.ClientSize;
            this.Name = "Client";
            this.ResumeLayout(false);
            //BoardLocation newLocation = new BoardLocation('B', 7);
            //BoardLocation secondLocation = new BoardLocation('D', 3);
            //Piece addPiece = factory.fromChar('p', newLocation);
            //Piece otherPiece = factory.fromChar('B', secondLocation);
            //this.model.AddPiece(addPiece, newLocation);
            //this.model.AddPiece(otherPiece, secondLocation);
            this.Controls.Add(boardComponent);
            this.Controls.Add(boardView);

            this.MouseDown += Client_MouseDown;
            this.MouseMove += Client_MouseMove;
            this.MouseUp += Client_MouseUp;

            this.Enabled = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            //this.Focus();
        }

        void Client_MouseUp(object sender, MouseEventArgs e)
        {
            this.boardComponent.Mouse_Up(sender, e);
        }

        void Client_MouseMove(object sender, MouseEventArgs e)
        {
            this.boardComponent.Mouse_Moved(sender, e);
        }

        void Client_MouseDown(object sender, MouseEventArgs e)
        {
            this.boardComponent.Mouse_Down(sender, e);
        }
    }
}
