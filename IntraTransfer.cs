using System;
using System.Windows.Forms;

namespace _20220534_Advanced_Programming_Assessment_1
{
    public partial class IntraTransfer : Form
    {
        private Customer _customer;
        private Form1 _parentForm;
        private CustomerController _controller;

        public IntraTransfer(Customer customer, Form1 parentForm, CustomerController controller)
        {
            InitializeComponent();
            _customer = customer;
            _parentForm = parentForm;
            _controller = controller;

            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
        }

        private void IntraTransfer_Load(object sender, EventArgs e)
        {
            textBox1.Text = _customer.name;
            textBox2.Text = _customer.customerNumber;

            comboBox2.Items.Clear();
            comboBox2.Items.Add("Everyday Account");
            comboBox2.Items.Add("Investment Account");
            comboBox2.Items.Add("Omni Account");
            comboBox2.SelectedIndex = 0;

            comboBox3.Items.Clear();
            comboBox3.Items.Add("Everyday Account");
            comboBox3.Items.Add("Investment Account");
            comboBox3.Items.Add("Omni Account");
            comboBox3.SelectedIndex = 1;

            UpdateBalanceDisplay(comboBox2.SelectedItem.ToString());
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox2.SelectedItem.ToString();
            comboBox3.Items.Clear();

            if (selected != "Everyday Account") comboBox3.Items.Add("Everyday Account");
            if (selected != "Investment Account") comboBox3.Items.Add("Investment Account");
            if (selected != "Omni Account") comboBox3.Items.Add("Omni Account");

            comboBox3.SelectedIndex = 0;
            UpdateBalanceDisplay(selected);
        }

        private void UpdateBalanceDisplay(string accountType)
        {
            switch (accountType)
            {
                case "Everyday Account": textBox4.Text = _customer.EverydayAcc.Balance.ToString(); break;
                case "Investment Account": textBox4.Text = _customer.InvestmentAcc.Balance.ToString(); break;
                case "Omni Account": textBox4.Text = _customer.OmniAcc.Balance.ToString(); break;
            }
        }

        private Account GetAccount(string type)
        {
            if (type == "Everyday Account")
                return _customer.EverydayAcc;
            else if (type == "Investment Account")
                return _customer.InvestmentAcc;
            else
                return _customer.OmniAcc;
        }

        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Please select both accounts.");
                return;
            }

            if (!decimal.TryParse(textBox3.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Enter a valid amount.");
                return;
            }

            string fromAcc = comboBox2.SelectedItem.ToString();
            string toAcc = comboBox3.SelectedItem.ToString();

            if (fromAcc == toAcc)
            {
                MessageBox.Show("Cannot transfer within the same account.");
                return;
            }

            try
            {
                _controller.Transfer(_customer.customerNumber, GetAccount(fromAcc).uniqueID, GetAccount(toAcc).uniqueID, amount);
                MessageBox.Show("Transfer Successful!");
                _parentForm.UpdateBalances();
                _controller.Save();
                this.Hide();
                _parentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Transfer failed: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonTransfer_Click(sender, e);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
        }
    }
}
