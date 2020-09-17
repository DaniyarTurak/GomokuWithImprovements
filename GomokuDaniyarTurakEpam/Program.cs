using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomokuDaniyarTurakEpam
{
    class Program
    {
        static void Main(string[] args)
        {
            GameProcess gomoku = new GameProcess();

            Console.WriteLine($"Select game mode:");
            Console.WriteLine("1. Player1 vs Player2");
            Console.WriteLine("2. Player vs Computer");
            Console.WriteLine("3. Computer vs Computer");
            Console.Write("Enter the mode: ");

            string mode = Console.ReadLine();

            if (mode == "1")
                PlayerVsPlayer(gomoku);
            else if (mode == "2")
                PlayerVsComputer(gomoku);
            else if (mode == "3")
                ComputerVsComputer(gomoku);
            else
                throw new Exception("Wrong input");
        }

        static void PlayerVsPlayer(GameProcess gomoku)
        {
            bool startGame = true;
            while (startGame)
            {
                gomoku.PrintTheBoard();
                gomoku.PlayerMove();
                startGame = gomoku.GetWinner();
                if (startGame == true)
                {
                    gomoku.previousPlayer = gomoku.currentPlayer;
                    gomoku.currentPlayer = ChangeTurn(gomoku.currentPlayer);
                }
                // если startGame = false, то currentPlayer не меняется и объявляется победителем
            }
            gomoku.PrintTheBoard();
            gomoku.PrintTheResult();
        }

        static void PlayerVsComputer(GameProcess gomoku)
        {
            bool startGame = true;
            while (startGame)
            {
                gomoku.PrintTheBoard();
                gomoku.PlayerMove();
                startGame = gomoku.GetWinner();
                if (startGame == true)
                {
                    gomoku.previousPlayer = gomoku.currentPlayer;
                    gomoku.currentPlayer = ChangeTurn(gomoku.currentPlayer);
                }
                else // если startGame = false, то currentPlayer не меняется и объявляется победителем
                    break;

                gomoku.ComputerMove();
                startGame = gomoku.GetWinner();
                if (startGame == true)
                {
                    gomoku.previousPlayer = gomoku.currentPlayer;
                    gomoku.currentPlayer = ChangeTurn(gomoku.currentPlayer);
                }
                else // если startGame = false, то currentPlayer не меняется и объявляется победителем
                    break;
            }
            gomoku.PrintTheBoard();
            gomoku.PrintTheResult();
        }

        static void ComputerVsComputer(GameProcess gomoku)
        {
            bool startGame = true;
            while (startGame)
            {
                gomoku.PrintTheBoard();
                gomoku.ComputerMove();

                startGame = gomoku.GetWinner();
                if (startGame == true)
                {
                    gomoku.previousPlayer = gomoku.currentPlayer;
                    gomoku.currentPlayer = ChangeTurn(gomoku.currentPlayer);
                }
                // если startGame = false, то currentPlayer не меняется и объявляется победителем 
            }
            gomoku.PrintTheBoard();
            gomoku.PrintTheResult();
        }

        // функция для передачи хода следующему игроку
        static char ChangeTurn(char currentPlayer)
        {
            if (currentPlayer == 'X')
                return 'O';
            else
                return 'X';
        }
    }
}
