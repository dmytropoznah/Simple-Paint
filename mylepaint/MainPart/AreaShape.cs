using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using LePaint.Basic;
using System.Xml.Serialization;

namespace LePaint.MainPart
{
    public class AreaShape : BoundaryShape, IShape
    {
        public TextShape TextField;

        private AreaShape() : base() { }

        public AreaShape( Rectangle shape, int index)
            : base(shape)
        {
            TextField = new TextShape("Shape " + index, shape, this);
            Boundary = shape;

            Opaque = false;
            FromColor = new LeColor(Color.FromArgb(100,Color.Red));
            ToColor = new LeColor(Color.FromArgb(100, Color.Red));

            base.ShapeMoved += new BoundaryShape.ShapeMoveHandler(OnMoveBorder);
            base.ShapeResized += new BoundaryShape.ResizingShapeMoveHandler(ResizeBorder);
        }

        public bool ShapeSizeOK(Point ptOrigin, Point ptCurrent)
        {
            Rectangle areaRect = Common.GetRectangle(ptOrigin, ptCurrent);

            if (areaRect.Width > 20 && areaRect.Height > 10)
            {
                return true;
            }
            else return false;
        }

        void OnMoveBorder(object sender, Point dPoint)
        {
            TextField.MoveBorder(sender,dPoint);

            BaseCanvas.Canvas.Invalidate(); 
        }

        void ResizeBorder(object sender, Rectangle newRect, Rectangle oldRect)
        {
            Boundary = newRect;
            Point dPoint = new Point(newRect.X - oldRect.X, newRect.Y - oldRect.Y);

            Point pt = Common.MovePoint(TextField.Boundary.Location, dPoint);
            Rectangle rect = new Rectangle(pt, TextField.Boundary.Size);
            TextField.Boundary = rect;
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            base.MouseDown(sender, e);
            TextField.MouseDown(sender, e);
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            base.MouseMove(sender, e);
            TextField.MouseMove(sender, e);
        }
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);
            TextField.MouseUp(sender, e);
        }

        public override void Paint(object sender, PaintEventArgs e)
        {
            base.Paint(sender, e);
            TextField.Paint(sender, e);
        }
    }
}
