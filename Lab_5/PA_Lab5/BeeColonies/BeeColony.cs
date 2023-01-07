using System;
using System.Collections.Generic;
using System.Text;
using PA_Lab5.NodeTreeStructures;
using PA_Lab5.Bees;
using System.Linq;

namespace PA_Lab5.BeeColonies
{
    class BeeColony
    {
        private List<EmployedBee> _employedBees;
        private List<OnlookerBee> _onlookerBees;
        private List<Tuple<SubGraph,int>> _foodSourcesAndNectarValues;
        private int _cliqueSize;
        private Graph _graph;

        public BeeColony(Graph graph, int cliqueSize)
        {
            _foodSourcesAndNectarValues = new List<Tuple<SubGraph, int>>();
            _graph = graph;
            _cliqueSize = cliqueSize;
        }

        public void SendBees()
        {
            var discoveredAreas = new List<SubGraph>();
            foreach(var bee in _employedBees)
            {
                bee.PickArea(_cliqueSize);
                discoveredAreas.Add(bee.GetLocation());
            }
            ShareDiscoveredAreas(discoveredAreas);
            discoveredAreas = new List<SubGraph>();
            foreach(var bee in _onlookerBees)
            {
                bee.PickArea(_cliqueSize);
                bee.DiscoverAdjacentArea(_cliqueSize);
                discoveredAreas.Add(bee.GetLocation());
            }
            ShareDiscoveredAreas(discoveredAreas);
        }

        public void ShareDiscoveredAreas(List<SubGraph> discoveredAreas)
        {
            foreach(var area in discoveredAreas)
            {
                _foodSourcesAndNectarValues.Add(Tuple.Create(area, AnalyzeNectar(area)));
            }
        }

        public void SetBees(List<EmployedBee> employedBees,List<OnlookerBee> onlookerBees)
        {
            _employedBees = employedBees;
            _onlookerBees = onlookerBees;
        }

        public void UpdateFoodSourcesData()
        {
            /*var avarage = _foodSourcesAndNectarValues.Average(a => a.Item2+0.1);


            var updatedFoodSources = new List<Tuple<SubGraph, int>>();
            foreach(var source in _foodSourcesAndNectarValues)
            {
                if (source.Item2 > avarage)
                {
                    updatedFoodSources.Add(source);
                }
            }

            _foodSourcesAndNectarValues = updatedFoodSources;*/

            _foodSourcesAndNectarValues = _foodSourcesAndNectarValues.OrderBy(a => 1/(double)a.Item2).Take(60).ToList();
        }

        public static int AnalyzeNectar(SubGraph subGraph) 
        {
            int nectarValue = 0;
            foreach(var initialNode in subGraph.GetMembers())
            {
                foreach(var subNode in subGraph.GetMembers().Where(a=> a != initialNode))
                {
                    if (subNode.GetAdjacents().Contains(initialNode)) nectarValue += 1;
                }
            }
            return nectarValue;
        }

        public Graph GetSearchArea()
        {
            return _graph;
        }
        public List<Tuple<SubGraph,int>> GetDiscoveredFoodSources()
        {
            return _foodSourcesAndNectarValues;
        }

        public SubGraph FindClique()
        {
            SendBees();
            for (int i = 0; i < int.MaxValue; i++)
            {
                UpdateFoodSourcesData();
                SendBees();
                foreach(var source in _foodSourcesAndNectarValues)
                {
                    if (source.Item1.IsClique()) return source.Item1;
                }
                if (i % 100 == 0) Console.WriteLine("{0}, {1}",_foodSourcesAndNectarValues.Max(a=>a.Item2), _foodSourcesAndNectarValues.Count);
                //Console.WriteLine(_foodSourcesAndNectarValues.Count);
            }

            return null;
        }
    }
}
