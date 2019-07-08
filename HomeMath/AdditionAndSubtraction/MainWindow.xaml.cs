using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdditionAndSubtraction
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly static SolidColorBrush GREEN = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff002300"));
       // private readonly static SolidColorBrush GRAY = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffc5c5c5"));
        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private Point currentPoint;
        //private SolidColorBrush color;

        private List<string> problemList;
        private Random random;
        private string level_cbx_text;

        private int canvasChidren;
        private long time;
        private int answer;
        private bool attempted;


        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            currentPoint = new Point();
            //color = GRAY;
            canvasChidren = canvas.Children.Count;
            canvas.Children.RemoveRange(canvasChidren, canvas.Children.Count);
            problemList = new List<string>();
            random = new Random();
            time = 0;
            answer = 0;
        }
        private void exitClick(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            
            if (btn_start.IsEnabled)
            {
                startClick(null, null);
            }
            System.Windows.Application.Current.Shutdown();
        }

        private void helpClick(object sender, RoutedEventArgs e)
        {
            attempted = true;
            System.Media.SystemSounds.Beep.Play();
            answer_usr.Text = "" + answer;
            wrongs_count.Content = Int32.Parse(wrongs_count.Content.ToString()) + 1;
            
            if (problemList.Count > 0)
            {
                string str = problemList.ElementAt(problemList.Count - 1) + "*";
                problemList.RemoveAt(problemList.Count - 1);
                problemList.Add(str);
            }

            help_btn.IsEnabled = false;
            answer_usr.Focus();

        }

        private void startClick(object sender, RoutedEventArgs e)
        {
            if (btn_start.Content.Equals("START"))
            {
                dispatcherTimer.Start();
                btn_start.Content = "STOP";
                level_cbx.IsEnabled = false;
                answer_usr.IsEnabled = true;

                addCheck.IsEnabled = false;
                subCheck.IsEnabled = false;

                display_prob.Content = createPloblem(level_cbx.Text);
                canvas.Children.RemoveRange(canvasChidren, canvas.Children.Count);
                clearClick(null, null);
                answer_usr.Focus();
            }
            else
            {
                dispatcherTimer.Stop();
                answer_usr.IsEnabled = false;

                if (!counter.Content.Equals("0"))
                {
                    String date = DateTime.Now.ToString("yyyy MM dd tt hh mm ss", 
                        System.Globalization.CultureInfo.InvariantCulture);

                    using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(date + ".txt"))
                    {
                        writetext.WriteLine("*DATA*");
                        writetext.WriteLine("DATE:         " + date);
                        writetext.WriteLine("LEVEL:        " + level_cbx.Text);
                        writetext.WriteLine("Addition:     " + addCheck.IsChecked);
                        writetext.WriteLine("Subtraction:  " + subCheck.IsChecked);
                        writetext.WriteLine("TOTAL:        " + counter.Content);
                        writetext.WriteLine("TIME:         " + timer.Content);
                        writetext.WriteLine("#CORRECTS:    " + corrects_count.Content);
                        writetext.WriteLine("#WRONGS:      " + wrongs_count.Content);
                        writetext.WriteLine(new String('*', 29));
                        for (int i = 0; i < problemList.Count - 1; i++)
                        {
                            writetext.WriteLine(problemList.ElementAt(i));
                        }
                        
                    }
                }
                btn_start.IsEnabled = false;
                //TEST
                //btn_start.Content = "START";
                //level_cbx.IsEnabled = true;
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            time++;
            timer.Content = String.Format("{0:00}:{1:00}:{2:00}", (time / 3600), (time / 60) % 60 , (time % 60));
        }

        private void answer_usr_TextChanged(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (Int32.TryParse(answer_usr.Text, out int asrChck))
                {
                    if (answer_usr.Text.Trim().Equals(answer + ""))
                    {
                        if (help_btn.IsEnabled)
                        {
                            System.Media.SystemSounds.Beep.Play();
                        }

                        display_prob.Content = createPloblem(level_cbx.Text);
                        counter.Content = Int32.Parse(counter.Content.ToString()) + 1;
                        corrects_count.Content = Int32.Parse(corrects_count.Content.ToString()) + (attempted ? 0 : 1);
                        attempted = false;
                        answer_usr.Text = "";
                        help_btn.IsEnabled = true;
                        clearClick(null, null);

                    }
                    else
                    {
                        System.Media.SystemSounds.Hand.Play();
                        wrongs_count.Content = Int32.Parse(wrongs_count.Content.ToString()) + 1;
                        attempted = true;

                        if (problemList.Count > 0)
                        {
                            string str = problemList.ElementAt(problemList.Count - 1) + "*";
                            problemList.RemoveAt(problemList.Count - 1);
                            problemList.Add(str);
                        }
                    }
                }
                else
                {
                    System.Media.SystemSounds.Hand.Play();
                    MessageBox.Show("Please enter real numbers!!!!!");
                }
            }
        }

        public string createPloblem(string level)
        {
            Int32.TryParse(level, out int lev);
            
            Int32.TryParse(new String('9', (lev % 7) + (lev >= 7 ? 1 : 0)), out int max);
            int min = (max / 9 == 1 ? 0 : max / 9);

            char opp = (bool) addCheck.IsChecked ? '+' : ' ';
            opp = opp == ' ' && (bool) subCheck.IsChecked ? '-' : opp;
            opp = (bool) addCheck.IsChecked && (bool)subCheck.IsChecked ? ((random.Next(0, 2) == 0) ? '+' : '-') : opp;

            int num1 = random.Next(min, max + 1);
            int num2 = random.Next(min, max + 1);

            if (lev < 7 && num1 < num2)
            {
                int temp = num1;
                num1 = num2;
                num2 = temp;
            }

            answer = sum(num1, opp, num2);
            problemList.Add(String.Format("{0,15} = {1,-10}", num1 + " " + opp + " " + num2, answer));


            return num1 + " " + opp + " " + num2 + " =";
        }

        private int sum(int num1, char opp, int num2)
        {
            return opp == '+' ? (num1 + num2) : (num1 - num2);
        }



        private void clearClick(object sender, RoutedEventArgs e)
        {
            answer_usr.Focus();
            canvas.Children.RemoveRange(canvasChidren, canvas.Children.Count);
        }

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(this);
        }

        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();
                //line.Stroke = new SolidColorBrush(Colors.SeaGreen);
                line.StrokeThickness = 5;

                line.Stroke = SystemColors.WindowFrameBrush;
                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y;

                currentPoint = e.GetPosition(this);

                canvas.Children.Add(line);
            }
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(level_cbx.Text, out int lev);

            if (!(bool) addCheck.IsChecked && !(bool) subCheck.IsChecked)
            {
                ((CheckBox)sender).IsChecked = true;
            }

            if (lev >= 7 && (!(bool) addCheck.IsChecked || !(bool) subCheck.IsChecked))
            {
                MessageBox.Show("Level " + lev +
                    " is available on addition subtraction checked only!!!\n" +
                    " " + (lev % 7 + 1) + " Matches your selection." );
                level_cbx.Text = level_cbx_text = "" + (lev % 7 + 1);
            }

        }

        private void level_cbx_DropDownOpened(object sender, EventArgs e)
        {
            level_cbx_text = level_cbx.Text;
            canvas.Children.RemoveRange(canvasChidren, canvas.Children.Count);
        }

        private void level_cbx_DropDownClosed(object sender, EventArgs e)
        {
            Int32.TryParse(level_cbx.Text, out int lev);

            if (lev >= 7 && (!(bool) addCheck.IsChecked || !(bool) subCheck.IsChecked))
            {
                MessageBox.Show("Level " + lev + " is available on addition subtraction checked only!!!");
                level_cbx.Text = level_cbx_text;
            }
            level_cbx_text = level_cbx.Text;
            answer_usr.Focus();
        }
    }
}
