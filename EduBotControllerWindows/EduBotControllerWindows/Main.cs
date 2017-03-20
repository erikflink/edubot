using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EduBotControllerWindows
{
    public partial class Main : Form
    {
        //Instans av EduBotClient för koppling till roboten
        public static EduBotClient eduBotClient = new EduBotClient();
        //Objekt som hanterar spara och öppna
        IO io = new IO("EduBot files (*.EduBot)|*.EduBot|All files (*.*)|*.*");

        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Färjar rad med angivet index grönt om kommandot på raden är giltigt.
        /// Färjar annars raden rött.
        /// </summary>
        /// <param name="lineIndex"></param>
        void colorLine(int lineIndex)
        {
            if (rtbxInput.Lines.Length > lineIndex)
            {
                int cursorIndex = rtbxInput.SelectionStart;

                int firstCharIndex = rtbxInput.GetFirstCharIndexFromLine(lineIndex);
                string lineText = rtbxInput.Lines[lineIndex];
                rtbxInput.Select(firstCharIndex, lineText.Length);

                string validatedCommand = eduBotClient.ValidateCommand(lineText);
                if (validatedCommand == null)
                {
                    rtbxInput.SelectionColor = Color.Red;
                }

                else
                {
                    rtbxInput.SelectionColor = Color.Green;
                }

                rtbxInput.Select(cursorIndex, 0);
                rtbxInput.SelectionColor = SystemColors.WindowText;
            }
            
        }

        /// <summary>
        /// Uppdaterar textbox med logg
        /// </summary>
        void updateLog()
        {
            rtbxLog.Text = eduBotClient.GetLog();
            rtbxLog.SelectionStart = rtbxLog.Text.Length;
            rtbxLog.ScrollToCaret();
        }

        private void rtbxInput_TextChanged(object sender, EventArgs e)
        {
            //Färjar rad
            int firstCharIndex = rtbxInput.GetFirstCharIndexOfCurrentLine();
            int lineIndex = rtbxInput.GetLineFromCharIndex(firstCharIndex);
            colorLine(lineIndex);
        }

        //Lägger till kommandon då knappar trycks ned:

        private void btnFrwd_Click(object sender, EventArgs e)
        {
            addCommand("100,100,1000");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            addCommand("-100,-100,1000");
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            addCommand("-100,100,700");
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            addCommand("100,-100,700");
        }

        private void btnLedFrwd_Click(object sender, EventArgs e)
        {
            addCommand("led,0,100");
        }

        private void btnLedBack_Click(object sender, EventArgs e)
        {
            addCommand("led,1,100");
        }

        private void btnLedLeft_Click(object sender, EventArgs e)
        {
            addCommand("led,2,100");
        }

        private void btnLedRight_Click(object sender, EventArgs e)
        {
            addCommand("led,3,100");
        }

        //Lägger till angiven sträng vid markör
        void addCommand(string command)
        {
            rtbxInput.Select();

            int cursorIndex = rtbxInput.SelectionStart;
            int lineIndex = rtbxInput.GetLineFromCharIndex(cursorIndex);

            string lineText = "";

            if (rtbxInput.Lines.Length > lineIndex)
            {
                lineText = rtbxInput.Lines[lineIndex];
                rtbxInput.Select(rtbxInput.GetFirstCharIndexOfCurrentLine() + lineText.Length, 0);
                
            }

            if (lineText == "")
            {
                rtbxInput.SelectedText = command;
            }
            else
            {
                rtbxInput.SelectedText = "\n" + command;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //Skickar kommandon
            string request = rtbxInput.Text.Replace("\n", ";");
            if (request != null)
            {
                eduBotClient.SendRequest(request);
            }
            updateLog();
        }
        
        private void btnSettings_Click(object sender, EventArgs e)
        {
            //Öppnar nytt Inställnings-fönster
            Form settings = new Settings();
            settings.ShowDialog();
            updateLog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Sparar kommandon till fil
            io.Save(rtbxInput.Lines);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            //Läser kommandon från fil
            rtbxInput.Lines = io.Open();
            for (int i = 0; i < rtbxInput.Lines.Length; i++)
            {
                colorLine(i);
            }
        }
    }
}
