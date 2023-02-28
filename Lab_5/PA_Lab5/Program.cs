using System;
using System.Collections.Generic;
using PA_Lab5.NodeTreeStructures;
using PA_Lab5.BeeColonies;
using PA_Lab5.Bees;
using System.Diagnostics;


namespace PA_Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph(GenerateNodes(200));
            BeeColony beeColony = new BeeColony(graph,4);
            beeColony.SetBees(GenerateEmployedBees(10,beeColony), GenerateOnlookerBees(100,beeColony));
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var clique = beeColony.FindClique();//нашо тут ше раз розмір
            sw.Stop();
            var ts = sw.Elapsed;
            Console.WriteLine("Success; Time to find: {0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Adjacents:");
            var temp2 = clique.GetMembers();
            foreach(var member in temp2)
            {
                Console.WriteLine("Member identificator:{0}", member.Identificator);
                Console.Write("Adjacents");
                foreach(var a in member.GetAdjacents())
                {
                    Console.Write("{0} ",a.Identificator);
                }
                Console.WriteLine();

            }
        }

        public static List<Node> GenerateNodes(int size)
        {
            var tempList = new List<Node>();
            for (int i = 0; i < size; i++)
            {
                tempList.Add(new Node(i));
            }
            return tempList;
        }

        public static List<EmployedBee> GenerateEmployedBees(int count,BeeColony colony)
        {
            var bees = new List<EmployedBee>();

            for (int i = 0; i < count; i++)
            {
                bees.Add(new EmployedBee(colony));
            }

            return bees;
        }

        public static List<OnlookerBee> GenerateOnlookerBees(int count, BeeColony colony)
        {
            var bees = new List<OnlookerBee>();

            for (int i = 0; i < count; i++)
            {
                bees.Add(new OnlookerBee(colony));
            }
            return bees;
        }
    }
}
