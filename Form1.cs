using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProiectPAWRusuMihnea
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = "Data Source=database.db";
        List<Route> routes;
        List<Company> CompanyList;
        List<Booking> BookingList;
        public Form1()
        {
            CompanyList = new List<Company>();
            BookingList=new List<Booking>();
            InitializeComponent();
            routes = new List<Route>();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidareCompanies())
                return;
            Company c = new Company(textBox2.Text);


            try
            {
                AddCompany(c);
                DisplayCompanies();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void DisplayCompanies()
        {
            listView1.Items.Clear();
            comboBox1.Items.Clear();
            System.Windows.Forms.ComboBox MyComboBox = new System.Windows.Forms.ComboBox();


            foreach (Company companies in CompanyList)
            {
                var listViewItem = new ListViewItem(companies.companyId.ToString());
                listViewItem.SubItems.Add(companies.companyName);
                listView1.Items.Add(listViewItem);
                listViewItem.Tag = companies;
                comboBox1.Items.Add(companies);

            }
        }
        private void listView1_ItemActivate_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var result = MessageBox.Show(
                    "Delete this item?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    var selectedItem = listView1.SelectedItems[0];
                    Company companyToDelete = (Company)selectedItem.Tag;

                    DeleteCompany(companyToDelete);

                    DisplayCompanies();  // refresh ListView and ComboBox
                }
            }



        } //deletion of items in the Company list view
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCompany();
                DisplayCompanies();
                LoadRoute();
                DisplayRoutes();
                LoadBooking();
                DisplayBooking();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void RouteView_ItemActivate_1(object sender, EventArgs e)
        {
            if (RouteView.SelectedItems.Count > 0)
            {
                var result = MessageBox.Show(
                    "Delete this item?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    var selectedItem = RouteView.SelectedItems[0];
                    Route routeToDelete = (Route)selectedItem.Tag;

                    DeleteRoute(routeToDelete);

                    DisplayRoutes();  // refresh ListView and ComboBox

                }
            }
        }

        public void DisplayRoutes()
        {
            comboBoxBooking.Items.Clear();
            RouteView.Items.Clear();

            foreach (Route route in routes)
            {
                var listViewItem2 = new ListViewItem(route.departure.ToString());
                listViewItem2.SubItems.Add(route.destination);
                listViewItem2.SubItems.Add((route.routeId).ToString());
                listViewItem2.SubItems.Add(route.company != null ? route.company.ToString() : "Unknown");
                RouteView.Items.Add(listViewItem2);
                listViewItem2.Tag = route;
                comboBoxBooking.Items.Add(route);

            }
            UpdateRouteCountStatus();
        }
       public bool ValidareRoutes()
        {
            errorProvider1.Clear();
            bool valid = true;
            errorProvider1 = new ErrorProvider();

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                valid = false;
                errorProvider1.SetError(textBox3, "Departure should no be empty");


            }
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                valid = false;
                errorProvider1.SetError(textBox4, "Destination name should no be empty");

            }
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                valid = false;
                errorProvider1.SetError(comboBox1, "Company  should no be empty");
            }
            return valid;
        }
        //AICI
        private void button2_Click(object sender, EventArgs e)
        {
           
            if (!ValidareRoutes())
                return;
            Route r = new Route(textBox3.Text, textBox4.Text,(Company)comboBox1.SelectedItem);


            try
            {
                AddRoute(r);
                DisplayRoutes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void AddRoute(Route route)
        {
            var query = @"INSERT INTO Route (departure, destination, companyId)
                  VALUES (@departure, @destination, @companyId);
                  SELECT last_insert_rowid();";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@departure", route.departure);
                command.Parameters.AddWithValue("@destination", route.destination);
                command.Parameters.AddWithValue("@companyId", route.company.companyId);

                // Get the newly inserted routeId
                route.routeId = (long)command.ExecuteScalar();

                routes.Add(route);
                UpdateRouteCountStatus();
            }
        }
        private void AddCompany(Company company)
        {
            var query = "insert into Company( companyName)" +
                                " values(@companyName);  " +
                                "SELECT last_insert_rowid()";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                //1. Add the new companies to the database
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@companyName", company.companyName);

                company.companyId = (long)command.ExecuteScalar();

                //2. Add the new companies to the local collection
                CompanyList.Add(company);
            }
        }
        private void DeleteRoute(Route route)
        {
            const string query = "DELETE FROM Route WHERE routeId=@id";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                //Remove from the database
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", route.routeId);

                command.ExecuteNonQuery();

                //Remove from the local copy
                routes.Remove(route);
                UpdateRouteCountStatus();
            }
        }
        private void DeleteCompany(Company company)
        {
            const string query = "DELETE FROM Company WHERE companyId=@id";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                //Remove from the database
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", company.companyId);

                command.ExecuteNonQuery();

                //Remove from the local copy
                CompanyList.Remove(company);
            }
        }
        private void LoadCompany()
        {
            const string query = "SELECT * FROM Company";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long id = (long)reader["companyId"];
                        string companyName = (string)reader["companyName"];


                        Company company = new Company(id, companyName);
                        CompanyList.Add(company);
                    }
                }
            }
        }
        public bool ValidareCompanies()
        {
            errorProvider1.Clear();
            bool valid = true;
            errorProvider1 = new ErrorProvider();

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                valid = false;
                errorProvider1.SetError(textBox2, "Company name should no be empty");

                

            }
            return valid;
        }
        private void LoadRoute()
        {
            const string query = "SELECT * FROM Route";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Read Route fields
                        long routeId = (long)reader["routeId"];
                        string departure = (string)reader["departure"];
                        string destination = (string)reader["destination"];
                        long companyId = (long)reader["companyId"];

                        // Find company in CompanyList
                        Company company = CompanyList.FirstOrDefault(c => c.companyId == companyId);
                        // (Optional) If not found, handle as you need

                        // Create Route object
                        Route route = new Route(departure, destination, (int)routeId, company);
                        routes.Add(route);
                    }
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        //AICI
        private void button3_Click(object sender, EventArgs e)
        {
            if (!ValidareBookings())
                return;
            Booking b = new Booking(
                textBoxBooking1.Text,
                textBoxBooking2.Text,
                (Route)comboBoxBooking.SelectedItem,
                dateTimePicker1.Value.Date,
                dateTimePicker2.Value.Date
            );
            try
            {
                AddBooking(b);
                DisplayBooking();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddBooking(Booking booking)
        {
            const string query = @"
        INSERT INTO Booking (firstName, lastName, routeId, dateOfDeparture, dateOfReturn)
        VALUES (@firstName, @lastName, @routeId, @dep, @ret);
        SELECT last_insert_rowid();";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@firstName", booking.FirstName);
                command.Parameters.AddWithValue("@lastName", booking.LastName);
                command.Parameters.AddWithValue("@routeId", booking.route.routeId);
                command.Parameters.AddWithValue("@dep", booking.dateOfDeparture.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@ret", booking.dateOfReturn.ToString("yyyy-MM-dd"));

                booking.bookingId = (int)(long)command.ExecuteScalar();
                BookingList.Add(booking);
            }
        }
        public void DisplayBooking()
        {

            BookingView.Items.Clear();

            foreach (Booking booking in BookingList)
            {
                var listViewItem3 = new ListViewItem(booking.FirstName.ToString());
                listViewItem3.SubItems.Add(booking.LastName);
                listViewItem3.SubItems.Add(booking.route != null ? booking.route.ToString() : "Unknown");
                listViewItem3.SubItems.Add(booking.dateOfDeparture.ToString("dd/MM/yyyy"));
                listViewItem3.SubItems.Add(booking.dateOfReturn.ToString("dd/MM/yyyy"));
                listViewItem3.SubItems.Add((booking.bookingId).ToString());
                BookingView.Items.Add(listViewItem3);
                listViewItem3.Tag = booking;


            }

        }
        private void LoadBooking()
        {
            const string query = "SELECT * FROM Booking";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int bookingId = Convert.ToInt32(reader["bookingId"]);
                        string firstName = reader["firstName"].ToString();
                        string lastName = reader["lastName"].ToString();
                        long routeId = (long)reader["routeId"];
                        DateTime dateOfDeparture = DateTime.Parse(reader["dateOfDeparture"].ToString());
                        DateTime dateOfReturn = DateTime.Parse(reader["dateOfReturn"].ToString());

                        // Find associated Route from loaded list
                        Route route = routes.FirstOrDefault(r => r.routeId == routeId);

                        // Create and store booking
                        Booking booking = new Booking(bookingId, firstName, lastName, route, dateOfDeparture, dateOfReturn);
                        BookingList.Add(booking);
                    }
                }
            }
        }

        private void DeleteBooking(Booking booking)
        {
            const string query = "DELETE FROM Booking WHERE bookingId = @id";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@id", booking.bookingId);
                command.ExecuteNonQuery();
            }

            BookingList.Remove(booking);
        }

        private void BookingView_ItemActivate(object sender, EventArgs e)
        {
            if (BookingView.SelectedItems.Count > 0)
            {
                var result = MessageBox.Show(
                    "Delete this booking?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    var selectedItem = BookingView.SelectedItems[0];
                    Booking bookingToDelete = (Booking)selectedItem.Tag;

                    DeleteBooking(bookingToDelete);
                    DisplayBooking();
                }
            }
        }
        public bool ValidareBookings()
        {
            errorProvider1.Clear();
            bool valid = true;

            if (string.IsNullOrWhiteSpace(textBoxBooking1.Text))
            {
                valid = false;
                errorProvider1.SetError(textBoxBooking1, "First name cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(textBoxBooking2.Text))
            {
                valid = false;
                errorProvider1.SetError(textBoxBooking2, "Last name cannot be empty");
            }

            if (comboBoxBooking.SelectedItem == null)
            {
                valid = false;
                errorProvider1.SetError(comboBoxBooking, "Route must be selected");
            }

            if (dateTimePicker1.Value.Date == dateTimePicker2.Value.Date)
            {
                valid = false;
                errorProvider1.SetError(dateTimePicker1, "Departure and return dates can't be the same");
                errorProvider1.SetError(dateTimePicker2, "Departure and return dates can't be the same");
            }

            return valid;
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }



        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        private void serializeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Serializare CompanyList în XML
                using (var fs = new System.IO.FileStream("companies.xml", System.IO.FileMode.Create))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Company>));
                    serializer.Serialize(fs, CompanyList);
                }
                MessageBox.Show("CompanyList salvat cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la salvare: " + ex.Message);
            }
        }

        private void deserializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Deserializare CompanyList din XML
                using (var fs = new System.IO.FileStream("companies.xml", System.IO.FileMode.Open))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Company>));
                    CompanyList = (List<Company>)serializer.Deserialize(fs);
                }
                DisplayCompanies(); // ca să vezi datele în ListView
                MessageBox.Show("CompanyList încărcat cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcare: " + ex.Message);
            }
        }
        private void UpdateRouteCountStatus()
        {
            toolStripStatusLabelRoutes.Text = $"Total routes: {routes.Count}";
        }


        private void ExportToTextFile<T>(List<T> list)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                sfd.FileName = typeof(T).Name.ToLower() + "s.txt"; // ex: companies.txt

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var writer = new System.IO.StreamWriter(sfd.FileName))
                        {
                            foreach (var item in list)
                                writer.WriteLine(item.ToString());
                        }
                        MessageBox.Show($"Exported successfully to {sfd.FileName}!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error exporting: " + ex.Message);
                    }
                }
            }
        }




        private void routesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToTextFile(routes);
        }

        private void companiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToTextFile(CompanyList);
        }

        private void bookingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToTextFile(BookingList);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }

}
