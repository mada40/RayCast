
using System.Drawing;


namespace WindowsFormsApp2
{
    static class EditingImage 
    {


        static public Color AlphaBlending(Color back, Color front)
        {
            double a = (double)front.A / byte.MaxValue;
            int R = (int)(back.R * (1 - a) + front.R * a);
            int G = (int)(back.G * (1 - a) + front.G * a);
            int B = (int)(back.B * (1 - a) + front.B * a);
            return Color.FromArgb(R, G, B);
        }


    }
}
