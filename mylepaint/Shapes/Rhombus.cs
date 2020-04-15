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
    public class Rhombus : BoundaryShape  
    {
        public Rhombus(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            CreatePath();
        }

        private Rhombus() {
            LeMenu.ShapeReloaded += new LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            RegisterEvents();
        }

        public override bool DrawMouseUp(MouseEventArgs e)
        {
            bool check = false;
            if (AreaRect.Width > LeMenu.Size && AreaRect.Height > LeMenu.Size) check = true;

            if (check == true)
            {
                Boundary = AreaRect;
                CreatePath();
                RegisterEvents();
            }
            else path = null;

            LeCanvas.self.Canvas.Invalidate();

            return check;
        }

        private void CreatePath()
        {
            ArrayList origin = Common.GetPointsFromRect(Boundary);
            Point[] pt = new Point[origin.Count];

            pt[0] = Common.GetMidPoint((Point)origin[0], (Point)origin[1]);
            pt[1] = Common.GetMidPoint((Point)origin[1], (Point)origin[2]);
            pt[2] = Common.GetMidPoint((Point)origin[2], (Point)origin[3]);
            pt[3] = Common.GetMidPoint((Point)origin[3], (Point)origin[0]);

            path = new GraphicsPath();
            path.AddPolygon(pt);
        }

        private void RegisterEvents()
        {
            base.ShapeMoved += new BoundaryShape.ShapeMoveHandler(OnMoveBorder);
            base.ShapeResized += new BoundaryShape.ResizingShapeMoveHandler(ResizeBorder);
        }
        void OnMoveBorder(object sender, Point dPoint)
        {
            Boundary = base.Boundary;
            CreatePath(); 
        }

        void ResizeBorder(object sender, Rectangle newRect, Rectangle oldRect)
        {
            Boundary = newRect;
            CreatePath();
        }

        public override void Paint(object sender, Graphics g)
        {

            if (path != null)
            {
                if (path.GetBounds().Width != 0 && path.GetBounds().Height != 0)
                {
                    g.FillPath(new System.Drawing.Drawing2D.LinearGradientBrush(
                        path.GetBounds(), FromColor, ToColor, LightAngle), path);
                    g.DrawPath(new Pen(BorderColor, BorderWidth), path);
                }
            }

        }

    }
}
