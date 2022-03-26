using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        #region configuration

        Graphics g;
        Bitmap bmp;
        Camera camera;
        Map map;

        World world;

        #endregion

        public Form1()
        {
            int WinWidth = 1200;
            int WinHeight = 675;



            //WinHeight = 1000;
            InitializeComponent();
            Width = WinWidth;
            Height = WinHeight;

            pictureBox1.Width = Width;
            pictureBox1.Height = Height - FontHeight;
            bmp = new Bitmap(WinWidth, WinHeight);
            g = Graphics.FromImage(bmp);

            float R = 150F;
            int Step = Width / 4;

            camera = new Camera(-2, -2, R, Step, 50, Width, bmp.Height);


            map = new Map(500, 1F);
            world = new World();

            Random rnd = new Random();
            int S = 100;
            for (int i = 0; i < 1000; i++)
            {
                RectangleF rect = new RectangleF(rnd.Next(S), rnd.Next(S), 1,1);
                Color[] colors = new Color[54];
                colors[0] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                colors[1] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                colors[2] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                colors[3] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                colors[4] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                bool[] refl = { false, false, false, false };
                world.Prisms.Add(new Prism(rect, colors, refl, 1, rnd.Next(S)));
            }
         

        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {


            camera.Move();
            g.Clear(camera.BGcolor);
            

            try
            {
               camera.Rendering(g, world);
            }
            catch (Exception)
            {
                Close();
            }

            Map.Prism = world.QuanPrism.ToString();
            g.DrawImage(map.Draw(world, camera), new PointF());
            pictureBox1.Image = bmp;

            Text = FPS().ToString();

        }





        #region menegment

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            camera.Arc = e.X/100.0;
        }
        List<double> t = new List<double>();
        double answer = 0;
        private double FPS()
        {
            if (t.Count == 10)
            {
                answer = t.Average();
                t.Clear();
            }
            else
            {
                t.Add(1000.0 / camera.deltaMS);
            }
            return answer;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                camera.PressW = true;
            }
            if (e.KeyCode == Keys.S)
            {
                camera.PressS = true;
            }
            if (e.KeyCode == Keys.A)
            {
                camera.PressA = true;
            }
            if (e.KeyCode == Keys.D)
            {
                camera.PressD = true;
            }
            if (e.KeyCode == Keys.ControlKey)
            {
                camera.TotalH -= 0.1f;
            }
            if (e.KeyCode == Keys.Space)
            {
                camera.TotalH += 0.1f;
            }
            if (e.KeyCode == Keys.Right)
            {
                camera.D -= 0.1;
            }
            if (e.KeyCode == Keys.Left)
            {
                camera.D += 0.1;
            }
            if (e.KeyCode == Keys.Up)
            {
                camera.Horizon += 20;
            }
            if (e.KeyCode == Keys.Down)
            {
                camera.Horizon -= 20;
            }
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }





        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                camera.PressW = false;
            }
            if (e.KeyCode == Keys.S)
            {
                camera.PressS = false;
            }
            if (e.KeyCode == Keys.A)
            {
                camera.PressA = false;
            }
            if (e.KeyCode == Keys.D)
            {
                camera.PressD = false;
            }

        }
        #endregion
    }
}





