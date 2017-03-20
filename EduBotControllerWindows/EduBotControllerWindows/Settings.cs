using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EduBotControllerWindows
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            //Fyller textrutor med inställda värden
            tbxIP.Text = Main.eduBotClient.LocalEndPoint.Address.ToString();
            tbxPort.Text = Main.eduBotClient.LocalEndPoint.Port.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Sparar ipadress och port
            IPAddress ip;
            int port;
            if (IPAddress.TryParse(tbxIP.Text, out ip) && int.TryParse(tbxPort.Text, out port))
            {
                Main.eduBotClient.LocalEndPoint = new IPEndPoint(IPAddress.Parse(tbxIP.Text), int.Parse(tbxPort.Text));
                Main.eduBotClient.LogMessage("Successfully saved IP and Port!");
                Close();
            }
            else
            {

            }
        }
    }
}
