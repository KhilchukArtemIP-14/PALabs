using System;
using System.Collections.Generic;
using System.Linq;

namespace PALab3
{
    class Program
    {
        static void Main(string[] args)
        {
            int cityNum = -1,
                regularAnts=-1,
                wildAnts=-1;

            double alpha=-1,
                   beta= -1,
                   ro= -1;


            Console.WriteLine("Welcome to ant colony algorithm!\n");

            Console.WriteLine("First off, determine prameters:");

            Console.WriteLine("Would you like to choose default values? [Y/N]");
            var decision = Console.ReadLine().ToUpper();

            while (decision != "Y"&&decision != "N")
            {
                Console.WriteLine("Please, enter one of the two options");
                decision = Console.ReadLine().ToUpper();
            }

            switch (decision)
            {
                case "Y": 
                    {
                        cityNum = 200;
                        regularAnts = 30;
                        wildAnts = 15;
                        alpha = 2;
                        beta = 4;
                        ro=0.7;
                      break; }
                default: {
                        Console.WriteLine("\tPlease, enter the number of cities:");

                        var tempValue = Console.ReadLine();

                        while (!Int32.TryParse(tempValue, out cityNum) && cityNum < 0)
                        {
                            Console.WriteLine("Please, enter the correct value");
                            tempValue = Console.ReadLine();
                        }

                        Console.WriteLine("\tEnter number of regular ants:");

                        tempValue = Console.ReadLine();

                        while (!Int32.TryParse(tempValue, out regularAnts) && regularAnts < 1)
                        {
                            Console.WriteLine("Please, enter the correct value");
                            tempValue = Console.ReadLine();
                        }

                        Console.WriteLine("\tEnter number of wild ants:");

                        tempValue = Console.ReadLine();

                        while (!Int32.TryParse(tempValue, out wildAnts) && wildAnts < 1)
                        {
                            Console.WriteLine("Please, enter the correct value");
                            tempValue = Console.ReadLine();
                        }

                        Console.WriteLine("\tEnter alpha:");

                        tempValue = Console.ReadLine();

                        while (!Double.TryParse(tempValue, out alpha) && alpha < 0)
                        {
                            Console.WriteLine("Please, enter the correct value");
                            tempValue = Console.ReadLine();
                        }

                        Console.WriteLine("\tEnter Beta:");

                        tempValue = Console.ReadLine();

                        while (!Double.TryParse(tempValue, out beta) && beta < 0)
                        {
                            Console.WriteLine("Please, enter the correct value");
                            tempValue = Console.ReadLine();
                        }

                        Console.WriteLine("\tEnter ro:");

                        tempValue = Console.ReadLine();

                        while (!Double.TryParse(tempValue, out ro) && ro < 0&&ro>=1)
                        {
                            Console.WriteLine("Please, enter the correct value");
                            tempValue = Console.ReadLine();
                        }
                        break; }
            }



            Console.WriteLine("\nEverything's ready. Launching the algorithm...\n");
            Console.WriteLine("To exit, press ctrl+c");

            AntColony colony = new AntColony(cityNum, regularAnts, wildAnts, alpha, beta, ro);

            colony.InitCityTravelCost()
                .InitAnts()
                .InitPheromones();
            int i = -1;
            foreach (var a in colony.SolveTravellingMerchantProblem(2001))
            {
                i++;
                if (i %20 == 0)
                {
                    Console.WriteLine("Iteration: {0}",i+1);
                    Console.WriteLine("Minimal:{0}\nAverage:{1}", a.Item1,a.Item2);
                }
            }
        }
    }
}
