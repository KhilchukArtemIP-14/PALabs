﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PA_Lab5.NodeTreeStructures
{
    class SubGraph
    {
        private List<Node> _members;
        private double lifetTime;
        private SubGraph _parent = null;
        private List<SubGraph> descendants = new List<SubGraph>();

        public int Common;
        public SubGraph()
        {
            _members = new List<Node>();
            Common = AnalyzeAdjacentCommonNodes().Count;
            AddDescendants(AnalyzeAdjacentCommonNodes().ToList());
        }

        public SubGraph(List<Node> members)
        {
            _members = members;
            Common = AnalyzeAdjacentCommonNodes().Count;
            AddDescendants(AnalyzeAdjacentCommonNodes().ToList());
        }
        public SubGraph(List<Node> members, SubGraph parent)
        {
            _parent = parent;
            _members = members;
            Common = AnalyzeAdjacentCommonNodes().Count;
            AddDescendants(AnalyzeAdjacentCommonNodes().ToList());
        }

        public bool IsClique()
        {
            foreach (var node in _members)
            {
                foreach (var cliqueMember in _members.Where(a => a != node))
                {
                    if (!cliqueMember.GetAdjacents().Contains(node))
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        public List<Node> GetMembers()
        {
            return _members;
        }

        public int GetTotalAdjacentsCount()
        {
            List<Node> adjacent = new List<Node>();

            foreach (var node in _members)
            {
                adjacent.AddRange(node.GetAdjacents());
            }
            return adjacent.Distinct().ToList().Count;
        }

        public void SetLifeTime()
        {
            lifetTime = 0;
        }
        public double GetLifeTeme()
        {
            return lifetTime;
        }
        public void IncrementLifetime(double value)
        {
            lifetTime += (1D / (value * value));
        }
        public void SetLifeTIme(double life)
        {
            lifetTime = life;
        }

        public List<SubGraph> GetDescendants()
        {
            return descendants;
        }

        public void AddDescendants(List<Node> ddescendants)
        {
            foreach (var node in ddescendants)
            {
                List<Node> tempList = new List<Node>();
                foreach (var member in _members)
                {
                    tempList.Add(member);
                }

                //tut
                tempList.Add(node);
                var tempGraph = new SubGraph(tempList, this);
                descendants.Add(tempGraph);
            }
        }

        public HashSet<Node> AnalyzeAdjacentCommonNodes()
        {
            HashSet<Node> adjacents = new HashSet<Node>();
            foreach (var a in GetMembers()[0].GetAdjacents())
            {
                adjacents.Add(a);
            }

            foreach (var b in GetMembers())
            {
                adjacents.IntersectWith(b.GetAdjacents());
            }
            return adjacents;
        }

        public void PopDescendant(SubGraph descendant)
        {
            descendants.Remove(descendant);
        }
    }
}
