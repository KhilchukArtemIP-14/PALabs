using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BetterOne.NodeStructures;
using BetterOne.Bees;

namespace BetterOne.BeeColonies
{
    class BeeColony
    {
        private List<EmployedBee> _employedBees;
        private List<OnlookerBee> _onlookerBees;
        private List<Tuple<SubGraph, int>> _foodSourcesAndSizes;
        private int _cliqueSize;
        private Graph _graph;

        public BeeColony(Graph graph, int cliqueSize)
        {
            _foodSourcesAndSizes = new List<Tuple<SubGraph, int>>();
            _graph = graph;
            _cliqueSize = cliqueSize;
        }

        public void SendBees()
        {
            var discoveredAreas = new List<SubGraph>();
            foreach (var bee in _employedBees)
            {
                bee.PickArea(_cliqueSize);
                discoveredAreas.Add(MakeAClique(bee.GetLocation()));
            }
            ShareDiscoveredAreas(discoveredAreas);
            _foodSourcesAndSizes = _foodSourcesAndSizes.Where(a => a.Item1.GetDescendants().Count != 0).ToList();
            discoveredAreas = new List<SubGraph>();
            foreach (var bee in _onlookerBees)
            {
                bee.PickArea(_cliqueSize);
                bee.DiscoverAdjacentArea(_cliqueSize);
                discoveredAreas.Add(MakeAClique(bee.GetLocation()));
            }
            ShareDiscoveredAreas(discoveredAreas);
        }

        public void ShareDiscoveredAreas(List<SubGraph> discoveredAreas)
        {
            foreach (var area in discoveredAreas)
            {
                _foodSourcesAndSizes.Add(Tuple.Create(area, AnalyzeNectar(area)));
            }
        }

        public void SetBees(List<EmployedBee> employedBees, List<OnlookerBee> onlookerBees)
        {
            _employedBees = employedBees;
            _onlookerBees = onlookerBees;
        }

        public void UpdateFoodSourcesData()
        {            
            _foodSourcesAndSizes = _foodSourcesAndSizes.Where(a=> a.Item1.GetDescendants().Count!=0).OrderBy(a => 1/(double)a.Item2).Take(_foodSourcesAndSizes.Count/2).ToList();
        }

        public static int AnalyzeNectar(SubGraph subGraph)
        {
            return subGraph.GetMembers().Count * subGraph.GetMembers().Count*(1+subGraph.GetDescendants().Count);
        }

        public Graph GetSearchArea()
        {
            return _graph;
        }

        public List<Tuple<SubGraph, int>> GetDiscoveredFoodSources()
        {
            return _foodSourcesAndSizes;
        }

        public SubGraph FindClique()
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                SendBees();
                foreach (var source in _foodSourcesAndSizes)
                {
                    source.Item1.IncrementLifetime(source.Item2);
                    if (source.Item1.GetMembers().Count>=_cliqueSize) return source.Item1;
                }
                UpdateFoodSourcesData();
                if (i % 100 == 0) Console.WriteLine("Biggest clique found so far:{3}", _foodSourcesAndSizes.Max(a => a.Item2), _foodSourcesAndSizes.Count,_foodSourcesAndSizes.Average(a=>a.Item1.GetMembers().Count),_foodSourcesAndSizes.Max(a=>a.Item1.GetMembers().Count));
            }
            return null;
        }

        public SubGraph MakeAClique(SubGraph original)
        {
            List<Node> traversed= new List<Node>();
            List<Node> toRemove = new List<Node>();

            foreach (var member in original.GetMembers())
            {
                foreach(var traversedOne in traversed)
                {
                    if (!traversedOne.GetAdjacents().Contains(member)) { toRemove.Add(member); break; }
                }
                if (!toRemove.Contains(member)&&member.GetAdjacents().Count>=_cliqueSize) traversed.Add(member);
            }
            return new SubGraph(traversed);
        }
    }
}
