using Windows.Devices.Gpio;

namespace Zlingo
{

    class PinsInit
    {
        private const int enablePin = 16;
        private const int RWPin = 5;
        private const int registerSelectPin = 20;
        private const int dataPin07 = 6;
        private const int dataPin08 = 13;
        private const int dataPin09 = 19;
        private const int dataPin10 = 26;
        private const int dataPin11 = 22;
        private const int dataPin12 = 27;
        private const int dataPin13 = 4;
        private const int dataPin14 = 17;

        private GpioController gpio;

        private const int ButtonPin1 = 24;
        public const int ButtonPin2 = 25;

        public GpioPin buttonpin1;
        public GpioPin buttonpin2;

        public LCD lcd = new LCD();

        public PinsInit()
        {
            lcd.Init(registerSelectPin, RWPin, enablePin, dataPin14, dataPin13, dataPin12, dataPin11, dataPin10, dataPin09, dataPin08, dataPin07);
            InitGPIO(out buttonpin1, ButtonPin1);
            InitGPIO(out buttonpin2, ButtonPin2);
        }
        public void InitGPIO(out GpioPin pinnetje, int nr)
        {
            gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                pinnetje = null;
                return;
            }

            pinnetje = gpio.OpenPin(nr);
            pinnetje.Write(GpioPinValue.Low);
            
            pinnetje.SetDriveMode(GpioPinDriveMode.InputPullUp);

        }
    }
}
