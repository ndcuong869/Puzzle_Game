using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzeGame
{
    class Puzzle
    {
        private IList<IList<int>> board;
        private IList<string> path;
        private int weight;
        private int length;
        private static int dimension = 3;

        public Puzzle()
        {
            board = new List<IList<int>>();

            for (int i = 0; i < dimension; i++)
            {
                board.Add(new List<int>());
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    board[i].Add(0);
                }
            }

            path = new List<string>();
            weight = 0;
            length = 0;
        }

        public void PrintConsole()
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    Console.Write("{0} ", board[i][j]);
                }
                Console.WriteLine();
            }
        }

        public void InitBoard()
        {
            int count = 1;
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    board[i][j] = count;
                    count++;
                }
            }

            board[dimension - 1][dimension - 1] = 0;
        }

        public void InputConsole()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write("A[{0}][{1}] = ", i, j);

                    string temp = Console.ReadLine();
                    int number = 0;

                    Int32.TryParse(temp, out number);

                    board[i][j] = number;
                }
            }
        }

        public int GetLength()
        {
            return this.length;
        }

        public string GetState()
        {
            string state = "";
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    state += board[i][j].ToString();
                }
            }

            return state;
        }

        public bool IsCompleted()
        {
            if (this.GetState() == "123456780")
            {
                return true;
            }

            return false;
        }

        public void SetWeight(int weight)
        {
            this.weight = weight;
        }

        public int GetWeight()
        {
            return this.weight;
        }

        private void Swap(int row1, int col1, int row2, int col2)
        {
            int temp = board[row1][col1];
            board[row1][col1] = board[row2][col2];
            board[row2][col2] = temp;
        }

        private void GetMove(string move, ref int row, ref int col)
        {
            if (move == "LEFT")
            {
                col--;
                return;
            }

            if (move == "RIGHT")
            {
                col++;
                return;
            }

            if (move == "UP")
            {
                row--;
                return;
            }

            if (move == "DOWN")
            {
                row++;
                return;
            }
        }

        public void Move(string move)
        {
            int row = 0;
            int col = 0;

            this.GetPosition(0, ref row, ref col);
            int row1 = row;
            int col1 = col;

            GetMove(move, ref row1, ref col1);

            Swap(row, col, row1, col1);
            length++;
        }

        public void Copy(Puzzle other)
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    this.board[i][j] = other.board[i][j];
                }
            }

            this.path.Clear();
            for (int i = 0; i < other.path.Count; i++)
            {
                this.path.Add(other.path[i]);
            }

            this.length = other.length;
        }

        public void AddPath(string path)
        {
            this.path.Add(path);
        }

        public void GetPosition(int key, ref int row, ref int col)
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (board[i][j] == key)
                    {
                        row = i;
                        col = j;
                        return;
                    }
                }
            }
        }

        public IList<string> GetPath()
        {
            return path;
        }
    }

    class SlidePuzzleGame
    {
        private Puzzle board;
        private IList<Puzzle> openSet;
        private Puzzle sample;
        private IList<string> result;
        private string[] move = { "LEFT", "RIGHT", "UP", "DOWN" };
        private bool[] validMove = { false, false, false, false };

        public SlidePuzzleGame()
        {
            board = new Puzzle();
            openSet = new List<Puzzle>();
            sample = new Puzzle();

            // Create board in normal mode
            board.InitBoard();
            sample.InitBoard();
        }

        public void InputConsole()
        {
            this.board.InputConsole();
        }

        private int GetLeftChild(int parent)
        {
            return parent * 2 + 1;
        }

        private int GetRightChild(int parent)
        {
            return parent * 2 + 2;
        }

        private void SwapNode(int i, int j)
        {
            Puzzle temp = openSet[i];
            openSet[i] = openSet[j];
            openSet[j] = temp;
        }

        private void HeapDefine(int parent)
        {
            int leftChild = GetLeftChild(parent);
            int rightChild = GetRightChild(parent);

            int min = parent;

            if (leftChild < openSet.Count && openSet[leftChild].GetWeight() < openSet[min].GetWeight())
            {
                min = leftChild;
            }

            if (rightChild < openSet.Count && openSet[rightChild].GetWeight() < openSet[min].GetWeight())
            {
                min = rightChild;
            }

            if (min != parent)
            {
                SwapNode(min, parent);
                HeapDefine(min);
            }
        }

        private void PrintHeap()
        {
            for (int i = 0; i < openSet.Count; i++)
            {
                Console.Write("{0} ", openSet[i].GetWeight());
            }
            Console.WriteLine();
        }

        private void HeapMin(Puzzle puzzle)
        {
            openSet.Add(puzzle);

            HeapDefine(0);
            PrintHeap();
        }

        private void HeapDefineInsert(int child)
        {
            int parent = (child - 1) / 2;

            if (openSet[child].GetWeight() < openSet[parent].GetWeight())
            {
                SwapNode(child, parent);
                HeapDefineInsert(parent);
            }
        }

        private void InsertHeap(Puzzle puzzle)
        {
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].GetState() == puzzle.GetState())
                {
                    return;
                }
            }

            openSet.Add(puzzle);

            int parent = openSet.Count - 1;
            HeapDefineInsert(parent);
        }

        private void GetMove(string move, ref int row, ref int col)
        {
            if (move == "LEFT")
            {
                col--;
                return;
            }

            if (move == "RIGHT")
            {
                col++;
                return;
            }

            if (move == "UP")
            {
                row--;
                return;
            }

            if (move == "DOWN")
            {
                row++;
                return;
            }
        }

        private void FindValidMove(Puzzle puzzle)
        {
            int row = 0;
            int col = 0;

            for (int i = 0; i < 4; i++)
            {
                validMove[i] = false;
            }

            for (int i = 0; i < 4; i++)
            {
                puzzle.GetPosition(0, ref row, ref col);
                GetMove(move[i], ref row, ref col);

                if (row > -1 && row < 3 && col > -1 && col < 3)
                {
                    validMove[i] = true;
                }
            }
        }

        private void Visited(Puzzle temp)
        {
            FindValidMove(temp);
            for (int i = 0; i < 4; i++)
            {
                if (validMove[i])
                {
                    Puzzle puzzle = new Puzzle();
                    puzzle.Copy(temp);

                    puzzle.Move(move[i]);

                    int weight = 0;
                    for (int j = 1; j < 9; j++)
                    {
                        weight += CalManhattanDistance(puzzle, j);
                    }

                    weight += puzzle.GetLength();
                    puzzle.SetWeight(weight);
                    puzzle.AddPath(move[i]);

                    InsertHeap(puzzle);
                }
            }
        }

        private int CalManhattanDistance(Puzzle puzzle, int value)
        {
            int row = 0;
            int col = 0;
            puzzle.GetPosition(value, ref row, ref col);

            int orginalRow = 0;
            int orginalCol = 0;
            this.sample.GetPosition(value, ref orginalRow, ref orginalCol);

            return Math.Abs(row - orginalRow) + Math.Abs(col - orginalCol);
        }

        private Puzzle Pop()
        {
            Puzzle temp = openSet[0];

            SwapNode(0, openSet.Count - 1);
            openSet.RemoveAt(openSet.Count - 1);
            HeapDefine(0);

            return temp;
        }

        public IList<string> Random()
        {
            IList<string> pathRandom = new List<string>();
            Random random = new Random();
            int times = 10;

            while (times != 0)
            {
                FindValidMove(this.board);
                int direct = random.Next(0, 4);

                if (validMove[direct])
                {
                    board.Move(move[direct]);
                    pathRandom.Add(move[direct]);
                    times--;
                }
            }

            PrintBoard();

            return pathRandom;
        }

        public IList<string> Solve()
        {
            openSet.Add(board);

            while (openSet.Count > 0)
            {
                var temp = Pop();

                if (temp.IsCompleted())
                {
                    result = temp.GetPath();
                    return result;
                }

                Visited(temp);
            }

            board.InitBoard();
            return result;
        }

        public void PrintResult()
        {
            for (int i = 0; i < result.Count; i++)
            {
                Console.Write("{0} ", result[i]);
            }
            Console.WriteLine();
        }

        private void PrintBoard()
        {
            this.board.PrintConsole();
        }

    }
}
