using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace _20220534_Advanced_Programming_Assessment_1
{
    public partial class Form1 : Form
    {
        private Customer customer;
        private CustomerController customerController = new CustomerController();


        public Form1()
        {
            
                InitializeComponent();
                string name = "";
                string id = "";
                string contact = "";

                bool isStaff = false;

                customer = new Customer(id, name, contact, isStaff);

                textBox1.Text = name;
                textBox2.Text = id;
                textBox3.Text = contact;
                checkBoxStaffYes.Checked = isStaff;
             

        }

        private Account selectedAccount;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            comboBox1.Items.Add("Everyday Account");
            comboBox1.Items.Add("Investment Account");
            comboBox1.Items.Add("Omni Account");


        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                
                return;
            }

            string selectedType = comboBox1.SelectedItem.ToString();

            if (selectedType == "Everyday Account")
                selectedAccount = new EverydayAccount("", 0m);
            else if (selectedType == "Investment Account")
                selectedAccount = new InvestmentAccount("", 0m);
            else if (selectedType == "Omni Account")
                selectedAccount = new OmniAccount("", 0m);
            else
            {
                MessageBox.Show("Unknown account type selected.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            label5.Text = $"Balance: {selectedAccount.Balance:C}";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedAccount == null)
            {
                MessageBox.Show("Please select an account type first.");
                return;
            }

            if (decimal.TryParse(textBox4.Text, out decimal depositAmount) && depositAmount > 0)
            {
                selectedAccount.deposit(depositAmount);
                label5.Text = $"Balance: {selectedAccount.Balance:C}";
                MessageBox.Show($"Deposited {depositAmount:C} successfully!");
            }
            else
            {
                MessageBox.Show("Please enter a valid positive amount.");
            }
            if (selectedAccount is OmniAccount omniAccount)
            {
                AccountInfo.Text = $"Omni {omniAccount.uniqueID} Interest Rate: {omniAccount.InterestRate}% Overdraft Limit: ${omniAccount.overdraftAmount} Fee: ${omniAccount.failedTransactionfee(customer.isStaff)} Current Balance: ${omniAccount.Balance};";
            }

            if (selectedAccount is EverydayAccount everydayAccount)
            {
                AccountInfo.Text = $"Everyday {everydayAccount.uniqueID} Fee: ${everydayAccount.failedTransactionfee(customer.isStaff)} Current Balance: ${everydayAccount.Balance}";
            }
            if (selectedAccount is InvestmentAccount investmentAccount)
            {
                AccountInfo.Text = $"Investment {investmentAccount.uniqueID} Interest Rate: {investmentAccount.InterestRate}% Fee: ${investmentAccount.failedTransactionfee(customer.isStaff)} Current Balance: ${investmentAccount.Balance};";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedAccount == null)
            {
                MessageBox.Show("Please select an account type first.");
                return;
            }

            if (decimal.TryParse(textBox4.Text, out decimal withdrawAmount) && withdrawAmount > 0)
            {
                try
                {
                    selectedAccount.withdraw(withdrawAmount);
                    label5.Text = $"Balance: {selectedAccount.Balance:C}";
                    MessageBox.Show($"Withdrawn {withdrawAmount:C} successfully!");
                }
                catch (FailedWithdrawalException ex)
                {
                     decimal fee = selectedAccount.failedTransactionfee(customer.isStaff);

                     selectedAccount.deposit(-fee);  
                    label5.Text = $"Balance: {selectedAccount.Balance:C}";

                    MessageBox.Show($"Failed withdrawal of {ex.AttemptedAmount:C} from account {ex.AccountID}.\nTransaction fee of {fee:C} has been applied.");

                   
                    if (selectedAccount is InvestmentAccount investmentAccount1)
                    {
                        AccountInfo.Text = $"Investment {investmentAccount1.uniqueID}; Withdrawal Attempt: ${withdrawAmount}; Transaction Status: Failed; Fee:${fee}; Updated Balance:${investmentAccount1.Balance};";
                    }
                    else if (selectedAccount is OmniAccount omniAccount1)
                    {
                        AccountInfo.Text = $"Omni {omniAccount1.uniqueID}; Withdrawal Attempt: ${withdrawAmount}; Transaction Status: Failed; Fee:${fee}; Updated Balance:${omniAccount1.Balance};";
                    }
                    else if (selectedAccount is EverydayAccount everydayAccount1)
                    {
                        AccountInfo.Text = $"Everyday {everydayAccount1.uniqueID}; Withdrawal Attempt: ${withdrawAmount}; Transaction Status: Failed; Fee:${fee}; Updated Balance:${everydayAccount1.Balance};";
                    }

                    return;
                }

                 
                if (selectedAccount is OmniAccount omniAccount)
                {
                    AccountInfo.Text = $"Omni {omniAccount.uniqueID} Interest Rate: {omniAccount.InterestRate}%; Overdraft Limit: ${omniAccount.overdraftAmount}; Fee: ${omniAccount.failedTransactionfee(customer.isStaff)}; Current Balance: ${omniAccount.Balance};";
                }

                if (selectedAccount is EverydayAccount everydayAccount)
                {
                    AccountInfo.Text = $"Everyday {everydayAccount.uniqueID} Fee: ${everydayAccount.failedTransactionfee(customer.isStaff)}; Current Balance: ${everydayAccount.Balance};";
                }
                if (selectedAccount is InvestmentAccount investmentAccount)
                {
                    AccountInfo.Text = $"Investment {investmentAccount.uniqueID} Interest Rate: {investmentAccount.InterestRate}% Fee: ${investmentAccount.failedTransactionfee(customer.isStaff)} Current Balance: ${selectedAccount.Balance};";
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid positive amount.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                if (selectedAccount is InvestmentAccount investmentAccount)
                {
                    investmentAccount.AddInterest();
                    label5.Text = $"Balance: {investmentAccount.Balance:C}";
                    MessageBox.Show("Interest calculated and added to balance.");
                }
                else
                {
                    MessageBox.Show("This account does not earn interest.");
                }

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

            string name = textBox1.Text.Trim();
            string id = textBox2.Text.Trim();
            string contact = textBox3.Text.Trim();
            bool isStaff = checkBoxStaffYes.Checked;

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Customer ID and Name are required.");
                return;
            }

            customer = new Customer(id, name, contact, isStaff);


            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();



            MessageBox.Show("Customer added successfully!");

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStaffYes.Checked)
            {
                customer.isStaff = true;
                checkBoxStaffNo.Checked = false;
            }
            else
            {
                customer.isStaff = false;
                checkBoxStaffNo.Checked = true;
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
           

            if (checkBoxStaffNo.Checked)
            {
                customer.isStaff = false;
     
            }
            else
            {
               customer.isStaff = true;
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem == null || selectedAccount == null)
                {
                    MessageBox.Show("Please select an account type before adding a customer.",
                                    "Missing Account Type",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                string name = textBox1.Text.Trim();              
                string customerNumber = textBox2.Text.Trim();    
                string contact = textBox3.Text.Trim();           
                bool isStaff = checkBoxStaffYes.Checked;

                if (string.IsNullOrWhiteSpace(customerNumber) || string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Customer ID and Name are required.",
                                    "Validation Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

             
                Customer newCustomer = customerController.AddCustomer(customerNumber, name, contact, isStaff);

            
                customerController.AddAccountToCustomer(customerNumber, selectedAccount);

                MessageBox.Show(
    $"Customer '{newCustomer.name}' added successfully with a '{comboBox1.SelectedItem}'!",
    "Success",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information
);


                 textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                checkBoxStaffYes.Checked = false;
                checkBoxStaffNo.Checked = true;
                comboBox1.SelectedIndex = -1; 
                selectedAccount = null;
                label5.Text = "Balance: $0.00";
                AccountInfo.Text = "";

     
            }
            catch (ArgumentException ex)
            {
                
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(customerController, this);  
            form2.ShowDialog();
        }

        public void SetCustomerFields(string customerNumber, string name, string contact, bool isStaff)
        {
            textBox1.Text = name;
            textBox2.Text = customerNumber;
            textBox3.Text = contact;
            checkBoxStaffYes.Checked = isStaff;
            checkBoxStaffNo.Checked = !isStaff;
        }
    }
}
