using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace _20220534_Advanced_Programming_Assessment_1
{
    public partial class Form2 : Form
    {
        private CustomerController customerController;
        private Form1 parentForm;

         
        public Form2(CustomerController controller, Form1 parent)
        {
            InitializeComponent();
            customerController = controller;
            parentForm = parent;   
            LoadCustomerList();
        }

         private void LoadCustomerList()
        {
            listBox1.Items.Clear();

             
            var customersField = customerController
                .GetType()
                .GetField("customers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var customersDict = customersField.GetValue(customerController) as Dictionary<string, Customer>;

            foreach (var c in customersDict.Values)
            {
                 
                listBox1.Items.Add($"{c.customerNumber} - {c.name} - {c.contactDetails} - {(c.isStaff ? "Staff" : "Non-staff")}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             
        }

         
        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer to delete.");
                return;
            }

            string selectedItem = listBox1.SelectedItem.ToString();
            string customerNumber = selectedItem.Split('-')[0].Trim();

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to delete customer {customerNumber}?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    customerController.RemoveCustomer(customerNumber);
                    MessageBox.Show($"Customer {customerNumber} removed successfully.");
                    LoadCustomerList();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        
        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer to update.");
                return;
            }

            string selectedItem = listBox1.SelectedItem.ToString();
            string customerNumber = selectedItem.Split('-')[0].Trim();

            try
            {
                Customer existingCustomer = customerController.GetCustomer(customerNumber);

                string newName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter new name:",
                    "Update Customer",
                    existingCustomer.name);

                if (string.IsNullOrWhiteSpace(newName))
                {
                    MessageBox.Show("Customer name cannot be empty.");
                    return;
                }

                string newContact = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter new contact details:",
                    "Update Customer",
                    existingCustomer.contactDetails);

                DialogResult staffConfirm = MessageBox.Show(
                    "Is this customer staff?",
                    "Update Customer",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (staffConfirm == DialogResult.Cancel)
                    return;

                bool isStaff = staffConfirm == DialogResult.Yes;

                customerController.UpdateCustomer(customerNumber, newName, newContact, isStaff);

                MessageBox.Show("Customer updated successfully!");
                LoadCustomerList();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

  
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            string selectedItem = listBox1.SelectedItem.ToString();
            string customerNumber = selectedItem.Split('-')[0].Trim();

            try
            {
                Customer c = customerController.GetCustomer(customerNumber);

                 
                parentForm.SetCustomerFields(c.customerNumber, c.name, c.contactDetails, c.isStaff);

                 
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}