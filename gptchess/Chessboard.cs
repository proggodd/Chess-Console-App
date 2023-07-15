using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gptchess
{
    internal class Chessboard
    {
        public Piece[,] board { get; private set; }
        public Point EnPassantTarget { get; set; }
        public Color currentTurn;

        public Chessboard()
        {
            board = new Piece[8, 8];
            currentTurn = Color.White;
            EnPassantTarget = null;
        }

        public void Initialize()
        {
            // Initialize the white pieces
            board[0, 0] = new Rook(Color.White, 0, 0);
            board[1, 0] = new Knight(Color.White, 1, 0);
            board[2, 0] = new Bishop(Color.White, 2, 0);
            board[3, 0] = new Queen(Color.White, 3, 0);
            board[4, 0] = new King(Color.White, 4, 0);
            board[5, 0] = new Bishop(Color.White, 5, 0);
            board[6, 0] = new Knight(Color.White, 6, 0);
            board[7, 0] = new Rook(Color.White, 7, 0);

            for (int i = 0; i < 8; i++)
            {
                board[i, 1] = new Pawn(Color.White, i, 1);
            }

            // Initialize the black pieces
            board[0, 7] = new Rook(Color.Black, 0, 7);
            board[1, 7] = new Knight(Color.Black, 1, 7);
            board[2, 7] = new Bishop(Color.Black, 2, 7);
            board[3, 7] = new Queen(Color.Black, 3, 7);
            board[4, 7] = new King(Color.Black, 4, 7);
            board[5, 7] = new Bishop(Color.Black, 5, 7);
            board[6, 7] = new Knight(Color.Black, 6, 7);
            board[7, 7] = new Rook(Color.Black, 7, 7);

            for (int i = 0; i < 8; i++)
            {
                board[i, 6] = new Pawn(Color.Black, i, 6);
            }
        }

        public override string ToString()
        {
            string result = "  a b c d e f g h\n";

            for (int y = 7; y >= 0; y--)
            {
                result += $"{y + 1} ";

                for (int x = 0; x < 8; x++)
                {
                    string symbol = board[x, y]?.Symbol.ToString() ?? ".";
                    result += symbol + " ";
                }

                result += $"{y + 1}\n";
            }

            result += "  a b c d e f g h\n";
            return result;
        }

        public bool IsCheck(Color color)
        {
            // Find the king of the given color
            King king = null;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = board[x, y];

                    if (piece is King && piece.Color == color)
                    {
                        king = (King)piece;
                        break;
                    }
                }

                if (king != null)
                {
                    break;
                }
            }

            if (king == null)
            {
                return false;
            }

            // Check if any of the opponent's pieces can attack the king
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = board[x, y];

                    if (piece != null && piece.Color != color)
                    {
                        if (piece.IsValidMove(king.Position.X, king.Position.Y, this))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsCheckmate(Color color)
        {
            // Check if the king is in check
            if (!IsCheck(color))
            {
                return false;
            }

            // Check if the king can move out of check
            King king = null;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = board[x, y];

                    if (piece is King && piece.Color == color)
                    {
                        king = (King)piece;
                        break;
                    }
                }

                if (king != null)
                {
                    break;
                }
            }

            if (king == null)
            {
                return false;
            }

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    int toX = king.Position.X + dx;
                    int toY = king.Position.Y + dy;

                    if (toX < 0 || toX > 7 || toY < 0 || toY > 7)
                    {
                        continue;
                    }

                    if (board[toX, toY] == null || board[toX, toY].Color != color)
                    {
                        if (!IsCheckAfterMove(king.Position.X, king.Position.Y, toX, toY, color))
                        {
                            return false;
                        }
                    }
                }
            }

            // Check if any piece can block the attack
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = board[x, y];

                    if (piece != null && piece.Color == color)
                    {
                        for (int toX = 0; toX < 8; toX++)
                        {
                            for (int toY = 0; toY < 8; toY++)
                            {
                                if (piece.IsValidMove(toX, toY, this))
                                {
                                    if (!IsCheckAfterMove(x, y, toX, toY, color))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public bool IsStalemate(Color color)
        {
            // Check if the king is in check
            if (IsCheck(color))
            {
                return false;
            }

            // Check if any piece can move
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Piece piece = board[x, y];

                    if (piece != null && piece.Color == color)
                    {
                        for (int toX = 0; toX < 8; toX++)
                        {
                            for (int toY = 0; toY < 8; toY++)
                            {
                                if (piece.IsValidMove(toX, toY, this))
                                {
                                    if (!IsCheckAfterMove(x, y, toX, toY, color))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public bool Move(int fromX, int fromY, int toX, int toY, Color turn)
        {
            Piece piece = board[fromX, fromY];

            if (piece == null || piece.Color != turn)
            {
                return false;
            }

            if (!piece.IsValidMove(toX, toY, this))
            {
                return false;
            }

            if (IsCheckAfterMove(fromX, fromY, toX, toY, turn))
            {
                return false;
            }

            Piece capturedPiece = board[toX, toY];
            board[toX, toY] = piece;
            board[fromX, fromY] = null;

            if (capturedPiece != null && capturedPiece.Color != turn)
            {
                capturedPiece = capturedPiece.Copy();
                capturedPiece.Move(-1, -1);
            }

            if (piece is Pawn && (toY == 0 || toY == 7))
            {
                board[toX, toY] = new Queen(turn, toX, toY);
            }

            return true;
        }

        private bool IsCheckAfterMove(int fromX, int fromY, int toX, int toY, Color turn)
        {
            Piece piece = board[fromX, fromY]; if (piece == null || piece.Color != turn)
            {
                return false;
            }

            Piece capturedPiece = board[toX, toY];
            board[toX, toY] = piece;
            board[fromX, fromY] = null;

            bool isCheck = IsCheck(turn);

            board[fromX, fromY] = piece;
            board[toX, toY] = capturedPiece;

            return isCheck;
        }
    }
}
