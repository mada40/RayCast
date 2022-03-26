using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;


namespace WindowsFormsApp2
{
    class Camera
    {
        #region 
        public int WidthWin { get; private set; }
        public int HeightWin { get; private set; }
        public PointF Location { get; set; }
        public float R { get; private set; }
        public int CountStep { get; set; }
        public double Speed { get; set; } = 0.015;

        private float _Horizon;
        public float Horizon
        {
            get
            {
                return _Horizon;
            }
            set
            {
                if (value < GetMinHorizon())
                {
                    _Horizon = GetMinHorizon();
                }
                else if (value > GetMaxHorizon())
                {
                    _Horizon = GetMaxHorizon();
                }
                else
                {
                    _Horizon = value;
                }
            }
        }
        public float TotalH { get; set; }
        public float GetMaxHorizon() => HeightWin;
        public float GetMinHorizon() => 0;
        public double Arc { get; set; }
        public SizeF GetSizeWindow() => new SizeF(WidthWin, HeightWin);


        private double _D;
        public double D
        {
            get => _D;

            set
            {
                if (value < GetMinD())
                {
                    D = GetMinD();
                }
                else if (value > GetMaxD())
                {
                    _D = GetMaxD();
                }
                else
                {
                    _D = value;
                }
            }
        }
        public double GetMaxD() => 50;
        public double GetMinD() => 0;
        public Color BGcolor = Color.Black;

        public double FovRad { get; set; }

        public double deltaMS;

        private DateTime starting = DateTime.Now;
        private DateTime Time = DateTime.Now;

        public double Tick()
        {
            double a = (DateTime.Now - Time).TotalMilliseconds;
            Time = DateTime.Now;
            return a;
        }

        public Camera(float x, float y, float r, int step, float totalH, int WinW, int WinH)
        {
            Location = new PointF(x, y);
            R = r;
            CountStep = step;
            

            FovRad = Math.PI*2.0/3.0;
            TotalH = totalH;
            WidthWin = WinW;
            HeightWin = WinH;
            D = 2;
            Arc = Math.PI + 0.5 * FovRad;
            Horizon = 2 * WinH / 3.0F;

        }
        #endregion

        public void Rendering(Graphics g, World world)
        {
            deltaMS = Tick();
            MyRectangle[] rectangle = BinaryCasting(world);
            Map.Rect = rectangle.Length.ToString();


            for (int i = 0; i < rectangle.Length; i++)
            {
                Color color = rectangle[i].Color;
                RectangleF rect = rectangle[i].RectangleF;
                g.FillRectangle(new SolidBrush(color), rect); 
            }   
        }


        private static object locker = new object();

        private MyRectangle[] BinaryCasting(World w)
        { 
            double Tan = Math.Tan(FovRad / CountStep);
            float W = (float)WidthWin / (float)CountStep;

            List<MyRectangle> rect = new List<MyRectangle>();


            Ray.Position[] p1 = M(0, Enumerable.Range(0, w.QuanPrism));

            List<int> list1 = new List<int>();

            for (int i = 0; i < w.QuanPrism; i++)
            {
                if ((p1[i] == Ray.Position.Left || p1[i] == Ray.Position.Line ) )
                {
                    list1.Add(i);
                }
            }
            Ray.Position[] p2 = M(CountStep - 1, list1);
            qqq(0, CountStep - 1, p1, p2);

            return rect.ToArray();


            //возращает расположение относительно луча
            Ray.Position[] M(int i, IEnumerable<int> vs1)
            {
                Ray ray = new Ray(this, i);
                var myRectangles = ray.Cast(w, vs1.ToList(), out Ray.Position[] pos, W, Tan) ;
                lock (locker)
                {
                    rect.AddRange(myRectangles);
                }

                
                return pos;
            }



            void qqq(int first, int last, Ray.Position[] posF, Ray.Position[] posL)
            {
                List<int> list = new List<int>();
                bool none = true;
                for (int i = 0; i < posL.Length; i++)
                {
                    bool a = posF[i] == Ray.Position.Line;
                    bool b = posL[i] == Ray.Position.Line;
                    bool c = posF[i] == Ray.Position.Left;
                    bool d = posL[i] == Ray.Position.Right;
                    
                    if ((a || b || c&&d ) )
                    {
                        list.Add(i);
                        none = false;
                    }
                }
                int n = (first + last) / 2;
                Ray.Position[] posN = M(n, list);
                if (!none && list.Count > 0)
                {
                    if (list.Count >= 16 && n - first > 1 && last - n > 1)
                    {
                        Parallel.Invoke(() => qqq(first, n, posF, posN), () => qqq(n, last, posN, posL));
                    }
                    else
                    {
                        if (n - first > 1)
                        {
                            qqq(first, n, posF, posN);
                        }
                        if (last - n > 1)
                        {
                            qqq(n, last, posN, posL);
                        }
                    }

                }
            }

        }

       

        public bool PressW = false;
        public bool PressS = false;
        public bool PressA = false;
        public bool PressD = false;
        public void Move()
        {
            double LocX = Location.X;
            double LocY = Location.Y;
            double cos_a = Math.Cos(Arc) * Speed * deltaMS;
            double sin_a = Math.Sin(Arc) * Speed * deltaMS;

            if (PressW)
            {
                LocX += cos_a;
                LocY += sin_a;
            }
            if (PressS)
            {
                LocX -= cos_a;
                LocY -= sin_a;
            }
            if (PressA)
            {
                LocX += sin_a;
                LocY -= cos_a;
            }
            if (PressD)
            {
                LocX -= sin_a;
                LocY += cos_a;
            }
            Location = new PointF((float)LocX, (float)LocY);
        }



    }
}
