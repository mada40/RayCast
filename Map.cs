using System;
using System.Drawing;
using System.Linq;

namespace WindowsFormsApp2
{
    class Map
    {
        static public Bitmap bmp;
        static private Graphics g;
        static private Pen pen;

        public static string Rect;
        public static string Prism;

        public float Size { get; set; }
        public float Zoom { get; set; }

        private float center;

        SolidBrush brush;
        Font font;
        PointF pointF0;
        PointF pointF1;
        PointF pointF2;
        PointF pointF3;
        string str0;
        string str1;

        public Map(float size, float zoom)
        {
            Size = size;
            Zoom = zoom;
            bmp = new Bitmap((int)Size, (int)Size);

            pen = new Pen(Color.White, 1);
            g = Graphics.FromImage(bmp);

            center = Size / 2;

            float F = 15;
            brush = new SolidBrush(Color.White);
            font = new Font(FontFamily.GenericSansSerif, F, FontStyle.Regular);
            pointF0 = new PointF(0, 0 * F);
            pointF1 = new PointF(0, 2 * F);
            pointF2 = new PointF(0, 4 * F);
            pointF3 = new PointF(0, 6 * F);

        }

        public Bitmap Draw(World world, Camera camera)
        {
            float k = Size * Zoom / (2 * camera.R);
            PointF pointCenter = new PointF(center, center);
            g.Clear(Color.FromArgb(0, Color.Black));
            g.DrawEllipse(new Pen(Color.White, 5), center - 2.5F, center - 2.5F, 5, 5);
            PointF[] PointFs;
            for (var i = 0; i < world.QuanPrism; i++)
            {
                PointFs = world.Prisms[i].PointFs.ToArray();
                for (int j = 0; j < world.Prisms[i].N; j++)
                {
                    PointFs[j] = Transform(PointFs[j]);
                }
                g.DrawPolygon(pen, PointFs);
            }

            str0 = camera.Location.ToString();
            str1 = camera.TotalH.ToString();

            g.DrawString(str0, font, brush, pointF0);
            g.DrawString(str1, font, brush, pointF1);
            g.DrawString(Rect, font, brush, pointF2);
            g.DrawString(Prism, font, brush, pointF3);

            g.DrawLine(pen, center, center, center, 0);



            PointF ray = new PointF();
            double ArcRay = Math.PI * 1.5 + camera.FovRad / 2.0;
            ray.X = center + center * (float)Math.Cos(ArcRay);
            ray.Y = center + center * (float)Math.Sin(ArcRay);
            g.DrawLine(pen, pointCenter, ray);


            ray = new PointF();
            ArcRay = Math.PI * 1.5 - camera.FovRad / 2.0;
            ray.X = center + center * (float)Math.Cos(ArcRay);
            ray.Y = center + center * (float)Math.Sin(ArcRay);
            g.DrawLine(pen, pointCenter, ray);

            float arc = (float)(camera.FovRad * 180.0 / Math.PI);
            g.DrawArc(pen, 0, 0, 2 * center, 2 * center, 270 - arc / 2.0F, arc);

            return bmp;

            PointF Transform(PointF p)
            {
                p = Vector.Scale(p, camera.Location, k);
                p.X = center - p.X;
                p.Y = center - p.Y;
                p = Vector.Rotation(p, new PointF(center, center), camera.Arc - Math.PI / 2);
                return p;
            }

        }
    }
}
