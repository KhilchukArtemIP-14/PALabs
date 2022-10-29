using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PaLab2
{
    class Board
    {

        private int[] _ColumnQueenCount, _RowQueenCount, _diagonalDescendingCount, _diagonalAscendingCount;
        private List<Queen> _pieces= new List<Queen>();
        public Board(int dimension)
        {
            if (dimension < 4) Console.WriteLine("Note: the size of the board is too samll to fit {0} queens", dimension);

            _ColumnQueenCount = new int[dimension];
            _RowQueenCount = new int[dimension];

            for (int i = 0; i < dimension; i++)
            {
                _ColumnQueenCount[i] = 0;
                _RowQueenCount[i] = 0;
            }

            _diagonalAscendingCount = new int[(dimension - 1) * 2+1];
            _diagonalDescendingCount = new int[(dimension - 1) * 2+1];

            for (int i = 0; i < (dimension - 1) * 2+1; i++)
            {
                _diagonalAscendingCount[i] = 0;
                _diagonalDescendingCount[i] = 0;
            }
        }

        public void SetPiece(Queen queen)
        {
            _pieces.Add(queen);
            var coords = queen.GetPosition();
            var row = coords.Item1;
            var column = coords.Item2;

            _RowQueenCount[row] += 1;
            _ColumnQueenCount[column] += 1;

            var central = _RowQueenCount.GetLength(0)-1;
            _diagonalDescendingCount[central - (row - column)]+=1;
            _diagonalAscendingCount[row + column] += 1;
        }

        public bool[,] PeekBoard()
        {
            var cells = new bool[_RowQueenCount.Length, _ColumnQueenCount.Length];

            var queenPositions = _pieces.Select(piece=>piece.GetPosition());

            for (int i = 0; i < _RowQueenCount.Length; i++)
            {
                for (int j = 0; j < _ColumnQueenCount.Length; j++)
                {
                    cells[i, j] = queenPositions.Contains(Tuple.Create(i, j));
                }
            }
            return cells;
        }

        public void RemovePiece(Queen queen)
        {
            var row = queen.GetPosition().Item1;
            var column = queen.GetPosition().Item2;

             _pieces.RemoveAll(r => r.GetPosition().Item1==row&&r.GetPosition().Item2 == column);

            _RowQueenCount[row] -= 1;
            _ColumnQueenCount[column] -= 1;

            var central = _RowQueenCount.GetLength(0) - 1;

            _diagonalDescendingCount[central - (row - column)] -= 1;
            _diagonalAscendingCount[row + column] -= 1;
        }
        public void RemovePiece(Tuple<int, int> coords)
        {
            var row = coords.Item1;
            var column = coords.Item2;

            _pieces.RemoveAll(r => r.GetPosition().Item1 == row && r.GetPosition().Item2 == column);

            _RowQueenCount[row] -= 1;
            _ColumnQueenCount[column] -= 1;

            var central = _RowQueenCount.GetLength(0) - 1;
            _diagonalDescendingCount[central - (row - column)] -= 1;
            _diagonalAscendingCount[row + column] -= 1;
        }
        public bool Validate()
        {
            var dimension = _RowQueenCount.GetLength(0);
            for (int i = 0; i < dimension; i++)
            {
                if ((_RowQueenCount[i] > 1) || (_ColumnQueenCount[i] > 1))
                {
                    return false;
                }
            }

            var diagonals = (dimension - 1) * 2+1;
            for (int i = 0; i < diagonals; i++)
            {
                if ((_diagonalAscendingCount[i] > 1) || (_diagonalDescendingCount[i] > 1))
                {
                    return false;
                }
            }
            return true;
        }
        public override string ToString()
        {
            var cells = PeekBoard();
            string baza = "\n";
            var dimensions = cells.GetLength(0);
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    if (cells[i, j])
                    {
                        baza = string.Concat(baza, "q", ' ');
                    }
                    else
                    {
                        baza = string.Concat(baza, "_", ' ');
                    }
                }
                baza = string.Concat(baza, '\n');
            }

            return baza;
        }

        public int GetSize()
        {
            return _RowQueenCount.Length;
        }

        public List<Queen> GetPieces()
        {
            return _pieces;
        }

        public Tuple<int[], int[], int[], int[]> GetQueenCountsData()
        {
            return Tuple.Create( _RowQueenCount, _ColumnQueenCount, _diagonalDescendingCount, _diagonalAscendingCount);
        }
    }
}
