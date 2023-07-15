using System;
using System.Drawing;
using System.Net.NetworkInformation;

namespace gptchess
{
    class actual
    {
        public static void Main(string[] args)
        {
            Chessboard chessboard = new Chessboard();
            chessboard.Initialize();
             bool isWhiteTurn = true;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(chessboard);

                if (chessboard.IsCheckmate(Color.White))
                {
                    Console.WriteLine("Checkmate! Black wins.");
                    break;
                }
                else if (chessboard.IsCheckmate(Color.Black))
                {
                    Console.WriteLine("Checkmate! White wins.");
                    break;
                }
                else if (chessboard.IsStalemate(Color.White) || chessboard.IsStalemate(Color.Black))
                {
                    Console.WriteLine("Stalemate!");
                    break;
                }

                Color turn = isWhiteTurn ? Color.White : Color.Black;
                Console.WriteLine($"{turn}'s turn.");

                Console.Write("Enter start position (e.g. 'a2'): ");
                string from = Console.ReadLine();

                Console.Write("Enter end position (e.g. 'a4'): ");
                string to = Console.ReadLine();

                try
                {
                    int fromX = from[0] - 'a';
                    int fromY = from[1] - '1';
                    int toX = to[0] - 'a';
                    int toY = to[1] - '1';
                    Console.WriteLine($"Move result: {chessboard.Move(fromX, fromY, toX, toY, turn)}");

                    if (chessboard.Move(fromX, fromY, toX, toY, turn))
                    {
                        isWhiteTurn = (!isWhiteTurn);
                    }
                    else
                    {
                        Console.WriteLine("Invalid move.");
                        Console.ReadLine();
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Invalid input string. The input string should be in the format 'a1' to 'h8'.");
                    Console.ReadLine();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. The input should be a number.");
                    Console.ReadLine();
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid input string. The input string should be a valid chessboard position.");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.ReadLine();
                }
            }

            Console.ReadLine();
        }
    }
}