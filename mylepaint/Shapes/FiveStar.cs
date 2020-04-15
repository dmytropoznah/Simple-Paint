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
    public class FiveStar : BoundaryShape
    {
        public List<Point> InnerPoints;
        public List<Point> OuterPoints;
        public int TotalStar;

        private float ratio = 2;
        public float Ratio
        {
            get { return ratio; }
            set
            {
                if (ratio <= 1)
                {
                    ratio = 1;
                }
                ratio = value;
                InitShape(TotalStar);
                LeCanvas.self.Canvas.Invalidate();
            }
        }

        public int InnerRadius
        {
            get { return (int)(outerRadius/ratio); }
        }

        private int outerRadius = LeMenu.Size;
        public int OuterRadius
        {
            set
            {
                outerRadius = value;
                InitShape(TotalStar);
                LeCanvas.self.Canvas.Invalidate();
            }
            get { return outerRadius; }
        }


        public FiveStar(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            FromColor = Color.Red;
            ToColor = Color.White;

            ptOrigin = Common.MovePoint(ptOrigin, new Point(outerRadius, outerRadius));
            InitShape(5); 
        }


        public FiveStar()
            : base()
        {
            LeMenu.ShapeReloaded += new LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            Point[] pt = new Point[TotalStar];

            int i = 0;
            foreach (Point p in OuterPoints)
            {
                pt[i++] = p;
            }

            Point[] pt1 = new Point[InnerPoints.Count];

            i = 0;
            foreach (Point p in InnerPoints)
            {
                pt1[i++] = p;
            }

            CreateNewShape(pt,pt1);

            RegisterEvents();

        }

        #region create shape part

        public override void DrawMouseDown(MouseEventArgs e)
        {
            base.DrawMouseDown(e);
            ptOrigin = e.Location;

            InitShape(TotalStar);
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


        protected void InitShape(int n)
        {
            TotalStar = n;
            int singleAngle = 360 / n;

            OuterPoints = new List<Point>();
            InnerPoints = new List<Point>();
 
            Point[] pt = new Point[n];

            centerPoint = ptOrigin;

            pt[0] = Common.MovePoint(ptOrigin, new Point(outerRadius, 0));
            for (int i = 1; i < n; i++)
            {
                int dx = (int)(outerRadius * Math.Cos(singleAngle * i * Math.PI / 180));
                int dy = (int)(outerRadius * Math.Sin(singleAngle * i * Math.PI / 180));

                pt[i] = Common.MovePoint(ptOrigin, new Point(dx, dy));
            }

            Point[] pt1 = new Point[TotalStar];
            {
                int dx = (int)(InnerRadius  * Math.Cos((singleAngle/2) * Math.PI / 180));
                int dy = (int)(InnerRadius * Math.Sin((singleAngle / 2) * Math.PI / 180));

                pt1[0] = Common.MovePoint(ptOrigin, new Point(dx, dy));
                for (int i = 1; i < n; i++)
                {
                    dx = (int)(InnerRadius * Math.Cos((singleAngle * (i) + singleAngle / 2) * Math.PI / 180));
                    dy = (int)(InnerRadius * Math.Sin((singleAngle * (i) + singleAngle / 2) * Math.PI / 180));

                    pt1[i] = Common.MovePoint(ptOrigin, new Point(dx, dy));
                }
            }

            CreateNewShape(pt,pt1);
        }

        protected void CreateNewShape(Point[] outerPt,Point[] innerPt)
        {
            OuterPoints = new List<Point>();
            InnerPoints = new List<Point>();

            OuterPoints.AddRange(outerPt);
            InnerPoints.AddRange(innerPt);
            
            
            Point[] pt = new Point[TotalStar * 2];

            int j = 0;
            for (j = 0; j < TotalStar; j++)
            {
                pt[j * 2] = outerPt[j];
                pt[j * 2 + 1] = innerPt[j];
            }

            path = new GraphicsPath();
            path.AddPolygon(pt);
            Rectangle rect = Common.Convert(path.GetBounds());
            Boundary = rect;
        }

        internal void DrawReversiblePoints(Point ptDown, ref Point ptCurrent)
        {
            isDrawingOK = false;
            List<Point> ret = RotatePoints(ptOrigin, ptCurrent);
            if (CheckShape(ret.ToArray()))
            {
                DrawReversibleLines(ret);
                isDrawingOK = true;
            }
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

        List<Point> tempOuterPoints;
        List<Point> tempInnerPoints;
        private List<Point> RotatePoints(Point originPoint, Point endPoint)
        {
            tempOuterPoints = Common.TurnPoints(OuterPoints, centerPoint, originPoint, endPoint, 1);
            tempInnerPoints = Common.TurnPoints(InnerPoints, centerPoint, originPoint, endPoint, ratio);

            List<Point> ret0 = new List<Point>();
            for (int i = 0; i < TotalStar; i++)
            {
                ret0.Add(tempOuterPoints[i]);
                ret0.Add(tempInnerPoints[i]); 
            }

            return ret0;
        }


        private void RegisterEvents()
        {
            base.ShapePrepareResize += delegate(MouseEventArgs e)
            {
                centerPoint = Common.GetCentre(OuterPoints);
                shapeResizing = true;
            };
            base.ShapeResized += new ResizingShapeMoveHandler(Pentagon_ShapeResized);
            base.ShapeMoved += new ShapeMoveHandler(Pentagon_ShapeMoved);
        }

        void Pentagon_ShapeMoved(object sender, Point e)
        {
            Point[] pt = new Point[OuterPoints.Count];

            int i = 0;
            foreach (Point p in OuterPoints)
            {
                pt[i++] = Common.MovePoint(p, e);
            }

            i = 0;
            Point[] pt1 = new Point[InnerPoints.Count];
            foreach (Point p in InnerPoints)
            {
                pt1[i++] = Common.MovePoint(p, e);
            }

            CreateNewShape(pt,pt1);
        }


        void Pentagon_ShapeResized(object sender, Rectangle newRect, Rectangle oldRect)
        {
            if (isDrawingOK == true)
            {
                Point[] pt1 =tempOuterPoints.ToArray();
                Point[] pt2 = tempInnerPoints.ToArray();
                CreateNewShape(pt1, pt2);
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
