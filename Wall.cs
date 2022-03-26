using System.Drawing;

namespace WindowsFormsApp2
{
    class Wall : Barrier
    {
        public bool Reflection { get; set; }


        public Wall(Color color, bool refl)
        {
            Color = color;
            Height = 0;
            HAH = 0;
            Reflection = refl;
            PointFs = (new PointF[] { new PointF(), new PointF() });

        }




    }
}
