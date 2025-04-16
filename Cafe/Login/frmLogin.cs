using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cafe.DataLayer;
using Cafe.DataLayer.Context;

namespace Cafe.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private bool ValidateInputs()
        {
            if (this.txtUsername.Text == "")
            {
                MessageBox.Show("نام کاربری نمیتواند خالی باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (this.txtPassword.Text == "")
            {
                MessageBox.Show("پسورد نمیتواند خالی باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.ValidateInputs())
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    User user = db.UserRepository.Get().First();
                    if (user.password == this.txtPassword.Text && user.username == this.txtUsername.Text)
                    {
                        MessageBox.Show("خوش آمدید", "ورود موفقیت آمیز بود", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("نام کاربری یا گذرواژه اشتباه است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ckShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (ckShowPassword.Checked)
                this.txtPassword.PasswordChar = '\0';

            else
                this.txtPassword.PasswordChar = '*';

        }
    }
}
