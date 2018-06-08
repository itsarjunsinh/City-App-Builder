using System;
using System.Windows.Forms;

namespace CityAppBuilder
{
    public partial class DataForm : Form
    {
        Util util = new Util();

        public DataForm()
        {
            InitializeComponent();
            util.LoadAssets();
        }

        private void Btn_build_Click(object sender, EventArgs e)
        {
            ContentData data = new ContentData(tb_description.Text, tb_appname.Text, tb_city.Text, tb_country.Text, tb_hotels.Text, tb_places.Text);

            if (data.IsValid())
            {
                //Saving data.
                data.Save();

                //Display status.
                label_status.Text = "Building App.";

                //Disable controls.
                panel_main.Enabled = false;

                //Get main string contents.
                String valuesString = data.GetStringValues();

                //Get string array contents.
                String valuesArray = data.GetStringArrayValues();

                //Write string files to disk.
                util.WriteResource("strings.xml", valuesString);
                util.WriteResource("arrays.xml", valuesArray);

                //Build & sign APK
                if (util.BuildApp())
                {
                    //Update status.
                    label_status.Text = "App Successfully Built.";
                }
                else
                {
                    //Update status.
                    label_status.Text = "Could not build App.";
                }

                //Enable controls.
                panel_main.Enabled = true;

            }
            else
            {
                //Data is not valid. Alert user.
                MessageBox.Show("Incomplete data. All fields are required.");
            }
        }

        private void Btn_restore_Click(object sender, EventArgs e)
        {
            //Restore previously entered data.
            ContentData cd = new ContentData();
            if (cd.Restore())
            {
                tb_appname.Text = cd.AppName;
                tb_city.Text = cd.City;
                tb_country.Text = cd.Country;
                tb_description.Text = cd.Description;
                tb_hotels.Text = cd.Hotels;
                tb_places.Text = cd.Places;
            }
            else
            {
                MessageBox.Show("Could not load data.");
            }
        }
    }
}
