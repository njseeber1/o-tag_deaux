using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace o_tag_deaux0
{
    /// <summary>
    /// Interaction logic for ScanBand.xaml
    /// </summary>
    public partial class ScanBand : Window
    {
        private string nfcid = String.Empty;
        private int count = 1;
        private Reservation reservation;
        private MainWindow main;
        private DispatcherTimer timer;
        public ScanBand(Reservation res, MainWindow main)
        {
            InitializeComponent();
            this.Title = this.Title + " - Confirmation # " + res.confirmationid;
            reservationid.Text = res.roomnum;
            label_guest_count.Content = res.pax.Count;
            label_primary.Content = res.pax[0].name;
            count_bands.Text = res.paxcount.ToString();
            label_indate.Content = res.indate;
            label_outdate.Content = res.outdate;
            string s = (res.dateOutdate - res.dateIndate).ToString();
            nights.Text = s.Substring(0, s.IndexOf('.'));
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            reservation = res;
            this.main = main;
            text_name.Visibility = Visibility.Hidden;
        }

        private void setTimer()
        {
            timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Tick += new EventHandler(onTimer);
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer.Start();
        }

        private void onTimer(Object source, EventArgs ea)
        {
            text_name.Visibility = Visibility.Hidden;
        }

        private void GoFail(string msg, bool success)
        {
            nfcid = String.Empty;
            Visibility = Visibility.Hidden;
            Window window = this;
            Error error = new Error(msg + " " + nfcid, this, success);
            error.Show();
        }

      

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {

                //call endpoint
                GenericResponse response = DAL.ActivateKey(
                    mediacode: nfcid,
                    confirmationNumber: reservation.confirmationid
                    );
                nfcid = String.Empty;
                if (response.result == "FAIL")
                {
                    GoFail(response.msg, false);
                }
                else
                {
                    //we need a one second(?) timer here
                    text_name.Content = "Media was successfully scanned";
                    text_name.Visibility = Visibility.Visible;
                    setTimer();     
                    
                }

            }
            else if (e.Key != Key.Capital)
            {
                string val = e.Key.ToString();
                if (val.Length == 2)
                {
                    val = val.Substring(1);
                }
                nfcid += val;
            }
        }

        private void btn_abort_Click(object sender, RoutedEventArgs e)
        {
            main.reservationid.Clear();
            Visibility = Visibility.Hidden;
            main.Show();
        }
    }
}
