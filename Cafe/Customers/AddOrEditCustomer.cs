using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cafe.DataLayer.Context;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Cafe.Products
{
    public partial class AddOrEditCustomer : Form
    {
        public int CustomerId { get; set; } = 0;
        public AddOrEditCustomer()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateInputs())
            {
                DataLayer.Customer customer = new DataLayer.Customer()
                {
                    CustomerName = txtCName.Text,
                    CustomerEmail = txtCEmail.Text,
                    CustomerMobile = txtCMobile.Text,
                    Address = txtCAddress.Text,
                };

                
                if (this.CustomerId == 0)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.CustomerRepository.Create(customer);
                        db.Save();
                    }
                }
                else
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        customer.CustomerID = this.CustomerId;
                        db.CustomerRepository.Update(customer);
                        db.Save();
                    }
                }
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateInputs()
        {

            if (this.txtCName.Text == "")
            {
                MessageBox.Show("نام نمیتواند خالی باشد", "خطا",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
            if (this.txtCMobile.Text == "")
            {
                MessageBox.Show("موبایل نمیتواند خالی باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            if (!IsValidMobile(txtCMobile.Text))
            {
                MessageBox.Show("شماره موبایل معتبر نیست یا قبلا استفاده شده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (this.txtCEmail.Text == "")
            {
                MessageBox.Show("ایمیل نمیتواند خالی باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            if (IsValidEmail(txtCEmail.Text))
            {
                MessageBox.Show("ایمیل معتبر نیست", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }



            return true;
        }



        private void AddOrEditCustomer_Load(object sender, EventArgs e)
        {
            if (this.CustomerId != 0)
            {
                this.Text = "ویرایش مشتری";
                this.btnSave.Text = "ویرایش";
                using (UnitOfWork db = new UnitOfWork())
                {
                    DataLayer.Customer customer = db.CustomerRepository.GetById(this.CustomerId);
                    this.txtCName.Text = customer.CustomerName;
                    this.txtCMobile.Text = customer.CustomerMobile;
                    this.txtCEmail.Text = customer.CustomerEmail;
                    this.txtCAddress.Text = customer.Address;

                }
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var test = new MailAddress(email);
                using (UnitOfWork db = new UnitOfWork())
                {
                    bool exists = db.CustomerRepository.Get(c => c.CustomerEmail == email && c.CustomerID != this.CustomerId).Any();
                    if (exists)
                    {
                        return true;
                    }
                }
                    return false;
            }
            catch
            {
                return true;
            }
        }

        private bool IsValidMobile(string mobile)
        {
            if (!Regex.IsMatch(mobile, @"^09\d{9}$"))
            {
                return false;
            }

            using (UnitOfWork db = new UnitOfWork())
            {
                bool exists = db.CustomerRepository.Get(c => c.CustomerMobile == mobile && c.CustomerID != this.CustomerId).Any();
                if (exists)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
