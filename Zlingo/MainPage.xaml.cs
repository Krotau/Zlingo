using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Zlingo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        PinsInit pin = new PinsInit();
        SocketServer server;
        public MainPage()
        {
            this.InitializeComponent();
            server = new SocketServer(9000, pin);
            pin.buttonpin1.ValueChanged += Buttonpin_ValueChanged;
            pin.buttonpin2.ValueChanged += Buttonpin_ValueChanged;
            pin.buttonpin1.DebounceTimeout = new System.TimeSpan(0, 0, 0, 0, 5);
            pin.buttonpin2.DebounceTimeout = new System.TimeSpan(0, 0, 0, 0, 5);
            server.OnDataRecived += server.Server_OnDataRecived;
        }


        private void Buttonpin_ValueChanged(Windows.Devices.Gpio.GpioPin sender, Windows.Devices.Gpio.GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == Windows.Devices.Gpio.GpioPinEdge.FallingEdge)
            {
                if (sender == pin.buttonpin1)
                {
                    if (server.Begonnen == 0) // Begin een spel tussen spelers
                    {
                        pin.lcd.ClearDisplay();
                        Task.Delay(5).Wait();
                        server.StartSpel();
                        server.Begonnen += 1;
                    }

                    else if (server.Begonnen == 1 & server.controleModus == 1) // Controleert bestaand woord
                    {
                        server.CommunicationOrders();
                    }
                }
                
                else if (sender == pin.buttonpin2 & server.Begonnen == 0) // Begin een testrun
                {
                    server.Testmodus = 1;
                    pin.lcd.ClearDisplay();
                    Task.Delay(5).Wait();
                    server.Begonnen += 1;
                    server.StartSpel();
                }
                else if (sender == pin.buttonpin2 & server.Begonnen == 1 & server.controleModus == 1) // Keurt niet bestaand woord af
                {
                    server.nieuwspel.AantalFoutief += 1;
                    server.NietBestaandAntwoord();
                }
            }
        }
        
    }
    
}
