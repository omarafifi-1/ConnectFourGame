using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectFourGame
{
    public partial class GameForm : Form
    {
        private Form MenuForm;
        private Logic GameLogic;
        private string GameMode;
        private int Player1Score = 0;
        private int Player2Score = 0;
        public GameForm(string Mode, Form Menu)
        {
            InitializeComponent();
            this.GameMode = Mode;
            this.GameLogic = new Logic();
            this.MenuForm = Menu;
            foreach (Control tablecell in tableLayoutPanel1.Controls)
            {
                if (tablecell is PictureBox cell)
                {
                    MakeCellCircular(cell);
                }
            }
            if (Mode == "VS Player")
            {
                this.Text = "Connect Four - Player vs Player";
                label1.Visible = true;
                label2.Visible = true;
            }
            else
            {
                this.Text = "Connect Four - Player vs AI";
                label1.Visible = true;
                label3.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.MenuForm.Show();
            this.Close();
        }

        private void MakeCellCircular(PictureBox Cell)
        {
            Cell.Width = Cell.Height;
            System.Drawing.Drawing2D.GraphicsPath Placeholder = new System.Drawing.Drawing2D.GraphicsPath();
            Placeholder.AddEllipse(0, 0, Cell.Width, Cell.Height);
            Cell.Region = new System.Drawing.Region(Placeholder);
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            PictureBox ClickedCell = (PictureBox)sender;
            TableLayoutPanelCellPosition CellPosition = tableLayoutPanel1.GetPositionFromControl(ClickedCell);
            int Column = CellPosition.Column;
            if (GameLogic.PlacePiece(Column))
            {
                UpdateBoard();
                if(GameMode == "VS Player")
                {
                    if (GameLogic.IsGameOver)
                    {
                        GameLogic.CurrentPlayer = (GameLogic.CurrentPlayer == 1) ? 2 : 1;
                        MessageBox.Show($"Player {GameLogic.CurrentPlayer} Won", "Game Over");
                        tableLayoutPanel1.Enabled = false;
                        if (GameLogic.CurrentPlayer == 1)
                        {
                            Player1Score++;
                            label1.Text = $"Player 1: {Player1Score}";
                        }
                        else
                        {
                            Player2Score++;
                            label2.Text = $"Player 2: {Player2Score}";
                        }

                    }
                    else if (GameLogic.CheckForDraw())
                    {
                        MessageBox.Show("It's a Draw!", "Game Over");
                    }
                }
            }
        }

        private void UpdateBoard()
        {
            for (int r = 0; r < 6; r++)
            {
                for (int c = 0; c < 7; c++)
                {
                    int CellValue = GameLogic.Board[5 - r, c];
                    PictureBox Cell = (PictureBox)tableLayoutPanel1.GetControlFromPosition(c, r);
                    if (CellValue == 1)
                    {
                        Cell.BackColor = Color.Red;
                    }
                    else if (CellValue == 2)
                    {
                        Cell.BackColor = Color.Yellow;
                    }
                    else
                    {
                        Cell.BackColor = Color.Gainsboro;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.GameLogic = new Logic();
            UpdateBoard();
            tableLayoutPanel1.Enabled = true;
        }
    }
}
