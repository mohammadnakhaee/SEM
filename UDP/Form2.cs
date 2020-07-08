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
    public partial class Analyse : Form
    {
        public byte[] receivedData = new byte[512 * 512 * 128];
        public int[] window_row = new int[512 * 128];
        int count, startindex;
        public int iX = 0;
        public int iY = 0;
        public int nX = 512;
        public int nY = 512;
        public Analyse()
        {
            InitializeComponent();
           
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
count = 0;
            startindex = 0;
            for (int i = 0; i < 512*128; i++)
                {
                if (window_row[i] == numericUpDown1.Value)
                {
                    count++;
                    startindex = i;
                }
                else if (count > 0) break;
                }
            startindex -= count-1;

            for (int i = 0; i < count; i++)
                for (int j = 0; j < nX; j++)
                {
                    chart1.Series[0].Points.AddXY(i * nX + j, receivedData[(startindex + i) * 512 + j]);
                    if(   2*  ((int)( (i * nX + j )/ count/2 ))  == ((i * nX + j) / count)   )
                    chart1.Series[0].Points[i * nX + j].Color = Color.Red;
                    else
                    chart1.Series[0].Points[i * nX + j].Color = Color.Blue;

                }
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
