using System;
using static System.Threading.Thread;

namespace XO
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] array;
            Console.WriteLine("Нажмите соответствующую клавишу:\n");
            while (true)
            {
                Console.WriteLine("1. Начать игру\n" +
                                  "2. Правила\n" +
                                  "0. Выйти из игры");
                int n = int.Parse(Console.ReadLine());
                if (n == 1)
                {
                    Console.WriteLine("Игра начинается: ");
                    array = FillArray();
                    for(int i = 0; i < 5; i++)
                    {
                        int[] pos = XO(array, 1);
                        if (CheckWinner(array, 1))
                        {
                            Draw(array, ConsoleColor.Green, pos);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nВыирал X\n");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        }
                        Draw(array, ConsoleColor.Green, pos);

                        if (i == 4)
                        {
                            Console.WriteLine("Ничья");
                            break;
                        }

                        pos = XO(array, 0);
                        if (CheckWinner(array, 0))
                        {
                            Draw(array, ConsoleColor.Red, pos);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nВыирал О\n");
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        }
                        Draw(array, ConsoleColor.Red, pos);
                    }
                    Console.WriteLine("Игра завершена");
                }
                else if (n == 2)
                {
                    Console.WriteLine("\nВведите строку, а затем столбец, чтобы поставить свой символ в нужное место\n" +
                                      "Пример ввода: 2 3 поставит свой символ на вторую строку в третем столбце\n");
                }
                else if (n == 0)
                {
                    Console.WriteLine("Выход из игры...");
                    Sleep(1000);
                    return;
                }
                else
                    Console.WriteLine("Ошибка (нажмите 1 или 0)");
            }
        }

        private static int[] XO(int[,] array, int num)
        {
            bool check;
            int[] pos;
            do
            {
                Console.WriteLine("Ход игрока " + num + "(символ " + (num == 1 ? "X" : "O") + " )");
                string[] str = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                pos = new int[str.Length];

                check = false;
                for (int i = 0; i < str.Length; i++)
                {
                    if (!int.TryParse(str[i], out pos[i]))
                    {
                        check = true;
                        break;
                    }
                }

                if (check || CheckArray(array, pos, 1, 0))
                {
                    Console.WriteLine("Неверно введена позиция символа");
                    check = true;
                    continue;
                }
                else
                    check = false;

                array = FillArray(array, pos, num);

            } while (check);

            return pos;
        }

        private static bool CheckWinner(int[,] array, int num)
        {
            int n = array.GetLength(0);
            num *= 3;
            //1 столбец
            int count = 0;
            for (int i = 0; i < n; i++)
                count += array[i, 0];
            if (count == num)
                return true;

            //2 столбец
            count = 0;
            for (int i = 0; i < n; i++)
                count += array[i, 1];
            if (count == num)
                return true;

            //2 столбец
            count = 0;
            for (int i = 0; i < n; i++)
                count += array[i, 2];
            if (count == num)
                return true;

            //1 cтрока
            count = 0;
            for (int i = 0; i < n; i++)
                count += array[0, i];
            if (count == num)
                return true;

            //2 cтрока
            count = 0;
            for (int i = 0; i < n; i++)
                count += array[1, i];
            if (count == num)
                return true;

            //3 cтрока
            count = 0;
            for (int i = 0; i < n; i++)
                count += array[2, i];
            if (count == num)
                return true;

            //1 диагональ
            count = 0;
            for (int i = 0; i < n; i++)
                count += array[i, n - i - 1];
            if (count == num)
                return true;

            //2 диагональ
            count = 0;
            for (int i = 0; i < n; i++)
                count += array[i, i];
            if (count == num)
                return true;

            return false;
        }

        private static bool CheckArray(int[,] array, int[] pos, int symbol1, int symbol2)
        {
            if (pos.Length != 2)
                return true;

            if (pos[0] < 1 | pos[0] > 3 | pos[1] < 1 | pos[1] > 3)
                return true;

            if(array[pos[0] - 1, pos[1] - 1] != symbol1 &
               array[pos[0] - 1, pos[1] - 1] != symbol2)
                    return false;

            return true;
        }

        private static void Draw(int[,] array, ConsoleColor color, int[] pos = null)
        {
            int n = array.GetLength(0);
            if (pos != null)
            {
                for (int i = 0; i < n; i++)
                {
                    Console.Write("|  ");
                    for (int j = 0; j < n; j++)
                    {
                        if (i == pos[0] - 1 && j == pos[1] - 1)
                            Console.ForegroundColor = color;
                        Console.Write((array[i, j] == 0) ? "O" : (array[i, j] == 1) ? "X" : " ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("  |  ");
                    }
                    Console.WriteLine();
                }
            }
        }

        private static int[,] FillArray(int[,] array = null, int[] pos = null, int? symbol = null)
        {
            if (array == null)
            {
                int[,] newArray = new int[3, 3];
                int n = newArray.GetLength(0);
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        newArray[i, j] = -4;
                array = newArray;
            }
            else if(pos != null && (symbol == 1 || symbol == 0))
            {
                int n = array.GetLength(0);
                while (true)
                {
                    if (pos[0] <= n && pos[1] <= n)
                    {
                        array[pos[0] - 1, pos[1] - 1] = symbol.Value;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неверно введено положение знака " + 
                            (symbol == 0 ? "O" : symbol == 1 ? "X" : ""));
                        break;
                    }
                }
            }
            else
                new Exception("Ошибка");

            return array;
        }
    }
}
