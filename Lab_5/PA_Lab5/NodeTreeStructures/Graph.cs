using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace PA_Lab5.NodeTreeStructures
{
    class Graph
    {
        List<Node> _nodes;

        public Graph(List<Node> nodes)
        {
            _nodes = nodes;
            ReInitNodes();
        }

        public void ReInitNodes()
        {
            Random rand = new Random();

            foreach (var node in _nodes)
            {
                var temp = _nodes.Where(a => a.Identificator != node.Identificator).OrderBy(_ => rand.Next()).ToList();

                var limit = rand.Next(25, 31);
                var tempAdjacents = new List<Node>();

                for (int i = 0; i < limit; i++)
                {
                    tempAdjacents.Add(temp[i]);
                }

                node.SetAdjacents(tempAdjacents);
            }
        }
        public List<Node> GetNodes()
        {
            return _nodes;
        }
    }
}
