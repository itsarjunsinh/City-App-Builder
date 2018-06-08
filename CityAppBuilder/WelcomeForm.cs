using System;
using System.Windows.Forms;

namespace CityAppBuilder
{

    public partial class WelcomeForm : Form
    {

        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void Btn_getStarted_Click(object sender, EventArgs e)
        {
            //Open Main (Data) form.
            DataForm dataForm = new DataForm();
            dataForm.Show();

            this.Hide();
        }
    }

}
