using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace u2048
{
    public partial class Menu : Form
    {
        public Menu()
        {
            CenterToScreen();
            InitializeComponent();
        }

        private void SatartGame(object sender, EventArgs e)
        {
            using (Main myForm = new Main())
            {
                this.Hide();
                myForm.ShowDialog();
                this.Show();
            }

        }

        private void Quit(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void Options(object sender, EventArgs e)
        {

            using (Options adam = new Options())
            {
                this.Hide();
                adam.ShowDialog();
                this.Show();
            }
        }

        /* private void panel1_Click(object sender, EventArgs e)
         {
             Main h = new Main();
             this.Hide();
             h.BringToFront();
             h.Show();         
         }
*/
    }
}
