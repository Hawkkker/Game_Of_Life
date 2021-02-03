using System;
using System.Threading;
using System.Diagnostics;
using System.Text;

namespace Jeu_de_la_vie
{
    class Program
    {
        const int rows = 25;
        const int cols = 100;
        static Random rand = new Random();
        
        public static void Main(string[] args)
        {
            var grid = new Boolean[rows, cols];
            grid = Filler(grid);
            int day = 0;
            //Thread.Sleep(5000);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                Printer(grid);
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        grid[row, col] = Life(grid, row, col);
                    }
                }
                //Printer(grid);
                Info(day, grid, stopwatch);
                Thread.Sleep(50);
                Console.SetCursorPosition(0, 0);
                day += 1;
            }
        }

        public static void Info(int day, bool[,] grid, Stopwatch stopwatch)
        {
            int nbAlive = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (grid[row, col])
                        nbAlive += 1;
                }
            }
            Console.Write("\nJour n°" + day + '\n');
            Console.Write("Cellules vivantes : " + nbAlive + '\n');
            Console.WriteLine("Temps écoulé : {0}", stopwatch.Elapsed);
        }

        public static bool Check(int posx, int posy, int row, int col)
        {
            if (row + posx > rows - 1 || row + posx < 0 || col + posy > cols - 1 || col + posy < 0)
                return true;
            else
                return false;
        }

        public static bool Life(bool[,] grid, int row, int col)
        {
            int neighbors = 0;
            for (int posx = -1; posx <= 1; posx++)
            {
                for (int posy = -1; posy <= 1; posy++)
                {
                    //Console.WriteLine("posx : " + posx + " posy : " + posy);
                    if ((posx == 0 && posy == 0) || Check(posx, posy, row, col))
                        continue;
                    if (grid[row + posx, col + posy])
                    {
                        neighbors += 1;
                    }
                }
            }
            //Console.WriteLine("row : " + row + " col : " + col);
            //Console.WriteLine("None : " + none + " Neightbors : " + neighbors);
            if (neighbors == 3 && !grid[row, col])
                return true;
            else if ((neighbors == 2 || neighbors == 3) && grid[row, col])
                return true;
            else
                return false;
        }

        public static bool[,] Filler(bool[,] grid)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int rnd_num = rand.Next(2);
                    if (rnd_num == 1)
                    { 
                        grid[row, col] = true;
                    }
                }
            }
            return grid;
        }

        private static void Printer(bool[,] grid)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < rows; row++)
            {
                //string line = string.Empty;
                for (int col = 0; col < cols; col++)
                {
                    if (!grid[row, col])
                    {
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append('█');
                    }
                }
                sb.Append('\n');
                //Console.WriteLine(line);
            }
            Console.Write(sb.ToString());
        }
    }
}
