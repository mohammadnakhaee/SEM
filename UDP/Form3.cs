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
        public Settings()
        {
            InitializeComponent();

            propertyGrid.SelectedObject = Settings1.Default;
        }

        private void propertyGrid_Click(object sender, EventArgs e)
        {

        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings1.Default.Save();
        }
    }
}
