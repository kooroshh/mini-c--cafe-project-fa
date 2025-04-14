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
            this.SetInfo();
        }

        private void SetInfo()
        {
            this.lblTotalProducts.Text = this.dgvProducts.Rows.Count.ToString();
            int totalPrice = 0;
            foreach (DataGridViewRow item in this.dgvProducts.Rows)
            {
                totalPrice += int.Parse(item.Cells["TotalPrice"].Value.ToString());
            }
            this.lblTotalPrice.Text = totalPrice.ToString();
        }

        private void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    Order newOrder = new Order()
                    {
                        CustomerID = int.Parse(this.dgvCustomers.CurrentRow.Cells[0].Value.ToString()),
                        OrderDate = DateTime.Now,
                        Address = this.txtAddress.Text,
                        TotalAmount = int.Parse(lblTotalPrice.Text),
                        OrderCode = this.GetOrderCode(int.Parse(this.dgvCustomers.CurrentRow.Cells[0].Value.ToString())),
                    };
                    db.OrderRepository.Create(newOrder);
                    db.Save();
                    int orderID = newOrder.OrderID;
                    foreach(DataGridViewRow product in dgvProducts.Rows)
                    {
                        Orders_Products orderProduct = new Orders_Products() 
                        {
                            Count = int.Parse(product.Cells["Count"].Value.ToString()),
                            OrderID = orderID,
                            ProductID = int.Parse(product.Cells[0].Value.ToString()),
                        };
                        db.OrdersProductsRepository.Create(orderProduct);
                    }
                    db.Save();
                }
                MessageBox.Show("با موفقیت ثبت شد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Reset();
            }
        }

        private bool ValidateInputs()
        {

            if (this.txtCustomer.Text == "")
            {
                MessageBox.Show("طرف حساب انتخاب نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (this.IsWrongProductCount())
            {
                MessageBox.Show("محصولی انتخاب نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool IsWrongProductCount()
        {
            try
            {
                int test = int.Parse(this.lblTotalProducts.Text);
                if (test <= 0)
                    return true;
                return false;
            }
            catch
            {
                return true;
            }
        }

        private void Reset()
        {
            this.dgvProducts.Rows.Clear();
            this.SetInfo();
            if (dgvCustomers.Rows.Count > 0)
            {
                dgvCustomers.ClearSelection();
                dgvCustomers.Rows[0].Selected = true;
                dgvCustomers.CurrentCell = dgvCustomers.Rows[0].Cells[1];
                if (dgvCustomers.CurrentRow.Cells[2].Value != null)
                {
                    this.txtAddress.Text = dgvCustomers.CurrentRow.Cells[2].Value.ToString();
                }
                else
                {
                    this.txtAddress.Text = "";
                }
            }
        }

        private string GetOrderCode(int CustomerId)
        {
            DateTime my = new DateTime(DateTime.Now.Ticks);
            int year = my.Year;
            int month = my.Month;
            int day = my.Day;
            int hour = my.Hour;
            int minute = my.Minute;
            int second = my.Second;
            int milisecond = my.Millisecond;
            int sum = year + month + day + hour + minute + second + milisecond;
            int productCode = int.Parse(lblTotalProducts.Text) + this.GetProductCode();
            string orderCode = $"{CustomerId}{productCode}{sum}";
            return orderCode;
        }

        private int GetProductCode()
        {
            int codes = 0;
            foreach (DataGridViewRow item in dgvProducts.Rows)
            {
                codes += int.Parse(item.Cells[0].Value.ToString());
            }
            return codes;
        }

    }
}
