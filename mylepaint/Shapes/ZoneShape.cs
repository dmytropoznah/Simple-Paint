using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml.Serialization;

using LePaint.Basic;
using LePaint.Shapes;

namespace LePaint.MainPart
{
    public class ZoneShape : BoundaryShape
    {
        public TextShape TextField;

        public string Caption
        {
            set { TextField.Caption = value; }
            get { return TextField.Caption; }
        }
        public ZoneShape(Point pt) :base(pt)
        {
            TextField = new TextShape("Z", pt, this);
            TextField.ShowBorder = false;
        }

        public ZoneShape() : base() {
            LeMenu.ShapeReloaded += new LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            RegisterEvents();
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
            TextField.MoveBorder(sender, dPoint);

            LeCanvas.self.Canvas.Invalidate();
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
            if (TextField.Boundary.Contains(e.Location))
            {
                TextField.MouseDown(sender, e);
            }
            else
            {
                base.MouseDown(sender, e);
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            TextField.MouseMove(sender, e);
            base.MouseMove(sender, e);
        }
        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);
            TextField.MouseUp(sender, e);
        }

        public override void Paint(object sender, Graphics g)
        {
            base.Paint(sender, g);
            if (TextField != null)
            {
                TextField.Paint(sender, g);
            }
        }

        public override bool DrawMouseUp(MouseEventArgs e)
        {
            bool check = false;
            if (AreaRect.Width > LeMenu.Size && AreaRect.Height > LeMenu.Size) check = true;

            if (check == true)
            {
                Boundary = AreaRect;
                TextField = new TextShape("Shape " + XMLShapes.Total, Boundary.Location, this);
                RegisterEvents();
            }
            else path = null;

            LeCanvas.self.Canvas.Invalidate();

            return check;
        }

        public override bool UpdateSelected(Point point, ref LeShape shape0)
        {
            if (TextField.UpdateSelected(point, ref shape0))
            {
                return true;
            }

            return base.UpdateSelected(point, ref shape0);
        }
        private void RegisterEvents()
        {
            base.ShapeMoved += new BoundaryShape.ShapeMoveHandler(OnMoveBorder);
            base.ShapeResized += new BoundaryShape.ResizingShapeMoveHandler(ResizeBorder);
        }
    }
}
