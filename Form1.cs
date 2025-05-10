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
        List<Company> CompanyList;
        public Form1()
        {
            CompanyList=new List<Company>();
            InitializeComponent();
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
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

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void routeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
