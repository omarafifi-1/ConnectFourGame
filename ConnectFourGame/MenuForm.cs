namespace ConnectFourGame
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm("VS AI", this);
            gameForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm("VS Player", this);
            gameForm.Show();
            this.Hide();
        }
    }
}
