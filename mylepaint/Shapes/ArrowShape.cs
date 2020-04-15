using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

using LePaint.Basic;

namespace LePaint.MainPart
{
    public class ArrowShape : BoundaryShape 
    {
        #region Properties
        private Point ptStart;
        public Point StartPoint
        {
            get { return ptStart; }
            set { ptStart = value; }
        }

        private Point ptEnd;
        public Point EndPoint
        {
            get { return ptEnd; }
            set { ptEnd = value; }
        }

        private int arrowTipWidth = 40;
        [XmlElement("ArrowTipWidth")]
        public int ArrowTipWidth
        {
            get { return arrowTipWidth; }
            set
            {
                arrowTipWidth = value;
                LeMenu_ShapeReloaded(this);
                LeCanvas.self.Canvas.Invalidate(); 
            }
        }

        private int arrowButtWidth = 20;
        [XmlElement("ArrowButtWidth")]
        public int ArrowButtWidth
        {
            get { return arrowButtWidth; }
            set
            {
                arrowButtWidth = value;
                LeMenu_ShapeReloaded(this);
                LeCanvas.self.Canvas.Invalidate();
            }
        }

        #endregion

        #region private fields
        private ArrayList arrowPoints;
        ArrayList tempPointList;
        #endregion

        #region constructor
        public ArrowShape(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            arrowButtWidth = 15;
            arrowTipWidth = 30;

            FromColor = Color.Blue;
            ToColor = Color.White ;
            Rectangle rect = Boundary;

            ptOrigin =new Point( pt.X+5,pt.Y+5 );
            ptCurrent = Common.MovePoint(ptOrigin, new Point(Boundary.Width-10, Boundary.Height-10 ));
            tempPointList = CreateLines(this.ptOrigin, this.ptCurrent);
            CreatePath();
            Boundary = rect;
        }

        public ArrowShape()
        {
            LeMenu.ShapeReloaded += new LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        public void Change()
        {
            arrowTipWidth = 20;
            arrowButtWidth = 10;
            Rectangle rect = Boundary;
            this.ptCurrent=Common.MovePoint(this.ptCurrent,new Point (-8,4)); 
            tempPointList = CreateLines(this.ptCurrent,this.ptOrigin);
            FromColor = Color.Black;
            ToColor = Color.Black;

            CreatePath();

            Boundary = rect;
        }
        void LeMenu_ShapeReloaded(object sender)
        {
            tempPointList = CreateLines(StartPoint, EndPoint);
            CreatePath();
        }
        #endregion

        public void CreatePath()
        {
            if (tempPointList.Count > 0)
            {
                path = new GraphicsPath();
                path.AddPolygon((Point[])tempPointList.ToArray(typeof(Point)));
                Boundary = Common.Convert(path.GetBounds());
                arrowPoints = tempPointList;
            }
        }

        #region temp drawing rubber arrow
        internal void DrawReversibleArrow( Point ptOrigin, ref Point ptCurrent)
        {
            CheckBoundary(ref ptCurrent);
            Rectangle rect = new Rectangle();
            tempPointList = new ArrayList();

            rect = Common.GetRectangle(ptOrigin, ptCurrent);

            if (rect.Width < LeMenu.Size  && rect.Height < LeMenu.Size  )
            {
                isDrawingOK = false;
            }
            else
            {
                tempPointList = CreateLines(ptOrigin,ptCurrent);
                if (tempPointList.Count > 0)
                {
                    DrawReversibleLines(tempPointList);
                }

                isDrawingOK = true;
            }
        }

        private ArrayList CreateLines(Point startPoint,Point endPoint)
        {
            int angle = GetAngle(startPoint,endPoint);

            Point[] pt = new Point[7];
            pt[0] = endPoint;

            double ra = (angle - 30) * Math.PI / 180;
            int dx = (int)(arrowTipWidth  * Math.Cos(ra));
            int dy = (int)(arrowTipWidth * Math.Sin(ra));
            pt[1] = new Point(endPoint.X - dx, endPoint.Y - dy);

            dx = (int)(arrowTipWidth * Math.Sin(angle * Math.PI / 180) / 4);
            dy = (int)(arrowTipWidth * Math.Cos(angle * Math.PI / 180) / 4);
            pt[2] = new Point(pt[1].X + dx, pt[1].Y - dy);

            pt[5] = new Point(pt[1].X + 3 * dx, pt[1].Y - 3 * dy);
            pt[6] = new Point(pt[1].X + 4 * dx, pt[1].Y - 4 * dy);

            dx = (int)(arrowButtWidth * Math.Sin(angle * Math.PI / 180) / 2);
            dy = (int)(arrowButtWidth * Math.Cos(angle * Math.PI / 180) / 2);

            pt[3] = new Point(startPoint.X - dx, startPoint.Y + dy);
            pt[4] = new Point(startPoint.X + dx, startPoint.Y - dy);

            ArrayList ret = new ArrayList(); 
            ret.AddRange(pt);
            return ret;
        }

        private int GetAngle(Point startPoint, Point endPoint)
        {
            int dx = (endPoint.X - startPoint.X);
            int dy =(endPoint.Y - startPoint.Y);
            double val = Math.Atan2(dy, dx);

            int ret = (int)(180*val / Math.PI);
            return ret;
        }

        private void DrawReversibleLines(ArrayList tempPointList)
        {
            Point[] points = (Point[])tempPointList.ToArray(typeof(Point));
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

        private void CheckBoundary( ref Point ptCurrent)
        {
            Rectangle toTest = GDIApi.GetViewableRect(LeCanvas.self.Canvas);
            if (ptCurrent.X < toTest.Left + 5) ptCurrent.X = toTest.Left + 5;
            if (ptCurrent.Y < toTest.Top + 5) ptCurrent.Y = toTest.Top + 5;

            if (ptCurrent.X > toTest.Width + toTest.X - 5)
            {
                ptCurrent.X = toTest.Width + toTest.X - 5;
            }
            if (ptCurrent.Y > toTest.Height + toTest.Y - 5)
            {
                ptCurrent.Y = toTest.Height + toTest.Y - 5;
            }
        }

        #endregion

        #region IShape implementation

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (shapeResizing)
            {
                if (ptCurrent.X > 0)
                {
                    DrawReversibleArrow(ptOrigin, ref ptCurrent);
                }
                ptCurrent = new Point(e.X, e.Y);
                DrawReversibleArrow(ptOrigin, ref ptCurrent);
            }
            else
               base.MouseMove(sender, e);
        }

        public override void DrawMouseMove(MouseEventArgs e)
        {
            if (ptCurrent.X > 0)
            {
                this.DrawReversibleArrow(ptOrigin, ref ptCurrent);
            }

            ptCurrent = new Point(e.X, e.Y);
            this.DrawReversibleArrow(ptOrigin, ref ptCurrent);
        }

        public override bool DrawMouseUp(MouseEventArgs e)
        {
            if (isDrawingOK  == true)
            {
                ptStart = ptOrigin;
                ptEnd = ptCurrent; 
                this.CreatePath();
                base.ShapeMoved+=new ShapeMoveHandler(ArrowShape_ShapeMoved);
                base.ShapePrepareResize += delegate(MouseEventArgs e0)
                {
                    shapeResizing = true;
                };
                base.ShapeResized += new ResizingShapeMoveHandler(ArrowShape_ShapeResized);
            }

            LeCanvas.self.Canvas.Invalidate();
            return isDrawingOK; 
        }

        void ArrowShape_ShapeMoved(object sender, Point e)
        {
            tempPointList = new ArrayList();
            foreach (Point p in arrowPoints)
            {
                tempPointList.Add(Common.MovePoint(p, e));
            }
            ptOrigin = Common.MovePoint(ptOrigin, e);
            ptCurrent = Common.MovePoint(ptCurrent, e);

            CreatePath();
        }

        void ArrowShape_ShapeResized(object sender, Rectangle newRect, Rectangle oldRect)
        {
            CreateLines(ptOrigin, ptCurrent);
            CreatePath();
            shapeResizing = false;
        }

        #endregion

        public override void Paint(object sender, Graphics g)
        {

            if (path != null)
            {
                g.FillPath(new System.Drawing.Drawing2D.LinearGradientBrush(
                    path.GetBounds(), FromColor, ToColor, LightAngle), path);
                if (ShowBorder == true)
                {
                    g.DrawPath(new Pen(BorderColor, BorderWidth), path);
                }
            }

        }

    }
}
