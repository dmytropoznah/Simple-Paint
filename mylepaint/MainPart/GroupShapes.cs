using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

using LePaint.MainPart;
using LePaint.Basic;

namespace LePaint.MainPart
{
    public class GroupShapes : BoundaryShape
    {
        public int Count
        {
            get { return selectedShapes.Count; }
        }

        List<LeShape> selectedShapes = new List<LeShape>();
        internal void SetSelectedShapes(System.Drawing.Rectangle AreaRect)
        {
            selectedShapes = new List<LeShape>();
            foreach (LeShape shape in LeCanvas.self.xmlShapes.GetList())
            {
                if (AreaRect.Contains(shape.Boundary.Location))
                {
                    shape.Selected = true;
                    selectedShapes.Add(shape);
                }
                else
                {
                    shape.Selected = false;
                }
            }
            Boundary = AreaRect;
        }

        public override void Paint(object sender, Graphics g)
        {
            base.Paint(sender, g);
        }
 
        public void Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Aqua)), Boundary);
        }


        internal void Move()
        {
            int dx = AreaRect.X - Boundary.X;
            int dy = AreaRect.Y - Boundary.Y;

            foreach (BoundaryShape shape in selectedShapes)
            {
                shape.OnShapeMoved(new Point(dx, dy));
            }
        }
    }
}