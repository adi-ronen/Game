using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public class Player3
    {
        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "203194048";  //id1
            player1_2 = "313193393";  //id2
        }
        public Tuple<int, int> playYourTurn(Board board, TimeSpan timesup)
        {
            Tuple<int, int> toReturn = null;
            int row; int col;
            int boardLength = board._board.GetLength(1);
            int boardHeight = board._board.GetLength(0);
            //Random Algorithm - Start
            /*int randomRow;
            int randomCol;
            Random random = new Random();
            do
            {
                randomRow = random.Next(0, board._rows);
                randomCol = random.Next(0, board._cols);
            } while (board._board[randomRow, randomCol] != 'X'); //&& board.isTheGameEnded() == ' ');
            toReturn = new Tuple<int, int>(randomRow, randomCol);*/
            //Random Algorithm - End


            int colSize = 0; int rowSize = 0;
            if (atStart_cols_Bigger_rows(board, ref colSize, ref rowSize))
            {
                row = rowSize - 1; col = colSize - 1;
                toReturn = new Tuple<int, int>(row, col);
                return toReturn;
            }
            if (board._board[2, 0] != 'X')
            {
                toReturn = randSelection(board); //selects the bottom right cube
                return toReturn;
            }
            row = 2; col = 0;
            toReturn = new Tuple<int, int>(row, col);

            // toReturn = randSelection(board);

            return toReturn;
        }
        private bool atStart_cols_Bigger_rows(Board board, ref int colSize, ref int rowSize)
        {
            char[,] _board = board._board;
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);
            if (cols > rows && _board[rows - 1, cols - 1] == 'X')
            {
                colSize = cols;
                rowSize = rows;
                return true;
            }

            return false;
        }

        private Tuple<int, int> randSelection(Board board)
        {
            Tuple<int, int> ans;
            char[,] _board = board._board;
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);
            int i = rows - 1; int j = cols - 1;
            while (true)
            {
                if (_board[i, j] == 'X')
                {
                    ans = new Tuple<int, int>(i, j);
                    break;
                }
                i--; j--;
            }
            return ans;
        }

    }
}
