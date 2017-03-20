using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IoT.Devices;
using Microsoft.IoT.DeviceCore.Pwm;
using Microsoft.IoT.Devices.Pwm;
using Windows.Devices.Gpio;
using Microsoft.IoT.Devices.Lights;
using Windows.System;

namespace EduBot
{
    class RobotControl
    {
        //Objekt för GPIO- och PWM-kontroller
        private GpioController gpioController;
        private PwmProviderManager pwmManager;
        private IReadOnlyList<Windows.Devices.Pwm.PwmController> pwmControllers;
        private Windows.Devices.Pwm.PwmController pwmController;

        //PwmPin-objekt för vänster respektive höger servo
        private Windows.Devices.Pwm.PwmPin servoLeft; 
        private Windows.Devices.Pwm.PwmPin servoRight;

        //Pin-nummer för vänster respektive höger servo på PWM-kontrollern
        private int servoLeftPin = 0;
        private int servoRightPin = 1;

        //Frekvens för PWM-kontrollern
        private int pwmFrequency = 100;

        //Pin-nummer för knapparnas LED-lampor på PWM-kontrollern
        private int[] LEDPins = { 3, 4, 5, 6, 7 };
        //PwmPin-objekt för LED-lamporna
        private Windows.Devices.Pwm.PwmPin[] LEDs = new Windows.Devices.Pwm.PwmPin[5];

        //Pin-nummer för knapparna (GPIO på Raspberry Pi)
        private int[] buttonPins = { 23, 24, 17, 27, 22 };
        //GpioPin-objekt för knapparna
        private GpioPin[] buttons = new GpioPin[5];
        //Kommandosträngar för knapparna
        private string[] buttonCommands =
        {
            "100,100,1000",     //Frammåt
            "-100,-100,1000",   //Bakåt
            "-100,100,700",     //Vänster
            "100,-100,700",     //Höger
        };

        //Anger ifall programmeringsläge är aktivt
        private bool programingmode = false;
        //Sträng som lagrar kommandon som anges under programmeringsläget
        private string programingrequest = "";

        //Anger ifall roboten har påbörjat avstängning
        private bool shutdown = false;

        //Lista som lagrar logg-meddelanden
        private List<string> log = new List<string>();

        //Event för när knappen har tryckts ned och kommandona utförts

        public RobotControl()
        {
            initiate();
        }

        /// <summary>
        /// Initierar robotkontrollern
        /// </summary>
        async void initiate()
        {
            try
            {
                // Startar GPIO
                gpioController = GpioController.GetDefault();

                //Initierar PWM-kontrollerkortet
                pwmManager = new PwmProviderManager();
                pwmManager.Providers.Add(new PCA9685());
                pwmControllers = await pwmManager.GetControllersAsync();
                pwmController = pwmControllers[0];
                pwmController.SetDesiredFrequency(pwmFrequency);

                //Initierar servon
                servoLeft = pwmController.OpenPin(servoLeftPin);
                servoRight = pwmController.OpenPin(servoRightPin);

                //Initierar knapparna samt dess LED-lmapor
                for (int i = 0; i < 5; i++)
                {
                    LEDs[i] = pwmController.OpenPin(LEDPins[i]);

                    buttons[i] = gpioController.OpenPin(buttonPins[i]);
                    buttons[i].SetDriveMode(GpioPinDriveMode.InputPullDown);
                    buttons[i].DebounceTimeout = TimeSpan.FromMilliseconds(50);
                }

                buttons[0].ValueChanged += forwardButton_ValueChanged;
                buttons[1].ValueChanged += backButton_ValueChanged;
                buttons[2].ValueChanged += leftButton_ValueChanged;
                buttons[3].ValueChanged += rightButton_ValueChanged;
                buttons[4].ValueChanged += stopButton_ValueChanged;
                
                //Tänder de gröna lamporna
                setGreenLEDs(100);
            }
            catch
            {
                Log("Error while initiating!");
            }

        }

        /// <summary>
        /// Utför rörelse-kommando
        /// </summary>
        /// <param name="leftPercentage"></param>
        /// <param name="rightPercentage"></param>
        /// <param name="timeInMilliseconds"></param>
        public void move(int leftPercentage, int rightPercentage, int timeInMilliseconds)
        {
            try
            {
                //Beräkna och ställ in procentsats för PWM-kontrollern
                double dutyCyclePercentageLeft = (15 + (8 * (double)leftPercentage / 100)) / 100;
                double dutyCyclePercentageRight = (15 + (8 * (double)-rightPercentage / 100)) / 100;
                
                servoLeft.SetActiveDutyCyclePercentage(dutyCyclePercentageLeft);
                servoRight.SetActiveDutyCyclePercentage(dutyCyclePercentageRight);

                //Starta servona
                servoLeft.Start();
                servoRight.Start();

                //Vänta så länge som servona ska vara igång
                Task.Delay(timeInMilliseconds).Wait();

                //Stoppa servona
                servoLeft.Stop();
                servoRight.Stop();
            }
            catch
            {
                //Loggar och stoppar servon
                Log("Error while executing move command!");
                if (servoLeft.IsStarted) servoLeft.Stop();
                if (servoRight.IsStarted) servoRight.Stop();
            }
            
        }

        /// <summary>
        /// Utför rörelse-kommando från fält med 3 heltal
        /// </summary>
        /// <param name="validatedCommand"></param>
        public void move(int[] validatedCommand)
        {
            if (validatedCommand.Length == 3)
            {
                move(validatedCommand[0], validatedCommand[1], validatedCommand[2]);
            }
            else Log("Move command not valid!");
        }

        /// <summary>
        /// Utför flera kommandon från sträng
        /// </summary>
        /// <param name="request"></param>
        public void Run(string request)
        {
            setGreenLEDs(0);
            string[] commandStrings = request.Split(';');

            for (int i = 0; i < commandStrings.Length; i++)
            {
                int[] validatedCommand = validateCommand(commandStrings[i]);
                if (validatedCommand != null)
                {
                    if (validatedCommand.Length == 3)
                    {
                        move(validatedCommand);
                    }
                    else if (validatedCommand.Length == 2)
                    {
                        setLED(validatedCommand);
                    }
                    Log("Command \"" + commandStrings[i] + "\" executed successfully");
                }
                else Log("Move command not valid!");
            }
            setGreenLEDs(100);
        }

        /// <summary>
        /// Validerar sträng med kommando och retturnerar det som ett fält.
        /// Returnerar null om kommandot inte är giltigt.
        /// </summary>
        /// <param name="commandToValidate"></param>
        /// <returns></returns>
        int[] validateCommand(string commandToValidate)
        {
            string[] commandValueStrings = commandToValidate.Split(',');
            if (commandValueStrings[0] == "led" && commandValueStrings.Length == 3)
            {
                int[] validatedCommand = { 0, 0 };
                if (int.TryParse(commandValueStrings[1], out validatedCommand[0]) &&
                    int.TryParse(commandValueStrings[2], out validatedCommand[1]) &&
                    validatedCommand[0] >= 0 && validatedCommand[0] <= 4 &&
                    validatedCommand[1] >= 0 && validatedCommand[1] <= 100)
                {
                    return validatedCommand;
                }
            }
            else if (commandValueStrings.Length == 3)
            {
                int[] validatedCommand = { 0, 0, 0 };
                if (int.TryParse(commandValueStrings[0], out validatedCommand[0]) &&
                    int.TryParse(commandValueStrings[1], out validatedCommand[1]) &&
                    int.TryParse(commandValueStrings[2], out validatedCommand[2]) &&
                    validatedCommand[0] >= -100 && validatedCommand[0] <= 100 &&
                    validatedCommand[1] >= -100 && validatedCommand[1] <= 100 &&
                    validatedCommand[2] >= 0)
                {
                    return validatedCommand;
                }
            }
            else Log("Command could not be validated!");
            return null;
        }

        /// <summary>
        /// Ställer in ljusstyrka för LED
        /// </summary>
        /// <param name="LEDIndex"></param>
        /// <param name="percentage"></param>
        public void setLED(int LEDIndex, int percentage)
        {
            try
            {
                double dutyCyclePercentage = (double)percentage / 100;
                if (percentage > 0)
                {
                    LEDs[LEDIndex].SetActiveDutyCyclePercentage(dutyCyclePercentage);
                    if (!LEDs[LEDIndex].IsStarted) LEDs[LEDIndex].Start();
                }
                else
                {
                    if (LEDs[LEDIndex].IsStarted) LEDs[LEDIndex].Stop();
                }
            }
            catch
            {
                Log("Error while executing LED command!");
            }
            
        }

        /// <summary>
        /// Ställer in ljusstyrka för LED från fält med 2 heltal.
        /// </summary>
        /// <param name="validatedCommand"></param>
        public void setLED(int[] validatedCommand)
        {
            if (validatedCommand.Length == 2)
            {
                setLED(validatedCommand[0], validatedCommand[1]);
            }
            else Log("Move command not valid!");
        }


        private void forwardButton_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            if (e.Edge == GpioPinEdge.RisingEdge)
            {
                buttonPress(0);
            }
        }

        private void backButton_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            if (e.Edge == GpioPinEdge.RisingEdge)
            {
                buttonPress(1);
            }
        }

        private void leftButton_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            if (e.Edge == GpioPinEdge.RisingEdge)
            {
                buttonPress(2);
            }
        }

        private void rightButton_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            if (e.Edge == GpioPinEdge.RisingEdge)
            {
                buttonPress(3);
            }
        }

        /// <summary>
        /// Utför kommando för knapp med det index som anges.
        /// Om programmeringsläge är aktivt läggs kommandot till programingrequest.
        /// </summary>
        /// <param name="buttonIndex"></param>
        private void buttonPress(int buttonIndex)
        {
            Log("Button with index " + buttonIndex + " was pressed.");
            setLED(buttonIndex, 0);
            if (programingmode)
            {
                if (programingrequest != "") programingrequest += ";";
                programingrequest += buttonCommands[buttonIndex];
            }
            else
            {
                Run(buttonCommands[buttonIndex]);
            }
            Task.Delay(500).Wait();
            setLED(buttonIndex, 100);
        }

        private void stopButton_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            if (e.Edge == GpioPinEdge.RisingEdge)
            {
                //Stänger av programmeringsläge om det är aktivt
                if (programingmode)
                {
                    programingmode = false;                    
                    setLED(4, 0);
                    Log("Exited programmingmode.");
                }

                //Utför lagrad kommandosträng om den inte är tom
                else if (programingrequest != "")
                {
                    Log("Executing programmed command.");
                    Run(programingrequest);
                    programingrequest = "";
                    setLED(4, 100);
                }

                //Avbryter avstängning ifall sådan påbörjats
                else if (shutdown)
                {
                    shutdown = false;
                    Log("Aborting shutdown.");
                }

                else
                {
                    //Kollar om knappen hålls in längre än en sekund
                    bool longPress = true;
                    setLED(4, 0);

                    for (int i = 0; i < 10; i++)
                    {
                        Task.Delay(100).Wait();
                        if (sender.Read() == GpioPinValue.Low)
                        {
                            longPress = false;
                            break;
                        }
                    }

                    //Påbörjar avstängning om knappen hållts ned längre än en sekund
                    if (longPress)
                    {
                        beginShutdown();
                    }

                    //Startar annars programmeringsläge
                    else
                    {
                        Log("Entering programmingmode.");
                        programingmode = true;
                        pulseRedLEDWhileProgrammingmode();
                    }
                }
            }
        }

        /// <summary>
        /// Ställer in ljusstyrka för samtliga gröna LED-lampor
        /// </summary>
        /// <param name="percentage"></param>
        void setGreenLEDs(int percentage)
        {
            for (int i = 0; i < 5; i++)
            {
                setLED(i, percentage);
            }
        }

        /// <summary>
        /// Pulserar röd LED så länge som programmeringsläget är aktivt
        /// </summary>
        async void pulseRedLEDWhileProgrammingmode()
        {
            while (programingmode)
            {
                for (int i = 100; i >= 0; i -= 4)
                {
                    setLED(4, i);
                    await Task.Delay(10);
                }
                for (int i = 0; i <= 100; i += 4)
                {
                    setLED(4, i);
                    await Task.Delay(10);
                }
            }
            setLED(4, 0);
        }

        /// <summary>
        /// Påbörjar avstängning
        /// </summary>
        async void beginShutdown()
        {
            Log("Beginning shutdown.");
            shutdown = true;
            //Blinkar röda lampan 8 gånger
            for (int i = 0; i < 8; i++)
            {
                setLED(4, 100);
                await Task.Delay(500);
                if (!shutdown) break;
                setLED(4, 0);
                await Task.Delay(500);
                if (!shutdown) break;
            }

            //Stänger av om avstängning inte avbrutits
            if (shutdown)
            {
                setGreenLEDs(0);
                setLED(4, 0);
                new System.Threading.Tasks.Task(() =>
                {
                    ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.FromSeconds(0));
                }).Start();
            }
            else setLED(4, 100);
        }

        /// <summary>
        /// Loggar meddelande
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            log.Add(DateTime.Now.ToString("h:mm:ss tt") + ":" + message);
        }

        /// <summary>
        /// Returnerar logg
        /// </summary>
        /// <returns></returns>
        public string[] GetLog()
        {
            return log.ToArray();
        }
    }
}
