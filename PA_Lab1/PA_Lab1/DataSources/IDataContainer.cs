using System;
using System.Collections.Generic;
using System.Text;

namespace PA_Lab1
{
    interface IDataContainer
    {
        public int? SpitOutInt();
        public void Clear();
        public void InputInt(int a);
        public long GetSize();
        public void ResetPointer();
        public long GetIndex();
    }
}
