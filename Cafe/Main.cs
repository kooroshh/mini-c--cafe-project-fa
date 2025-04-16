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
using Cafe.Login;
using Cafe.Orders;
using Cafe.Products;
using Cafe.Tools;
using Cafe.Utilities.Converter;
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

        private int GetProductCode()
        {
            int codes = 0;
            foreach (DataGridViewRow item in dgvProducts.Rows)
            {
                codes += int.Parse(item.Cells[0].Value.ToString());
            }
            return codes;
        }

        private string GetOrderCode(int CustomerId)
        {
            DateTime my = DateTime.Now;
            int sum = my.Year + my.Month + my.Day + my.Hour + my.Minute + my.Second + my.Millisecond;
            int productCode = int.Parse(lblTotalProducts.Text) + this.GetProductCode();
            string orderCode = $"{CustomerId}{productCode}{sum}";
            return orderCode;
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
                this.txtCustomer.Text = dgvCustomers.CurrentRow.Cells[1].Value.ToString();
            }
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

        private void SetCustomersInputsValues()
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

        private bool ValidateCount()
        {

            foreach(DataGridViewRow item in this.dgvProducts.Rows)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    Product product = db.ProductRepository.GetById((int)item.Cells["ProductID"].Value);
                    if (product.Is_Active == false)
                    {
                        MessageBox.Show($"محصول {product.ProductName} موجود نمیباشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;
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

        private void SetDatas() // Change total price for new count in edit or add
        {
            int totalPrice = int.Parse(this.dgvProducts.CurrentRow.Cells["ProductPrice"].Value.ToString()) * int.Parse(this.dgvProducts.CurrentRow.Cells["Count"].Value.ToString());
            this.dgvProducts.CurrentRow.Cells[4].Value = totalPrice;
            this.SetInfo();
        }

        private void ShowDialog(Form form)
        {
            form.ShowDialog();
        }

        private void LoadCustomers(string filter = "")
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (filter == "")
                {
                    this.dgvCustomers.DataSource = db.CustomerRepository.GetListOfCustomers();
                    this.txtCustomer.Text = dgvCustomers.CurrentRow.Cells[1].Value.ToString();
                }
                else
                {
                    this.dgvCustomers.DataSource = db.CustomerRepository.GetListOfCustomers().Where(c => c.CustomerName.Contains(filter)).ToList();
                }
            }
        }

        private void IsLogged()
        {
            frmLogin frm = new frmLogin();
            if (frm.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
            }
        }



        private void Main_Load(object sender, EventArgs e)
        {
            this.IsLogged();
            this.dgvCustomers.AutoGenerateColumns = false;
            this.dgvProducts.AutoGenerateColumns = false;
            this.LoadCustomers();
            this.SetCustomersInputsValues();
            this.tTime.Enabled = true;
        }
        private void btnFood_Click(object sender, EventArgs e)
        {
            this.ShowDialog(new Products.Products());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.ShowDialog(new Customers());
            this.LoadCustomers();
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.SetCustomersInputsValues();
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
                    this.SetInfo();
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


        private void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            if (ValidateInputs() && ValidateCount())
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

        private void tTime_Tick(object sender, EventArgs e)
        {
            this.lblTime.Text = DateTime.Now.ToShamsiTick();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            this.ShowDialog(new frmOrders());
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword();
            if(frm.ShowDialog() == DialogResult.OK)
            {
                Application.Restart();
            }
        }
    }
}
