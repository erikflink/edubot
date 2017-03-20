using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EduBotControllerWindows
{
    class IO
    {
        OpenFileDialog openDialog = new OpenFileDialog();
        SaveFileDialog saveDialog = new SaveFileDialog();
        string filter;

        public IO(string filter)
        {
            //Ställer in angivet filter
            this.filter = filter;
        }

        //Skriver strängar från fält till fil
        public void Save(string[] data)
        {
            saveDialog.Filter = filter;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllLines(saveDialog.FileName, data);
            }
        }

        //Läser strängar från fil och returnerar som fält
        public string[] Open()
        {
            string[] data = new string[0];

            openDialog.Filter = filter;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                data = System.IO.File.ReadAllLines(openDialog.FileName);
            }
            return data;
        }
    }
}
