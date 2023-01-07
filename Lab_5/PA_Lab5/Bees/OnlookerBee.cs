using PA_Lab5.NodeTreeStructures;
using System;
using System.Collections.Generic;
using System.Text;
using PA_Lab5.BeeColonies;
using System.Linq;
namespace PA_Lab5.Bees
{
    class OnlookerBee : IBee
    {
        private BeeColony _colony;
        private SubGraph _subGraph;
        
        public OnlookerBee(BeeColony colony)
        {
            _colony = colony;
        }

        public SubGraph GetLocation()
        {
            return _subGraph;
        }

        public void PickArea(int areaSize)
        {
            var sources =_colony.GetDiscoveredFoodSources().OrderBy(a=>a.Item2);

            int cumulativeNectar = 0;
            foreach(var source in sources)
            {
                cumulativeNectar += source.Item2;
            }

            var subGraphsAndChances = new Dictionary<SubGraph, double>();
            foreach(var source in sources)
            {
                subGraphsAndChances.Add(source.Item1, (double)source.Item2 / (double)cumulativeNectar);
            }

            var rand = new Random();

            var chance = rand.NextDouble();

            double cumulativeChance = 0; 
            foreach(var probability in subGraphsAndChances)
            {
                if (chance > cumulativeChance && chance < (cumulativeChance + probability.Value))
                {

                    var temp = probability.Key;
                    var templist = new List<Node>();
                    foreach(var tempNode in temp.GetMembers())
                    {
                        templist.Add(tempNode);
                    }
                    _subGraph = new SubGraph(templist);
                    return;

                }
                cumulativeChance += probability.Value;
            }
        }
        public void DiscoverAdjacentArea(int areaSize)
        {
            Random rand = new Random();

            var tempSubGraphMembers = _subGraph.GetMembers();
            List<Node> subGraphMembers = new List<Node>();
            foreach(var node in tempSubGraphMembers)
            {
                subGraphMembers.Add(node);
            }

            Node toReplace=null;
            var cumulative = 0D;
            foreach (var member in subGraphMembers)
            {
                cumulative += NumberOFConnections(subGraphMembers.Where(a => a!= member).ToList(),member);
            }

            Dictionary<Node, double> chances = new Dictionary<Node, double>();
            foreach (var member in subGraphMembers)
            {
                chances.Add(member, (double)NumberOFConnections(subGraphMembers.Where(a => a != member).ToList(), member) / cumulative);
            }

            var chance = rand.NextDouble();

            double cumulativeChance = 0;
            foreach (var probability in chances)
            {
                if (chance > cumulativeChance && chance < (cumulativeChance + probability.Value))
                {
                    toReplace = probability.Key;
                    break;
                }
                cumulativeChance += probability.Value;
            }

            var adjacents = toReplace.GetAdjacents();

            cumulative = 0;
            Node replacement=null;
            foreach(var a in adjacents.Where(a=>!tempSubGraphMembers.Contains(a)))
            {
                cumulative += CalculateReplacementValue(toReplace, a, areaSize);
            }

            chances = new Dictionary<Node, double>();
            foreach (var member in adjacents.Where(a => !tempSubGraphMembers.Contains(a)))
            {
                chances.Add(member, (double)CalculateReplacementValue(toReplace, member, areaSize) / cumulative);
            }

            chance = rand.NextDouble();

            cumulativeChance = 0;
            foreach (var probability in chances)
            {
                if (chance > cumulativeChance && chance < (cumulativeChance + probability.Value))
                {
                    replacement = probability.Key;
                    break;
                }
                cumulativeChance += probability.Value;
            }
            var index = subGraphMembers.IndexOf(toReplace);
            subGraphMembers[index] = replacement;
           
            //tuta
            int counter = 0;
            foreach (var a in subGraphMembers)
            {
                foreach (var b in subGraphMembers)
                {
                    if (a.Identificator == b.Identificator) counter++;
                }
            }
            if (counter > subGraphMembers.Count)
            {
                foreach(var a in tempSubGraphMembers)
                {
                    Console.WriteLine(a.Identificator);
                }

                throw new Exception();
            }


            _subGraph = new SubGraph(subGraphMembers);
        }

        public int NumberOFConnections(List<Node> cliqueMembers, Node node)
        {
            int num = 0;
            foreach(var a in cliqueMembers)
            {
                if (a.GetAdjacents().Contains(node))
                {
                    num++;
                }
            }
            return num;
        }

        public double CalculateReplacementValue(Node nodeToReplace, Node replacement, int size)
        {
            var subGraphMembers = _subGraph.GetMembers();

            var tempList = new List<Node>();

            foreach(var a in subGraphMembers)
            {
                tempList.Add(a);
            }
            int indexToReplace = tempList.IndexOf(nodeToReplace);

            tempList[indexToReplace] = replacement;
            //return (double)BeeColony.AnalyzeNectar(new SubGraph(tempList))*2/ (size * (size - 1))*replacement.GetAdjacents().Count;
            return (double)BeeColony.AnalyzeNectar(new SubGraph(tempList)) / (size * (size - 1)) * replacement.GetAdjacents().Count;

        }

    }
}
