using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;

using LePaint.MainPart;
using LePaint.Basic;

namespace LePaint.Shapes
{
    public class RoundRectShape : BoundaryShape
    {
        private int radius = 10;
        public int Radius
        {
            set
            {
                radius = value;
                LeCanvas.self.Canvas.Invalidate();
            }
            get { return radius; }
        }

        public RoundRectShape(Point pt)
            : base(pt)
        {
            ShowBorder = true;
            FromColor = Color.Black;
            ToColor = Color.Aquamarine;
        }
        private RoundRectShape() { }

        public bool ShapeSizeOK(Point ptOrigin, Point ptCurrent)
        {
            Rectangle areaRect = Common.GetRectangle(ptOrigin, ptCurrent);

            if (areaRect.Width > 20 && areaRect.Height > 10)
            {
                return true;
            }
            else return false;
        }

        public override void Paint(object sender, Graphics g)
        {
            Point[] pt = new Point[8];
            pt[0] = Common.MovePoint(Boundary.Location, new Point(radius, 0));
            pt[1] = Common.MovePoint(pt[0], new Point(Boundary.Width - radius * 2, 0));

            pt[2] = Common.MovePoint(pt[1], new Point(radius, radius));
            pt[3] = Common.MovePoint(pt[2], new Point(0, Boundary.Height - radius * 2));

            pt[4] = Common.MovePoint(pt[1], new Point(0, Boundary.Height));
            pt[5] = Common.MovePoint(pt[0], new Point(0, Boundary.Height));

            pt[6] = Common.MovePoint(pt[3], new Point(-Boundary.Width, 0));
            pt[7] = Common.MovePoint(Boundary.Location, new Point(0, radius));

            path = new GraphicsPath();

            path.AddArc(new Rectangle(Boundary.Location, new Size(radius, radius)), 180, 90);
            path.AddLine(pt[0], pt[1]);
            path.AddArc(new Rectangle(pt[1], new Size(radius, radius)), 270, 90);
            path.AddLine(pt[2], pt[3]);
            path.AddArc(new Rectangle(new Point(pt[1].X, pt[3].Y), new Size(radius, radius)),
                0, 90);
            path.AddLine(pt[4], pt[5]);
            path.AddArc(new Rectangle(pt[6], new Size(radius, radius)),
                90, 90);
            path.AddLine(pt[6], pt[7]);

            if (path != null)
            {
                g.FillPath(new System.Drawing.Drawing2D.LinearGradientBrush(
                    Boundary, FromColor, ToColor, LightAngle), path);
            }
        }

        public override bool DrawMouseUp(MouseEventArgs e)
        {
            bool check = false;
            if (AreaRect.Width > LeMenu.Size && AreaRect.Height > LeMenu.Size) check = true;

            if (check == true)
            {
                Boundary = AreaRect;
                base.ShapeResized += new ResizingShapeMoveHandler(RoundRectShape_ShapeResized);
            }
            else path = null;

            LeCanvas.self.Canvas.Invalidate();

            return check;
        }

        void RoundRectShape_ShapeResized(object sender, Rectangle newRect, Rectangle oldRect)
        {
            Boundary = newRect;
        }

    }
}
