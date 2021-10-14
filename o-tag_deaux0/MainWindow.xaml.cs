
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace o_tag_deaux0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double x = 0;
        private double y = 0;
        private string serialNumber = string.Empty;

        public MainWindow(User user, double x = 0, double y = 0)
        {
            InitializeComponent();
            reservationid.Clear();
            Left = x;
            Top = y;
            Loaded += new RoutedEventHandler(_loaded);
            lbl_name.Content = user.name;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void _loaded(object sender, RoutedEventArgs e)
        {

            {
                //var workingArea = SystemParameters.WorkArea;
                //Left = workingArea.Right - Width;
                //Top = workingArea.Bottom - Height;
                reservationid.Clear();
                reservationid.Text = string.Empty;
            }

        }
       

        private void _moved(object sender, DragEventArgs e)
        {
            x = e.GetPosition(this).X;
            y = e.GetPosition(this).Y;
        }

        private void GoFail(string msg)
        {
            Error error = new Error(msg, this, false);
            Visibility = Visibility.Hidden;
            reservationid.Text = string.Empty;
            reservationid.Clear();
            error.Show();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Reservation reservation = DAL.GetReservation(reservationid.Text);
            if (reservation.result == "FAIL" || String.IsNullOrEmpty(reservation.roomnum))
            {
                GoFail(reservation.msg);
            }

            else
            {
                Start start = new Start(reservation, this);
                Visibility = Visibility.Hidden;
                start.Show();
            }

        }



        private void reservationid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (reservationid.Text.Trim().Length != 0)
            {
                btn_go.IsEnabled = true;
            }
            else
            {
                btn_go.IsEnabled = false;
            }

        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            serialNumber = string.Empty;
            lbl_username.Content = string.Empty;
            lbl_name.Content = string.Empty;
            btn_go.IsEnabled = false;
            reservationid.IsReadOnly = true;
            var workingArea = SystemParameters.WorkArea;
            Left = workingArea.Right - Width;
            Top = workingArea.Bottom - Height;
            reservationid.Clear();
            reservationid.Text = string.Empty;
            Login login = new Login();
            Visibility = Visibility.Hidden;
            login.Show();

        }
    }

}
