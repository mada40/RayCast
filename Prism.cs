using System;
using System.Drawing;
using System.Linq;


namespace WindowsFormsApp2
{
    class Prism : Barrier
    {
        public Wall[] walls { get; set; }

        public override PointF[] PointFs 
        {
            get => Array.AsReadOnly(_PointFs).ToArray();
            protected set
            {
                _PointFs = new PointF[value.Length];
                for (int i = 0; i < N; i++)
                {
                    _PointFs[i] = value[i];
                    walls[i].SetPointF(new PointF[] { value[i], value[(i + 1) % N] });
                }
            }
        } 
        public override float Height 
        {
            get => _Height;
            set
            {
                _Height = value;
                for (int i = 0; i < N; i++)
                {
                    walls[i].Height = _Height;
                }
            }
        }
        public override float HAH
        {
            get => _HAH;
            set
            {
                _HAH = value;
                for (int i = 0; i < N; i++)
                {
                    walls[i].HAH = _HAH;
                }
            }
        }

        public Prism(PointF[] points, Color[] colors, bool[] refl, float h, float hah)
        {
            PointF[] P = points;
            walls = new Wall[P.Length];
            for (int i = 0; i < P.Length; i++)
            {
                walls[i] = new Wall(colors[i], refl[i]);
            }
            PointFs = P;
            Height = h;
            HAH = hah;
            Color = colors[P.Length];
        }

        public Prism(RectangleF rect, Color[] colors, bool[] refl, float h, float hah)
        {
            walls = new Wall[4];
            for (int i = 0; i < 4; i++)
            {
                walls[i] = new Wall(colors[i], refl[i]);
            }

            PointF[] P = new PointF[4];
            P[0] = rect.Location;
            P[1] = new PointF(rect.Right, rect.Location.Y);
            P[2] = new PointF(rect.Right, rect.Bottom);
            P[3] = new PointF(rect.Location.X, rect.Bottom);
            PointFs = P; 
            Height = h;
            HAH = hah;
            Color = colors[4];
        }

    }
}
