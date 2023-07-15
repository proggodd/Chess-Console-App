using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gptchess
{
    internal abstract class Piece
    {
        public Color Color { get; }
        public Position Position { get; private set; }

        public Piece(Color color, int x, int y)
        {
            Color = color;
            Position = new Position(x, y);
        }

        public abstract char Symbol { get; }

        public abstract bool IsValidMove(int toX, int toY, Chessboard chessboard);

        public void Move(int toX, int toY)
        {
            Position = new Position(toX, toY);
        }

        public abstract Piece Copy();
    }
}
