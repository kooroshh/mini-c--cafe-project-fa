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
    public partial class frmChangePassword : Form
    {
        public frmChangePassword()
        {
            InitializeComponent();
        }

        private bool ValidateInputes()
        {

            if (this.txtLastPass.Text == "")
            {
                MessageBox.Show("پسورد قدیمی نمیتواند خالی باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (this.txtNewPass.Text == "")
            {
                MessageBox.Show("پسورد جدید نمیتواند خالی باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (this.txtNewPassAgain.Text == "")
            {
                MessageBox.Show("تکرار پسورد جدید نمیتواند خالی باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (this.txtNewPass.Text != this.txtNewPassAgain.Text)
            {
                MessageBox.Show("پسورد های جدید یکی نیستن", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateInputes())
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    User user = db.UserRepository.Get().First();
                    if (user.password != this.txtLastPass.Text)
                    {
                        MessageBox.Show("پسورد قدیمی درست نیست", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (MessageBox.Show("آیا از تعویض پسورد اطمینان دارید؟", "خطا", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            user.password = this.txtNewPass.Text;
                            db.UserRepository.Update(user);
                            db.Save();
                            MessageBox.Show("پسورد شما با موفقیت عوض شد", "عملیات موفقیت آمیز بود", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                }
            }
        }
    }
}
