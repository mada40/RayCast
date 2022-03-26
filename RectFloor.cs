using System;

using System.Drawing;


namespace WindowsFormsApp2
{
    class RectFloor : MyRectangle
    {
        public float MinH { get; set; }
        public double Max { get; set; }


        public RectFloor(Prism barrier, int s, double min, double max, double ArcRay, Camera camera, float w, double tan)
        {
            float TotalHeigh;
            sbyte b;
            Length = Function(min, camera.R, camera.D);
            
            if (camera.TotalH >= barrier.TotalH)
            {
                TotalHeigh = CalculatingTotalH(barrier, camera);
                MinH = TotalHeigh;
                b = 1;
            }
            else
            {
                TotalHeigh = barrier.HAH - camera.TotalH;
                MinH = TotalHeigh + barrier.Height;
                b = -1;
            }

            double F = CalculatingF(max, tan, w);
            double MinF = CalculatingF(min, tan, w);

            float RectX = CalculatingX(s, w);
            float RectY = CalculatingY(TotalHeigh, F, camera);

            float RectYMin = camera.Horizon - (float)(TotalHeigh * MinF);
            float RectW = w;
            float RectH = (RectYMin - RectY) * b;


            RectangleF RF = new RectangleF(RectX, Math.Min(RectY, RectYMin), RectW, RectH);

            RectangleF = RF;
            Min = min;
            Color = CalculatingColor(barrier, camera);
            Max = max;
        }

        public static bool Intersection(RectFloor line, RectFloor line1)
        {
            double dx = 0.001;
            bool c0 = line1.Min < line.Max - dx && line.Min < line1.Max - dx;
            bool c1 = line.Min < line1.Max - dx && line1.Min < line.Max - dx;
            return c0 || c1;
        }
    }
}
