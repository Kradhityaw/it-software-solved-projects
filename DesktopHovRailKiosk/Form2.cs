using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopHovRailKiosk
{
    public partial class Form2 : Form
    {
        HovRailKioskEntities db = new HovRailKioskEntities();
        List<Label> labels = new List<Label>();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            routeBindingSource.DataSource = db.Route.ToList();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = e.Graphics;
            //Pen pen = new Pen(Color.Green, 2);
            //g.DrawLine(pen, 0, panel1.Height / 2, panel1.Width, panel1.Height / 2);

            //var dataCount = db.RouteDetail.Where(f => f.routeID == (int)comboBox1.SelectedValue).Count();
            //var getWidth = panel1.Width / (dataCount + 1);
            //var getCurrentWidth = getWidth / dataCount;
            //var xValue = getWidth;

            //foreach (RouteDetail r in db.RouteDetail.Where(f => f.routeID == (int)comboBox1.SelectedValue).ToList())
            //{
            //    Rectangle rect = new Rectangle(xValue - 8, panel1.Height / 2 - 8, 16, 16);
            //    Brush brush = new SolidBrush(Color.Red);

            //    g.FillRectangle(brush, rect);

            //    xValue += getWidth;
            //}

            Graphics g = e.Graphics;
            Pen greenPen = new Pen(Color.Green, 5); // Garis hijau tebal
            Brush redBrush = new SolidBrush(Color.Red);

            // Data stasiun
            var routeDetails = db.RouteDetail.Where(f => f.routeID == (int)comboBox1.SelectedValue).ToList();
            int dataCount = routeDetails.Count;

            if (dataCount > 1)
            {
                // Gambar garis hijau
                int startX = 50; // Padding kiri
                int endX = panel1.Width - 50; // Padding kanan
                int centerY = panel1.Height / 2; // Posisi vertikal di tengah panel
                g.DrawLine(greenPen, startX, centerY, endX, centerY);

                // Jarak antar stasiun
                int spacing = (endX - startX) / (dataCount - 1);

                for (int i = 0; i < dataCount; i++)
                {
                    Label label = new Label()
                    {
                        Text = routeDetails[i].Station.stationName,
                        AutoSize = true,
                        Tag = routeDetails[i],
                        Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
                    };

                    // Hitung posisi setiap stasiun
                    int stationX = startX + (i * spacing);

                    if (i % 2 != 0)
                    {
                        label.Location = new Point(stationX - 24, centerY + 32);
                    }
                    else
                    {
                        label.Location = new Point(stationX - 24, centerY - 52);
                    }

                    // Gambar kotak merah
                    Rectangle rect = new Rectangle(stationX - 8, centerY - 8, 16, 16);
                    g.FillRectangle(redBrush, rect);

                    labels.Add(label);
                }

                foreach (Label label in labels)
                {
                    if (label.Tag is RouteDetail rd)
                    {
                        if (rd.routeID == (int)comboBox1.SelectedValue)
                        {
                            panel1.Controls.Add(label);
                        }
                        else
                        {
                            panel1.Controls.Remove(label);
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            Hide();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Form1().Show();
            Hide();
        }
    }
}
