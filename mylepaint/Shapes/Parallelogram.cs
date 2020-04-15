using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using LePaint.Basic;
using LePaint.MainPart;

namespace LePaint.Shapes
{
    public class Parallelogram : BoundaryShape
    {
        public List<Point> Points = new List<Point>();
        public Parallelogram(Point pt)
            : base(pt)
        {
            ShowBorder = true;
            FromColor = Color.Red;
            ToColor = Color.White;
            int size = LeMenu.Size;

            ptOrigin = Common.MovePoint(ptOrigin, new Point(size/3, size/2));

            Points = new List<Point>();

            Point[] ptArray = new Point[4];

            ptArray[0] = Common.MovePoint(ptOrigin, new Point(10, 0));
            ptArray[1] = Common.MovePoint(ptArray[0], new Point(size, 0));
            ptArray[2] = Common.MovePoint(ptArray[1], new Point(-10, size));
            ptArray[3] = Common.MovePoint(ptOrigin, new Point(0, size));

            CreateNewShape(ptArray);
        }


        public Parallelogram()
            : base()
        {
            LeMenu.ShapeReloaded += new LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            Point[] pt = new Point[Points.Count];

            int i = 0;
            foreach (Point p in Points)
            {
                pt[i++] = p;
            }

            CreateNewShape(pt);

            RegisterEvents();
        }

        #region create shape part


        public override bool DrawMouseUp(MouseEventArgs e)
        {
            bool check = false;
            if (AreaRect.Width > LeMenu.Size && AreaRect.Height > LeMenu.Size) check = true;

            if (check == true)
            {
                Boundary = AreaRect;
                InitShape(AreaRect);
                RegisterEvents();
            }
            else path = null;

            LeCanvas.self.Canvas.Invalidate();

            return check;
        }

        private void InitShape(Rectangle AreaRect)
        {
            Point[] pt = new Point[4];
            pt[0] = Common.MovePoint(AreaRect.Location, new Point(10, 0));
            pt[1] = Common.MovePoint(AreaRect.Location, new Point(AreaRect.Width, 0));
            pt[2] = Common.MovePoint(pt[1], new Point(-10, AreaRect.Height));
            pt[3] = Common.MovePoint(AreaRect.Location, new Point(0, AreaRect.Height));

            CreateNewShape(pt);
        }
        #endregion

        #region IShape implementation

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (shapeResizing)
            {
                if (ptCurrent.X > 0)
                {
                    DrawReversiblePoints(ptOrigin, ref ptCurrent);
                }
                ptCurrent = new Point(e.X, e.Y);
                DrawReversiblePoints(ptOrigin, ref ptCurrent);
            }
            else
            {
                base.MouseMove(sender, e);
            }
        }

        #endregion

        protected void CreateNewShape(Point[] pt)
        {
            Points = new List<Point>();

            Points.AddRange(pt);

            path = new GraphicsPath();
            path.AddPolygon(pt);
            Rectangle rect = Common.Convert(path.GetBounds());
            Boundary = rect;
        }

        List<Point> tempPoints;

        internal void DrawReversiblePoints(Point ptFixed, ref Point ptCurrent)
        {
            isDrawingOK = false;
            tempPoints = RotatePoints(ptFixed, ptCurrent);
            if (CheckShape(tempPoints.ToArray()))
            {
                DrawReversibleLines(tempPoints);
                isDrawingOK = true;
            }
        }

        private List<Point> RotatePoints(Point ptFixed, Point ptCurrent)
        {
            int dx = ptCurrent.X - hotPoint.X;
            int dy = ptCurrent.Y - hotPoint.Y;

            Point[] pt = Points.ToArray(); 
            if (PointAt == PointAtPosition.BottomLeft ||
                PointAt == PointAtPosition.RightBottom)
            {
                pt[2] = Common.MovePoint(pt[2], new Point(dx, dy));
                pt[3] = Common.MovePoint(pt[3], new Point(dx, dy));
            }
            else if (PointAt == PointAtPosition.TopLeft ||
                PointAt == PointAtPosition.TopRight)
            {
                pt[0] = Common.MovePoint(pt[0], new Point(dx, dy));
                pt[1] = Common.MovePoint(pt[1], new Point(dx, dy));
            }
            else if (PointAt == PointAtPosition.RightMiddle)
            {
                pt[1] = Common.MovePoint(pt[1], new Point(dx, dy));
                pt[2] = Common.MovePoint(pt[2], new Point(dx, dy));
            }

            List<Point> ret = new List<Point>();
            ret.AddRange(pt); 
            return ret;
        }

        protected bool CheckShape(Point[] pt)
        {
            Rectangle oldRect = Common.Convert(path.GetBounds());
            GraphicsPath newPath = new GraphicsPath();
            newPath.AddPolygon(pt);
            Rectangle rect = Common.Convert(newPath.GetBounds());
            bool check = Common.CheckForBoundary(LeCanvas.self.Canvas, ref rect, oldRect);
            newPath = null;
            return check;
        }

        protected void DrawReversibleLines(List<Point> tempPointList)
        {
            Point[] points = (Point[])tempPointList.ToArray();
            Point p0 = points[0];
            for (int i = 1; i < points.GetLength(0); i++)
            {
                Point p1 = points[i];
                Point p00 = LeCanvas.self.Canvas.PointToScreen(p0);
                Point p11 = LeCanvas.self.Canvas.PointToScreen(p1);
                ControlPaint.DrawReversibleLine(p00, p11, Color.Black);
                p0 = points[i];
            }

            Point p000 = LeCanvas.self.Canvas.PointToScreen(p0);
            Point p111 = LeCanvas.self.Canvas.PointToScreen(points[0]);
            ControlPaint.DrawReversibleLine(p000, p111, Color.Black);
        }

        private void RegisterEvents()
        {
            base.ShapePrepareResize += delegate(MouseEventArgs e)
            {
                centerPoint = Common.GetCentre(Points);
                ptCurrent.X = -1;
                shapeResizing = true;
            };
            base.ShapeResized += new ResizingShapeMoveHandler(Pentagon_ShapeResized);
            base.ShapeMoved += new ShapeMoveHandler(Pentagon_ShapeMoved);
        }

        void Pentagon_ShapeMoved(object sender, Point e)
        {
            Point[] pt = new Point[Points.Count];

            int i = 0;
            foreach (Point p in Points)
            {
                pt[i++] = Common.MovePoint(p, e);
            }

            CreateNewShape(pt);
        }


        void Pentagon_ShapeResized(object sender, Rectangle newRect, Rectangle oldRect)
        {
            if (isDrawingOK == true)
            {
                Point[] pt1 = tempPoints.ToArray();
                CreateNewShape(pt1);
            }
            shapeResizing = false;
        }

        public override void Paint(object sender, Graphics g)
        {
            if (path != null)
            {
                g.FillPath(new System.Drawing.Drawing2D.LinearGradientBrush(
                    path.GetBounds(), FromColor, ToColor, LightAngle), path);

                if (ShowBorder)
                {
                    g.DrawPath(new Pen(BorderColor, BorderWidth), path);
                }
            }

        }

    }
}
