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
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        public void BindGrid(string filter = "")
        {
            List<DataLayer.Product> products = new List<DataLayer.Product>();

            if (filter == "")
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    products = (List<DataLayer.Product>)db.ProductRepository.Get();
                }

            }
            else
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    products = (List<DataLayer.Product>)db.ProductRepository.Get(p => p.ProductName.Contains(filter));
                }
            }

            this.dgvProducts.Rows.Clear();
            foreach (DataLayer.Product product in products)
            {
                string isActive = (product.Is_Active) ? "موجود" : "نا موجود";
                this.dgvProducts.Rows.Add(product.ProductID, product.ProductName, product.ProductPrice.ToToman(), isActive);
            }
        }


        private void Products_Load(object sender, EventArgs e)
        {
            this.dgvProducts.AutoGenerateColumns = false;
            BindGrid();
        }

        private void btnRefreshProducts_Click(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            this.BindGrid();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            AddOrEditProduct addOrEditProduct = new AddOrEditProduct();
            if (addOrEditProduct.ShowDialog() == DialogResult.OK)
            {
                this.BindGrid();
            }
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            if (this.dgvProducts.CurrentRow != null)
            {
                AddOrEditProduct addOrEditProduct = new AddOrEditProduct();
                addOrEditProduct.ProductId = (int)this.dgvProducts.CurrentRow.Cells[0].Value;
                if (addOrEditProduct.ShowDialog() == DialogResult.OK)
                {
                    this.BindGrid();
                }
            }
            else
            {
                MessageBox.Show("لطفا یک محصول را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (this.dgvProducts.CurrentRow != null)
            {
                DataLayer.Product product = new DataLayer.Product();
                using (UnitOfWork db = new UnitOfWork())
                {
                    product = db.ProductRepository.GetById(this.dgvProducts.CurrentRow.Cells[0].Value);
                    if (MessageBox.Show($"آیا از حذف {product.ProductName} مطمعن هستید؟", "اخطار", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        db.ProductRepository.Delete(product);
                        db.Save();
                        this.BindGrid();
                    }
                }

            }
            else
            {
                MessageBox.Show("لطفا یک محصول را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            this.BindGrid(this.txtFilter.Text);
        }
    }
}
