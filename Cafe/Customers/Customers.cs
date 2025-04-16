using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cafe.DataLayer.Context;
using Cafe.Utilities.Converter;

namespace Cafe.Products
{
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
        }

        public void BindGrid(string filter = "")
        {
            List<DataLayer.Customer> customers = new List<DataLayer.Customer>();

            if (filter == "")
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    customers = (List<DataLayer.Customer>)db.CustomerRepository.Get();
                }

            }
            else
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    customers = (List<DataLayer.Customer>)db.CustomerRepository.Get(c => c.CustomerName.Contains(filter) || c.CustomerMobile.Contains(filter) || c.CustomerEmail.Contains(filter));
                }
            }

            DataLayer.Customer defaultCustomer = customers.SingleOrDefault(c => c.CustomerID == 1);
            if (defaultCustomer != null)
            {
                customers.Remove(defaultCustomer);
            }
            this.dgvCustomers.Rows.Clear();
            foreach (DataLayer.Customer customer in customers)
            {
                string email = customer.CustomerEmail != "" ? customer.CustomerEmail : "ثبت نشده";
                this.dgvCustomers.Rows.Add(customer.CustomerID, customer.CustomerName, customer.CustomerMobile, email);
            }
        }



        private void Products_Load(object sender, EventArgs e)
        {
            this.dgvCustomers.AutoGenerateColumns = false;
            this.BindGrid();
        }


        private void btnRefreshCustomers_Click(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            this.BindGrid();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            AddOrEditCustomer addOrEditCustomer = new AddOrEditCustomer();
            if (addOrEditCustomer.ShowDialog() == DialogResult.OK)
            {
                this.BindGrid();
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (this.dgvCustomers.CurrentRow != null)
            {
                AddOrEditCustomer addOrEditCustomer = new AddOrEditCustomer();
                addOrEditCustomer.CustomerId = (int)this.dgvCustomers.CurrentRow.Cells[0].Value;
                if (addOrEditCustomer.ShowDialog() == DialogResult.OK)
                {
                    this.BindGrid();
                }
            }
            else
            {
                MessageBox.Show("لطفا یک مشتری را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (this.dgvCustomers.CurrentRow != null)
            {
                DataLayer.Customer customer = new DataLayer.Customer();
                using (UnitOfWork db = new UnitOfWork())
                {
                    customer = db.CustomerRepository.GetById(this.dgvCustomers.CurrentRow.Cells[0].Value);
                    if (MessageBox.Show($"آیا از حذف {customer.CustomerName} مطمعن هستید؟", "اخطار", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        db.CustomerRepository.Delete(customer);
                        db.Save();
                        this.BindGrid();
                    }
                }

            }
            else
            {
                MessageBox.Show("لطفا یک مشتری را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            this.BindGrid(this.txtFilter.Text);
        }



    }
}
