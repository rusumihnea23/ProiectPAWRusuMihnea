using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectPAWRusuMihnea
{
    public partial class Form1 : Form
    {
        List<Route> routes;
        List<Company> CompanyList;
        public Form1()
        {
            CompanyList=new List<Company>();
            InitializeComponent();
            routes=new List<Route>();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidareCompanies())
                return;
            Company c=new Company(int.Parse(textBox1.Text),textBox2.Text);
            CompanyList.Add(c);
            DisplayCompanies();


        }


        public void DisplayCompanies()
        {
            listView1.Items.Clear();
            comboBox1.Items.Clear();
            ComboBox MyComboBox = new ComboBox();


            foreach (Company companies in CompanyList)
            {
                var listViewItem = new ListViewItem(companies.companyId.ToString());
                listViewItem.SubItems.Add(companies.companyName);             
                listView1.Items.Add(listViewItem);
                listViewItem.Tag = companies;
                comboBox1.Items.Add(companies);
                
            }
        }

        public bool ValidareCompanies()
        {
            errorProvider1.Clear();
            bool valid = true;
            errorProvider1 = new ErrorProvider();
            

            if(!int.TryParse(textBox1.Text,out int id))
            {
                valid = false;
                errorProvider1.SetError(textBox1, "id should be a number");
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                valid = false;
                errorProvider1.SetError(textBox2, "Company name should no be empty");
            }


            return valid;
        }//validation of ID and Company Name in the first tab

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

                    CompanyList.Remove(companyToDelete); //remove the company that we double clicked from the lst

                    DisplayCompanies();  // refresh ListView and ComboBox
                }
            }
        
        
            
        } //deletion of items in the Company list view
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RouteView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void DisplayRoutes()
        {

            RouteView.Items.Clear();

            foreach (Route route in routes)
            {
                var listViewItem2 = new ListViewItem(route.departure.ToString());
                listViewItem2.SubItems.Add(route.destination);
                listViewItem2.SubItems.Add((route.routeId).ToString());
                listViewItem2.SubItems.Add((route.company).ToString());
                RouteView.Items.Add(listViewItem2);
                listViewItem2.Tag = route;
                

            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Route r=new Route(textBox3.Text,textBox4.Text,int.Parse(textBox5.Text),(Company)comboBox1.SelectedItem);
            routes.Add(r);
            DisplayRoutes();
        }


    }
}
