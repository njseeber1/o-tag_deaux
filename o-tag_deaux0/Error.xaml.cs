using System.Windows;

namespace o_tag_deaux0
{
    /// <summary>
    /// Interaction logic for Error.xaml
    /// </summary>
    public partial class Error : Window
    {
        Window back = null;
        public Error(string msg, Window window, bool success)
        {
            InitializeComponent();
            if(success)
            {
                text_box.Content = "Success";
                color_box.Fill = System.Windows.Media.Brushes.Green;
                var timer = new System.Windows.Threading.DispatcherTimer { Interval = System.TimeSpan.FromSeconds(1) };
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    Visibility = Visibility.Hidden;
                    window.Show();
                };
                

            } else
            {
                text_box.Content = "ERROR";
                color_box.Fill = System.Windows.Media.Brushes.Red;
            }
            error_msg.Content = msg;
            back = window;
        }

        private void btn_continue_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            back.Show();
        }


    }
}
