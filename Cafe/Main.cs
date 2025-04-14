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
using Cafe.Products;
using Cafe.Tools;
using Cafe.ViewModels.Customer;
using Cafe.ViewModels.Product;

namespace Cafe
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnFood_Click(object sender, EventArgs e)
        {
            Products.Products products = new Products.Products();
            products.ShowDialog();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            customers.ShowDialog();
            this.LoadCustomers();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            dgvCustomers.AutoGenerateColumns = false;
            dgvProducts.AutoGenerateColumns = false;
            this.LoadCustomers();
            this.SetInputsValues();
        }

        private void LoadCustomers(string filter = "")
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (filter == "")
                {
                    this.dgvCustomers.DataSource = db.CustomerRepository.GetListOfCustomers();
                }
                else
                {
                    this.dgvCustomers.DataSource = db.CustomerRepository.GetListOfCustomers().Where(c => c.CustomerName.Contains(filter)).ToList();
                }
            }
        }

        private void SetInputsValues()
        {
            if (dgvCustomers.CurrentRow != null)
            {
                this.txtCustomer.Text = this.dgvCustomers.CurrentRow.Cells["CustomerName"].Value.ToString();
                if (this.dgvCustomers.CurrentRow.Cells["Address"].Value != null)
                {
                    this.txtAddress.Text = this.dgvCustomers.CurrentRow.Cells["Address"].Value.ToString();
                }
                else
                {
                    this.txtAddress.Text = "";
                }
            }
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.SetInputsValues();
        }

        private void txtSearchCustomer_TextChanged(object sender, EventArgs e)
        {
            this.LoadCustomers(this.txtSearchCustomer.Text);
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            AddProduct frmAddProduct = new AddProduct();

            if (frmAddProduct.ShowDialog() == DialogResult.OK)
            {
                ProductsListViewModel product = frmAddProduct.Product;
                bool isAdded = true;
                foreach (DataGridViewRow item in this.dgvProducts.Rows)
                {
                    if (item.Cells[0].Value.ToString() == product.ProductID.ToString())
                    {
                        int count = int.Parse(item.Cells[3].Value.ToString()) + product.Count;
                        item.Cells[3].Value = count;
                        isAdded = false;
                        break;
                    }
                }
                if (isAdded)
                {
                    this.dgvProducts.Rows.Add(product.ProductID, product.ProductName, product.ProductPrice, product.Count, product.TotalPrice);
                }
                this.SetDatas();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvProducts.CurrentRow != null)
            {
                if (MessageBox.Show("آیا از حذف محصول مطمعن هستید؟", "توجه", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    this.dgvProducts.Rows.Remove(this.dgvProducts.CurrentRow);
                    this.SetDatas();
                }
            }
            else
            {
                MessageBox.Show("هیچ محصولی انتخاب نشده", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvProducts.CurrentRow != null)
            {
                frmEditCount frmEditCount = new frmEditCount();
                frmEditCount.Count = int.Parse(this.dgvProducts.CurrentRow.Cells[3].Value.ToString());
                if (frmEditCount.ShowDialog() == DialogResult.OK)
                {
                    this.dgvProducts.CurrentRow.Cells[3].Value = frmEditCount.Count;
                    this.SetDatas();
                }
            }
            else
            {
                MessageBox.Show("هیچ محصولی انتخاب نشده", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void SetDatas()
        {
            int totalPrice = int.Parse(this.dgvProducts.CurrentRow.Cells["ProductPrice"].Value.ToString()) * int.Parse(this.dgvProducts.CurrentRow.Cells["Count"].Value.ToString());
            this.dgvProducts.CurrentRow.Cells[4].Value = totalPrice;
            this.lblTotalProducts.Text = this.dgvProducts.Rows.Count.ToString();
            totalPrice = 0;
            foreach(DataGridViewRow item in this.dgvProducts.Rows)
            {
                totalPrice += int.Parse(item.Cells["TotalPrice"].Value.ToString());
            }
            this.lblTotalPrice.Text = totalPrice.ToString();


        }
    }
}
