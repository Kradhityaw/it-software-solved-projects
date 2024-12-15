using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopCurrencyConverter
{
    public partial class Form1 : Form
    {
        CurrencyConverterEntities db = new CurrencyConverterEntities();

        public Form1()
        {
            InitializeComponent();
        }

        private void ConverterEvent()
        {
            Currency originAmountId = comboBox2.SelectedItem as Currency;
            Currency convertedToId = comboBox3.SelectedItem as Currency;
            var userInput = string.IsNullOrEmpty(textBox1.Text) ? 0 : Convert.ToDecimal(textBox1.Text);

            label3.Text = originAmountId == null ? "" : originAmountId.name;
            label4.Text = convertedToId == null ? "" : convertedToId.name;

            var checkOrigin = originAmountId == null ? 0 : originAmountId.id;
            var checkConvert = convertedToId == null ? 0 : convertedToId.id;
            int periodId = comboBox1.SelectedValue == null ? 1 : (int)comboBox1.SelectedValue;

            var getOriginAmount = db.USDExchangeRates.FirstOrDefault(f => f.period_id == periodId && f.currency_id == checkOrigin);
            var getConveredTo = db.USDExchangeRates.FirstOrDefault(f => f.period_id == periodId && f.currency_id == checkConvert);

            decimal divide = 0;

            if (getOriginAmount == null && getConveredTo == null) divide = 1;
            else if (getOriginAmount == null) divide = getConveredTo.rate;
            else if (getConveredTo == null) divide = 1 / getOriginAmount.rate;
            else divide = getConveredTo.rate / getOriginAmount.rate;

            var multiple = userInput * divide;

            textBox2.Text = $"{multiple:n3}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            periodBindingSource.DataSource = db.Periods.ToList();
            currencyBindingSource.DataSource = db.Currencies.ToList();
            currencyBindingSource1.DataSource = db.Currencies.ToList();
            ConverterEvent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ConverterEvent();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConverterEvent();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConverterEvent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConverterEvent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int originCbb = (int)comboBox2.SelectedValue;
            int converedCbb = (int)comboBox3.SelectedValue;

            string originTb = textBox1.Text;
            string converedTb = textBox2.Text;

            comboBox2.SelectedValue = converedCbb;
            comboBox3.SelectedValue = originCbb;

            textBox2.Text = originTb;
            textBox1.Text = converedTb;

            ConverterEvent();
        }
    }
}
