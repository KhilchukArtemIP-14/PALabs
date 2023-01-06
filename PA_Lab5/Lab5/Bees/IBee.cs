using System;
using System.Collections.Generic;
using System.Text;
using BetterOne.NodeStructures;

namespace BetterOne.Bees
{
    interface IBee
    {
            public void PickArea(int areaSize);
            public SubGraph GetLocation();
            
    }
}
