using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskForms
{
    public partial class ProductMaster : Form
    {
        private static int currentRow;
        private static bool isAdd = false;
        private static int productId = 0;
        Dictionary<int, string> categories = new Dictionary<int, string>();
        public ProductMaster()
        {
            InitializeComponent();
        }

        private void ProductMaster_Load(object sender, EventArgs e)
        {
            GetAllProducts();
            GetProductCategories();
            dataGridView1.Height = this.Height;
            dataGridView1.Width = this.Width / 2;
            dataGridView1.Columns[0].HeaderText = "Product Id";
            dataGridView1.Columns[1].HeaderText = "Product Name";
            dataGridView1.Columns[2].HeaderText = "Category Name";
        }

        private void GetProductCategories()
        {
            using (DBHandler db = new DBHandler())
            {
                var cats = db.GetAllCategories();
                if (cats.Rows.Count > 0)
                {
                    foreach (DataRow category in cats.Rows)
                    {
                        categories.Add(Convert.ToInt32(category[0]), category[1].ToString());
                    }
                }
            }
            comboBox1.DataSource = categories.Select(x => x.Value).ToList();
        }

        private void GetAllProducts()
        {
            DataTable products = new DataTable();
            using (DBHandler db = new DBHandler())
            {
                products = db.GetAllProducts();
            }
            dataGridView1.DataSource = products;
            if (products.Rows.Count > 0)
            {
                currentRow = 0;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            txtProduct.Text = string.Empty;
            isAdd = true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var productName = txtProduct.Text;
            int categoryId = categories.Where(x => x.Value == comboBox1.SelectedItem.ToString()).FirstOrDefault().Key;

            if (string.IsNullOrWhiteSpace(productName))
                return;

            if (categoryId < 1)
            {
                MessageBox.Show("Please Add Category");
                return;
            }
            try
            {
                using (DBHandler db = new DBHandler())
                {
                    if (isAdd)
                    {
                        db.AddProduct(productName, categoryId);
                        GetAllProducts();
                        MessageBox.Show("Product Created");
                    }
                    else
                    {
                        db.EditProduct(productId, productName, categoryId);
                        GetAllProducts();
                        MessageBox.Show("Product Updated");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                txtProduct.Text = string.Empty;
                panel1.Enabled = false;
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            currentRow = e.RowIndex;
            txtProduct.Text = string.Empty;
            panel1.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var rows = dataGridView1.Rows;
            if (currentRow >= 0)
            {
                var row = rows[currentRow];
                panel1.Enabled = true;
                txtProduct.Text = row.Cells[1].Value.ToString();
                productId = Convert.ToInt32(row.Cells[0].Value);
                comboBox1.SelectedItem = row.Cells[2].Value.ToString();
                isAdd = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var rows = dataGridView1.Rows;
            if (currentRow >= 0)
            {
                try
                {
                    var row = rows[currentRow];
                    var productId = Convert.ToInt32(row.Cells[0].Value);
                    using (DBHandler db = new DBHandler())
                    {
                        db.DeleteProduct(productId);
                        MessageBox.Show("Deleted Successfully");
                        GetAllProducts();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
