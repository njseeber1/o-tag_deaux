using System;
using System.Windows;
using System.Windows.Controls;


namespace o_tag_deaux0
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        Reservation reservation = null;
        //Family family = null;
        MainWindow main = null;
        private int namebox_width = 180;

        public Start(Reservation res, MainWindow main)
        {
            InitializeComponent();
            reservation = res;
            //family = DAL.CreateFamily(res);
            this.main = main;
            this.Title = this.Title + " - Confirmation # " + res.confirmationid;
           
             if (res.result == "FAIL")
            {
                Error error = new Error(res.msg, this, false);
                reservationid.Clear();
                Visibility = Visibility.Hidden;
                error.Show();
            }
            else
            {
                reservationid.Text = res.roomnum;
                label_guest_count.Content = res.paxcount.ToString();
                label_primary.Content = res.pax[0].name;
                count_bands.Text = res.paxcount.ToString();
                cal_from.SelectedDate = res.dateIndate;
                cal_from.DisplayDate = res.dateIndate;
                cal_to.SelectedDate = res.dateOutdate;
                cal_to.DisplayDate = res.dateOutdate;
                label_indate.Content = res.dateIndate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                label_outdate.Content = res.dateOutdate.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                cal_from.DisplayDateStart = res.dateIndate;
                cal_to.DisplayDateStart = res.dateIndate;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;

                for (var i = 0; i < res.pax.Count; i++)
                {
                    TextBox t = new TextBox();

                    t.Text = res.pax[i].name;
                    t.Width = namebox_width;
                    t.Margin = new Thickness(0, 10, 0, 10);
                    t.HorizontalAlignment = HorizontalAlignment.Left;
                    scroller_panel.Children.Add(t);
                }

                //unnamed guests
                for (var i = 0; i < res.paxcount - res.pax.Count; i++)
                {
                    TextBox t = new TextBox();

                    t.Text = "Guest";
                    t.Width = namebox_width;
                    t.Margin = new Thickness(0, 10, 0, 10);
                    t.HorizontalAlignment = HorizontalAlignment.Left;
                    scroller_panel.Children.Add(t);
                }

                var tv = (cal_to.SelectedDate - cal_from.SelectedDate).ToString();
                nights.Text = tv.Substring(0, tv.IndexOf('.'));
            }

        }



        private void on_fromChange(object sender, RoutedEventArgs e)
        {
            cal_to.DisplayDateStart = cal_from.SelectedDate;
        }


        private void cal_ValueChanged(object sender, RoutedEventArgs e)
        {

            var date_diff = (cal_to.SelectedDate - cal_from.SelectedDate).ToString();
            int delta = 0;
            try
            {
                delta = Int32.Parse(date_diff.Substring(0, date_diff.IndexOf('.')));
                if (delta < 0)
                {
                    //throw some error
                    delta = delta * -1;
                }
                else if (delta == 0)
                {
                    //dates are the same...do we care?
                }

            }
            catch (Exception)
            {
                delta = 0;
            }
            nights.Text = delta.ToString();
        }

        private void btn_activate_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < scroller_panel.Children.Count-1; i++)
            {
                TextBox t = scroller_panel.Children[i] as TextBox;
                reservation.pax[i].name = t.Text;
            }
            GenericResponse response = DAL.CreateHotelReservation(reservation);
            ScanBand scan = new ScanBand(reservation, main);

            Visibility = Visibility.Hidden;
            scan.Show();
        }

        private void btn_abort_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            reservationid.Clear();
            main.Show();
        }

        private void checkbox_clear_keys_Checked(object sender, RoutedEventArgs e)
        {
            //clear scanned keys
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DAL.ClearRoomKeys(reservation.confirmationid);
        }
    }
}
