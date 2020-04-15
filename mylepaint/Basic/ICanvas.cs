using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;

namespace LePaint.Basic
{
    public interface ICanvas
    {
        void MouseDown(object sender, MouseEventArgs e);
        void MouseMove(object sender, MouseEventArgs e);
        void MouseUp(object sender, MouseEventArgs e);
        void Paint(object sender, PaintEventArgs e);
        void Save(string fileName);
        void Load(string fileName);
    }
}
