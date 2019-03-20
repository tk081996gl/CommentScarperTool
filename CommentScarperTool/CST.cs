using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommentScarperTool
{
    public partial class CST : Form
    {
        public CST()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-PMIF4BT\SQLEXPRESS;Initial Catalog=CommentScrapTool;Integrated Security=True");
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) From Accout Where Username='" + textBox1.Text + "'and Password ='" + textBox2.Text + "'", con);
            DataTable dataTable = new DataTable();
            sda.Fill(dataTable);
            if (dataTable.Rows[0][0].ToString() == "1")
            {
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();

            }

            else
               
                MessageBox.Show("Sai CODE or Mã Xác Nhận! Vui lòng đăng ký mã với nhà cung cấp phần mềm");
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

    }
}
