using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LePaint.Basic;
using LePaint.MainPart;

namespace LePaint.Shapes
{
    public class PolyStar:FiveStar 
    {
        public PolyStar(Point pt)
            : base(pt)
        {
            FromColor = Color.Red;
            ToColor = Color.White;
            
            InitShape(30);
        }

        private PolyStar()
            : base()
        {

        }
    }
}
