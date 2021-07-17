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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
          
        private void productCategoryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ProductCategory catForm = new ProductCategory();
            catForm.MdiParent = this;
            catForm.Size = this.Size;
            catForm.Show();
        }

        private void productMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductMaster proForm = new ProductMaster();
            proForm.MdiParent = this;
            proForm.Size = this.Size;
            proForm.Show();
        }
    }
}
