using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpheroBand
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        BandUI band;
        SpheroManager sp;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            //band = new BandUI();
            //txtStatus.DataContext = band;
            sp = new SpheroManager();

        }
        bool run = false;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (!run)
            //{
            //    band.StartReading();
            //    btnRun.Content = "Stop";
            //}

            //else
            //{
            //    band.StopReading();
            //    btnRun.Content = "Start";
            //}


            //run = !run;
            sp.m_robot.Roll(0, 1);
            await System.Threading.Tasks.Task.Delay(100);
        }

        private void ConnectionToggle_Toggled(object sender, RoutedEventArgs e)
        {

            ConnectionToggle.OnContent = "Connecting...";
            if (ConnectionToggle.IsOn)
            {
                if (sp.m_robot == null)
                {
                    sp.SetupRobotConnection();
                }
            }
            else
            {
                sp.ShutdownRobotConnection();
            }
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
    }
}
