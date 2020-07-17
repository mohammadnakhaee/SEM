using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloWorld
{
    public partial class Settings : Form
    {
        FormMain formMain;
        public Settings(FormMain formmain, object Obj)
        {
            formMain = formmain;
            InitializeComponent();

            propertyGrid.SelectedObject = Obj;
        }

        private void propertyGrid_Click(object sender, EventArgs e)
        {

        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            formMain.ApplyGeneralSettings();
            Settings1.Default.Save();
        }
    }
}
