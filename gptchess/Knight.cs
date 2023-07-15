using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gptchess
{
    internal class Knight:Piece
    {
        public Knight(Color color, int x, int y) : base(color, x, y)
        {
        }

        public override char Symbol
        {
            get { return Color == Color.White ? 'N' : 'n'; }
        }

        public override bool IsValidMove(int toX, int toY, Chessboard chessboard)
        {
            // Check if the move is within the board
            if (toX < 0 || toX > 7 || toY < 0 || toY > 7)
            {
                return false;
            }

            int dx = Math.Abs(toX - Position.X);
            int dy = Math.Abs(toY - Position.Y);

            // Check if the move is a L-shape
            if (dx == 1 && dy == 2 || dx == 2 && dy == 1)
            {
                // Check if the destination square is not occupied by a piece of the same color
                Piece destinationPiece = chessboard.board[toX, toY];

                if (destinationPiece != null && destinationPiece.Color == Color)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public override Piece Copy()
        {
            return new Knight(Color, Position.X, Position.Y);
        }
    }
}
