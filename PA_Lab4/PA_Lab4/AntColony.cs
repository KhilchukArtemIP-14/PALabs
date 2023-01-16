using System;
using System.Collections.Generic;
using System.Text;
using PA_Lab4.Ants;
using System.Linq;

namespace PA_Lab4
{
    class AntColony
    {
        private int _size;
        private List<City> _cities;
        private int _antCount = 0;
        private int _wildAntCount = 0;
        private List<IAnt> _ants;
        private Dictionary<Tuple<City, City>, Double> _edgesAndPheromoneLevels;

        public double Alpha
        {
            get;
            private set;
        }
        public double Beta
        {
            get;
            private set;
        }
        public double Ro
        {
            get;
            private set;
        }


        public AntColony(int size, int antCount, int wildAntCount, double alpha, double beta, double ro)
        {
            _size = size;

            _cities = new List<City>();

            for (int i = 0; i < _size; i++)
            {
                _cities.Add(new City(i));
            }

            _antCount = antCount;
            _wildAntCount = wildAntCount;

            Alpha = alpha;
            Beta = beta;
            Ro = ro;
        }

        public AntColony InitCityTravelCost()
        {
            foreach (var a in _cities)
            {
                a.MapAdjacents(_cities);
            }
            return this;
        }

        public AntColony InitAnts()
        {
            _ants = new List<IAnt>();

            Random rand = new Random();
            _cities.OrderBy(_ => rand.Next());

            int i;
            for (i = 0; i < _antCount; i++)
            {
                _ants.Add(new RegularAnt(_cities[i], this));
            }

            for (int j = 0; j < _wildAntCount; j++)
            {
                _ants.Add(new WildAnt(_cities[i + j], this));
            }
            return this;
        }
        public AntColony InitPheromones()
        {
            Random rand = new Random();

            _edgesAndPheromoneLevels = new Dictionary<Tuple<City, City>, double>();

            foreach (var city in _cities)
            {
                var temp = city.GetAdjacents();

                foreach (var adjacentCity in temp)
                {
                    var tempDouble = rand.NextDouble();
                    while (tempDouble == 0) { tempDouble = rand.NextDouble(); }// щоб переконатися, що буде ненульове, щоб потім не було проблем з виконанянм алгоритму
                    _edgesAndPheromoneLevels.Add(
                        Tuple.Create(city, adjacentCity.Key),
                        tempDouble);
                }
            }

            return this;
        }

        public Dictionary<City, double> GetPheromones(City fromWhere)
        {
            Dictionary<City, double> goal = _edgesAndPheromoneLevels
                .Where(a => a.Key.Item1 == fromWhere)
                .ToDictionary(a => a.Key.Item2, a => a.Value);

            return goal;
        }

        public void EvaporatePheromones(Dictionary<Tuple<City, City>, List<IAnt>> pathwaysAndAntsThatUsedThem)
        {
            var keys = pathwaysAndAntsThatUsedThem.Keys.Select(a => a);
            foreach (var pathway in keys)
            {
                double deltaT = 0;
                if (pathwaysAndAntsThatUsedThem.ContainsKey(pathway))
                {
                    foreach (var ant in pathwaysAndAntsThatUsedThem[pathway])
                    {
                        deltaT += (double)ant.GetLMin() / (double)ant.GetPathLength();
                    }
                }
                _edgesAndPheromoneLevels[pathway] = (1 - Ro) * _edgesAndPheromoneLevels[pathway] + deltaT;
            }
        }

        public IEnumerable<Tuple<int, double>> SolveTravellingMerchantProblem(int colonyLifeSpan)
        {
            for (int i = 0; i < colonyLifeSpan; i++)
            {

                Dictionary<Tuple<City, City>, List<IAnt>> pathwaysAndAntsThatUsedThem = new Dictionary<Tuple<City, City>, List<IAnt>>();
                foreach (var ant in _ants)
                {
                    foreach (var path in ant.SeekPath(_size))
                    {
                        if (pathwaysAndAntsThatUsedThem.ContainsKey(path))
                        {
                            pathwaysAndAntsThatUsedThem[path].Add(ant);
                        }
                        else
                        {
                            pathwaysAndAntsThatUsedThem.Add(path, new List<IAnt>());
                            pathwaysAndAntsThatUsedThem[path].Add(ant);
                        }
                    }
                }

                var minimum = _ants.Min(a => a.GetPathLength());
                double average = _ants.Average(a => a.GetPathLength());
                EvaporatePheromones(pathwaysAndAntsThatUsedThem);

                foreach (var ant in _ants)
                {
                    ant.Refresh();
                }

                yield return Tuple.Create(minimum, average);
            }
        }
    }
}
