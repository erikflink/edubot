using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace EduBotControllerAndroid
{
    public class EduBotClient
    {
        //Socket för att skicka kommandon
        static Socket sender;
        //Lokal EndPoint med adress till roboten
        public static IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("192.168.137.1"), 1337);
        //Logg
        public static string log = "";

        public EduBotClient()
        {

        }

        /// <summary>
        /// Lokal EndPoint med adress till roboten
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get
            {
                return localEndPoint;
            }
            set
            {
                localEndPoint = value;
            }
        }

        /// <summary>
        /// Validerar kommando och returnerar det som sträng.
        /// Returnerar null om kommando inte är giltigt.
        /// </summary>
        /// <param name="commandToValidate"></param>
        /// <returns></returns>
        public string ValidateCommand(string commandToValidate)
        {
            commandToValidate = commandToValidate.Replace(" ", string.Empty);

            string[] commandValueStrings = commandToValidate.Split(',');

            //Validerar LED-kommando
            if (commandValueStrings[0] == "led" && commandValueStrings.Length == 3)
            {
                int[] validatedCommand = { 0, 0 };
                if (int.TryParse(commandValueStrings[1], out validatedCommand[0]) &&
                    int.TryParse(commandValueStrings[2], out validatedCommand[1]) &&
                    validatedCommand[0] >= 0 && validatedCommand[0] <= 3 &&
                    validatedCommand[1] >= 0 && validatedCommand[1] <= 100)
                {
                    return string.Format("led,{0},{1}", validatedCommand[0], validatedCommand[1]);
                }
            }
            //Validerar servo-kommando
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
                    return string.Format("{0},{1},{2}", validatedCommand[0], validatedCommand[1], validatedCommand[2]);
                }
            }
            return null;
        }

        /// <summary>
        /// Validerar sträng med kommandon och returnerar som sträng.
        /// Returnerar null om något kommando inte är giltigt.
        /// </summary>
        /// <param name="requestToValidate"></param>
        /// <returns></returns>
        string valideteRequest(string requestToValidate)
        {
            string[] commandsToValidate = requestToValidate.Split(';');

            string validatedRequest = "";
            string validatedCommand;
            for (int i = 0; i < commandsToValidate.Length; i++)
            {
                if (i != 0) validatedRequest += ";";
                validatedCommand = ValidateCommand(commandsToValidate[i]);
                if (validatedCommand == null)
                {
                    LogMessage(string.Format("Command number {0} \"{1}\" is not valid!", i + 1, commandsToValidate[i]));
                    return null;
                }
                validatedRequest += validatedCommand;
            }
            return validatedRequest;
        }

        /// <summary>
        /// Skickar sträng med kommandon till roboten om det är giltigt
        /// </summary>
        /// <param name="request"></param>
        public void SendRequest(string request)
        {
            string validatedRequest = valideteRequest(request);
            if (validatedRequest != null)
            {
                connect();
                send(validatedRequest);
                disconnect();
            }
        }

        /// <summary>
        /// Ansluter till roboten
        /// </summary>
        void connect()
        {
            try
            {
                sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sender.SendTimeout = 500;

                IAsyncResult result = sender.BeginConnect(localEndPoint.Address, localEndPoint.Port, null, null);

                bool success = result.AsyncWaitHandle.WaitOne(500, true);

                if (!success)
                {
                    sender.Close();
                    throw new ApplicationException("Failed to connect server.");
                }
            }
            catch
            {
                LogMessage("Connection error!");
            }
        }


        /// <summary>
        /// Skickar till roboten
        /// </summary>
        /// <param name="command"></param>
        void send(string command)
        {
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(command);
                sender.Send(msg);
                LogMessage("Successfully sent: " + command);
            }
            catch
            {
                LogMessage("Sending error!");
            }
        }

        /// <summary>
        /// Kopplar från roboten
        /// </summary>
        void disconnect()
        {
            try
            {
                sender.Close();
            }
            catch
            {
                LogMessage("Could not close socket!");
            }
        }

        /// <summary>
        /// Loggar meddelande
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string LogMessage(string message)
        {
            log += DateTime.Now.ToString("h:mm:ss: ") + message + "\n";
            return log;
        }

        /// <summary>
        /// Returnerar logg
        /// </summary>
        /// <returns></returns>
        public string GetLog()
        {
            return log;
        }

        /// <summary>
        /// Rensar logg
        /// </summary>
        public void ClearLog()
        {
            log = "";
        }
    }
}