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
    public class LePolyGon : BoundaryShape
    {
        public List<Point> Points;
        public int TotalPoints;

        int size = LeMenu.Size ;

        protected List<Point> tempPointList;
        #region constructor
        public LePolyGon(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            ptOrigin = Common.MovePoint(ptOrigin, new Point(size,size));
        }

        public LePolyGon()
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

            Points.AddRange(pt);
            CreateNewShape(pt);

            RegisterEvents();
        }
        #endregion

        #region create shape part

        public override void DrawMouseDown(MouseEventArgs e)
        {
            base.DrawMouseDown(e);
            size = LeMenu.Size * 2;
            ptOrigin = e.Location;

            InitShape(TotalPoints);
            RegisterEvents();
        }

        public override bool DrawMouseUp(MouseEventArgs e)
        {

            return true;
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

        #region Shape creation
        /// <summary>
        /// Mouse down create a equal side length triangle
        /// </summary>
        /// <param name="ptTemp"></param>
        public virtual void InitShape(int n)
        {
            TotalPoints = n;
            int totalAngle = 180 * (n - 2);
            int singleAngle = 360 / n;

            Points = new List<Point>();

            Point[] pt = new Point[n];

            centerPoint = ptOrigin;

            pt[0] = Common.MovePoint(ptOrigin, new Point(size, 0));
            for (int i = 1; i < n; i++)
            {
                int dx = (int)(size * Math.Cos(singleAngle * i * Math.PI / 180));
                int dy = (int)(size * Math.Sin(singleAngle * i * Math.PI / 180));

                pt[i] = Common.MovePoint(ptOrigin, new Point(dx, dy));
            }
            Points.AddRange(pt);
            CreateNewShape(pt);
        }

        protected void CreateNewShape(Point[] pt)
        {
            path = new GraphicsPath();
            path.AddPolygon(pt);
            Rectangle rect = Common.Convert(path.GetBounds());
            Boundary = rect;

        }

        private void MovePoints(Point[] pt)
        {
            Points = new List<Point>();
            Points.AddRange(pt);

            path = new GraphicsPath();
            path.AddPolygon(pt);
            Rectangle rect = Common.Convert(path.GetBounds());
            Boundary = rect;
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

        #endregion

        #region rotate triangle
        internal virtual void DrawReversiblePoints(Point ptDown, ref Point ptCurrent)
        {
            isDrawingOK = false;
            tempPointList = RotatePoints(ptOrigin, ptCurrent);
            if (CheckShape(tempPointList.ToArray()))
            {
                DrawReversibleLines(tempPointList);
                isDrawingOK = true;
            }
        }

        private List<Point> RotatePoints(Point originPoint, Point endPoint)
        {
            List<Point> ret = Common.TurnPoints(Points, centerPoint, originPoint, endPoint,1);
            return ret;
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

            MovePoints(pt);
        }

        void Pentagon_ShapeResized(object sender, Rectangle newRect, Rectangle oldRect)
        {
            if (isDrawingOK == true)
            {
                MovePoints(tempPointList.ToArray());
            }
            shapeResizing = false;
        }
        #endregion

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
