using System;
using System.Drawing;
using System.Linq;



namespace WindowsFormsApp2
{
    abstract class Barrier
    {
        protected float _Height;
        virtual public float Height { get; set; }

        protected float _HAH;
        virtual public float HAH { get; set; }

        protected PointF[] _PointFs;
        virtual public PointF[] PointFs
        {
            get => Array.AsReadOnly(_PointFs).ToArray();
            protected set => _PointFs = value;
        }

        public int N => PointFs.Length;

        public float TotalH => Height + HAH;
        public Color Color { get; set; }

        public void SetPointF(PointF[] pointFs)
        {
            PointFs = pointFs;
        }

    }
}
