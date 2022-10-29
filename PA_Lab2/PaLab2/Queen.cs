using System;
using System.Collections.Generic;
using System.Text;

namespace PaLab2
{
    class Queen
    {

        private int _row, _column;
        
        public Queen(int row, int column)
        {
            ChangePosition(row, column);
        }

        public void ChangePosition(int row, int column)
        {
            _row = row;
            _column = column;
        }

        public Tuple<int,int> GetPosition()
        {
            return Tuple.Create(_row, _column);
        }

    }
}
