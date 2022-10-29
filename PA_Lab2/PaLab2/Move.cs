using System;
using System.Collections.Generic;
using System.Text;

namespace PaLab2
{
    class Move
    {
        private Queen _piece;
        private Board _board;
        private Tuple<int, int> _bufferCoords;
        private bool _done=false;
        public Move(Queen piece, Board board, Tuple<int,int> newCoords)
        {
            _piece = piece;
            _board = board;
            _bufferCoords = newCoords;
        }

        public void Make()
        {
            if (!_done)
            {
                _done = true;
                var temp = _piece.GetPosition();

                _board.RemovePiece(_piece);

                _piece.ChangePosition(_bufferCoords.Item1,_bufferCoords.Item2);
                _bufferCoords = temp;

                _board.SetPiece(_piece);
            }
        }

        public void Undo()
        {
            if (_done)
            {
                _done = false;
                var temp = _piece.GetPosition();

                _board.RemovePiece(_piece);

                _piece.ChangePosition(_bufferCoords.Item1, _bufferCoords.Item2);
                _bufferCoords = temp;

                _board.SetPiece(_piece);
            }
        }

    }
}
