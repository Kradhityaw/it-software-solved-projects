using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopHovLibrary
{
    public partial class Form3 : Form
    {
        HovLibraryEntities db = new HovLibraryEntities();

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            memberBindingSource.DataSource = db.Member.Where(f => f.deleted_at == null).ToList();
            bindingSource1.AddNew();
        }

        private void memberDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (memberDataGridView.Rows[e.RowIndex].DataBoundItem is Member member)
            {
                if (e.ColumnIndex == editCol.Index)
                {
                    var dataMember = db.Member.AsNoTracking().FirstOrDefault(f => f.id == member.id);
                    bindingSource1.DataSource = dataMember;
                    groupBox1.Enabled = true;
                    if (dataMember.gender is "Male") radioButton1.Checked = true; else radioButton2.Checked = true;
                }
            } 
        }

        private void memberDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (memberDataGridView.Rows[e.RowIndex].DataBoundItem is Member member)
            {
                if (e.ColumnIndex == editCol.Index) e.Value = "Edit";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (groupBox1.Controls.OfType<TextBox>().Any(f => string.IsNullOrEmpty(f.Text)))
            {
                Alert.Error("Please fill all inputs!");
                return;
            }

            if (bindingSource1.Current is Member member)
            {
                member.gender = radioButton1.Checked ? "Male" : "Female";

                db.Member.AddOrUpdate(member);
                db.SaveChanges();

                groupBox1.Enabled = false;
                OnLoad(EventArgs.Empty);
            }
        }
    }
}
