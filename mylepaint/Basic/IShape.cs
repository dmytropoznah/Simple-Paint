using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms ;
using System.Drawing;

namespace LePaint.Basic
{
    public interface IShape
    {
        void MouseDown(object sender, MouseEventArgs e);
        void MouseMove(object sender, MouseEventArgs e);
        void MouseUp(object sender, MouseEventArgs e);
        void Paint(object sender, Graphics g);
        bool DrawMouseUp(MouseEventArgs e);
        void DrawMouseDown(MouseEventArgs e);
        void DrawMouseMove(MouseEventArgs e);
    }
}
