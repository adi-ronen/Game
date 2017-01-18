using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public class Player2
    {
        long m_NumToCheck;
        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "321790883";  //id1
            player1_2 = "204564256";  //id2
        }
        private Tuple<int, int> playRandomTurn(Board board, TimeSpan timesup)
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
        public Tuple<int, int> playYourTurn(Board board, TimeSpan timesup)
        {
            Tuple<int, int> toReturn = null;
            long timeleft = timesup.Milliseconds;
            Stopwatch stopWatch = new Stopwatch();
            possibleMoves = new List<Tuple<int, int>>();

            stopWatch.Start();
            Thread t = new Thread(() => startMinMax(board));
            t.Start();
            while (stopWatch.ElapsedMilliseconds < timeleft - 20 && bestMove == null)
            {
            }
            t.Abort();
            //Console.WriteLine(stopWatch.ElapsedMilliseconds);
            if (bestMove == null)
            {
                //Console.WriteLine("random");
                toReturn = possibleMoves[possibleMoves.Count - 1];
            }
            else
                toReturn = bestMove;
            bestMove = null;
            return toReturn;

        }

        Tuple<int, int> bestMove;
        List<Tuple<int, int>> possibleMoves;
        private void startMinMax(Board board)
        {
            bool maxPlayer = true;
            possibleMoves = new List<Tuple<int, int>>();
            int cRow = 0, cColumn = 0;
            int r = board._rows, c = board._cols;
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    if (board.isLegalMove(i, j))
                    {
                        if (i == 0) cRow++;
                        if (j == 0) cColumn++;
                        possibleMoves.Add(new Tuple<int, int>(i, j));
                    }
                    else
                    {
                        //if we are at the first column in any row and it is not legal - everything below and right to it is not legal and we don't have to go through the rest of the board
                        if (j == 0)
                        {
                            r = i;
                        }
                        c = j;

                    }
                }
            }
            if (cRow == cColumn && 2 * cRow - 1 < possibleMoves.Count)
            {
                bestMove = new Tuple<int, int>(1, 1);
                Thread.EndThreadAffinity();
            }

            if (cColumn >= 30 && cRow >= 30)
            {
                if (cColumn / 2 == cRow) bestMove = new Tuple<int, int>(0, cColumn / 2 + 1);
                bestMove = new Tuple<int, int>(0, cColumn / 2);
                Thread.EndThreadAffinity();
            }
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                Board newBoard = new Board(board);
                newBoard.fillPlayerMove(possibleMoves[i].Item1, possibleMoves[i].Item2);
                double score = minMax(newBoard, Double.NegativeInfinity, Double.PositiveInfinity, !maxPlayer);
                if (score == 1)
                {
                    bestMove = new Tuple<int, int>(possibleMoves[i].Item1, possibleMoves[i].Item2);
                    Thread.EndThreadAffinity();
                }
            }
        }
        private double minMax(Board board, double alpha, double beta, bool maxPlayer)
        {
            List<Tuple<int, int>> possibleMoves = new List<Tuple<int, int>>();
            if (board.isTheGameEnded())
            {
                if (maxPlayer)
                    return 1;
                else
                    return -1;
            }
            else
            {
                int cRow = 0, cColumn = 0;
                int r = board._rows, c = board._cols;
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        if (board.isLegalMove(i, j))
                        {
                            if (i == 0) cRow++;
                            if (j == 0) cColumn++;
                            possibleMoves.Add(new Tuple<int, int>(i, j));
                        }
                        else
                        {
                            //if we are at the first column in any row and it is not legal - everything below and right to it is not legal and we don't have to go through the rest of the board
                            if (j == 0)
                            {
                                r = i;
                            }
                            c = j;
                        }
                    }
                }
                if (maxPlayer == true)
                {
                    if (cRow == cColumn && 2 * cRow - 1 > possibleMoves.Count)
                        return 1;
                    for (int i = 0; i < possibleMoves.Count && i < 10; i++)
                    {
                        Board newBoard = new Board(board);
                        newBoard.fillPlayerMove(possibleMoves[i].Item1, possibleMoves[i].Item2);
                        alpha = Math.Max(alpha, minMax(newBoard, alpha, beta, !maxPlayer));

                        if (beta < alpha)
                        {
                            break;
                        }
                    }

                    return alpha;
                }
                else
                {
                    if (cRow == cColumn && 2 * cRow - 1 > possibleMoves.Count)
                        return -1;
                    for (int i = 0; i < possibleMoves.Count && i < 10; i++)
                    {
                        Board newBoard = new Board(board);
                        newBoard.fillPlayerMove(possibleMoves[i].Item1, possibleMoves[i].Item2);
                        beta = Math.Min(beta, minMax(newBoard, alpha, beta, !maxPlayer));

                        if (beta < alpha)
                        {
                            break;
                        }
                    }
                    return beta;
                }
            }



        }


    }
}
