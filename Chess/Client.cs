using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    class Client : Form
    {
        private Board containedBoard;

        public Client()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Client
            // 
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Name = "Client";
            this.Load += new System.EventHandler(this.Client_Load);
            this.ResumeLayout(false);

            //this.BackColor = System.Drawing.Color.Black;

            this.containedBoard = new Board();
            System.Collections.ArrayList controls = containedBoard.Controls();
            foreach (Control c in controls)
            {
                this.Controls.Add(c);
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }
    }
}
