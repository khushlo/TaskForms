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
    public partial class ProductCategory : Form
    {
        private static bool isAdd = false;
        private static int catId = 0;
        private static int currentRow;
        public ProductCategory()
        {
            InitializeComponent();
        }

        private void ProductCategory_Load(object sender, EventArgs e)
        {
            GetAllCategories();
            dataGridView1.Height = this.Height;
            dataGridView1.Width = this.Width/3;
            dataGridView1.Columns[0].HeaderText = "Category Id";
            dataGridView1.Columns[1].HeaderText = "Category Name";
        }

        private void GetAllCategories()
        {
            DataTable categories;
            using (DBHandler db = new DBHandler())
            {
                categories = db.GetAllCategories();
            }
            dataGridView1.DataSource = categories;
            if(categories.Rows.Count > 0)
            {
                currentRow = 0;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            txtCategory.Text = string.Empty;
            isAdd = true;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            var rows = dataGridView1.Rows;
            if (currentRow >= 0)
            {
                var row = rows[currentRow];
                panel1.Enabled = true;
                txtCategory.Text = row.Cells[1].Value.ToString();
                catId = Convert.ToInt32(row.Cells[0].Value);
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
                    var categoryId = Convert.ToInt32(row.Cells[0].Value);
                    using (DBHandler db = new DBHandler())
                    {
                        db.ValidateCategoryReference(categoryId);
                        db.DeleteCategory(categoryId);
                        MessageBox.Show("Deleted Successfully");
                        GetAllCategories();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }           
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var categoryName = txtCategory.Text;
            if (string.IsNullOrWhiteSpace(categoryName))
                return;
            try
            {
                using (DBHandler db = new DBHandler())
                {
                    if (isAdd)
                    {
                        db.AddCategory(categoryName);
                        GetAllCategories();
                        MessageBox.Show("Category Created");
                    }
                    else
                    {
                        db.EditCategory(catId, categoryName);
                        GetAllCategories();
                        MessageBox.Show("Category Updated");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                txtCategory.Text = string.Empty;
                panel1.Enabled = false;
            }
        }


        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            currentRow = e.RowIndex;
            txtCategory.Text = string.Empty;
            panel1.Enabled = false;
        }

    }
}
