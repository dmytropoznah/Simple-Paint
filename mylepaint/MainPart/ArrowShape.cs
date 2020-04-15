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
    public class ArrowShape : BaseShape, IShape 
    {
        private Point arrowStart, arrowEnd;
        public Point Start
        {
            get { return arrowStart; }
            set { arrowEnd = value; }
        }

        public Point End
        {
            get { return arrowEnd; }
            set { arrowEnd = value; }
        }

        ArrayList tempPointList;
        bool isDrawingOK = false;
        bool shapeResizing;

        private int arrowThick = 20;
        [XmlElement("ArrowThick")]
        public int ArrowThick
        {
            get { return arrowThick; }
            set { arrowThick = value; }
        }

        private BoundaryShape boundaryShape;
        private ArrayList arrowPoints;

        public ArrowShape( Rectangle rect)
            : base(rect)
        {
            ArrayList points = Common.GetPointsFromRect(rect);

            FromColor = new LeColor(Color.Red);
            ToColor = new LeColor(Color.Blue);

            path = new GraphicsPath();
            path.AddPolygon((Point[])points.ToArray(typeof(Point)));
            this.arrowPoints = points;

            boundaryShape = new BoundaryShape(rect); 
        }

        private ArrowShape()
        {
            boundaryShape = new BoundaryShape();
            boundaryShape.Boundary = base.Boundary;
        }

        public void CreatePath()
        {
            if (tempPointList.Count > 0)
            {
                path = new GraphicsPath();
                path.AddPolygon((Point[])tempPointList.ToArray(typeof(Point)));

                RectangleF rect = path.GetBounds();

                Boundary = new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
                boundaryShape = new BoundaryShape(Boundary);

                boundaryShape.ShapeMoved += new BoundaryShape.ShapeMoveHandler(boundaryShape_ShapeMoved);
                boundaryShape.ShapeResized += new BoundaryShape.ResizingShapeMoveHandler(boundaryShape_ShapeResized);
                boundaryShape.ShapePrepareResize += delegate(MouseEventArgs e)
                {
                    shapeResizing = true;
                };
                arrowPoints = tempPointList;
            }
        }

        void boundaryShape_ShapeResized(object sender, Rectangle newRect, Rectangle oldRect)
        {
            CreateLines(arrowStart,arrowEnd);
            CreatePath();
            shapeResizing=false;
        }

        void boundaryShape_ShapeMoved(object sender, Point e)
        {
            tempPointList = new ArrayList();
            foreach (Point p in arrowPoints)
            {
                tempPointList.Add(Common.MovePoint(p,e)); 
            }
            arrowStart = Common.MovePoint(arrowStart, e);
            arrowEnd = Common.MovePoint(arrowEnd, e);

            boundaryShape.ShapeMoved -= new BoundaryShape.ShapeMoveHandler(boundaryShape_ShapeMoved);
            boundaryShape.ShapeResized -= new BoundaryShape.ResizingShapeMoveHandler(boundaryShape_ShapeResized);
            CreatePath();
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            boundaryShape.MouseDown(sender, e); 
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (shapeResizing)
            {
                if (arrowEnd.X > 0)
                {
                    DrawReversibleArrow(arrowStart, ref arrowEnd);
                }
                arrowEnd = new Point(e.X, e.Y);
                DrawReversibleArrow(arrowStart, ref arrowEnd);
            }
            else
                boundaryShape.MouseMove(sender, e); 
        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            boundaryShape.MouseUp(sender, e);
        }


        public bool TempDrawingOK()
        {
            return isDrawingOK; 
        }
        #region temp drawing rubber arrow
        internal void DrawReversibleArrow( Point ptOriginal, ref Point ptCurrent)
        {
            CheckBoundary(ref ptCurrent);
            Rectangle rect = new Rectangle();
            tempPointList = new ArrayList();

            arrowStart  = ptOriginal;
            arrowEnd  = ptCurrent;

            rect = Common.GetRectangle(ptOriginal, ptCurrent);

            if (rect.Width < arrowThick  && rect.Height < arrowThick )
            {
                isDrawingOK = false;
            }
            else
            {
                tempPointList = CreateLines(ptOriginal,ptCurrent);
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
            int dx = (int)(arrowThick * Math.Cos(ra));
            int dy = (int)(arrowThick * Math.Sin(ra));
            pt[1] = new Point(endPoint.X - dx, endPoint.Y - dy);

            dx = (int)(arrowThick * Math.Sin(angle * Math.PI / 180) / 4);
            dy = (int)(arrowThick * Math.Cos(angle * Math.PI / 180) / 4);
            pt[2] = new Point(pt[1].X + dx, pt[1].Y - dy);

            pt[5] = new Point(pt[1].X + 3 * dx, pt[1].Y - 3 * dy);
            pt[6] = new Point(pt[1].X + 4 * dx, pt[1].Y - 4 * dy);

            dx = (int)(ArrowThick   * Math.Sin(angle * Math.PI / 180)/2);
            dy = (int)(ArrowThick * Math.Cos(angle * Math.PI / 180) / 2);

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
                Point p00 = BaseCanvas.Canvas.PointToScreen(p0);
                Point p11 = BaseCanvas.Canvas.PointToScreen(p1);
                ControlPaint.DrawReversibleLine(p00, p11, Color.Black);
                p0 = points[i];
            }

            Point p000 = BaseCanvas.Canvas.PointToScreen(p0);
            Point p111 = BaseCanvas.Canvas.PointToScreen(points[0]);
            ControlPaint.DrawReversibleLine(p000, p111, Color.Black);
        }

        private void CheckBoundary( ref Point ptCurrent)
        {
            Rectangle toTest = GDIApi.GetViewableRect(BaseCanvas.Canvas);
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
    }
}
