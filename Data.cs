using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class Data
    {
        public double Cos { get; set; }
        public float W { get; set; }
        public double Tan { get; set; }
        public float R { get; set; }
        public Data(Camera camera, double Arc)
        {
            Cos = Math.Cos(Arc - camera.Arc);
            Tan = Math.Tan(camera.FovRad / camera.Step);
            W = (float)camera.WidthWin / (float)camera.Step;
        }
    }
}
