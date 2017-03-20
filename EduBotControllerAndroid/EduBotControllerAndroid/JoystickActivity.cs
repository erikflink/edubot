using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Diagnostics;

namespace EduBotControllerAndroid
{
    [Activity(Label = "Joystick", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class JoystickActivity : Activity, View.IOnTouchListener
    {
        private LinearLayout layout;
        private static TextView textViewCommand;
        //Lagrar senast skickade kommando
        string commandText = "";

        //Stoppur
        Stopwatch stopWatch = new Stopwatch();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Initierar
            SetContentView(Resource.Layout.Joystick);
            layout = FindViewById<LinearLayout>(Resource.Id.linearLayout);
            layout.SetOnTouchListener(this);
            textViewCommand = FindViewById<TextView>(Resource.Id.textViewPreview);

            base.OnCreate(savedInstanceState);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    //Startar stoppur
                    stopWatch.Start();
                    var x = 0;
                    var y = 0;
                    break;
                case MotionEventActions.Move:
                    //Skickar kommando baserat på tryckpunkt om det gått mer än 500 millisekunder sedan senast.
                    if(stopWatch.ElapsedMilliseconds >= 500)
                    {
                        x = 100 * (int)e.GetX() / (v.Width / 2) - 100;
                        y = -(100 * (int)e.GetY() / (v.Height / 2) - 100);
                        //displayData(x, y);
                        move(x, y);
                        stopWatch.Restart();
                        textViewCommand.Text = commandText;
                    }
                    break;
                case MotionEventActions.Up:
                    //Återställer
                    x = y = 0;
                    textViewCommand.Text = x + " " + y;
                    stopWatch.Reset();
                    break;
            }
            return true;
        }

        //Skickar kommando som utförs i 400 millisekunder
        private void move(int x, int y)
        {
            int left = y + x;
            int right = y - x;
            if (left > 100) left = 100;
            if (right > 100) right = 100;
            if (left < -100) left = -100;
            if (right < -100) right = -100;
            string command = string.Format("{0},{1},{2}", left, right, 400);
            MainActivity.client.SendRequest(command);
            commandText = command;
        }
        
    }
}