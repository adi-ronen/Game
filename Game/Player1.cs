using System;

namespace Game
{
    public class Player1 
    {
        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "303004030";  //id1
            player1_2 = "205589211";  //id2
        }

        //public Tuple<int, int> playYourTurn(Board board, TimeSpan timesup)
        //{
        //    Tuple<int, int> toReturn = null;
        //    //Random Algorithm - Start
        //    int randomRow;
        //    int randomCol;
        //    Random random = new Random();
        //    do
        //    {
        //        randomRow = random.Next(0, board._rows);
        //        randomCol = random.Next(0, board._cols);
        //    } while (board._board[randomRow, randomCol] != 'X'); //&& board.isTheGameEnded() == ' ');
        //    toReturn = new Tuple<int, int>(randomRow, randomCol);
        //    //Random Algorithm - End
        //    return toReturn;
        //}
        public Tuple<int, int> playYourTurn(Board board, TimeSpan timesup)
        {
            Tuple<int, int> toReturn = null;

            //rolls
            int FirstLineCol = board._cols-1;
            while (FirstLineCol > 0 && board._board[0, FirstLineCol] == ' ') FirstLineCol--;

            int FirstLineRow = board._rows - 1;
            while (FirstLineRow > 0 && board._board[FirstLineRow, 0] == ' ') FirstLineRow--;
 
            if(board._board[1,1]==' ')
            {   
                if (FirstLineCol - FirstLineRow > 0)
                {
                    return new Tuple<int, int>(0, FirstLineRow+1);
                }
                else
                {
                    return new Tuple<int, int>(FirstLineCol+1, 0);
                }
            }

            if (FirstLineRow == FirstLineCol)
                return new Tuple<int, int>(1, 1);

            int SecendLineRow = FirstLineRow;
            while (board._cols > 0 && SecendLineRow > 0 && board._board[SecendLineRow,1] == ' ') SecendLineRow--;

            if (FirstLineCol == 1)
            {
                int dist = FirstLineRow - SecendLineRow - 1;
                if (dist >= 0)
                {
                    return new Tuple<int, int>(FirstLineRow + 1 - dist, 0);
                }
                else
                {
                    return new Tuple<int, int>(FirstLineRow , 1);
                }
            }

            if(FirstLineRow - SecendLineRow - 1 == 0)
            {
                return new Tuple<int, int>(0, 2);
            }


            int SecendLineCol = FirstLineCol;
            while (board._rows>1&& SecendLineCol > 0 && board._board[1, SecendLineCol] == ' ') SecendLineCol--;

            if (FirstLineRow == 1)
            {
                int dist = FirstLineCol - SecendLineCol - 1;
                if (dist >= 0)
                {
                    return new Tuple<int, int>(0, FirstLineCol + 1 - dist);
                }
                else
                {
                    return new Tuple<int, int>(1, FirstLineCol);
                }
            }

            if (FirstLineCol - SecendLineCol - 1 == 0)
            {
                return new Tuple<int, int>(2, 0);
            }

            //gun mode
            if (FirstLineRow == 3 && FirstLineCol == 2 && board._board[2, 1] != ' ')
            {
                return new Tuple<int, int>(2, 1);
            }

            if (FirstLineRow == 2 && FirstLineCol == 3 && board._board[1, 2] != ' ')
            {
                return new Tuple<int, int>(1, 2);
            }


            if(FirstLineCol> FirstLineRow)
            {
                int longestRow = 0;
                while (FirstLineRow >= longestRow + 1 && board._board[longestRow + 1, FirstLineCol] != ' ') longestRow++;
                return new Tuple<int, int>(longestRow, FirstLineCol);
            }
            else
            {
                int longestCol = 0;
                while (FirstLineCol >= longestCol + 1 && board._board[FirstLineRow, longestCol + 1] != ' ') longestCol++;
                return new Tuple<int, int>(FirstLineRow, longestCol);
            }
        }

    }
}
