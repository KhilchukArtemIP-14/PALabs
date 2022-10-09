using System;
using System.Collections.Generic;
using System.Text;

namespace PA_Lab1.DataSources
{
    class ArrayDataSequence:IDataContainer
    {
        private List<int> _data;
        private int _index = 0;

        public ArrayDataSequence(List<int> data)
        {
            _data = data;
        }
        public void Clear()
        {
            _data = new List<int>();
            _index = 0;
        }

        public long GetSize()
        {
            return _data.Count;
        }

        public void InputInt(int a)
        {
            _data.Add(a);
        }

        public int? SpitOutInt()
        {/*
            foreach(var a in _data)
            {
                Console.Write("{0} ", a);
                Console.WriteLine();
            }
            Console.WriteLine();*/

            if (GetSize()<=_index)
            {
                //Console.WriteLine("null");
                return null;
            }
           //Console.WriteLine(_index);
            _index++;
            return _data[_index-1];
        }

        public List<int> GetData()
        {
            return _data;
        }

        public void ResetPointer()
        {
            _index = 0;
        }

        public long GetIndex()
        {
            return _index;
        }
    }
}
