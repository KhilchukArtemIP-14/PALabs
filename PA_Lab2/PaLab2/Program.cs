using System;
using System.Threading;
using System.Diagnostics;
using PaLab2.Boards;
using PaLab2.Searchers;
namespace PaLab2
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("Iteration: {0}", i);
                ValuedBoard vb = new ValuedBoard(8);
                RecursiveBestFirstSearcher rbfs = new RecursiveBestFirstSearcher(vb);
                rbfs.InitBoard();
                rbfs.Solve8Q();
                Console.WriteLine(vb.ToString());
            }
            // 10 1 20 20 3 20 19 10
            /*for (int i = 0; i < 20; i++)
            {
                Stopwatch sw = new Stopwatch();
                Console.WriteLine("Iteration: {0}", i);
                Board bd = new Board(8);
                IterativeDeepeningSearcher ids = new IterativeDeepeningSearcher(bd);
                ids.InitBoard();
                sw.Restart();
                ids.Solve8Q(DescendantNodeAlgorithm.Advanced);
                sw.Stop();
                var ts = sw.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine(elapsedTime);
                if (bd.Validate())
                {
                    Console.WriteLine("Yeah");
                }
                else Console.WriteLine("No");
            }*/
            

        }
    }
}
