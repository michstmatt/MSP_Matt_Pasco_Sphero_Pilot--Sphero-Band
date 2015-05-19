using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Band;
using System.ComponentModel;
namespace SpheroBand
{
    class BandUI : INotifyPropertyChanged
    {
        private string statusMessage = "Pair a Microsoft Band with your device and click Run.";
        public string StatusMessage
        {
            get { return statusMessage; }
            set
            {
                statusMessage = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StatusMessage"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private IBandClient bandClient;
        public SpheroManager sp;
        public BandUI()
        {
            connect();
        }
        private async void connect()
        {
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                if (pairedBands.Length < 1)
                {
                   StatusMessage = "This sample app requires a Microsoft Band paired to your device. Also make sure that you have the latest firmware installed on your Band, as provided by the latest Microsoft Health app.";
                    return;
                }

                // Connect to Microsoft Band.
               bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]);
           
               StatusMessage = "Band Connected ";
            }
            catch { StatusMessage = "Error connecting to band"; }
            }
          
        public async void StartReading()
        {
        
            
            StatusMessage = "Reading Started...";
            if (bandClient.SensorManager.Gyroscope.GetCurrentUserConsent() != UserConsent.Granted)
                await bandClient.SensorManager.Gyroscope.RequestUserConsentAsync();

            bandClient.SensorManager.Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
           await bandClient.SensorManager.Gyroscope.StartReadingsAsync();
        }
        double speed = 0;
        double angle = 0;
        async void Gyroscope_ReadingChanged(object sender, Microsoft.Band.Sensors.BandSensorReadingEventArgs<Microsoft.Band.Sensors.IBandGyroscopeReading> e)
        {
            speed += e.SensorReading.AccelerationZ / 100;
            angle += e.SensorReading.AccelerationX / 100;
            sp.m_robot.Roll((int)angle, (float)10);
            await Task.Delay(100);
        }

         public async void StopReading()
        {
            StatusMessage = "Reading Stopped";
            await bandClient.SensorManager.Gyroscope.StopReadingsAsync();
        }

     
    }
}
