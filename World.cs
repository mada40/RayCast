using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp2
{
    class World
    {
        public List<Prism> Prisms { get; set; }
        public int QuanPrism => Prisms.Count();
        

        public World()
        {
            Prisms = new List<Prism>();
        }
        public World(List<Prism> prisms)
        {
            Prisms = prisms;
        }


        

    }
}
