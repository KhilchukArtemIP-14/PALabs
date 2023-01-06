﻿using System;
using System.Collections.Generic;
using BetterOne.Bees;
using BetterOne.BeeColonies;
using BetterOne.NodeStructures;
using System.Diagnostics;
using System.Linq;

namespace BetterOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph(GenerateNodes(200));

            Console.WriteLine("Welcome to bee algorithm for clique-searching");

            Console.WriteLine("Please, enter the bee number (for instance, 400)\n");

            int beeCount = -1;
            var tempValue = Console.ReadLine();

            while (Int32.TryParse(tempValue, out beeCount) && beeCount < 10)
            {
                Console.WriteLine("Please, enter the correct value");
                tempValue = Console.ReadLine();
            }

            
            Console.WriteLine("Value accepted. Now enter clique size:\n(Warning, occurence of clique sized more than 5 is unlikely)\n");

            int cliqueSize = -1;
            tempValue = Console.ReadLine();

            while (Int32.TryParse(tempValue, out cliqueSize) && cliqueSize <1)
            {
                Console.WriteLine("Please, enter the correct value");
                tempValue = Console.ReadLine();
            }

            BeeColony beeColony = new BeeColony(graph, cliqueSize);

            var employedBees = GenerateEmployedBees((int)(beeCount*0.1), beeColony);
            var onlookerBees = GenerateOnlookerBees(beeCount - employedBees.Count, beeColony);


            beeColony.SetBees(employedBees,onlookerBees);

            Console.WriteLine("Great! Everything is ready\nExecuting order 66....");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var clique = beeColony.FindClique();
            sw.Stop();
            var ts = sw.Elapsed;
            Console.WriteLine("Success; Time to find: {0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Adjacents:");
            var temp2 = clique.GetMembers();
            foreach (var member in temp2)
            {
                Console.WriteLine("Member identificator:{0}", member.Identificator);
                Console.Write("Adjacents");
                foreach (var a in member.GetAdjacents())
                {
                    Console.Write("{0} ", a.Identificator);
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

        public static List<EmployedBee> GenerateEmployedBees(int count, BeeColony colony)
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
