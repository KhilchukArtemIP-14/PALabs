using System;
using PA_Lab1.DataSources;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace PA_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            FileGeneratorService fileGen = new FileGeneratorService(@"D:\KPI_2_1\PA\Lab_1/keker.txt");
            fileGen.GenerateFile(FileSize.OneMB);
            Stopwatch sw = new Stopwatch();
            var ts = new TimeSpan();
            var a = new FileDataSequence(@"D:\KPI_2_1\PA\Lab_1/keker.txt");
            var b = new FileDataSequence(@"D:\KPI_2_1\PA\Lab_1/bufA.txt");
            var c = new FileDataSequence(@"D:\KPI_2_1\PA\Lab_1/bufB.txt");

            #region Simple sorter
            DirectMergeSorterService ws = new DirectMergeSorterService(a, b, c);
            sw.Start();
            ws.DirectMergeSort();
            sw.Stop();
            ts = sw.Elapsed;
            Console.WriteLine("Time to sort in simple way: {0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);

            a.Close();
            b.Close();
            c.Close();
            #endregion

            #region Advanced sort
            AdvancedMergeSorter am = new AdvancedMergeSorter(a, b);
            fileGen.GenerateFile(FileSize.TenMB);

            a.ResetPointer();
            b.ResetPointer();

            sw.Reset();
            sw.Start();
            am.MergeSort();
            sw.Stop();
            ts = sw.Elapsed;
            Console.WriteLine("Time to sort in advanced way: {0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.Read();
            a.Clear();
            b.Clear();
            #endregion
        }
    }
}
