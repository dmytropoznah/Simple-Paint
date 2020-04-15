using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing ;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LePaint.MainPart;
using LePaint.Basic;

namespace LePaint.MainPart
{
    public class EqualTriangle : BoundaryShape 
    {
        public List<Point> Points;

        private List<Point> tempPointList;
        #region constructor
        public EqualTriangle(Point pt)
            : base(pt)
        {
        }

        private EqualTriangle()
        {
            LeMenu.ShapeReloaded+=new LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded); 
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
        }
#endregion

        #region create shape part

        public override void DrawMouseDown(MouseEventArgs e)
        {
            ptOrigin = e.Location;
            ptCurrent.X = -1;
            InitTrangle(e.Location);
        }

        public override void DrawMouseMove(MouseEventArgs e)
        {
        }

        public override bool DrawMouseUp(MouseEventArgs e)
        {
            LeCanvas.self.Canvas.Invalidate();
            return true;
        }

        #endregion

        #region IShape implementation
        public override void MouseDown(object sender, MouseEventArgs e)
        {
            //boundaryShape.MouseDown(sender, e);
        }

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
                //boundaryShape.MouseMove(sender, e);
            }
        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            //boundaryShape.MouseUp(sender, e);
        }
        #endregion

        #region Shape creation
        /// <summary>
        /// Mouse down create a equal side length triangle
        /// </summary>
        /// <param name="ptTemp"></param>
        private void InitTrangle(Point ptTemp)
        {
            ptOrigin = ptTemp;

            Points = new List<Point>();

            System.Console.WriteLine(ptTemp.ToString());  
            int size = LeMenu.Size * 2;
            Point[] pt = new Point[3];
            int sideLength = (int)(size * Math.Cos(30 * Math.PI / 180) * 2);

            pt[0] = new Point(ptTemp.X - sideLength / 2, ptTemp.Y + size / 2);
            pt[1] = new Point(pt[0].X + sideLength, pt[0].Y);
            pt[2] = new Point(ptTemp.X, ptTemp.Y - size);

            if (CheckShape(pt))
            {
                CreateNewShape(pt);
            }
        }

        private void CreateNewShape(Point[] pt)
        {
            path = new GraphicsPath();
            path.AddPolygon(pt);
            Rectangle rect = Common.Convert(path.GetBounds());

            Points.AddRange(pt);
            /*
            boundaryShape = new BoundaryShape();
            boundaryShape.Boundary = rect;
            Boundary = rect;

            boundaryShape.ShapeMoved += new BoundaryShape.ShapeMoveHandler(boundaryShape_ShapeMoved);
            boundaryShape.ShapePrepareResize+=new BoundaryShape.ShapePrepareResizeHandler(boundaryShape_ShapePrepareResize);
            boundaryShape.ShapeResized += new BoundaryShape.ResizingShapeMoveHandler(boundaryShape_ShapeResized);
        */
        }

        void boundaryShape_ShapeMoved(object sender, Point e)
        {
            Point[] pt = new Point[Points.Count];

            int i = 0;
            foreach (Point p in Points)
            {
                pt[i++] = Common.MovePoint(p, e);
            }

            MovePoints(pt);
        }
        
        void boundaryShape_ShapePrepareResize(MouseEventArgs e)
        {
            shapeResizing = true;
            ptOrigin = e.Location;
            centerPoint = Common.GetCentre(Points);
            ptCurrent.X = -1;
        }

        void boundaryShape_ShapeResized(object sender, Rectangle newRect, Rectangle oldRect)
        {
            if (isDrawingOK == true)
            {
                MovePoints(tempPointList.ToArray());
            }
            shapeResizing = false;
        }

        private void MovePoints(Point[] pt)
        {
            Points = new List<Point>();
            Points.AddRange(pt);

            path = new GraphicsPath();
            path.AddPolygon(pt);
            Rectangle rect = Common.Convert(path.GetBounds());
            //boundaryShape.Boundary = rect;
            Boundary = rect;
        }

        private bool CheckShape(Point[] pt)
        {
            bool ret = false;
            Rectangle oldRect = Common.Convert(path.GetBounds());
            GraphicsPath newPath = new GraphicsPath();
            newPath.AddPolygon(pt);
            Rectangle rect = Common.Convert(newPath.GetBounds());
            bool check=Common.CheckForBoundary(LeCanvas.self.Canvas, ref rect, oldRect);
            newPath = null;
            return check;
        }

        #endregion
        
        #region rotate triangle
        internal void DrawReversiblePoints(Point ptDown, ref Point ptCurrent)
        {
            isDrawingOK = false;
            tempPointList = RotatePoints(ptOrigin, ptCurrent);
            if (CheckShape(tempPointList.ToArray())){ 
                DrawReversibleLines(tempPointList);
                isDrawingOK = true;
            }
        }

        private List<Point> RotatePoints(Point originPoint, Point endPoint)
        {
            List<Point> ret = Common.TurnPoints(Points,centerPoint, originPoint, endPoint,1);
            return ret;
        }

        private void DrawReversibleLines(List<Point> tempPointList)
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

        #endregion

    }
}
