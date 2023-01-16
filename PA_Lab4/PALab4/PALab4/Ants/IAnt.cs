using System;
using System.Collections.Generic;
using System.Text;

namespace PALab3.Ants
{
    interface IAnt
    {
        public int CalculateGreedyPath(int length);
        public IEnumerable<Tuple<City,City>> SeekPath(int length);
        public void Refresh();
        public int GetLMin();
        public int GetPathLength();
        public Tuple<City, City> ReturnToStart();
    }
}
