using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProiectPAWRusuMihnea
{
    public partial class ChartForm : Form
    {
        private List<Route> _routes;

        public ChartForm(List<Route> routes)
        {
            InitializeComponent();
            _routes = routes;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var stats = new Dictionary<string, int>();
            foreach (var route in _routes)
            {
                string companyName = route.company?.companyName ?? "Unknown";
                if (!stats.ContainsKey(companyName))
                    stats[companyName] = 0;
                stats[companyName]++;
            }

            Graphics g = e.Graphics;
            int width = panel1.Width;
            int height = panel1.Height;

            int margin = 40;
            int barWidth = 40;
            int space = 30;

            int x = margin;
            int max = stats.Count > 0 ? stats.Values.Max() : 1;
            int chartHeight = height - 2 * margin;

            Font labelFont = new Font("Arial", 9);
            Brush barBrush = Brushes.SkyBlue;
            Brush textBrush = Brushes.Black;

            foreach (var pair in stats)
            {
                int barHeight = (int)((pair.Value / (float)max) * chartHeight);
                int y = height - margin - barHeight;
                g.FillRectangle(barBrush, x, y, barWidth, barHeight);

                g.DrawString(pair.Value.ToString(), labelFont, textBrush, x, y - 18);
                g.DrawString(pair.Key, labelFont, textBrush, x, height - margin + 4);

                x += barWidth + space;
            }

            Pen axisPen = Pens.Black;
            g.DrawLine(axisPen, margin - 10, height - margin, width - margin + 10, height - margin); // X axis
            g.DrawLine(axisPen, margin - 10, margin, margin - 10, height - margin); // Y axis
        }
    }
}
