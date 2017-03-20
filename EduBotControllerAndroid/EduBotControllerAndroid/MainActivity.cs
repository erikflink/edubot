using Android.App;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EduBotControllerAndroid
{
    [Activity(Label = "EduBot Controller", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        
        //Objekt för knappar
        ImageButton btnUp;
        ImageButton btnDown;
        ImageButton btnLeft;
        ImageButton btnRight;
        ImageButton btnJoystick;
        Button btnSettings;
        ImageButton btnProgram;

        //Instans av EduBotClien för kommunikation med roboten
        public static EduBotClient client;

        //Vibrator
        Vibrator vibrator;

        //Anger ifall programmeringsläge är aktivt
        bool programmingmode = false;
        //Lagrar sträng som skapas i programmeringsläge
        string programmingrequest = "";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            
            //Initierar knappar
            btnUp = FindViewById<ImageButton>(Resource.Id.imageButtonUp);
            btnDown = FindViewById<ImageButton>(Resource.Id.imageButtonDown);
            btnLeft = FindViewById<ImageButton>(Resource.Id.imageButtonLeft);
            btnRight = FindViewById<ImageButton>(Resource.Id.imageButtonRight);
            btnJoystick = FindViewById<ImageButton>(Resource.Id.imageButtonJoystick);
            btnSettings = FindViewById<Button>(Resource.Id.buttonSettings);
            btnProgram = FindViewById<ImageButton>(Resource.Id.imageButtonProgram);

            //Initierar EduBotClient
            client = new EduBotClient();

            //Initierar vibrator
            vibrator = (Vibrator)GetSystemService(VibratorService);

            btnUp.Click += delegate
            {
                addCommand("100,100,1000");
            };

            btnDown.Click += delegate
            {
                addCommand("-100,-100,1000");
            };

            btnLeft.Click += delegate
            {
                addCommand("-100,100,700");
            };

            btnRight.Click += delegate
            {
                addCommand("100,-100,700");
            };

            btnJoystick.Click += delegate
            {
                StartActivity(typeof(JoystickActivity));
            };

            btnSettings.Click += delegate
            {
                StartActivity(typeof(SettingsActivity));
            };

            btnProgram.Click += delegate
            {
                if (programmingmode)
                {
                    //Stoppar programmeringsläge om det är aktivt
                    programmingmode = false;
                    if (programmingrequest != "")
                    {
                        btnProgram.SetImageResource(Resource.Drawable.play);
                    }
                    else
                    {
                        btnProgram.SetImageResource(Resource.Drawable.record);
                    }
                }
                else if (programmingrequest != "")
                {
                    //Skickar kommandon
                    client.SendRequest(programmingrequest);
                    btnProgram.SetImageResource(Resource.Drawable.record);
                    programmingrequest = "";
                }
                else
                {
                    //Startar programmeringsläge
                    programmingmode = true;
                    btnProgram.SetImageResource(Resource.Drawable.stop);
                }
            };

            btnProgram.LongClick += delegate
            {
                //Resnsar inprogrammerade kommandon
                programmingmode = false;
                btnProgram.SetImageResource(Resource.Drawable.record);
                programmingrequest = "";
            };
        }

        /// <summary>
        /// Skickar kommando om programmeringsläge inte är aktivt.
        /// Lägger till kommando till sträng om programmeringsläge är aktivt.
        /// </summary>
        /// <param name="command"></param>
        void addCommand(string command)
        {
            if (programmingmode)
            {
                if (programmingrequest != "") programmingrequest += ";";
                programmingrequest += command;
                vibrator.Vibrate(100);
            }
            else
            {
                client.SendRequest(command);
            }
        }
    }
}

