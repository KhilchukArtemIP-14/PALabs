using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PA_Lab4.Ants
{
    class WildAnt : IAnt
    {

        private Stack<City> _path;
        private int _lmin;
        private int _pathLength;
        private AntColony _colony;

        public WildAnt(City startCity, AntColony colony)
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
                    .OrderBy(a => a.Value).First();

                tempPath.Push(minAdjacent.Key);
                pathLen += minAdjacent.Value;

            }
            var temp = tempPath.ToArray();

            var startCity = temp[tempPath.Count - 1];

            pathLen += tempPath.Peek().GetAdjacents()[startCity];//перевірити

            return pathLen;
        }

        public int GetLMin()
        {
            return _lmin;
        }

        public int GetPathLength()
        {
            return _pathLength;
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

        public Tuple<City, City> ReturnToStart()
        {
            var temp = _path.ToArray();

            var startCity = temp[_path.Count - 1];

            var currentCity = _path.Peek();

            _pathLength += currentCity.GetAdjacents()[startCity];//перевірити

            _path.Push(startCity);

            return Tuple.Create<City, City>(currentCity, startCity);

        }

        public IEnumerable<Tuple<City, City>> SeekPath(int length)
        {
            Random rand = new Random();
            _lmin = CalculateGreedyPath(length);
            while (_path.Count != length)
            {
                var temp = _path.Peek();

                var adjacents = temp.GetAdjacents()
                    .Where(a => !_path.Contains(a.Key))
                    .ToDictionary(a => a.Key, a => a.Value);

                var decision = adjacents.OrderBy(_ => rand.Next()).First();

                _pathLength += adjacents[decision.Key];
                _path.Push(decision.Key);

                yield return Tuple.Create<City, City>(temp, decision.Key);
            }
            yield return ReturnToStart();
        }
    }
}
