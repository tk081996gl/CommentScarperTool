using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommentScarperTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Collection a = new Collection();
            if (!CheckExstForm("Collection"))
            {
                a.MdiParent = this;
                a.Show();
            }
            else ActiveChildForm("Collection");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Analysis a = new Analysis();
            if (!CheckExstForm("Analysis"))
            {
                a.MdiParent = this;
                a.Show();
            }
            else ActiveChildForm("Analysis");
        }
        private bool CheckExstForm(string name)
        {
            bool check = false;
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Text == name)
                {
                    check = true;

                }
            }
            return check;
        }

        private void ActiveChildForm(string name)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Name == name)
                {
                    frm.Activate();
                    break;
                }
            }

        }
    }
}
