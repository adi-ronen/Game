using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Game
{
    public class Player1
    {
            Tuple<int, int> result = null;
            Tuple<int, int> resultTemp = null;
            Mutex resultTupleMute = new Mutex();
        Semaphore queueSem;
            Queue<Tuple<int, int>> nextTuples = new Queue<Tuple<int, int>>();
            List<Thread> threads = new List<Thread>();
            bool fin = false;
            int FirstLineCol=1, FirstLineRow=1;
        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "303004030";  //id1
            player1_2 = "205589211";  //id2
        }
        public Tuple<int, int> playYourTurn(Board board, TimeSpan timesup)
        {
            queueSem = new Semaphore(0, board._squaresLeft);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            #region rules
            FirstLineCol = board._cols - 1;
            while (FirstLineCol > 0 && board._board[0, FirstLineCol] == ' ') FirstLineCol--;

            FirstLineRow = board._rows - 1;
            while (FirstLineRow > 0 && board._board[FirstLineRow, 0] == ' ') FirstLineRow--;

            if (board._board[1, 1] == ' ')
            {
                if (FirstLineCol - FirstLineRow > 0)
                {
                    return new Tuple<int, int>(0, FirstLineRow+1);
                }
                else if (FirstLineCol - FirstLineRow < 0)
                {
                    return new Tuple<int, int>(FirstLineCol+1, 0);
                }
            }
            else if (FirstLineRow == FirstLineCol)
            {
                //board.printTheBoard();
                return new Tuple<int, int>(1, 1);
            }

            int SecendLineRow = FirstLineRow;
            while (board._cols > 1 && SecendLineRow > 0 && board._board[SecendLineRow, 1] == ' ') SecendLineRow--;

            if (FirstLineCol == 1 && resultTemp==null)
            {
                int dist = FirstLineRow - SecendLineRow - 1;
                if (dist > 0)
                {
                    return new Tuple<int, int>(FirstLineRow + 1 - dist, 0);
                }
                else if (dist < 0)
                {
                    return new Tuple<int, int>(FirstLineRow, 1);
                }
            }
            else if (FirstLineRow - SecendLineRow - 1 == 0 && resultTemp == null)
            {
                return new Tuple<int, int>(0,2);
            }


            int SecendLineCol = FirstLineCol;
            while (board._rows > 1 && SecendLineCol > 0 && board._board[1, SecendLineCol] == ' ') SecendLineCol--;

            if (FirstLineRow == 1 && resultTemp == null)
            {
                int dist = FirstLineCol - SecendLineCol - 1;
                if (dist > 0)
                {
                    return new Tuple<int, int>(0, FirstLineCol + 1 - dist);
                }
                else if (dist < 0)
                {
                    return new Tuple<int, int>(1, FirstLineCol);
                }
            }
            else if (FirstLineCol - SecendLineCol - 1 == 0 && resultTemp == null)
            {
                return new Tuple<int, int>(2,0);
            }

            //gun mode
            if (FirstLineRow == 3 && FirstLineCol == 2 && board._board[2, 1] != ' ' && resultTemp == null)
            {
                return new Tuple<int, int>(2, 1);
            }

            if (FirstLineRow == 2 && FirstLineCol == 3 && board._board[1, 2] != ' ' && resultTemp == null)
            {
                return new Tuple<int, int>(1, 2);
            }

            #endregion

            Thread turn = new Thread(() =>
              {
                  startSearching(board);
              });
            turn.Start();
            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds);
            Thread.Sleep(timesup.Milliseconds - (int)sw.ElapsedMilliseconds -10);
            //threadMaker.Abort();
            //foreach (Thread th in threads)
            //{
            //    th.Abort();
            //}
            //sw.Restart();
            fin = true;
            if (resultTemp == null)
            {

                if (FirstLineCol > FirstLineRow)
                {
                    int longestRow = 0;
                    while (FirstLineRow >= longestRow + 1 && board._board[longestRow + 1, FirstLineCol] != ' ') longestRow++;
                    result = new Tuple<int, int>(longestRow, FirstLineCol);
                }
                else
                {
                    int longestCol = 0;
                    while (FirstLineCol >= longestCol + 1 && board._board[FirstLineRow, longestCol + 1] != ' ') longestCol++;
                    result = new Tuple<int, int>(FirstLineRow, longestCol);
                }
            }
            else
                result = resultTemp;
            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds);
            //Console.WriteLine((int)sw.ElapsedMilliseconds);
            return result;

        }

        private void startSearching(Board board)
        {
            



            Object _lock = new object();


            Thread t = new Thread(() =>
            {
                for (int row = FirstLineRow; row >= 0&&!fin; row--)
                {
                    for (int col = FirstLineCol; col >= 0&&!fin; col--)
                    {
                        if (board.isLegalMove(row, col))
                        {
                            lock (_lock)
                            {
                                nextTuples.Enqueue(new Tuple<int, int>(row, col));
                                queueSem.Release(1);
                            }
                        }
                    }
                }

            });
            threads.Add(t);
            t.Start();


            Thread threadMaker = new Thread(() =>
            {

                for (int i = 0; i < 10&&resultTemp==null; i++)
                {
                    Thread finder = new Thread(() =>
                    {
                        while (!fin && resultTemp == null)
                        {
                            Board temp = new Board(board);
                            queueSem.WaitOne();
                            Tuple<int, int> q;
                            lock (_lock)
                            {
                                q = nextTuples.Dequeue();
                            }

                            temp.fillPlayerMove(q.Item1, q.Item2);
                            if (rules(temp))
                            {
                                resultTupleMute.WaitOne();
                                if (resultTemp == null)
                                    resultTemp = q;
                                resultTupleMute.ReleaseMutex();
                            }
                        }
                    });
                    threads.Add(finder);
                    finder.Start();
                }
            });
            //threads.Add(threadMaker);
            threadMaker.Start();

        }

        private bool rules(Board board)
        {
            #region rules
            int FirstLineCol = board._cols - 1;
            while (FirstLineCol > 0 && board._board[0, FirstLineCol] == ' ') FirstLineCol--;

            int FirstLineRow = board._rows - 1;
            while (FirstLineRow > 0 && board._board[FirstLineRow, 0] == ' ') FirstLineRow--;

            if (board._board[1, 1] == ' ')
                return false;
            

            if (FirstLineRow == FirstLineCol)
                return false;

            int SecendLineRow = FirstLineRow;
            while (board._cols > 0 && SecendLineRow > 0 && board._board[SecendLineRow, 1] == ' ') SecendLineRow--;

            if (FirstLineCol == 1 && FirstLineRow - SecendLineRow - 1 != 0)
            {
                return false;

            }
            else if (FirstLineRow - SecendLineRow - 1 == 0)
            {
                return false;
            }


            int SecendLineCol = FirstLineCol;
            while (board._rows > 1 && SecendLineCol > 0 && board._board[1, SecendLineCol] == ' ') SecendLineCol--;

            if (FirstLineRow == 1&&FirstLineCol - SecendLineCol - 1!=0)
            {
                return false;
            }
            else if (FirstLineCol - SecendLineCol - 1 == 0)
            {
                return false;
            }

            //gun mode
            if (FirstLineRow == 3 && FirstLineCol == 2 && board._board[2, 1] != ' ')
            {
                return false;
            }

            if (FirstLineRow == 2 && FirstLineCol == 3 && board._board[1, 2] != ' ')
            {
                return false;
            }

            return true;
            #endregion

        }

    }
}
