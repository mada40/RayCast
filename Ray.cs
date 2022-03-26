using System;
using System.Collections.Generic;
using System.Drawing;
namespace WindowsFormsApp2
{
    class Ray
    {
        private PointF Start { get; set; }
        private PointF End { get; set; }
        private int Step { get; set; }
        private double InitialLength { get; set; }
        private double Arc { get; set; }
        private bool Refl { get; set; }
        private Camera camera;
        public Ray(PointF start, PointF end, int step, double initialLength, double arc, bool refl, Camera _camera)
        {
            Start = start;
            End = end;
            Step = step;
            InitialLength = initialLength;
            Arc = arc;
            Refl = refl;
            camera = _camera;
        }
        public Ray(Camera _camera, int step)
        {
            Start = _camera.Location;
            camera = _camera;
            Step = step;
            Refl = false;
            InitialLength = 0;
            double ArcRay = _camera.FovRad * step / _camera.CountStep + _camera.Arc - 0.5 * _camera.FovRad;
            Arc = ArcRay;
            float X = _camera.Location.X + _camera.R * (float)Math.Cos(ArcRay);
            float Y = _camera.Location.Y + _camera.R * (float)Math.Sin(ArcRay);
            End = new PointF(X, Y);
        }
        public enum Position
        {
            None = 0,
            Left = 1,
            Right = 2,
            Line = 3,
        }
        public MyRectangle[] Cast(World w1, IList<int> vs, out Position[] positions, float w, double tan)
        {
            Wall wall;
            bool Ins;
            int Num;
            double Min;
            List<(RectWall, RectFloor)> rectPrism = new List<(RectWall, RectFloor)>();
            positions = new Position[w1.QuanPrism];
            for (int q = 0; q < vs.Count; q++)
            {
                double Max = -camera.R;
                int MinN = 0;
                Min = camera.R + camera.R;

                Num = vs[q];
                positions[Num] = Position.None;

                Prism prism = w1.Prisms[Num];
                double cos = Math.Abs(Math.Cos(Arc - camera.Arc));
                for (int i = 0; i < prism.N; i++)
                {
                    PointF b0 = prism.walls[i].PointFs[0];
                    PointF b1 = prism.walls[i].PointFs[1];
                   
                    bool RayBool = Vector.Intersection(Start, End, b0, b1, out double[] Lengths, out float K);
                    positions[Num] = K > 0 ? Position.Left : Position.Right;
                    double RayLength = Lengths[(int)Vector.InterLenght.CameraIntersection];
                    if (RayLength == 0)
                    {
                        continue;
                    }
                    if (RayBool)
                    {
                        RayLength = RayLength * cos + InitialLength;
                        if (RayLength < Min)
                        {
                            Min = RayLength;
                            MinN = i;
                        }
                        Max = Math.Max(Max, RayLength);

                    }
                }


                if (Vector.Inside(prism.PointFs, End))
                {
                    Max = camera.R * cos;
                }
                Ins = Vector.Inside(prism.PointFs, Start);
                if ((Min == Max && Refl) || Ins)
                {
                    Min = InitialLength;
                }

                wall = prism.walls[MinN];
                if (Min <= camera.R && Max > 0)
                {
                    RectWall rw = new RectWall(wall, Step, Min, camera, w, tan);
                    rw.ReflectionRect = Reflection();
                    RectFloor rf = new RectFloor(prism, Step, Min, Max, Arc, camera, w, tan);
                    rectPrism.Add((rw, rf));
                    positions[Num] = Position.Line;
                }
            }

            RectangleF window = new RectangleF(new PointF(0, 0), camera.GetSizeWindow());
            return MyRectangle.GetRectangles(rectPrism, window);

            MyRectangle[] Reflection()
            {
                if (wall.Reflection && !Ins && !Refl)
                {
                    PointF r0 = Vector.Intersection(Start, End, wall.PointFs[0], wall.PointFs[1]);
                    PointF r1 = Vector.Reflection(End, wall.PointFs[0], wall.PointFs[1]);
                    Ray ray = new Ray(r0, r1, Step, Min, Arc, true, camera);
                    int[] array = new int[w1.QuanPrism - 1];
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (i != Num)
                        {
                            array[i] = i;
                        }
                    }

                    return ray.Cast(w1, array, out _, w, tan);
                }
                else
                {
                    return null;
                }
            }

        }

        

    }
}
