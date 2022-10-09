using System;
using System.Collections.Generic;
using System.Text;

namespace PA_Lab1
{
    class AdvancedMergeSorter
    {
        private IDataContainer _pivot;
        private IDataContainer _buffer;
        private InternalSorter _sorter;

        public AdvancedMergeSorter(IDataContainer pivot, IDataContainer BufferA)
        {
            _pivot = pivot;
            _buffer = BufferA;
        }

        public void MergeSort()
        {
            _sorter = new InternalSorter(_buffer);
            var pivotSize = _pivot.GetSize() / 2;
            while (_pivot.GetIndex() <= pivotSize)
            {
                _sorter.InputInt(_pivot.SpitOutInt().Value);
            }
            _sorter.MergeSortPivot();
            _sorter.DumpDataToSource();

            _sorter = new InternalSorter();
            var g = _pivot.SpitOutInt();
            while (g.HasValue)
            {
                _sorter.InputInt(g.Value);
                g = _pivot.SpitOutInt();
            }

            _sorter.MergeSortPivot();
            MergeTwo(_buffer, _sorter, _pivot);
        }


        private void MergeTwo(IDataContainer fileSource, InternalSorter sorterSource, IDataContainer dump)
        {
            dump.Clear();
            sorterSource.ResetPointer();
            fileSource.ResetPointer();


            var tempIntFile = fileSource.SpitOutInt();
            var tempIntSorter = sorterSource.SpitOutInt();


            while (tempIntFile.HasValue && tempIntSorter.HasValue)
            {
                if (tempIntSorter.Value > tempIntFile.Value)
                {
                    dump.InputInt(tempIntFile.Value);
                    tempIntFile = fileSource.SpitOutInt();
                }
                else { 
                    dump.InputInt(tempIntSorter.Value); 
                    tempIntSorter = sorterSource.SpitOutInt(); }
            }
            if (!tempIntFile.HasValue & tempIntSorter.HasValue)
            {
                while (tempIntSorter.HasValue)
                {
                    dump.InputInt(tempIntSorter.Value);
                    tempIntSorter = sorterSource.SpitOutInt();
                }
            }
            else if (!tempIntSorter.HasValue & tempIntFile.HasValue)
            {
                while (tempIntFile.HasValue)
                {
                    dump.InputInt(tempIntFile.Value);
                    tempIntFile = fileSource.SpitOutInt();
                }
            };
        }
    }
}
