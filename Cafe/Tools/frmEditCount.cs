using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafe.Tools
{
    public partial class frmEditCount : Form
    {
        public int Count { get; set; }
        public frmEditCount()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateInputs())
            {
                this.Count = int.Parse(txtCount.Text);
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateInputs()
        {
            try
            {
                int test = int.Parse(this.txtCount.Text);
                return true;
            }
            catch
            {
                MessageBox.Show("ورودی اشتباه است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void frmEditCount_Load(object sender, EventArgs e)
        {
            this.txtCount.Text = this.Count.ToString();
        }
    }
}
