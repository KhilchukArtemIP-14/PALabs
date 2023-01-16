using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace PALab3.Ants
{
    class RegularAnt : IAnt
    {
        private Stack<City> _path;
        private int _lmin;
        private int _pathLength;
        private AntColony _colony;

        public RegularAnt(City startCity, AntColony colony)
        {
            _path = new Stack<City>();
            _path.Push(startCity);
            _colony = colony;
        }

        public int CalculateGreedyPath(int length)
        {
            var pathLen = 0;
            var tempPath = new Stack<City>();
            tempPath.Push(_path.Peek());

            while (tempPath.Count != length)
            {   
                var minAdjacent = tempPath.Peek().GetAdjacents()
                    .Where(a => !tempPath.Contains(a.Key))
                    .OrderBy(a=>a.Value).First();

                tempPath.Push(minAdjacent.Key);
                pathLen += minAdjacent.Value;
            }
            var temp = tempPath.ToArray();

            var startCity = temp[tempPath.Count - 1];

            pathLen += tempPath.Peek().GetAdjacents()[startCity];

            return pathLen;
        }


        public IEnumerable<Tuple<City, City>> SeekPath(int length)
        {
            Random rand = new Random();
            _lmin = CalculateGreedyPath(length);
            
            while (_path.Count != length)
            {
                var chance = rand.NextDouble();
                var chances = CalculateChances().ToList();
                var adjacents = _path.Peek().GetAdjacents();
                double cumulative = 0;
                foreach(var probability in chances)
                {
                    if (chance < cumulative + probability.Item2)
                    {
                        _pathLength += adjacents[probability.Item1];

                        var temp = _path.Peek();

                        _path.Push(probability.Item1);

                        yield return Tuple.Create<City, City>(temp, probability.Item1);
                        break;
                    }
                    cumulative += probability.Item2;
                }
            }
            yield return ReturnToStart();
        }

        public List<Tuple<City, double>> CalculateChances()
        {
            var pheromones = _colony.GetPheromones(_path.Peek())
                .Where(a => !_path.Contains(a.Key))
                .OrderBy(a=>a.Value);

            var adjacentsDistances = _path.Peek().GetAdjacents()
                .Where(a => !_path.Contains(a.Key))
                .ToDictionary(a=>a.Key,a=>a.Value);

            double totalPheromonesXVisibility = 0;
            foreach(var pheromone in pheromones)
            {
                totalPheromonesXVisibility += 
                    Math.Pow(pheromone.Value, _colony.Alpha) * 
                    Math.Pow(1D /(double)adjacentsDistances[pheromone.Key], _colony.Beta);
            }

            List<Tuple<City, double>> citiesAndProbabilities = new List<Tuple<City, double>>();
            foreach (var pheromone in pheromones)
            {
                citiesAndProbabilities.Add( Tuple.Create(
                    pheromone.Key,
                    (Math.Pow(pheromone.Value, _colony.Alpha) *
                    Math.Pow(1D / (double)adjacentsDistances[pheromone.Key], _colony.Beta))/ totalPheromonesXVisibility));
            }

            return citiesAndProbabilities;
        }


        public int GetLMin()
        {
            return _lmin;
        }

        public int GetPathLength()
        {
            return _pathLength;
        }

        public Tuple<City, City> ReturnToStart()
        {            
            var startCity = _path.ToArray()[_path.Count - 1];

            _pathLength += _path.Peek().GetAdjacents()[startCity];

            var currentCity = _path.Peek();

            _path.Push(startCity);

            return Tuple.Create<City, City>(currentCity, startCity);
        }

        public void Refresh()
        {
            while (_path.Count != 1)
            {
                _path.Pop();
            }

            _pathLength = 0;
            _lmin = 0;
        }
    }
}
