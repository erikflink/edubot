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
using System.Net;

namespace EduBotControllerAndroid
{
    [Activity(Label = "Settings and debugging", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SettingsActivity : Activity
    {
        //Kanppar
        Button btnSave;
        Button btnSend;
        //Textrutor
        EditText etIP;
        EditText etPort;
        public static EditText etCommand;
        TextView tvLog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.Settings);

            base.OnCreate(savedInstanceState);

            //Initierar knappar
            btnSave = FindViewById<Button>(Resource.Id.buttonSave);
            btnSend = FindViewById<Button>(Resource.Id.buttonSend);
            //Initierar textrutor
            etIP = FindViewById<EditText>(Resource.Id.editTextIP);
            etPort = FindViewById<EditText>(Resource.Id.editTextPort);
            etCommand = FindViewById<EditText>(Resource.Id.editTextCommand);
            tvLog = FindViewById<TextView>(Resource.Id.textViewLog);

            //Ställer in metod för att scrolla i loggrutan
            tvLog.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();

            //Fyller textrutor med värden
            etIP.Text = MainActivity.client.LocalEndPoint.Address.ToString();
            etPort.Text = MainActivity.client.LocalEndPoint.Port.ToString();
            tvLog.Text = MainActivity.client.GetLog();

            //Sparar inställningar
            btnSave.Click += delegate
            {
                IPAddress ip;
                int port;
                if (IPAddress.TryParse(etIP.Text, out ip) && int.TryParse(etPort.Text, out port))
                {
                    MainActivity.client.LocalEndPoint = new IPEndPoint(IPAddress.Parse(etIP.Text), int.Parse(etPort.Text));
                    MainActivity.client.LogMessage("Successfully saved IP and Port!");
                }
                else
                {
                    MainActivity.client.LogMessage("IP and Port not valid!");
                }
                tvLog.Text = MainActivity.client.GetLog();

            };

            //Skickar kommando
            btnSend.Click += delegate
            {
                MainActivity.client.SendRequest(etCommand.Text);
                tvLog.Text = MainActivity.client.GetLog();
            };
        }
    }
}