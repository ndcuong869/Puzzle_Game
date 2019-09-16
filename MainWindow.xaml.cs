using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace PuzzeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SlidePuzzleGame game;

        public MainWindow()
        {
            InitializeComponent();
            game = new SlidePuzzleGame();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MoveButton(string name, int row, int col)
        {
            Button btn = (Button)this.FindName(name);
            
            btn.SetValue(Grid.RowProperty, row);
            btn.SetValue(Grid.ColumnProperty, col);
            btn.SetValue(Canvas.ZIndexProperty, 3);
            AllowUIToUpdate();
        }

        private int FindButton(int row, int col)
        {
            for (int i = 0; i < 9; i++)
            {
                Button btn = (Button)this.FindName("Btn_Number" + i.ToString());

                if ((int)btn.GetValue(Grid.RowProperty) == row && (int)btn.GetValue(Grid.ColumnProperty) == col)
                {
                    return i;
                }
            }

            return 0;
        }

        void AllowUIToUpdate()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate (object parameter)
            {
                frame.Continue = false;
                return null;
            }), null);
            Dispatcher.PushFrame(frame);
        }

        private void GetMove(string move, ref int col, ref int row)
        {
            if (move == "UP")
            {
                row--;
            }
            else if (move == "DOWN")
            {
                row++;
            }
            else if (move == "LEFT")
            {
                col--;
            }
            else
            {
                col++;
            }
        }

        private void Btn_Random_Click(object sender, RoutedEventArgs e)
        {
            IList<string> path = new List<string>();
            path = game.Random();

            Button btn1 = (Button)this.FindName("Btn_Number1");


            for (int i = 0; i < path.Count; i++)
            {
                Button btn = (Button)this.FindName("Btn_Number0");
                var col = (int)btn.GetValue(Grid.ColumnProperty);
                var row = (int)btn.GetValue(Grid.RowProperty);

                int colx = col;
                int rowx = row;

                GetMove(path[i], ref col, ref row);

                int number = FindButton(row, col);

                MoveButton("Btn_Number" + number.ToString(), rowx, colx);
                MoveButton("Btn_Number0", row, col);
                Thread.Sleep(250);
            }

        }

        private void Btn_Solve_Click(object sender, RoutedEventArgs e)
        {
            IList<string> path = new List<string>();
            path = game.Solve();

            Button btn1 = (Button)this.FindName("Btn_Number1");


            for (int i = 0; i < path.Count; i++)
            {
                Button btn = (Button)this.FindName("Btn_Number0");
                var col = (int)btn.GetValue(Grid.ColumnProperty);
                var row = (int)btn.GetValue(Grid.RowProperty);

                int colx = col;
                int rowx = row;

                GetMove(path[i], ref col, ref row);

                int number = FindButton(row, col);

                MoveButton("Btn_Number" + number.ToString(), rowx, colx);
                MoveButton("Btn_Number0", row, col);
                Thread.Sleep(1000);
            }
        }
    }
}
