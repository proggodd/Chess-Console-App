using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gptchess
{
    internal class Rook:Piece
    {

        public Rook(Color color, int x, int y) : base(color, x, y)
        {
        }

        public override char Symbol
        {
            get { return Color == Color.White ? 'R' : 'r'; }
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

            // Check if the move is along a straight line
            if (dx != 0 && dy != 0)
            {
                return false;
            }

            // Check if there are no pieces between the current position and the destination position
            int signX = Math.Sign(toX - Position.X);
            int signY = Math.Sign(toY - Position.Y);

            int x = Position.X + signX;
            int y = Position.Y + signY;

            while (x != toX || y != toY)
            {
                if (chessboard.board[x, y] != null)
                {
                    return false;
                }

                x += signX;
                y += signY;
            }

            // Check if the destination square is not occupied by a piece of the same color
            Piece destinationPiece = chessboard.board[toX, toY];

            if (destinationPiece != null && destinationPiece.Color == Color)
            {
                return false;
            }

            return true;
        }

        public override Piece Copy()
        {
            return new Rook(Color, Position.X, Position.Y);
        }
    }
}
