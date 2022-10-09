using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace PA_Lab1
{
    class DirectMergeSorterService
    {
        private IDataContainer _pivot;
        private IDataContainer[] _buffers = new IDataContainer[2];

        public DirectMergeSorterService(IDataContainer pivot, IDataContainer bufferA, IDataContainer bufferB)
        {
            _pivot = pivot;
            _buffers[0] = bufferA;
            _buffers[1] = bufferB;
        }

        public void DirectMergeSort()
        {
            int i = -1;
            do
            {
                i++;
                Split(i);
            } while (!Merge(i));
        }

        public void Split(int iteration)// *iteration starting from zero
        {
            int num = 0;

            _buffers[0].Clear();
            _buffers[1].Clear();
            _pivot.ResetPointer();
            var a = _pivot.SpitOutInt();
            while (a!=null){

                _buffers[(num / (int)Math.Pow(2, iteration))%2].InputInt(a.Value);
                a = _pivot.SpitOutInt();
                num++;
            }

            _buffers[0].ResetPointer();
            _buffers[1].ResetPointer();
        }
        public bool Merge(int iteration)
        {
            if (_pivot.GetSize() == _buffers[0].GetSize()) { return true; }
            _pivot.Clear();
            int bufferCountA = (int)Math.Pow(2, iteration);
            int bufferCountB = (int)Math.Pow(2, iteration);

            int? tempIntA = _buffers[0].SpitOutInt();
            int? tempIntB = _buffers[1].SpitOutInt();

            while (tempIntA != null && tempIntB != null)
            {
                if (tempIntA.Value >tempIntB.Value)
                {
                    _pivot.InputInt(tempIntB.Value);
                    tempIntB = _buffers[1].SpitOutInt();
                    bufferCountB--;
                    if (bufferCountB == 0 || !tempIntB.HasValue)
                    {

                        while (tempIntA.HasValue && (bufferCountA > 0))
                        {
                            _pivot.InputInt(tempIntA.Value);
                            tempIntA = _buffers[0].SpitOutInt();
                            bufferCountA--;
                        }
                        bufferCountA = (int)Math.Pow(2, iteration);
                        bufferCountB = (int)Math.Pow(2, iteration);
                    }
                }
                else
                {
                    _pivot.InputInt(tempIntA.Value);
                    tempIntA = _buffers[0].SpitOutInt();
                    bufferCountA--;
                    if (bufferCountA == 0 || !tempIntA.HasValue)
                    {
                        while (tempIntB.HasValue && (bufferCountB > 0))
                        {
                            _pivot.InputInt(tempIntB.Value);
                            tempIntB = _buffers[1].SpitOutInt();
                            bufferCountB--;
                        }
                        bufferCountA = (int)Math.Pow(2, iteration);
                        bufferCountB = (int)Math.Pow(2, iteration);
                    }
                }
            }

            if (!tempIntA.HasValue & tempIntB.HasValue)
            {
                while (tempIntB.HasValue)
                {
                    _pivot.InputInt(tempIntB.Value);
                    tempIntB = _buffers[1].SpitOutInt();
                }
            }
            else if (!tempIntB.HasValue & tempIntA.HasValue) 
            {
                while (tempIntA.HasValue)
                {
                    _pivot.InputInt(tempIntA.Value);
                    tempIntA = _buffers[0].SpitOutInt();
                }
            };
            return false;
        }
    }
}
