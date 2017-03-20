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
        string commandText = "";
        Stopwatch stopWatch = new Stopwatch();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.Joystick);
            layout = FindViewById<LinearLayout>(Resource.Id.linearLayout);
            layout.SetOnTouchListener(this);
            textViewCommand = FindViewById<TextView>(Resource.Id.textViewPreview);

            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    stopWatch.Start();
                    var x = 0;
                    var y = 0;
                    break;
                case MotionEventActions.Move:
                    
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
                    x = y = 0;
                    textViewCommand.Text = x + " " + y;
                    stopWatch.Reset();
                    break;
            }
            return true;
        }

        private void move(int x, int y)
        {
            int left = y + x;
            int right = y - x;
            if (left > 100) left = 100;
            if (right > 100) right = 100;
            if (left < -100) left = -100;
            if (right < -100) right = -100;
            string command = string.Format("{0},{1},{2}", left, right, 450);
            MainActivity.client.sendCommand(command);
            commandText = command;
        }
        
    }
}