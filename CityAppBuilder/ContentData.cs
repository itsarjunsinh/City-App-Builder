using System;
using System.Data.SqlClient;

namespace CityAppBuilder
{
    class ContentData
    {
        String XMLSTART = Properties.Resources.DEFAULT_XML_TAGS + Properties.Resources.DEFAULT_STRING_RES;
        String XMLSTARTARRAY = Properties.Resources.DEFAULT_XML_TAGS;
        String XMLEND = "</resources>";

        public String Description = "";
        public String AppName = "";
        public String City = "";
        public String Country = "";
        public String Hotels = "";
        public String Places = "";

        public ContentData()
        {

        }

        public ContentData(String Description, String AppName, String City, String Country, String Hotels, String Places)
        {
            this.Description = FormatText(Description);
            this.AppName = FormatText(AppName);
            this.City = FormatText(City);
            this.Country = FormatText(Country);
            this.Hotels = FormatText(Hotels);
            this.Places = FormatText(Places);
        }

        String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Arjunsinh Jadeja\\Documents\\Projects\\Windows\\CityAppBuilder\\CityAppBuilder\\Database1.mdf\";Integrated Security=True";

        public Boolean Save()
        {
            Boolean saveSuccess = true;

            try
            {
                //Save data to the database.
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                String cmdInsert = "insert into AppContent(about_city,app_name,city_name,country_name,hotels,places) values('" + Description + "','" + AppName
                        + "','" + City + "','" + Country + "','" + Hotels + "','" + Places + "');";
                SqlCommand cmd = new SqlCommand(cmdInsert, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
                saveSuccess = false;
            }

            return saveSuccess;
        }

        public Boolean Restore()
        {
            Boolean restoreSuccess = true;

            try
            {
                //Fetch the last row of data from the database.
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                String cmdSelect = "SELECT TOP 1 * FROM AppContent ORDER BY ID DESC";
                SqlCommand cmd = new SqlCommand(cmdSelect, con);
                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    //Update data.
                    Description = reader["about_city"].ToString();
                    AppName = reader["app_name"].ToString();
                    City = reader["city_name"].ToString();
                    Country = reader["country_name"].ToString();
                    Hotels = reader["hotels"].ToString();
                    Places = reader["places"].ToString();
                }

                reader.Close();
                con.Close();
            }
            catch (Exception e)
            {
                restoreSuccess = false;
                Console.WriteLine(e.Message);
            }

            return restoreSuccess;
        }

        public String GetStringValues()
        {
            //Generate String resources file.
            String output = XMLSTART;
            output += "<string name=\"about_city\">" + FormatText(Description) + "</string>" + Environment.NewLine;
            output += "<string name=\"app_name\">" + FormatText(AppName) + "</string>" + Environment.NewLine;
            output += "<string name=\"city_name\">" + FormatText(City) + "</string>" + Environment.NewLine;
            output += "<string name=\"country_name\">" + FormatText(Country) + "</string>" + Environment.NewLine;
            output += XMLEND;

            return output;
        }

        public String GetStringArrayValues()
        {
            //Generate String Array resources file.
            String[] hotelsList = Hotels.Split('\n');
            String[] placesList = Places.Split('\n');
            String output = XMLSTARTARRAY;

            //Add hotels.
            output += "<string-array name=\"accomodation_list\">" + Environment.NewLine;
            foreach (String value in hotelsList)
            {
                output += "<item>" + FormatText(value) + "</item>" + Environment.NewLine;
            }
            output += "</string-array>" + Environment.NewLine;

            //Add places of interests.
            output += "<string-array name=\"sights_list\">" + Environment.NewLine;
            foreach (String value in placesList)
            {
                output += "<item>" + FormatText(value) + "</item>" + Environment.NewLine;
            }
            output += "</string-array>" + Environment.NewLine;

            output += XMLEND;
            return output;
        }

        public Boolean IsValid()
        {
            //Validate data.
            Boolean valid = true;
            if (String.IsNullOrWhiteSpace(AppName) || String.IsNullOrWhiteSpace(City) ||
                String.IsNullOrWhiteSpace(Country) || String.IsNullOrWhiteSpace(Description) ||
                String.IsNullOrWhiteSpace(Hotels) || String.IsNullOrWhiteSpace(Places))
            {
                valid = false;
            }

            return valid;
        }

        private String FormatText(String str)
        {
            //Add escape sequence to text.
            str = str.Replace("\'", "");
            str = str.Replace("\"", "");
            return str;
        }

    }
}
