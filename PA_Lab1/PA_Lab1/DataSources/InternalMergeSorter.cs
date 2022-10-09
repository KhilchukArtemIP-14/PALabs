using System;
using System.Collections.Generic;
using System.Text;

namespace PA_Lab1
{
    class InternalSorter: IDataContainer
    {
        private List<int> _pivot = new List<int>();
        private int _index = 0;
        private IDataContainer _dataSource;

        public InternalSorter(IDataContainer dataSource=null)
        {
            _dataSource = dataSource;
        }

        public void FillData()
        {
            _dataSource.ResetPointer();
            var a = _dataSource.SpitOutInt();
            while (a.HasValue)
            { 
                _pivot.Add(a.Value);
                a = _dataSource.SpitOutInt();
            }
        }


        public void DumpDataToSource()
        {
            _dataSource.Clear();
            foreach(var a in _pivot)
            {
                _dataSource.InputInt(a);
            }
            Clear();
        }

        public void MergeSortPivot()
        {
            _pivot.Sort();
        }

        public int? SpitOutInt()
        {
            if (_index < _pivot.Count)
            {
                _index++;
                return _pivot[_index - 1];
            }
            else return null;
        }

        public void InputInt(int a)
        {
            _pivot.Add(a);
        }

        public long GetSize()
        {
            return _pivot.Count;
        }

        public void ResetPointer()
        {
            _index = 0;
        }

        public void Clear()
        {
            _pivot = new List<int>();
            _index = 0;
        }


        public long GetIndex()
        {
            return _index;
        }
    }
}
