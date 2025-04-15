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
using Cafe.ViewModels.Product;

namespace Cafe.Tools
{
    public partial class AddProduct : Form
    {
        public ProductsListViewModel Product { get; set; }
        public AddProduct()
        {
            InitializeComponent();
        }

        private bool ValidateInputes()
        {
            if (this.dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("محصولی انتخاب نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (this.TestCount())
            {
                MessageBox.Show("تعداد درست وارد نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool TestCount()
        {
            try
            {
                int test = int.Parse(txtCount.Text);
                if (test <= 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return true;
            }
        }

        private void BindGrid(string filter = "")
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (filter == "")
                {
                    this.dgvProducts.DataSource = db.ProductRepository.Get(p => p.Is_Active == true);
                }
                else
                {
                    this.dgvProducts.DataSource = db.ProductRepository.Get(p => p.Is_Active == true && p.ProductName.Contains(filter) || p.ProductPrice.ToString().Contains(filter));
                }
            }
        }


        private void AddProduct_Load(object sender, EventArgs e)
        {
            this.dgvProducts.AutoGenerateColumns = false;
            this.BindGrid();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            this.BindGrid(txtSearch.Text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.ValidateInputes())
            {
                this.Product = new ProductsListViewModel()
                {
                    ProductID = int.Parse(this.dgvProducts.CurrentRow.Cells[0].Value.ToString()),
                    Count = int.Parse(this.txtCount.Text),
                    ProductName = this.dgvProducts.CurrentRow.Cells[1].Value.ToString(),
                    ProductPrice = int.Parse(this.dgvProducts.CurrentRow.Cells[2].Value.ToString()),
                    TotalPrice = int.Parse(this.dgvProducts.CurrentRow.Cells[2].Value.ToString()) * int.Parse(this.txtCount.Text),
                };
                this.DialogResult = DialogResult.OK;
            }

        }

    }
}
