using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;
using RobotKit;
using System.ComponentModel;
namespace SpheroBand
{
    class SpheroManager:INotifyPropertyChanged
    {
        public Sphero m_robot = null;
        public string SpheroName { get; set; }
        private string spheroName = "";
      
        private bool spheroConnected=false;


        public bool SpheroConnected
        {
            get { return spheroConnected; }
            set
            {
                spheroConnected = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SpheroConnected"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public SpheroManager()
        {
            //SetupRobotConnection();
        }



        private const string kNoSpheroConnected = "No Sphero Connected";

        //! @brief  the default string to show when connecting to a sphero ({0})
        private const string kConnectingToSphero = "Connecting to {0}";

        //! @brief  the default string to show when connected to a sphero ({0})
        private const string kSpheroConnected = "Connected to {0}";


        //! @brief  search for a robot to connect to
        public void SetupRobotConnection()
        {
            SpheroName ="No Sphero Connected";

            RobotProvider provider = RobotProvider.GetSharedProvider();
            provider.DiscoveredRobotEvent += OnRobotDiscovered;
            provider.NoRobotsEvent += OnNoRobotsEvent;
            provider.ConnectedRobotEvent += OnRobotConnected;
            provider.FindRobots();
        }
       public void OnRobotDiscovered(object sender, Robot robot)
        {
            //Debug.WriteLine(string.Format("Discovered \"{0}\"", robot.BluetoothName));

            if (m_robot == null)
            {
                RobotProvider provider = RobotProvider.GetSharedProvider();
                provider.ConnectRobot(robot);
              //  ConnectionToggle.OnContent = "Connecting...";
                m_robot = (Sphero)robot;
                SpheroConnected = true;
                SpheroName= string.Format(kConnectingToSphero, robot.BluetoothName);
            }
        }




        private void OnNoRobotsEvent(object sender, EventArgs e)
        {
            MessageDialog dialog = new MessageDialog("No Sphero Paired");
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            dialog.ShowAsync();
        }


        //! @brief  when a robot is connected, get ready to drive!
        private void OnRobotConnected(object sender, Robot robot)
        {
            //Debug.WriteLine(string.Format("Connected to {0}", robot));
            //ConnectionToggle.IsOn = true;
           // ConnectionToggle.OnContent = "Connected";
            SpheroConnected = true;
            m_robot.SetRGBLED(255, 255, 255);
            SpheroName = string.Format(kSpheroConnected, robot.BluetoothName);
            

            //m_robot.SensorControl.Hz = 10;
            //m_robot.SensorControl.AccelerometerUpdatedEvent += OnAccelerometerUpdated;
            //m_robot.SensorControl.GyrometerUpdatedEvent += OnGyrometerUpdated;

            //m_robot.CollisionControl.StartDetectionForWallCollisions();
            //m_robot.CollisionControl.CollisionDetectedEvent += OnCollisionDetected;
        }
        public void ShutdownRobotConnection()
        {
            if (m_robot != null)
            {
                m_robot.SensorControl.StopAll();
                m_robot.Sleep();
                // temporary while I work on Disconnect.
                //m_robot.Disconnect();
                //ConnectionToggle.OffContent = "Disconnected";
                SpheroConnected = false;
                SpheroName = kNoSpheroConnected;

                //m_robot.SensorControl.AccelerometerUpdatedEvent -= OnAccelerometerUpdated;
                //m_robot.SensorControl.GyrometerUpdatedEvent -= OnGyrometerUpdated;

                //m_robot.CollisionControl.StopDetection();
                //m_robot.CollisionControl.CollisionDetectedEvent -= OnCollisionDetected;

                RobotProvider provider = RobotProvider.GetSharedProvider();
                provider.DiscoveredRobotEvent -= OnRobotDiscovered;
                provider.NoRobotsEvent -= OnNoRobotsEvent;
                provider.ConnectedRobotEvent -= OnRobotConnected;
            }
        }
    }
}
