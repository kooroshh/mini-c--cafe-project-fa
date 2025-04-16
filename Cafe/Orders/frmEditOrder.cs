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
using Cafe.ViewModels.Product;

namespace Cafe.Orders
{
    public partial class frmEditOrder : Form
    {
        public int OrderID { get; set; }
        public frmEditOrder()
        {
            InitializeComponent();
        }

        private void BindGrid(string filter = "")
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (filter != "")
                {
                    this.dgvProducts.DataSource = db.OrderRepository.GetOrderProducts(this.OrderID, filter);
                }
                else
                {
                    this.dgvProducts.DataSource = db.OrderRepository.GetOrderProducts(this.OrderID);
                }
            }
            EditTotalPrice();
        }

        private void EditTotalPrice()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                Order newOrder = db.OrderRepository.GetById(this.OrderID);
                List<ProductsListViewModel> orderProducts = db.OrderRepository.GetOrderProducts(newOrder.OrderID);
                int totalPrice = 0;
                foreach(ProductsListViewModel product in orderProducts)
                {
                    totalPrice += product.TotalPrice;
                }
                newOrder.TotalAmount = totalPrice;
                db.OrderRepository.Update(newOrder);
                db.Save();
            }
        }

        private void SetDefaultValues()
        {
            this.BindGrid();
            using (UnitOfWork db = new UnitOfWork())
            {
                this.txtAddress.Text = db.OrderRepository.GetById(this.OrderID).Address;
            }
        }


        private void frmEditOrder_Load(object sender, EventArgs e)
        {
            this.dgvProducts.AutoGenerateColumns = false;
            this.SetDefaultValues();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvProducts.CurrentRow != null)
            {
                if (MessageBox.Show("آیا از حذف این محصول مطمعن هستید؟", "توجه", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        if (db.OrderRepository.GetOrderProducts(this.OrderID).Count > 1)
                        {

                            db.OrderRepository.DeleteOrderProduct(int.Parse(this.dgvProducts.CurrentRow.Cells["ProductID"].Value.ToString()), this.OrderID);
                            db.Save();

                            this.BindGrid();
                        }
                        else
                        {
                            MessageBox.Show("یک سفارش نمیتواند بدون محصول باشد !!!", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("هیچ سفارشی انتخاب نشده", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveAddress_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("آیا از ذخیره آدرس مطمعن هستید؟", "توجه", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    Order order = db.OrderRepository.GetById(this.OrderID);
                    order.Address = this.txtAddress.Text;
                    db.OrderRepository.Update(order);
                    db.Save();
                }
                this.SetDefaultValues();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.SetDefaultValues();
        }

        private void btnEditCount_Click(object sender, EventArgs e)
        {
            if (this.dgvProducts.CurrentRow != null)
            {
                frmEditCount frmEditCount = new frmEditCount();
                frmEditCount.Count = int.Parse(this.dgvProducts.CurrentRow.Cells["Count"].Value.ToString());
                if (frmEditCount.ShowDialog() == DialogResult.OK)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        int productId = (int)this.dgvProducts.CurrentRow.Cells["ProductID"].Value;
                        Product product = db.ProductRepository.GetById(productId);
                        if (product.Is_Active == false)
                        {
                            MessageBox.Show($"محصول {product.ProductName} موجود نمیباشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            Orders_Products orderProduct = db.OrdersProductsRepository.Get(op => op.ProductID == productId && op.OrderID == this.OrderID).First();
                            orderProduct.Count = frmEditCount.Count;
                            db.OrdersProductsRepository.Update(orderProduct);
                            db.Save();
                        }
                    }
                    this.BindGrid();
                }
            }
            else
            {
                MessageBox.Show("هیچ محصولی انتخاب نشده", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddProduct frmAddProduct = new AddProduct();
            if (frmAddProduct.ShowDialog() == DialogResult.OK)
            {
                ProductsListViewModel product = frmAddProduct.Product;
                using (UnitOfWork db = new UnitOfWork())
                {
                    if (db.OrdersProductsRepository.Get(op => op.OrderID == this.OrderID && op.ProductID == product.ProductID).Any())
                    {
                        Orders_Products orderProduct =  db.OrdersProductsRepository.Get(op => op.OrderID == this.OrderID && op.ProductID == product.ProductID).First();
                        orderProduct.Count = product.Count + orderProduct.Count;
                    }
                    else
                    {
                        db.OrdersProductsRepository.Create(new Orders_Products()
                        {
                            Count = product.Count,
                            OrderID = this.OrderID,
                            ProductID = product.ProductID
                        });
                    }
                    db.Save();
                }
                this.BindGrid();
            }
        }
    }
}
