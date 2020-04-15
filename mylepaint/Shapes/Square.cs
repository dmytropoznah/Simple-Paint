using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LePaint.MainPart;
using LePaint.Basic;

namespace LePaint.Shapes
{
    public class Square : LePolyGon 
    {
        public Square(Point pt)
            : base(pt)
        {
            InitShape( 4);
        }
        private Square() { }
    }
}
