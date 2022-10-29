using System;
using System.Collections.Generic;
using System.Text;

namespace PaLab2.Boards
{
    class ValuedBoard: Board
    {
        private int _conflictsCount;
        public ValuedBoard(int dimension): base(dimension) { _conflictsCount = 0; }

        public int F1GetConflicts()
        {
            int conflicts = 0;
            var queenCountsData = GetQueenCountsData();
            int[] rows = queenCountsData.Item1,
                diagonalDescending = queenCountsData.Item3,
                diagonalAscending = queenCountsData.Item4;
            for (int i = 0; i < rows.Length; i++)
            {
                //Console.WriteLine("I: {0}, Row:{1}, RowConflicts:{2}",i,rows[i], rows[i] * (rows[i] - 1) / 2);
                conflicts += rows[i] * (rows[i] - 1) / 2;
            }
            //Console.WriteLine("Result Conflicts");
            for (int i = 0; i < diagonalAscending.Length; i++)
            {
                conflicts += diagonalAscending[i]*(diagonalAscending[i]-1)/2;
                conflicts += diagonalDescending[i] * (diagonalDescending[i] - 1) / 2;
            }
            return conflicts;
        }

    }
}
