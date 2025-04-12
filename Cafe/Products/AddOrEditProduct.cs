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

namespace Cafe.Products
{
    public partial class AddOrEditProduct : Form
    {
        public int ProductId { get; set; }
        public AddOrEditProduct()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateInputs())
            {
                DataLayer.Product product = new DataLayer.Product();
                product.ProductName = txtPName.Text;
                product.ProductPrice = (int)txtPPrice.Value;
                product.Is_Active = ckPStatusFalse.Checked ? false : true;
                if (this.ProductId == 0)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.ProductRepository.Create(product);
                        db.Save();
                    }
                }
                else
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        product.ProductID = this.ProductId;
                        db.ProductRepository.Update(product);
                        db.Save();
                    }
                }
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateInputs()
        {
            if (this.txtPName.Text == "")
            {
                MessageBox.Show("نام نمیتواند خالی باشد", "خطا",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void AddOrEditProduct_Load(object sender, EventArgs e)
        {
            if (this.ProductId != 0)
            {
                this.Text = "ویرایش محصول";
                this.btnSave.Text = "ویرایش";
                using (UnitOfWork db = new UnitOfWork())
                {
                    DataLayer.Product product = db.ProductRepository.GetById(this.ProductId);
                    this.txtPName.Text = product.ProductName;
                    this.txtPPrice.Value = product.ProductPrice;
                    if(!product.Is_Active)
                    {
                        this.ckPStatusTrue.Checked = false;
                        this.ckPStatusFalse.Checked = true;
                    }
                }
            }
        }
    }
}
