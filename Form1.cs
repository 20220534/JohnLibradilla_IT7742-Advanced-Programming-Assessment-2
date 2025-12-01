using System;
using System.Linq;
using System.Windows.Forms;

namespace _20220534_Advanced_Programming_Assessment_1
{
    public partial class Form1 : Form
    {
        private Customer customer;
        private CustomerController customerController = new CustomerController();
        private Account selectedAccount;

        public Form1()
        {
            InitializeComponent();

            // Load persisted customers
            customerController.Load();

            // Populate account type combo box
            comboBox1.Items.Add("Everyday Account");
            comboBox1.Items.Add("Investment Account");
            comboBox1.Items.Add("Omni Account");

            // Set default staff
            checkBoxStaffNo.Checked = true;

            // Load first customer if any exist
            var firstCustomer = customerController.GetAllCustomers().FirstOrDefault();
            if (firstCustomer != null)
                LoadCustomerToForm(firstCustomer.customerNumber);
        }

        private void LoadCustomerToForm(string customerNumber)
        {
            customer = customerController.GetCustomer(customerNumber);

            textBox1.Text = customer.name;
            textBox2.Text = customer.customerNumber;
            textBox3.Text = customer.contactDetails;
            checkBoxStaffYes.Checked = customer.isStaff;
            checkBoxStaffNo.Checked = !customer.isStaff;

            // Default selected account
            selectedAccount = customer.EverydayAcc;
            comboBox1.SelectedItem = "Everyday Account";

            UpdateBalances();
            UpdateAccountInfo();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || customer == null) return;

            string selectedType = comboBox1.SelectedItem.ToString();
            if (selectedType == "Everyday Account" && customer.EverydayAcc != null)
            {
                selectedAccount = customer.EverydayAcc;
            }
            else if (selectedType == "Investment Account" && customer.InvestmentAcc != null)
            {
                selectedAccount = customer.InvestmentAcc;
            }
            else if (selectedType == "Omni Account" && customer.OmniAcc != null)
            {
                selectedAccount = customer.OmniAcc;
            }
            // else: selectedAccount remains unchanged

            UpdateBalances();
            UpdateAccountInfo();
        }

        public void UpdateBalances()
        {
            if (selectedAccount != null)
                label5.Text = $"Balance: {selectedAccount.Balance:C}";
            textBox4.Clear();
        }

        public void UpdateAccountInfo()
        {
            if (selectedAccount == null) return;

            if (selectedAccount is OmniAccount omni)
            {
                AccountInfo.Text = $"Omni {omni.uniqueID} Interest: {omni.InterestRate}% Overdraft: ${omni.overdraftAmount} Fee: ${omni.failedTransactionfee(customer.isStaff)} Balance: ${omni.Balance}";
            }
            else if (selectedAccount is InvestmentAccount inv)
            {
                AccountInfo.Text = $"Investment {inv.uniqueID} Interest: {inv.InterestRate}% Fee: ${inv.failedTransactionfee(customer.isStaff)} Balance: ${inv.Balance}";
            }
            else if (selectedAccount is EverydayAccount everyday)
            {
                AccountInfo.Text = $"Everyday {everyday.uniqueID} Fee: ${everyday.failedTransactionfee(customer.isStaff)} Balance: ${everyday.Balance}";
            }
        }

        private void buttonDeposit_Click(object sender, EventArgs e)
        {
            if (selectedAccount == null)
            {
                MessageBox.Show("Please select an account type.");
                return;
            }

            if (decimal.TryParse(textBox4.Text, out decimal amount) && amount > 0)
            {
                selectedAccount.deposit(amount);
                UpdateBalances();
                UpdateAccountInfo();
                MessageBox.Show($"Deposited {amount:C} successfully!");
                customerController.Save();
            }
            else
            {
                MessageBox.Show("Enter a valid positive amount.");
            }
        }

        private void buttonWithdraw_Click(object sender, EventArgs e)
        {
            if (selectedAccount == null)
            {
                MessageBox.Show("Please select an account type.");
                return;
            }

            if (!decimal.TryParse(textBox4.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Enter a valid positive amount.");
                return;
            }

            try
            {
                selectedAccount.withdraw(amount);
                UpdateBalances();
                UpdateAccountInfo();
                MessageBox.Show($"Withdrawn {amount:C} successfully!");
            }
            catch (FailedWithdrawalException ex)
            {
                decimal fee = selectedAccount.failedTransactionfee(customer.isStaff);
                selectedAccount.deposit(-fee);
                UpdateBalances();
                MessageBox.Show($"Failed withdrawal of {ex.AttemptedAmount:C}. Fee applied: {fee:C}");
                UpdateAccountInfo();
            }

            customerController.Save();
        }

        private void buttonAddInterest_Click(object sender, EventArgs e)
        {
            if (selectedAccount is InvestmentAccount inv)
            {
                inv.AddInterest();
                UpdateBalances();
                UpdateAccountInfo();
                MessageBox.Show("Interest added successfully!");
                customerController.Save();
            }
            else
            {
                MessageBox.Show("This account does not earn interest.");
            }
        }

        private void buttonAddCustomer_Click(object sender, EventArgs e)
        {
            string id = textBox2.Text.Trim();
            string name = textBox1.Text.Trim();
            string contact = textBox3.Text.Trim();
            bool isStaff = checkBoxStaffYes.Checked;
            string addAmount = textBox4.Text.Trim();
            string accountType = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Customer ID and Name are required.");
                return;
            }

            if (!decimal.TryParse(addAmount, out decimal amt) || amt < 0)
            {
                MessageBox.Show("Enter a valid initial amount.");
                return;
            }

            try
            {
                Customer newCustomer = customerController.AddCustomer(id, name, contact, isStaff, addAmount, accountType);
                customer = newCustomer;
                customerController.Save();

                MessageBox.Show($"Customer '{name}' added successfully!");

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
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}");
            }
        }

        private void checkBoxStaffYes_CheckedChanged(object sender, EventArgs e)
        {
            if (customer != null)
            {
                customer.isStaff = checkBoxStaffYes.Checked;
                checkBoxStaffNo.Checked = !checkBoxStaffYes.Checked;
            }
        }

        private void checkBoxStaffNo_CheckedChanged(object sender, EventArgs e)
        {
            if (customer != null)
            {
                customer.isStaff = !checkBoxStaffNo.Checked;
                checkBoxStaffYes.Checked = !checkBoxStaffNo.Checked;
            }
        }

        private void buttonOpenForm2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(customerController, this);
            form2.ShowDialog();
        }

        private void IntraTransfer_Click(object sender, EventArgs e)
        {
            if (customer == null)
            {
                MessageBox.Show("Please create or load a customer first.");
                return;
            }

            IntraTransfer transferForm = new IntraTransfer(customer, this, customerController);
            transferForm.Show();
            this.Hide();
        }

        public void SetCustomerFields(string customerNumber, string name, string contact, bool isStaff)
        {
            textBox1.Text = name;
            textBox2.Text = customerNumber;
            textBox3.Text = contact;
            checkBoxStaffYes.Checked = isStaff;
            checkBoxStaffNo.Checked = !isStaff;

            LoadCustomerToForm(customerNumber);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Add your event handling logic here
            // For example, validate input or update other controls
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void label9_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
