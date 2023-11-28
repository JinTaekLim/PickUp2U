﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace PickUp2U
{
    public partial class Shopform : Form
    {
        DBClass dbc;
        public Shopform()
        {
            InitializeComponent();
            dbc = new DBClass();
        }
        private void shop_list_Click(object sender, EventArgs e)
        {
            try
            {
                dbc.DB_Open();
                DBGrid.DataSource = dbc.PhoneTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DBGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataRowView selectedRow = (DataRowView)DBGrid.Rows[e.RowIndex].DataBoundItem;

                    txtid.Text = selectedRow["shop_name"].ToString();
                    txtName.Text = selectedRow["shop_location"].ToString();
                    txtNumber.Text = selectedRow["shop_Number"].ToString();
                    shop_id.Text = "매장번호 = " + selectedRow["shop_id"].ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Shop_addBtn_Click(object sender, EventArgs e)
        {
            try
            {

                int maxShopId = 0;


                foreach (DataRow row in dbc.PhoneTable.Rows)
                {
                    int currentId;
                    if (int.TryParse(row["SHOP_ID"].ToString(), out currentId))
                    {
                        if (currentId > maxShopId)
                        {
                            maxShopId = currentId;
                        }
                    }
                }


                int newShopId = maxShopId + 1;


                string shopName = txtid.Text;
                string shopLocation = txtName.Text;
                string shopNumber = txtNumber.Text;



                DataRow newRow = dbc.PhoneTable.NewRow();
                newRow["SHOP_ID"] = newShopId.ToString();
                newRow["PRODUCT_ID"] = DBNull.Value; // NULL
                newRow["SHOP_NAME"] = shopName;
                newRow["SHOP_NUMBER"] = shopNumber;
                newRow["SHOP_LOCATION"] = shopLocation;


                dbc.PhoneTable.Rows.Add(newRow);


                dbc.DBAdapter.Update(dbc.DS, "shop");


                dbc.DB_Open();
                DBGrid.DataSource = dbc.PhoneTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Shop_DelBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(shop_id.Text))
            {
                MessageBox.Show("삭제할 매장번호를 선택하세요.");
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(shop_id.Text))
                {
                    string selectedShopId = shop_id.Text.Replace("매장번호 = ", "");

                    DataRow[] rows = dbc.PhoneTable.Select($"SHOP_ID = {selectedShopId}");

                    if (rows.Length > 0)
                    {
                        rows[0].Delete();


                        dbc.DBAdapter.Update(dbc.DS, "shop");


                        dbc.DB_Open();
                        DBGrid.DataSource = dbc.PhoneTable.DefaultView;
                    }
                    else
                    {
                        MessageBox.Show("해당 매장번호를 찾을 수 없습니다.");
                    }
                }
                else
                {
                    MessageBox.Show("삭제할 매장번호를 선택하세요.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Shop_UdBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(shop_id.Text) || string.IsNullOrWhiteSpace(txtid.Text) || string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtNumber.Text))
            {
                MessageBox.Show("값이 비어있습니다. 값을 입력하세요.");
                return;
            }

            string selectedShopId = shop_id.Text.Replace("매장번호 = ", "");

            DataRow[] rows = dbc.PhoneTable.Select($"SHOP_ID = {selectedShopId}");

            if (rows.Length > 0)
            {
                rows[0]["SHOP_NAME"] = txtid.Text;
                rows[0]["SHOP_NUMBER"] = txtNumber.Text;
                rows[0]["SHOP_LOCATION"] = txtName.Text;


                dbc.DBAdapter.Update(dbc.DS, "shop");


                dbc.DB_Open();
                DBGrid.DataSource = dbc.PhoneTable.DefaultView;
            }
            else
            {
                MessageBox.Show("해당 매장번호를 찾을 수 없습니다.");
            }
        }


        // ↑ SHOP 관리  ↓PRODUCT 관리

        private void Product_list_Click(object sender, EventArgs e)
        {
            try
            {
                dbc.DB_Open_Product();
                DBGrid_PD.DataSource = dbc.PhoneTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Pd_addBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int maxProductId = 0;

                foreach (DataRow row in dbc.PhoneTable.Rows)
                {
                    int currentId;
                    if (int.TryParse(row["PRODUCT_ID"].ToString(), out currentId))
                    {
                        if (currentId > maxProductId)
                        {
                            maxProductId = currentId;
                        }
                    }
                }

                int newProductId = maxProductId + 1;

                string productName = Pd_name.Text;
                string productPrice = Pd_price.Text;
                string productStock = Pd_stock.Text;


                DataRow newProductRow = dbc.PhoneTable.NewRow();
                newProductRow["PRODUCT_ID"] = newProductId.ToString();
                newProductRow["PRODUCT_NAME"] = productName;
                newProductRow["PRICE"] = productPrice;
                newProductRow["STOCK_QUANTITY"] = productPrice;
                // 다른 제품 정보도 추가

                dbc.PhoneTable.Rows.Add(newProductRow);

                dbc.DBAdapter.Update(dbc.DS, "product");

                dbc.DB_Open_Product();
                DBGrid_PD.DataSource = dbc.PhoneTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Pd_UdBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Pd_name.Text) || string.IsNullOrWhiteSpace(Pd_price.Text) || string.IsNullOrWhiteSpace(Pd_stock.Text))
            {
                MessageBox.Show("값이 비어있습니다. 값을 입력하세요.");
                return;
            }

            string selectedProductId = Pd_id.Text.Replace("제품번호 = ", "");

            DataRow[] rows = dbc.PhoneTable.Select($"PRODUCT_ID = {selectedProductId}");

            if (rows.Length > 0)
            {
                rows[0]["PRODUCT_NAME"] = Pd_name.Text;
                rows[0]["PRICE"] = Pd_price.Text;
                rows[0]["STOCK_QUANTITY"] = Pd_stock.Text;


                dbc.DBAdapter.Update(dbc.DS, "product");

                dbc.DB_Open_Product();
                DBGrid_PD.DataSource = dbc.PhoneTable.DefaultView;
            }
            else
            {
                MessageBox.Show("해당 제품번호를 찾을 수 없습니다.");
            }
        }




        private void Pd_Del_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Pd_id.Text))
            {
                MessageBox.Show("삭제할 제품번호를 선택하세요.");
                return;
            }

            try
            {
                string selectedProductId = Pd_id.Text.Replace("제품번호 = ", "");

                // SHOP 테이블에서 해당 제품을 참조하는 레코드를 삭제합니다.
                DataRow[] shopRows = dbc.PhoneTable.Select($"PRODUCT_ID = {selectedProductId}");
                foreach (DataRow shopRow in shopRows)
                {
                    shopRow.Delete();
                }

                // 변경된 내용을 데이터베이스에 반영합니다.
                dbc.DBAdapter.Update(dbc.DS, "SHOP");

                // 업데이트된 데이터를 보여주기 위해 재로드하거나 그리드를 새로고침합니다.
                dbc.DB_Open_Product();
                DBGrid_PD.DataSource = dbc.PhoneTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void DBGrid_PD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataRowView selectedRow = (DataRowView)DBGrid_PD.Rows[e.RowIndex].DataBoundItem;

                    Pd_name.Text = selectedRow["PRODUCT_NAME"].ToString();
                    Pd_price.Text = selectedRow["PRICE"].ToString();
                    Pd_stock.Text = selectedRow["STOCK_QUANTITY"].ToString();
                    Pd_id.Text = selectedRow["PRODUCT_ID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
