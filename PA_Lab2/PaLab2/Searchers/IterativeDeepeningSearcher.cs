using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Diagnostics;
namespace PaLab2
{
    [Flags]
    enum Result
    {
        Failure = 0,
        Success = 1
    }
    [Flags]
    enum DescendantNodeAlgorithm
    {
        Primitive = 0,
        Advanced = 1
    }
    class IterativeDeepeningSearcher
    {

        public static long states;
        private Board _board;
        public IterativeDeepeningSearcher(Board board)
        {
            _board = board;
        }

        public void Solve8Q(DescendantNodeAlgorithm alg)
        {
            if (alg == DescendantNodeAlgorithm.Advanced)
            {
                for (int i = 0; i < 20; i++)
                {
                    states = 0;
                    if (RecursiveDepthLimitedSearch(0, i, GetAdvancedPossibleMoves) == Result.Success) break;
                    Console.WriteLine("States: {0}", states);
                }
            }
            else
            {
                for (int i = 0; i < 2147483646; i++)
                {
                    states = 0;
                    if (RecursiveDepthLimitedSearch(0, i, GetPossibleMoves) == Result.Success) break;
                    Console.WriteLine("States: {0}", states);
                }
            }
            Console.WriteLine("Success: {0}",_board.ToString());
        }
        private List<Tuple<int,int>> GetPossibleMoves()
        {
            var moves = new List<Tuple<int, int>>();
            var boardData = _board.PeekBoard();
            var dimension = _board.GetSize();
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (!boardData[i, j])
                    {
                        moves.Add(Tuple.Create(i, j));
                    }
                }
            }
            return moves;
        }

        public void InitBoard()
        {
            HashSet<Tuple<int, int>> positions = new HashSet<Tuple<int, int>>();
            Random rand = new Random();
            while (positions.Count < _board.GetSize())
            {
                positions.Add(Tuple.Create(rand.Next(_board.GetSize()), rand.Next(_board.GetSize())));
            }
            foreach(var position in positions) 
            { 

                Queen tempQ = new Queen(position.Item1, position.Item2);
                _board.SetPiece(tempQ);
            }
            Console.WriteLine("Board intial state: {0}", _board.ToString());
        }

        public Result RecursiveDepthLimitedSearch(int depth, int maxDepth, Func<List<Tuple<int, int>>> GetDescendants)
        {
            states++;
            if (_board.Validate())
            {
                return Result.Success;
            }
            //if (depth > maxDepth) return Result.Failure; 
            else
            {
                var temp = _board.GetPieces().ConvertAll(piece => new Queen(piece.GetPosition().Item1, piece.GetPosition().Item2));
                foreach (var piece in temp)
                {
                    var tempPossible = GetDescendants();
                    foreach (var movePosition in tempPossible)
                    {
                        Move tempMove = new Move(piece, _board, movePosition);
                        tempMove.Make();
                        if (depth + 1 > maxDepth||RecursiveDepthLimitedSearch(depth + 1, maxDepth, GetDescendants) != Result.Success)
                        {
                            tempMove.Undo();
                        }
                        else return Result.Success;
                    }
                }
            }
            return Result.Failure;
        }

        public List<Tuple<int, int>> GetAdvancedPossibleMoves()
        {
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
            var queenCountsData= _board.GetQueenCountsData();
            int[] rows=queenCountsData.Item1,
                columns=queenCountsData.Item2,
                diagonalDescending = queenCountsData.Item3,
                diagonalAscending=queenCountsData.Item4;
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < columns.Length; j++)
                {
                    if (rows[i] == 0 && columns[j] == 0 && diagonalDescending[columns.Length - 1 - i + j] == 0 && diagonalAscending[i + j] == 0)
                    {
                        moves.Add(Tuple.Create(i, j));
                    }
                }
            }
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < columns.Length; j++)
                {
                    if (rows[j] == 0 && columns[i] == 0 && diagonalDescending[columns.Length - 1 - i + j] == 0 && diagonalAscending[i + j] == 0&&!moves.Contains(Tuple.Create(i, j)))
                    {
                        moves.Add(Tuple.Create(i, j));
                    }
                }
            }
            return moves;
        }
    }
}
