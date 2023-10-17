using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cham_cong
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init();
        }

        public void init()
        {
            string sql = "select * from userinfo";
            dataGridView1.DataSource = cls0.bindingSource0;
            cls0.GetData(sql, dataGridView1, cls0.bindingSource0, cls0.dataAdapter0);
        }
    }
}
