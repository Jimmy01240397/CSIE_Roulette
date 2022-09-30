using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 賭場輪盤
{
    public partial class Form1 : Form
    {
        public ComponentResourceManager resources { get; private set; }
        Stopwatch stopwatch = new Stopwatch();

        public float deltatime
        {
            get
            {
                return (float)stopwatch.ElapsedMilliseconds / 1000;
            }
        }

        float time = 9;

        float speed = 10000;

        Roulette[] roulettes = new Roulette[5];

        string[] date;
        int count = 0;
        public Form1(string[] date)
        {
            InitializeComponent();

            this.date = date;

            resources = new ComponentResourceManager(typeof(Form1));

            int heigth = (int)(((float)Size.Height - 40f) / 2f);
            int width = (int)(119f / 132f * heigth);

            for (int i = 0; i < 5; i++)
            {
                roulettes[i] = new Roulette(this, time, speed);
                roulettes[i].Location = new System.Drawing.Point(i < 3 ? ((int)(Size.Width / 3 - width) + i * width) : (Size.Width / 2 - width) + (i - 3) * width, i < 3 ? 10 : heigth);
                roulettes[i].Size = new System.Drawing.Size(width, (int)(132f / 119f * width));
                roulettes[i].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                Controls.Add(roulettes[i]);
            }

            timer1.Start();
            stopwatch.Start();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            stopwatch.Stop();

            for (int i = 0; i < 5; i++)
            {
                roulettes[i].UpdateRoulette();
            }
            stopwatch.Restart();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool big = false;

            if (count < date.Length)
            {
                int targetmin = Convert.ToInt32(Convert.ToDateTime(date[count]).ToString("HH")) * 60 + Convert.ToInt32(Convert.ToDateTime(date[count]).ToString("mm"));
                int nowmin = Convert.ToInt32(DateTime.Now.ToString("HH")) * 60 + Convert.ToInt32(DateTime.Now.ToString("mm"));

                if (targetmin <= nowmin)
                {
                    big = true;
                    count++;
                }
            }

            if (e.KeyChar >= '1' && e.KeyChar <= '5')
            {
                roulettes[e.KeyChar - '1'].Start(big);
                big = false;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int heigth = (int)(((float)Size.Height - 40f) / 2f);
            int width = (int)(119f / 132f * heigth);

            for (int i = 0; i < 5; i++)
            {
                roulettes[i].Location = new System.Drawing.Point(i < 3 ? ((int)(Size.Width / 3 - width) + i * width) : (Size.Width / 2 - width) + (i - 3) * width, i < 3 ? 10 : heigth);
                roulettes[i].Size = new System.Drawing.Size(width, (int)(132f / 119f * width));
            }
        }
    }
}
