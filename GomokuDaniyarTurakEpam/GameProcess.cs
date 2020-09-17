using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomokuDaniyarTurakEpam
{
    class GameProcess
    {
        public char[,] Board { get; private set; }
        // создаем лист, содержашие все ходы совершенные в игре
        public List<string> allMoves;
        public char currentPlayer = 'X';
        public char previousPlayer = 'O';

        public GameProcess()
        {
            // инициализация доски (даем начальные значения)
            Board = new char[15, 15];
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (row == 7 && col == 7)
                        Board[row, col] = 'O';
                    else
                        Board[row, col] = '_';
                }
            }

            allMoves = new List<string>();
        }

        // вывести доску
        public void PrintTheBoard()
        {
            Console.Clear();
            
            // верхние координаты доски
            for (char horizontalCoordinates = 'a'; horizontalCoordinates <= 'o'; horizontalCoordinates++)
            {
                Console.Write(horizontalCoordinates);
                Console.Write(" ");
            }
            Console.WriteLine();

            int verticalCoordinates = 0;
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    Console.Write(Board[row, col]);
                    Console.Write(" ");
                }
                Console.Write(verticalCoordinates);
                Console.WriteLine();
                verticalCoordinates++;
            }

        }

        // вывести результат игры
        public void PrintTheResult()
        {
            // лист содержать все ходы, поэтому если он заполнен то ничья
            if (allMoves.Count < 255)
                Console.WriteLine("Winner is " + currentPlayer);
            else
                Console.WriteLine("Draw");

            int cnt = 1;
            // выводим все ходы
            foreach(var elements in allMoves)
            {
                Console.Write(cnt + ". ");
                Console.Write(elements);
                cnt++;
                Console.WriteLine();
            }
        }

        public bool GetWinner()
        {
            int horizontalCountWinner = 0;
            int verticalCountWinner = 0;
            int diagonalCountWinner_1 = 0; // диагональ от левого верхнего угла до правого нижнего угла
            int diagonalCountWinner_2 = 0; // диагональ от правого верхнего угла до левого нижнего угла

            // Определение победителя в горизонтальных линиях
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (Board[row, col] == currentPlayer)
                        horizontalCountWinner++;

                    else
                        horizontalCountWinner = 0;
                    //
                    if (horizontalCountWinner >= 5)
                        return false;
                }
                // чтобы поле не было замкнуто
                horizontalCountWinner = 0;
            }

            // Определение победителя в вертикальных линиях
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (Board[col, row] == currentPlayer)
                        verticalCountWinner++;
                    else
                        verticalCountWinner = 0;
                    //
                    if (verticalCountWinner >= 5)
                        return false;
                }
                // чтобы поле не было замкнуто
                verticalCountWinner = 0;
            }

            // Определение победителя в диагоналях от левого верхнего угла до правого нижнего угла
            for (int k = 0; k < 29; k++)
            {
                for (int col = 0; col < 15; col++)
                {
                    int row = k - col;
                    if ((row < 15 && col < 15) && row >= 0 && col >= 0)
                    {
                        if (Board[row, col] == currentPlayer)
                            diagonalCountWinner_1++;
                        else
                            diagonalCountWinner_1 = 0;
                        //
                        if (diagonalCountWinner_1 >= 5)
                            return false;
                    }
                }
                // чтобы поле не было замкнуто
                diagonalCountWinner_1 = 0;
            }

            // Определение победителя в диагоналях от правого верхнего угла до левого нижнего угла
            int len = 14;
            for (int k = 0; k < 29; k++)
            {
                int temp_row = 0;
                for (int col = len; col <= 14; col++)
                {
                    if ((temp_row < 15 && col < 15) && temp_row >= 0 && col >= 0)
                    {
                        if (Board[temp_row, col] == currentPlayer)
                        {
                            diagonalCountWinner_2++;
                        }
                        else
                            diagonalCountWinner_2 = 0;
                        //
                        if (diagonalCountWinner_2 >= 5)
                            return false;
                    }
                    temp_row++;
                }
                len--;
                // чтобы поле не было замкнуто
                diagonalCountWinner_2 = 0;
            }

            return true;
        }

        public void PlayerMove()
        {
            Console.Write("Enter row: ");
            int row = int.Parse(Console.ReadLine());
            if (row < 0 || row > 14)
                throw new ArgumentException("Out of limits");
            Console.Write("Enter col: ");
            int col = int.Parse(Console.ReadLine());
            if (col < 0 || col > 14)
                throw new ArgumentException("Out of limits");

            // если выбранное поле пустое, ставим текущий знак, если нет заново вводим координаты 
            if (Board[row, col] == '_')
            {
                Board[row, col] = currentPlayer;
                allMoves.Add("Player: " + currentPlayer + " Row: " + row + " Column: " + col);
            }
            else
            {
                PlayerMove();
            }
        }
        
        public void ComputerMove()
        {
            int bestScore = int.MinValue;
            int indexRow = 0;
            int indexCol = 0;
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    // если поле пустое
                    if (Board[row, col] == '_')
                    {
                        // делаем ход
                        Board[row, col] = currentPlayer;
                        int score = MyAlgorithm(Board, currentPlayer, previousPlayer,
                                                                 row, col);
                        // отменяем ход
                        Board[row, col] = '_';

                        if (score > bestScore)
                        {
                            bestScore = score;
                            indexRow = row;
                            indexCol = col;
                        }
                    }
                }
            }

            // делаем лучший ход
            Board[indexRow, indexCol] = currentPlayer;
            allMoves.Add("Player: " + currentPlayer + " Row: " + indexRow + " Column: " + indexCol);
        }


        // этот алгоритм проверяет является ли выбранный ход лучшим
        static int MyAlgorithm(char[,] board, char currentPlayer, char previousPlayer,
                                int indexRow, int indexCol)
        {
            int bestScoreCH = int.MinValue; // current player horizontal
            int bestScoreOH = int.MinValue; // opponent horizontal
            int bestScoreCV = int.MinValue; // current player vertical
            int bestScoreOV = int.MinValue; // opponent vertical
            int bestScoreCD1 = int.MinValue; // current player diagonal left-up to right-down
            int bestScoreOD1 = int.MinValue; // opponent diagonal left-up to right-down
            int bestScoreCD2 = int.MinValue; // current player diagonal right-up to left-down
            int bestScoreOD2 = int.MinValue; // opponent diagonal right-up to left-down


            int count = 0;

            // нахождения best score(в ряд) в горизонтальных линиях для currentPlayer
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (board[row, col] == currentPlayer)
                        count++;
                    else
                        count = 0;
                    //

                    if (bestScoreCH < count)
                    {
                        bestScoreCH = count;
                    }
                }
                count = 0;
            }

            // нахождения best score(в ряд) в вертикальных линиях для currentPlayer
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (board[col, row] == currentPlayer)
                        count++;
                    else
                        count = 0;
                    //
                    if (bestScoreCV < count)
                    {
                        bestScoreCV = count;
                    }

                }
                if (bestScoreCV >= 5)
                {
                    bestScoreCV = 100;
                }
                count = 0;
            }

            // нахождения best score(в ряд) в диагоналях от левого верхнего угла до правого нижнего угла
            // для currentPlayer
            for (int k = 0; k < 29; k++)
            {
                for (int col = 0; col < 15; col++)
                {
                    int row = k - col;
                    if ((row < 15 && col < 15) && row >= 0 && col >= 0)
                    {
                        if (board[row, col] == currentPlayer)
                            count++;
                        else
                            count = 0;
                        //
                        if (bestScoreCD1 < count)
                        {
                            bestScoreCD1 = count;
                        }
                    }
                }
                if (bestScoreCD1 >= 5)
                {
                    bestScoreCD1 = 100;
                }
                count = 0;
            }

            // нахождения best score(в ряд) в диагоналях от правого верхнего угла до левого нижнего угла
            // для currentPlayer
            int len = 14;
            for (int k = 0; k < 29; k++)
            {
                int temp_row = 0;
                for (int col = len; col <= 14; col++)
                {
                    if ((temp_row < 15 && col < 15) && temp_row >= 0 && col >= 0)
                    {
                        if (board[temp_row, col] == currentPlayer)
                        {
                            count++;
                        }
                        else
                            count = 0;
                        //
                        if (bestScoreCD2 < count)
                        {
                            bestScoreCD2 = count;
                        }
                    }
                    temp_row++;
                }
                if (bestScoreCD2 >= 5)
                {
                    bestScoreCD2 = 100;
                }
                len--;
                count = 0;
            }

            board[indexRow, indexCol] = previousPlayer;

            // нахождения best score(в ряд) в горизонтальных линиях для оппонента
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (board[row, col] == previousPlayer)
                        count++;
                    else
                        count = 0;
                    //
                    if (bestScoreOH < count)
                    {
                        bestScoreOH = count;
                    }
                }
                if (bestScoreOH >= 5)
                {
                    bestScoreOH = 80;
                }
                count = 0;
            }

            // нахождения best score(в ряд) в вертикальных линиях для оппонента
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (board[col, row] == previousPlayer)
                        count++;
                    else
                        count = 0;
                    //
                    if (bestScoreOV < count)
                    {
                        bestScoreOV = count;
                    }
                }
                if (bestScoreOV >= 5)
                {
                    bestScoreOV = 80;
                }
                count = 0;
            }

            // нахождения best score(в ряд) в диагоналях от левого верхнего угла до правого нижнего угла
            // для оппонента
            for (int k = 0; k < 29; k++)
            {
                for (int col = 0; col < 15; col++)
                {
                    int row = k - col;
                    if ((row < 15 && col < 15) && row >= 0 && col >= 0)
                    {
                        if (board[row, col] == previousPlayer)
                            count++;
                        else
                            count = 0;
                        //
                        if (bestScoreOD1 < count)
                        {
                            bestScoreOD1 = count;
                        }
                    }
                }
                if (bestScoreOD1 >= 5)
                {
                    bestScoreOD1 = 80;
                }
                count = 0;
            }

            // нахождения best score(в ряд) в диагоналях от правого верхнего угла до левого нижнего угла
            // для оппонента
            len = 14;
            for (int k = 0; k < 29; k++)
            {
                int temp_row = 0;
                for (int col = len; col <= 14; col++)
                {
                    if ((temp_row < 15 && col < 15) && temp_row >= 0 && col >= 0)
                    {
                        if (board[temp_row, col] == previousPlayer)
                        {
                            count++;
                        }
                        else
                            count = 0;
                        //
                        if (bestScoreOD2 < count)
                        {
                            bestScoreOD2 = count;
                        }
                    }
                    temp_row++;
                }
                if (bestScoreOD2 >= 5)
                {
                    bestScoreOD2 = 80;
                }
                len--;
                count = 0;
            }

            // создаем листы который будут содержать отсортированные best scores 
            List<int> sortScoresC = new List<int>(); // для currentPlayer
            List<int> sortScoresO = new List<int>(); // для оппонента

            sortScoresC.Add(bestScoreCH);
            sortScoresC.Add(bestScoreCV);
            sortScoresC.Add(bestScoreCD1);
            sortScoresC.Add(bestScoreCD2);
            sortScoresC.Sort();

            sortScoresO.Add(bestScoreOH);
            sortScoresO.Add(bestScoreOV);
            sortScoresO.Add(bestScoreOD1);
            sortScoresO.Add(bestScoreOD2);
            sortScoresO.Sort();

            return (sortScoresC[3] * 10 + sortScoresO[3] * 9) +
                   (sortScoresC[2] * 3 + sortScoresO[2] * 2) +
                   (sortScoresC[1] + sortScoresO[1]) +
                   (sortScoresC[0] + sortScoresO[0]);
        }
    }
}
