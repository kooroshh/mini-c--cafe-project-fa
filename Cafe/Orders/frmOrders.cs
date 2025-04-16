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
using Cafe.Utilities.Converter;
using Cafe.ViewModels.Orders;

namespace Cafe.Orders
{
    public partial class frmOrders : Form
    {
        public frmOrders()
        {
            InitializeComponent();
        }


        private void BindGrid()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<OrdersListViewModel> orders;
                string filter = this.txtSearch.Text;
                if (filter != "")
                {
                    orders = db.OrderRepository.GetOrders(o => o.OrderCode.Contains(filter) || o.CustomerName.Contains(filter));
                }
                else
                {
                    orders = db.OrderRepository.GetOrders();
                }

                IQueryable<OrdersListViewModel> filterOrders = orders.AsQueryable();

                if (this.txtFrom.Text != "    /  /")
                {
                    try
                    {
                        DateTime from = Convert.ToDateTime(this.txtFrom.Text).ToMiladi();
                        filterOrders = filterOrders.Where(o => o .OrderDate > from);
                    }
                    catch
                    {
                        MessageBox.Show("تاریخ شروع اشتباه است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (this.txtTo.Text != "    /  /")
                {
                    try
                    {
                        DateTime to = Convert.ToDateTime(this.txtTo.Text).ToMiladi();
                        filterOrders = filterOrders.Where(o => o.OrderDate < to);
                    }
                    catch
                    {
                        MessageBox.Show("تاریخ پایان اشتباه است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                this.dgvOrders.Rows.Clear();
                foreach(OrdersListViewModel item in filterOrders.ToList())
                {
                    this.dgvOrders.Rows.Add(item.OrderID, item.CustomerID, item.OrderCode, item.CustomerName, item.OrderDate.ToShamsi(), item.TotalAmount.ToToman());
                }
            }
        }

        private void frmOrders_Load(object sender, EventArgs e)
        {
            this.dgvOrders.AutoGenerateColumns = false;
            this.BindGrid();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            this.BindGrid();
        }

        private void btnRefreshOrders_Click(object sender, EventArgs e)
        {
            this.txtSearch.Text = "";
            this.txtFrom.Text = "    /  /";
            this.txtTo.Text = "    /  /";
            this.BindGrid();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            this.BindGrid();
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (this.dgvOrders.CurrentRow != null)
            {
                if (MessageBox.Show("آیا از حذف این سفارش مطمعن هستید؟", "توجه", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        int orderId = int.Parse(this.dgvOrders.CurrentRow.Cells["OrderID"].Value.ToString());
                        db.OrderRepository.DeleteWithProducts(orderId);
                        db.Save();
                    }
                    this.BindGrid();
                }
            }
            else
            {
                MessageBox.Show("هیچ سفارشی انتخاب نشده", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            if (this.dgvOrders.CurrentRow != null )
            {
                if (MessageBox.Show("با ویرایش سفارش قیمت ها بروز میشود.آیا از این کار اطمینان دارید؟", "هشدار", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    frmEditOrder frmEditOrder = new frmEditOrder();
                    frmEditOrder.OrderID = (int)this.dgvOrders.CurrentRow.Cells["OrderID"].Value;
                    frmEditOrder.ShowDialog();
                    this.BindGrid();
                }
            }
            else
            {
                MessageBox.Show("هیچ سفارشی انتخاب نشده", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("OrderCode");
            DT.Columns.Add("CustomerName");
            DT.Columns.Add("OrderDate");
            DT.Columns.Add("TotalPrice");

            foreach (DataGridViewRow item in this.dgvOrders.Rows)
            {
                DT.Rows.Add(item.Cells["OrderCode"].Value, item.Cells["CustomerName"].Value, item.Cells["OrderDate"].Value, item.Cells["TotalAmount"].Value);
            }

            string path = Application.StartupPath + @"\Report.mrt";
            stiReport.Load(path);
            stiReport.RegData("DT", DT);
            stiReport.Show();
        }
    }
}
