using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    static class Vector
    {
        static public float ProjectionX(PointF p0, PointF p1) => p1.X - p0.X;
        static public float ProjectionY(PointF p0, PointF p1) => p1.Y - p0.Y;
        static public double LengthSquared(PointF p0, PointF p1)
        {
            return ProjectionX(p0, p1) * ProjectionX(p0, p1) + ProjectionY(p0, p1) * ProjectionY(p0, p1);
        }
        static public double Length(PointF p0, PointF p1) => Math.Sqrt(LengthSquared(p0, p1));
        static public float VectorMultiplicationZ(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            return ProjectionX(p0, p1) * ProjectionY(p2, p3) - ProjectionY(p0, p1) * ProjectionX(p2, p3);
        }
        public enum InterLenght
        {
            CameraIntersection = 0,
            RayIntersection = 1,
            Barrier0Intersection = 2,
            Barrier1Intersection = 3
        }
        static public bool Intersection(PointF VectorRay0, PointF VectorRay1, PointF b0, PointF b1, out double[] intersection, out float position)
        {
            float K0 = VectorMultiplicationZ(b0, b1, b0, VectorRay0);
            float K1 = VectorMultiplicationZ(b0, b1, b0, VectorRay1);
            float K2 = VectorMultiplicationZ(VectorRay0, VectorRay1, VectorRay0, b0);
            float K3 = VectorMultiplicationZ(VectorRay0, VectorRay1, VectorRay0, b1);

            position = K3;
            intersection = new double[4];
            if (K0 * K1 > 0 || K2 * K3 > 0)
            {
                return false;
            }

            K0 = Math.Abs(K0);
            K1 = Math.Abs(K1);
            K2 = Math.Abs(K2);
            K3 = Math.Abs(K3);

            intersection[0] = (double)(Length(VectorRay0, VectorRay1) * K0) / (K0 + K1);
            intersection[1] = (double)(Length(VectorRay0, VectorRay1) * K1) / (K0 + K1);
            intersection[2] = (double)(Length(b0, b1) * K2) / (K2 + K3);
            intersection[3] = (double)(Length(b0, b1) * K3) / (K2 + K3);


            return true;
        }
        static public PointF Intersection(PointF a0, PointF a1, PointF b0, PointF b1)
        {

            float K0 = VectorMultiplicationZ(b0, b1, b0, a0);
            float K1 = VectorMultiplicationZ(b0, b1, b0, a1);
            float K2 = VectorMultiplicationZ(a0, a1, a0, b0);
            float K3 = VectorMultiplicationZ(a0, a1, a0, b1);


            if (K0 * K1 > 0 || K2 * K3 > 0)
            {
                return new PointF();
            }


            K0 = Math.Abs(K0);
            K1 = Math.Abs(K1);

            double X = (ProjectionX(a0, a1) * K0) / (K0 + K1);
            double Y = (ProjectionY(a0, a1) * K0) / (K0 + K1);
            return new PointF((float)(a0.X + X), (float)(a0.Y + Y));
        }
        static public bool Inside(PointF[] pointFs, PointF point)
        {
            int N = pointFs.Count();
            bool Control = VectorMultiplicationZ(pointFs.Last(), pointFs[0], pointFs.Last(), point) > 0;
            for (int i = 0; i < N-1; i++)
            {
                if(VectorMultiplicationZ(pointFs[i], pointFs[i + 1], pointFs[i], point) > 0 != Control)
                {
                    return false;
                }
            }
            return true;
        }
        static public PointF Rotation(PointF point, PointF center, double arc)
        {
            double Lx = ProjectionX(point, center);
            double Ly = ProjectionY(point, center);
            double X = Lx * Math.Cos(arc) + Ly * Math.Sin(arc);
            double Y = -Lx * Math.Sin(arc) + Ly * Math.Cos(arc);
            return new PointF(-(float)X + center.X, -(float)Y + center.Y);
        }
        static public PointF Scale(PointF point, PointF center, double k)
        {
            double X = ProjectionX(center, point) * k;
            double Y = ProjectionY(center, point) * k;

            return new PointF((float)X, (float)Y);
        }
        static public PointF Reflection(PointF point, PointF b0, PointF b1)
        {
            float k = (float)(Math.Max(Length(point, b0), Length(point, b1)) / Length(b0, b1));
            float X = point.X;
            float Y = point.Y;
            PointF normal = new PointF(X + ProjectionY(b0, b1) * k, Y - ProjectionX(b0, b1) * k);
            

            if (!Intersection(point, normal, b0, b1, out PointF reflPointF))
            {
                normal = new PointF(X - ProjectionY(b0, b1) * k, Y + ProjectionX(b0, b1) * k);
                Intersection(point, normal, b0, b1, out reflPointF);
            }


            return reflPointF;



            bool Intersection(PointF a0, PointF a1, PointF bb0, PointF bb1, out PointF interPointF)
            {
                float K0 = VectorMultiplicationZ(bb0, bb1, bb0, a0);
                float K1 = VectorMultiplicationZ(bb0, bb1, bb0, a1);


                if (K0 * K1 > 0)
                {
                    interPointF = new PointF();
                    return false;
                }

                K0 = Math.Abs(K0);
                K1 = Math.Abs(K1);

                double x = a0.X + (ProjectionX(a0, a1) * 2 * K0) / (K0 + K1);
                double y = a0.Y + (ProjectionY(a0, a1) * 2 * K0) / (K0 + K1);
                interPointF = new PointF((float)x, (float)y);

                return true;
            }


        }
       
    }



}
