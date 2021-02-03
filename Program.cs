using System;
using System.Threading;
using System.Diagnostics;
using System.Text;

namespace Jeu_de_la_vie
{
    enum Status
    {
        Alive = 0,
        Dead = 1
    }

    class Cell
    {
        private Status status;
        private int posX;
        private int posY;
        private Cell[] neighbors;

        public Cell(int X, int Y, Status rnd)
        {
            posX = X;
            posY = Y;
            status = rnd;
            neighbors = new Cell[9];
        }

        public void setNeighbors(int X, int Y)
        {
            int i = 0;
            for (int posx = -1; posx <= 1; posx++)
            {
                for (int posy = -1; posy <= 1; posy++)
                {
                    if ((posx == 0 && posy == 0) || Grid.Check(posx, posy, X, Y))
                    {
                        neighbors[i] = null;
                        i += 1;
                        continue;
                    }
                    Cell cell = Grid.getCell(X + posx, Y + posy);
                    if (cell is Cell)
                        neighbors[i] = cell;
                    else
                        neighbors[i] = null;
                    i += 1;
                }
            }
        }

        public void nextTurn()
        {
            int nbAlive = Array.FindAll(neighbors, el => el != null && el.isAlive() == Status.Alive).Length;
            if (isAlive() == Status.Alive && (nbAlive == 2 || nbAlive == 3))
                status = Status.Alive;
            else if (isAlive() == Status.Dead && nbAlive == 3)
                status = Status.Alive;
            else
                status = Status.Dead;
        }

        public int getX()
        {
            return posX;
        }

        public int getY()
        {
            return posY;
        }

        public Status isAlive()
        {
            return status;
        }
    }

    class Grid
    {
        static int rows = 25;
        static int cols = 100;
        static Cell[] cells;

        public static void Main(string[] args)
        {
            cells = new Cell[rows * cols];
            int day = 0;
            int i = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Status status;
            Random rand = new Random();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int rnd_num = rand.Next(2);
                    if (rnd_num == 1)
                        status = Status.Alive;
                    else
                        status = Status.Dead;
                    cells[i] = new Cell(row, col, status);
                    i += 1;
                }
            }
            foreach (Cell cell in cells)
            {
                cell.setNeighbors(cell.getX(), cell.getY());
            }
            while (true)
            {
                Printer();
                foreach (var cell in cells)
                {
                    cell.nextTurn();
                }
                Info(day, stopwatch);
                Thread.Sleep(50);
                Console.SetCursorPosition(0, 0);
                day += 1;
            }
        }

        private static void Info(int day, Stopwatch stopwatch)
        {
            int nbAlive = 0;
            foreach (var cell in cells)
            {
                if (cell.isAlive() == Status.Alive)
                    nbAlive += 1;
            }
            Console.Write("\nJour n°" + day + '\n');
            Console.Write("Cellules vivantes : " + nbAlive + '\n');
            Console.WriteLine("Temps écoulé : {0}", stopwatch.Elapsed);
        }
        public static Cell getCell(int X, int Y)
        {
            Cell cell = Array.Find(cells, el => el != null && el.getX() == X && el.getY() == Y);
            if (cell is Cell)
                return cell;
            else
                return null;
        }

        public static bool Check(int posx, int posy, int X, int Y)
        {
            if (X + posx > rows - 1 || X + posx < 0 || Y + posy > cols - 1 || Y + posy < 0)
                return true;
            else
                return false;
        }

        private static void Printer()
        {
            StringBuilder sb = new StringBuilder();
            int line = 0;
            for (int i = 0; i < rows * cols; i++)
            {
                if (cells[i].getX() > line)
                {
                    line += 1;
                    sb.Append('\n');
                }
                if (cells[i].isAlive() == Status.Alive)
                    sb.Append('█');
                else
                    sb.Append(' ');
            }
            Console.Write(sb.ToString());
        }
    }
}
