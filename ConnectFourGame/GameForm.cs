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
        private System.Windows.Forms.Timer PlayTimer;
        private bool isAITurn = false;
        
        public GameForm(string Mode, Form Menu)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.GameMode = Mode;
            this.GameLogic = new Logic();
            this.MenuForm = Menu;
            PlayTimer = new System.Windows.Forms.Timer();
            PlayTimer.Interval = 500;
            PlayTimer.Tick += PlayTimerElapsed;
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

        private void PlayTimerElapsed(object sender, EventArgs e)
        {
            PlayTimer.Stop();
            AIMove();
        }

        private void AIMove()
        {
            if (GameLogic.IsGameOver) return;

            int Column = GameLogic.GetAIMove();
            if (Column != -1 && GameLogic.PlacePiece(Column))
            {
                UpdateBoard();
                CheckGameEnd();
            }
            isAITurn = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayTimer.Stop();
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
            if (isAITurn) return;
            
            PictureBox ClickedCell = (PictureBox)sender;
            TableLayoutPanelCellPosition CellPosition = tableLayoutPanel1.GetPositionFromControl(ClickedCell);
            int Column = CellPosition.Column;
            if (GameLogic.PlacePiece(Column))
            {
                UpdateBoard();
                if (GameMode == "VS Player")
                {
                    CheckGameEnd();
                }
                else if (GameMode == "VS AI")
                {
                    CheckGameEnd();
                    if (!GameLogic.IsGameOver && !GameLogic.CheckForDraw())
                    {
                        isAITurn = true;
                        PlayTimer.Start();
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
                    int CellValue = GameLogic.Board[r, c];
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
            PlayTimer.Stop();
            isAITurn = false;
            this.GameLogic = new Logic();
            UpdateBoard();
        }

        private void CheckGameEnd()
        {
            if (GameLogic.IsGameOver)
            {
                PlayTimer.Stop();
                isAITurn = false;
                GameLogic.CurrentPlayer = (GameLogic.CurrentPlayer == 1) ? 2 : 1;

                if (GameMode == "VS Player")
                {
                    MessageBox.Show($"Player {GameLogic.CurrentPlayer} Won!", "Game Over");
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
                else if (GameMode == "VS AI")
                {
                    if (GameLogic.CurrentPlayer == 1)
                    {
                        MessageBox.Show("You Won!", "Game Over");
                        Player1Score++;
                        label1.Text = $"Player: {Player1Score}";
                    }
                    else
                    {
                        MessageBox.Show("AI Won!", "Game Over");
                        Player2Score++;
                        label3.Text = $"AI: {Player2Score}";
                    }
                }
            }
            else if (GameLogic.CheckForDraw())
            {
                PlayTimer.Stop();
                isAITurn = false;
                MessageBox.Show("It's a Draw!", "Game Over");
            }
        }
    }
}