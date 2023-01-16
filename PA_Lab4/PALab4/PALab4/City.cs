using System;
using System.Collections.Generic;
using System.Text;

namespace PALab3
{
    class City
    {
        private int _identificator;
        private Dictionary<City, int> _adjacentCitiesAndTravelCosts;
        public City(int identificator)
        {
            _identificator = identificator;
            _adjacentCitiesAndTravelCosts = new Dictionary<City, int>();
        }

        public void MapAdjacents(List<City> cities)
        {
            Random rand = new Random();

            foreach(var city in cities)
            {
                if (city == this) continue;
                _adjacentCitiesAndTravelCosts.Add(city, rand.Next(1, 41));
            }
        }

        public Dictionary<City, int> GetAdjacents()
        {
            return _adjacentCitiesAndTravelCosts;
        }

        private int GetIdentificator()
        {
            return _identificator;
        }
    }
}
