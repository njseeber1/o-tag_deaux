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
using System.Windows.Shapes;

namespace o_tag_deaux0
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private double x = 0;
        private double y = 0;
        private string serialNumber = string.Empty;
        public Login()
        {
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            InitializeComponent();
            Loaded += new RoutedEventHandler(_loaded);
        }

        private void _loaded(object sender, RoutedEventArgs e)
        {

            {
                var workingArea = SystemParameters.WorkArea;
                Left = workingArea.Right - Width;
                Top = workingArea.Bottom - Height;
            }

        }

        private void GoFail(string msg)
        {
            Error error = new Error(msg, this, false);
            Visibility = Visibility.Hidden;
            error.Show();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                if(serialNumber != string.Empty)
                {
                    User user = DAL.LogIn(serialNum: serialNumber);
                    serialNumber = String.Empty;
                    if (user.result == "FAIL")
                    {
                        GoFail(user.msg);
                    }
                    else if (user.isActive == "N")
                    {
                        GoFail(user.msg);
                    }
                    else //logged in 
                    {
                        MainWindow main = new MainWindow(user,Left,Top);
                        Visibility = Visibility.Hidden;
                        main.Show();
                    }
                }
            }

            else if (e.Key != Key.Capital)
            {
                string val = e.Key.ToString();
                if (val.Length == 2)
                {
                    val = val.Substring(1);
                }
                serialNumber += val;
            }
        }

        private void _moved(object sender, DragEventArgs e)
        {
            x = e.GetPosition(this).X;
            y = e.GetPosition(this).Y;
        }
    }
}
