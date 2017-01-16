using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public class Player1 
    {
        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "123456789";  //id1
            player1_2 = "123456789";  //id2
        }
        public Tuple<int, int> playYourTurn
        (
            Board board,
            TimeSpan timesup
        )
        {
            Tuple<int, int> toReturn = null;

            //Random Algorithm - Start
            int randomRow;
            int randomCol;
            Random random = new Random();
            do
            {
                randomRow = random.Next(0, board._rows);
                randomCol = random.Next(0, board._cols);
            } while (board._board[randomRow, randomCol] != 'X'); //&& board.isTheGameEnded() == ' ');
            toReturn = new Tuple<int, int>(randomRow, randomCol);
            //Random Algorithm - End

            return toReturn;
        }
    }
}
