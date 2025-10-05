using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFourGame
{
    public class Logic
    {
        enum CellState { Empty = 0, Red = 1, Yellow = 2 }
        public int[,] Board = new int[6, 7];
        public int CurrentPlayer = 1;
        public bool IsGameOver = false;
        public int Count = 0;
        public Random random = new Random();

        public Logic()
        {
            for (int r = 0; r < 6; r++)
            {
                for (int c = 0; c < 7; c++)
                {
                    Board[r, c] = (int)CellState.Empty;
                }
            }
        }

        public bool PlacePiece(int column)
        {
            if (IsGameOver || column < 0 || column > 6)
            {
                return false;
            }
            else
            {
                for (int row = 5; row >= 0; row--)
                {
                    if (Board[row, column] == (int)CellState.Empty)
                    {
                        Board[row, column] = CurrentPlayer;
                        if (CheckWin(row, column))
                        {
                            IsGameOver = true;
                        }
                        Count++;
                        CurrentPlayer = (CurrentPlayer == 1) ? 2 : 1;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckForDraw()
        {
            return Count == 42;
        }

        public bool CheckWin(int row, int column)
        {

            int player = Board[row, column];

            // Horizontal Check
            int count = 0;
            for (int c = 0; c < 7; c++)
            {
                if (Board[row, c] == player)
                {
                    count++;
                    if (count >= 4) return true;
                }
                else
                {
                    count = 0;
                }
            }

            // Vertical Check
            count = 0;
            for (int r = 0; r < 6; r++)
            {
                if (Board[r, column] == player)
                {
                    count++;
                    if (count >= 4) return true;
                }
                else
                {
                    count = 0;
                }
            }

            // Main Diagonals Check 
            count = 0;
            for (int d = -3; d <= 3; d++)
            {
                int r = row + d;
                int c = column + d;
                if (r >= 0 && r < 6 && c >= 0 && c < 7)
                {
                    if (Board[r, c] == player)
                    {
                        count++;
                        if (count >= 4) return true;
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }

            // Reverse Diagonals Check
            count = 0;
            for (int d = -3; d <= 3; d++)
            {
                int r = row - d;
                int c = column + d;
                if (r >= 0 && r < 6 && c >= 0 && c < 7)
                {
                    if (Board[r, c] == player)
                    {
                        count++;
                        if (count >= 4) return true;
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }
            return false;
        }
    
        private bool CanPlacePiece(int column)
        {
            if (column < 0 || column > 6) return false;
            return Board[0, column] == (int)CellState.Empty;
        }

        private int GetLowestEmptyRow(int column)
        {
            for (int row = 5; row >= 0; row--)
            {
                if (Board[row, column] == (int)CellState.Empty)
                {
                    return row;
                }
            }
            return -1;
        }
        public int GetAIMove()
        {
            // Check For AI Win
            for (int col = 0; col < 7; col++)
            {
                if (CanPlacePiece(col))
                {
                    int row = GetLowestEmptyRow(col);
                    Board[row, col] = 2;
                    if (CheckWin(row, col))
                    {
                        Board[row, col] = (int)CellState.Empty;
                        return col;
                    }
                    Board[row, col] = (int)CellState.Empty;
                }
            }

            // Check For Block Player Win
            for (int col = 0; col < 7; col++)
            {
                if (CanPlacePiece(col))
                {
                    int row = GetLowestEmptyRow(col);
                    Board[row, col] = 1;
                    if (CheckWin(row, col))
                    {
                        Board[row, col] = (int)CellState.Empty;
                        return col;
                    }
                    Board[row, col] = (int)CellState.Empty;
                }
            }

            // List of Valid Moves
            List<int> validColumns = new List<int>();
            for (int col = 0; col < 7; col++)
            {
                if (CanPlacePiece(col))
                {
                    validColumns.Add(col);
                }
            }

            if (validColumns.Count > 0)
            {
                return validColumns[random.Next(validColumns.Count)];
            }

            return -1;
        }
    } 
}
