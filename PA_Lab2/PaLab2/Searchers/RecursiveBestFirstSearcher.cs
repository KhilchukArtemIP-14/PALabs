using System;
using System.Collections.Generic;
using System.Text;
using PaLab2.Boards;
using System.Linq;

namespace PaLab2.Searchers
{
    enum Result
    {
        Failure = 0,
        Success = 1
    }
    class RecursiveBestFirstSearcher
    {
        private ValuedBoard _board;
        public RecursiveBestFirstSearcher(ValuedBoard board)
        {
            _board = board;
        }

        public void InitBoard()
        {
            Random rand = new Random();
            List<int> rows = new List<int>(),
                columns = new List<int>();
            for (int i = 0; i < _board.GetSize(); i++)
            {
                rows.Add(i);
                columns.Add(i);
            }
            rows = rows.Select(_ => rand.Next(_board.GetSize())).ToList();
            for (int i = 0; i < _board.GetSize(); i++)
            {

                Queen tempQ = new Queen(rows[i], columns[i]);
                _board.SetPiece(tempQ);
            }
            Console.WriteLine("Board intial state: {0}", _board.ToString());
        }

        public void Solve8Q()
        {
            RecursiveBestFirstSearch(2147483646);
            Console.WriteLine("Success: {0}", _board.ToString());
        }

        public Tuple<Result,int> RecursiveBestFirstSearch(int fLimit)
        {
            if (_board.Validate())
            {
                return Tuple.Create(Result.Success, 0);
            }
            List<Move> allMoves = new List<Move>(); 
            foreach (var piece in _board.GetPieces())
            {
                foreach(var coords in GetPossibleMoves(piece))
                {
                    allMoves.Add(new Move(piece, _board, coords));
                }
            }
            if(allMoves.Count==0) return Tuple.Create(Result.Failure, _board.F1GetConflicts());

            List<Tuple<int, Move>> valuesAndMoves = new List<Tuple<int, Move>>();
            foreach (var move in allMoves)
            {
                move.Make();
                valuesAndMoves.Add(Tuple.Create(_board.F1GetConflicts(), move));
                move.Undo();
            }
            while (true)
            {
                valuesAndMoves = valuesAndMoves.OrderBy(pair => pair.Item1).ToList();
                var best = valuesAndMoves[0];
                if (best.Item1 > fLimit) return Tuple.Create(Result.Failure, best.Item1);
                var alternative = valuesAndMoves[1].Item1;
                var min = alternative < fLimit ? alternative : fLimit;
                best.Item2.Make();
                var result = RecursiveBestFirstSearch(min);
                valuesAndMoves[0] = Tuple.Create(result.Item2,best.Item2);
                if (result.Item1 != Result.Failure) return result;
                else best.Item2.Undo();
            }
            
        }
        public List<Tuple<int,int>> GetPossibleMoves(Queen piece)
        {
            List<Tuple<int, int>> tempPossibleMoves= new List<Tuple<int, int>>();

            var queenPosition = piece.GetPosition();
            for (int i = 0; i < _board.GetSize(); i++)
            {
                if (i == queenPosition.Item1) continue;
                tempPossibleMoves.Add(Tuple.Create(i,queenPosition.Item2));
            }
            return tempPossibleMoves;
        }
    }
}
