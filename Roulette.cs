using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 賭場輪盤
{
    public class Roulette : Panel
    {
        private Form1 form1;

        public PictureBox picture { get; private set; }
        public PictureBox point { get; private set; }
        public Label nowstatus { get; private set; }

        Size startsize;
        Point startpicturelocation;
        Point startpointlocation;
        private Stopwatch timecount;
        public float angle { get; private set; } = 0;
        private float time, allspeed, speed;

        private int end = 0;

        public new Size Size
        {
            set
            {
                base.Size = value;

                startsize = new Size((int)(110f / 119f * base.Size.Width), (int)(110f / 119f * base.Size.Width));
                startpicturelocation = new Point((int)(2f / 119f * base.Size.Width), (int)(5f / 33f * base.Size.Height));

                nowstatus.Font = new Font("微軟正黑體", 36f / 595f * base.Size.Width, FontStyle.Regular, GraphicsUnit.Point, ((byte)(136)));
                nowstatus.Location = new Point(startpicturelocation.X + startsize.Width / 2 - nowstatus.Size.Width / 2, (int)(1f / 66f * base.Size.Height));

                point.Size = new Size((int)(1f / 11f * startsize.Width), (int)(19f / 275f * startsize.Height));
                startpointlocation = new Point(startsize.Width - point.Size.Width / 2, startsize.Height / 2 - point.Size.Height / 2);

                picture.Image = Rotating((System.Drawing.Image)(form1.resources.GetObject("pictureBox1.Image")), (int)angle, startsize.Width, startsize.Height);
                picture.Location = new Point(startpicturelocation.X - (picture.Size.Width - startsize.Width) / 2, startpicturelocation.Y - (picture.Size.Height - startsize.Height) / 2);
                point.Location = new Point(startpointlocation.X + (picture.Size.Width - startsize.Width) / 2, startpointlocation.Y + (picture.Size.Height - startsize.Height) / 2);
            }
            get
            {
                return base.Size;
            }
        }

        public Roulette(Form1 form1, float time, float speed) : base()
        {
            timecount = new Stopwatch();

            this.form1 = form1;
            this.time = time;
            this.allspeed = speed;


            nowstatus = new Label();
            picture = new PictureBox();
            point = new PictureBox();
            ((ISupportInitialize)(picture)).BeginInit();
            ((ISupportInitialize)(point)).BeginInit();

            nowstatus.AutoSize = true;
            nowstatus.Font = new System.Drawing.Font("微軟正黑體", 36f, FontStyle.Regular, GraphicsUnit.Point, ((byte)(136)));
            nowstatus.Location = new System.Drawing.Point(232, 30);
            nowstatus.Name = "nowstatus";
            nowstatus.Size = new System.Drawing.Size(132, 48);
            nowstatus.TabIndex = 2;
            nowstatus.Text = "";

            picture.Image = ((System.Drawing.Image)(form1.resources.GetObject("pictureBox1.Image")));
            picture.Location = new System.Drawing.Point(10, 10);
            picture.Name = "picture";
            picture.Size = new System.Drawing.Size(550, 550);
            picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            picture.TabIndex = 0;
            picture.TabStop = false;

            point.BackColor = System.Drawing.Color.Transparent;
            point.Image = ((System.Drawing.Image)(form1.resources.GetObject("pictureBox2.Image")));
            point.Location = new Point(picture.Size.Width - picture.Size.Width / 2, picture.Size.Height / 2 - picture.Size.Height / 2);

            point.Name = "point";
            point.Size = new System.Drawing.Size(50, 38);
            point.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            point.TabIndex = 1;
            point.TabStop = false;

            Controls.Add(nowstatus);
            Controls.Add(picture);
            picture.Controls.Add(point);

            startsize = picture.Size;
            startpicturelocation = picture.Location;
            startpointlocation = point.Location;

        }

        public void Start(bool big = false)
        {
            if (!timecount.IsRunning)
            {
                nowstatus.Text = "";

                speed = allspeed - Math.Abs(Guid.NewGuid().GetHashCode()) % 1000;

                int now = Math.Abs(Guid.NewGuid().GetHashCode()) % 10;

                if (!big) end = now % 10 == 0 ? 1 : 0;
                else end = 2;
                //nowstatus.Text = "" + ;

                //end = 2;

                timecount.Restart();
            }
        }
        bool xxx = false;
        public void UpdateRoulette()
        {
            float nowtime = (float)timecount.ElapsedMilliseconds / 1000f;
            nowtime = nowtime > time ? time : nowtime;
            float realspeed = speed * (float)Math.Pow((1 - Math.Abs(nowtime - time / 2) / (time / 2)), 2) * (float)Math.Sin(nowtime / time * Math.PI);

            /*realspeed = 50;
            nowstatus.Text = (angle <= 94 || angle >= 101 ? "沒" : "大");// + " " + angle + " " + form1.deltatime;
            if (angle > 94 && angle < 101) realspeed = 0;
            else if (xxx) realspeed = 0;*/

            if (timecount.IsRunning)
            {
                if (end != 2 && realspeed < 100)
                {
                    realspeed = 100;
                }
                else if (realspeed < 500)
                {
                    if (nowtime > time / 2)
                    {
                        if (!xxx)
                        {
                            if (angle > 0 && angle < 15) xxx = true;
                            realspeed = 500;
                        }
                        else
                        {
                            realspeed = (angle < 80 ? (80 - angle) / 80 * (500 - 100) : 0) + 100;
                        }
                    }
                }
            }
            else realspeed = 0;

            angle = (angle + realspeed * form1.deltatime) % 360;
            picture.Image = Rotating((System.Drawing.Image)(form1.resources.GetObject("pictureBox1.Image")), (int)angle, startsize.Width, startsize.Height);
            picture.Location = new Point(startpicturelocation.X - (picture.Size.Width - startsize.Width) / 2, startpicturelocation.Y - (picture.Size.Height - startsize.Height) / 2);
            point.Location = new Point(startpointlocation.X + (picture.Size.Width - startsize.Width) / 2, startpointlocation.Y + (picture.Size.Height - startsize.Height) / 2);

            nowstatus.Font = new Font("微軟正黑體", 36f / 595f * base.Size.Width, FontStyle.Regular, GraphicsUnit.Point, ((byte)(136)));
            nowstatus.Location = new Point(startpicturelocation.X + startsize.Width / 2 - nowstatus.Size.Width / 2, (int)(1f / 66f * base.Size.Height));

            //nowstatus.Text = angle.ToString();

            if (realspeed == 100 && nowtime > time / 2)
            {
                switch (end)
                {
                    case 0:
                        {
                            if ((angle + 360 - 36 + 2) % 60 >= 6)
                            {
                                timecount.Stop();
                                nowstatus.Text = "沒中";
                                xxx = false;
                            }
                            break;
                        }
                    case 1:
                        {
                            if ((angle + 360 - 36 + 2) % 60 > 3 && (angle + 360 - 36 + 2) % 60 < 6 && (angle <= 94 || angle >= 101))
                            {
                                timecount.Stop();
                                nowstatus.Text = "小獎";
                                xxx = false;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (angle > 96 && angle < 101)
                            {
                                timecount.Stop();
                                nowstatus.Text = "大獎";
                                xxx = false;
                            }
                            break;
                        }
                }
            }

            //if (timecount.ElapsedMilliseconds > (time * 1000)) timecount.Stop();
        }

        private Image Rotating(Image image, int angle, int w, int h)
        {
            Bitmap b = new Bitmap(image);
            angle = -angle % 360;

            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            //原图的宽和高
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));

            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            Graphics g = Graphics.FromImage(dsImage);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);

            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);

            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);

            //重至绘图的所有变换
            g.ResetTransform();

            g.Save();
            g.Dispose();
            return dsImage;
        }
    }
}