using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gptchess
{
    internal class Pawn:Piece
    {

        public bool HasMoved { get; set; }

        public Pawn(Color color, int x, int y) : base(color, x, y)
        {
            HasMoved = false;
        }

        public override char Symbol
        {
            get { return Color == Color.White ? 'P' : 'p'; }
        }

        public override bool IsValidMove(int toX, int toY, Chessboard chessboard)
        {
            // Check if the move is within the board
            if (toX < 0 || toX > 7 || toY < 0 || toY > 7)
            {
                return false;
            }

            int dx = toX - Position.X;
            int dy = toY - Position.Y;

            // Check if the pawn is moving forward
            if (Color == Color.White && dx < 0 || Color == Color.Black && dx > 0)
            {
                return false;
            }

            // Check if the pawn is moving too far forward
            if (Math.Abs(dx) > 2 || Math.Abs(dy) > 1)
            {
                return false;
            }
            // Check if the pawn is moving one or two squares forward
            if (Math.Abs(dx) == 2 && HasMoved || Math.Abs(dx) == 2 && chessboard.board[toX, toY] != null)
            {
                return false;
            }

            // Check if the pawn is moving one square forward
            if (Math.Abs(dx) == 1 && dy == 0)
            {
                // Check if the destination square is not occupied
                if (chessboard.board[toX, toY] != null)
                {
                    return false;
                }

                // Check if the pawn is not capturing en passant
                if (chessboard.EnPassantTarget != new Point(toX, toY))
                {
                    return false;
                }

                // Check if the captured pawn is of the opposite color and has just moved two squares forward
                int capturedPawnX = toX + (Color == Color.White ? -1 : 1);
                int capturedPawnY = toY;

                Pawn capturedPawn = chessboard.board[capturedPawnX, capturedPawnY] as Pawn;

                if (capturedPawn == null || capturedPawn.Color == Color || !capturedPawn.HasMoved)
                {
                    return false;
                }

                return true;
            }

            // Check if the pawn is capturing diagonally
            if (Math.Abs(dx) == 1 && Math.Abs(dy) == 1)
            {
                // Check if the destination square is occupied by a piece of the opposite color
                Piece destinationPiece = chessboard.board[toX, toY];

                if (destinationPiece == null || destinationPiece.Color == Color)
                {
                    return false;
                }

                return true;
            }

            // Check if the pawn is moving one square forward
            if (Math.Abs(dx) == 1 && dy == 0)
            {
                // Check if the destination square is not occupied
                if (chessboard.board[toX, toY] != null)
                {
                    return false;
                }

                // Check if the pawn is not capturing en passant
                if (chessboard.EnPassantTarget != new Point(toX, toY))
                {
                    return false;
                }

                // Check if the captured pawn is of the opposite color and has just moved two squares forward
                int capturedPawnX = toX + (Color == Color.White ? -1 : 1);
                int capturedPawnY = toY;

                Pawn capturedPawn = chessboard.board[capturedPawnX, capturedPawnY] as Pawn;

                if (capturedPawn == null || capturedPawn.Color == Color || !capturedPawn.HasMoved)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public override Piece Copy()
        {
            Pawn pawn = new Pawn(Color, Position.X, Position.Y);
            pawn.HasMoved = HasMoved;
            return pawn;
        }
    }
}
