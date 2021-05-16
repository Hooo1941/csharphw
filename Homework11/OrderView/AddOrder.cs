﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OrderManager;

namespace OrderView
{
    public partial class AddOrder : Form
    {
        public static List<OrderDetails> Goods;
        private readonly int id = 0;
        private readonly OrderService orderService = OrderView.orderService;
        public AddOrder()
        {
            InitializeComponent();
            Goods = new List<OrderDetails>();
        }
        private void dgvGoods_DataError(object sender, DataGridViewDataErrorEventArgs e) { }

        public AddOrder(int id):this()
        {
            this.id = id;
            var temp = new Order(id, "");
            if (!Enumerable.Contains(orderService.orders, temp))
            {
                MessageBox.Show(@"订单不存在", @"提示");
                this.Close();
            }
            txtCustomer.Text = orderService.orders.Find(o => o.OrderId == id).Customer;
            Goods = orderService.orders.Find(o => o.OrderId == id).OrderDetails;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCustomer.Text == "")
            {
                MessageBox.Show(@"顾客名不能为空");
                return;
            }
            if (Goods.Count == 0)
            {
                MessageBox.Show(@"货物为空");
                return;
            }

            if (id != 0)
            {
                orderService.DeleteOrder(id);
            }
            try
            {
                var id = orderService.AddOrder(txtCustomer.Text);
                orderService.ChangeGoodIntoOrder(id, Goods);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"发生错误"+ex+@"请重新保存");
                return;
            }
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Goods.Remove((OrderDetails) bdsGoods.Current);
            bdsGoods.ResetBindings(false);
        }

        private void btnAddGood_Click(object sender, EventArgs e)
        {
            var ag = new AddGood();
            ag.ShowDialog(this);
            bdsGoods.ResetBindings(false);
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            var ag = new AddGood((OrderDetails)bdsGoods.Current);
            ag.ShowDialog(this);
            bdsGoods.ResetBindings(false);
        }

        private void AddOrder_Load(object sender, EventArgs e)
        {
            bdsGoods.DataSource = Goods;
        }
    }
}
