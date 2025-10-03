using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectFourGame
{
    public partial class GameForm : Form
    {
        private Form MenuForm;
        public GameForm(string Mode, Form Menu)
        {
            InitializeComponent();
            this.MenuForm = Menu;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.MenuForm.Show();
            this.Close();
        }
    }
}
