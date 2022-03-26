
using System.Collections.Generic;
using System.Drawing;



namespace WindowsFormsApp2
{
    class RectWall : MyRectangle
    {
        private MyRectangle[] _ReflectionRect;
        private Color reflColor;
        public MyRectangle[] ReflectionRect
        {
            get => _ReflectionRect;
            set
            {
                if (value == null || RectangleF == null || value.Length == 0)
                {
                    _ReflectionRect = null;
                }
                else
                {
                    List<MyRectangle> r = new List<MyRectangle>();
                    MyRectangle rect;
                    for (int i = 0; i < value.Length; i++)
                    {
                        rect = value[i];
                        rect.Color = EditingImage.AlphaBlending(reflColor, Color.FromArgb(Length, value[i].Color));
                        rect.RectangleF = RectangleF.Intersect(rect.RectangleF, RectangleF);
                        
                        if (rect.RectangleF != new RectangleF())
                        {
                            r.Add(value[i]);
                        }
 
                    }
                    _ReflectionRect = r.ToArray();
                    _ReflectionRect = value;
                }
            }
        }
        public RectWall(Wall barrier, int s, double min, Camera camera, float w, double tan)
        {
            Length = Function(min, camera.R, camera.D);


            double F = CalculatingF(min, tan, w);
            float TotalHeigh = CalculatingTotalH(barrier, camera);

            float RectX = CalculatingX(s, w);
            float RectY = CalculatingY(TotalHeigh, F, camera);
            float RectW = w;
            float RectH = (float)(barrier.Height * F);

            RectangleF RF = new RectangleF(RectX, RectY, RectW, RectH);
            RectangleF = RF;
            Min = min;
            
            Color = CalculatingColor(barrier, camera);

            reflColor = Color;

            if (barrier.Reflection)
            {
                Color = EditingImage.AlphaBlending(camera.BGcolor, Color.FromArgb((byte)(255 - Length), Color));
            }







        }


    }
}
