using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    abstract class MyRectangle
    {

        public double Min { get; set; }
        public Color Color { get; set; }
        public RectangleF RectangleF { get; set; }
        public byte Length { get; set; }


        protected float CalculatingX(int s, float w) => w * s;
        protected float CalculatingY(float th, double f, Camera camera) => camera.Horizon - (float)(th * f);
        protected float CalculatingTotalH(Barrier barrier, Camera camera) => barrier.TotalH - camera.TotalH;
        protected double CalculatingF(double max, double tan, float w) => w / (max * tan + 0.00001);
        protected Color CalculatingColor(Barrier barrier, Camera c) => EditingImage.AlphaBlending(c.BGcolor, Color.FromArgb(Length, barrier.Color));

        protected byte Function(double X, float R, double D)
        {
            byte MaxAlpha = byte.MaxValue;
            double Sqrt = MaxAlpha * Math.Sqrt(D * X * D * X + 4 * R * R - 4 * X * X);
            return (byte)((-MaxAlpha * D * X + Sqrt) / (2.0 * R));
        }
        public static MyRectangle[] GetRectangles(List<(RectWall, RectFloor)> rectPrism, RectangleF window)
        {
            SortRect(ref rectPrism);

            List<MyRectangle> RayRect = new List<MyRectangle>();
            
            for (int i = 0; i < rectPrism.Count; i++)
            {
                if (rectPrism[i].Item1.Color == Color.White)
                {
                    MyRectangle a = rectPrism[i].Item1;
                    a.RectangleF = RectangleF.Union(a.RectangleF, rectPrism[i].Item2.RectangleF);
                    RayRect.Add(a);
                }
                else
                {

                    RayRect.Add(rectPrism[i].Item1);
                    if (rectPrism[i].Item1.ReflectionRect != null)
                    {
                        RayRect.AddRange(rectPrism[i].Item1.ReflectionRect);
                    }
                    RayRect.Add(rectPrism[i].Item2);
                }
            }
            if (RayRect.Count > 0)
            {
                DeliteRect(ref RayRect, window);
            }

            return RayRect.ToArray();
        }


        private static void DeliteRect(ref List<MyRectangle> myRectangles, RectangleF window)
        {
            RectangleF rect = myRectangles.Last().RectangleF;

            List<RectangleF> control = new List<RectangleF>();
            control.Add(rect);

            int count = myRectangles.Count;

            bool Intersect = RectangleF.Intersect(window, myRectangles[count - 1].RectangleF) == new RectangleF();
            if (Intersect) myRectangles.RemoveAt(count - 1);

            
            for (int i = count - 2; i >= 0; i--)
            {
                Intersect = RectangleF.Intersect(window, myRectangles[i].RectangleF) == new RectangleF();

                //высота <= 0 или прямоугольник вне окна
                if (myRectangles[i].RectangleF.Height <= 0 || Intersect)
                {
                    myRectangles.RemoveAt(i);
                    continue;
                }

                for (int j = 0; j < control.Count; j++)
                {
                    if (Inside(control[j], myRectangles[i].RectangleF)) //прямоугольник вообще не видно
                    {
                        myRectangles.RemoveAt(i);
                        break;
                    }//прямоугольник частично видно
                    else if (control[j].IntersectsWith(myRectangles[i].RectangleF))
                    {
                        RectangleF union = RectangleF.Union(control[j], myRectangles[i].RectangleF);
                        control[j] = union;
                    }
                    else
                    {// прямоугольник ничего не пересекает
                        control.Add(myRectangles[i].RectangleF);                           
                        break;
                    }
                }


            }




           


            //bool Inside(RectangleF outside, RectangleF inside) => outside.Y <= inside.Y && outside.Bottom >= inside.Bottom;
            bool Inside(RectangleF outside, RectangleF inside) => RectangleF.Intersect(outside, inside) == inside;
            
        }
        private static void SortRect(ref List<(RectWall, RectFloor)> q)
        {
            bool comp((RectWall, RectFloor) v0, (RectWall, RectFloor) v1)
            {
                RectFloor floor0 = v0.Item2;
                RectFloor floor1 = v1.Item2;

                if (!RectFloor.Intersection(floor0, floor1) || floor0.MinH == floor1.MinH)
                {
                    return v0.Item1.Min >= v1.Item1.Min;
                }
                else if (floor0.MinH * floor1.MinH < 0)
                {
                    return floor0.MinH < 0;
                }
                else
                {
                    return Math.Abs(floor0.MinH) > Math.Abs(floor1.MinH);
                }
            }
            Sort<(RectWall, RectFloor)>.QSort(ref q, 0, q.Count - 1, comp);
        }

    }

 
}
