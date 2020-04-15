using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using LePaint.MainPart;

namespace LePaint.Basic
{
    public class BoundaryShape : LeShape 
    {
        #region move or resize variables

        internal enum Position
        {
            Nothing,
            Corner,
            Side,
            Center,
        }

        protected enum PointAtPosition
        {
            TopLeft,
            TopMiddle,
            TopRight,
            RightMiddle,
            RightBottom,
            BottomMiddle,
            BottomLeft,
            LeftMidlle,
        }

        public enum Action
        {
            Nothing,
            MoveShape,
            MouseAtShape,
            AboutToMoveShape,
            ResizeShape
        }

        private Action curAction = Action.Nothing;
        protected bool shapeResizing=false ;

        private bool fill;
        public bool Fill
        {
            set { fill = value; }
            get { return fill; }
        }

        #endregion

        protected PointAtPosition PointAt;
        protected Point hotPoint;
        public BoundaryShape(Point pt)
            : base(pt)
        {
            int size = LeMenu.Size * 2;
            Boundary = new Rectangle(pt, new Size(size, size));

            Fill = true;
        }

        public BoundaryShape()
        {

        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            switch (curAction)
            {
                case Action.MouseAtShape:
                    DecideUserAction(e);
                    break;
                default:
                    break;
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            switch (curAction)
            {
                case Action.AboutToMoveShape:
                    Common.MyDrawReversibleRectangle(LeCanvas.self.Canvas, AreaRect);
                    curAction = Action.MoveShape;
                    ptPrevious = e.Location;
                    break;
                case Action.MoveShape:
                    MoveReversiableBoundary(e);
                    break;

                case Action.ResizeShape:
                    ptCurrent = new Point(e.X, e.Y);
                    Common.MyDrawReversibleRectangle(LeCanvas.self.Canvas, AreaRect);
                    Rectangle old0 = AreaRect;

                    AreaRect = Common.GetRectangle(ptOrigin, ptCurrent);

                    Common.CheckForBoundary(LeCanvas.self.Canvas, ref AreaRect, old0);
                    Common.MyDrawReversibleRectangle(LeCanvas.self.Canvas, AreaRect);
                    break;
                case Action.Nothing:
                    LeCanvas.self.Canvas.Cursor = GetMouseIcon(e);
                    if (Boundary.Contains(e.X, e.Y))
                    {
                        curAction = Action.MouseAtShape;
                        base.DrawMouseHoverShape();
                    }
                    break;
                case Action.MouseAtShape:
                    LeCanvas.self.Canvas.Cursor = GetMouseIcon(e);
                    if (Boundary.Contains(e.X, e.Y) == false)
                    {
                        curAction = Action.Nothing;
                    }
                    break;
            }
        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            switch (curAction)
            {
                case Action.MoveShape:
                    Point dPoint = new Point(AreaRect.X - Boundary.X,
                        AreaRect.Y - Boundary.Y);
                    OnShapeMoved(dPoint);
                    break;
                case Action.ResizeShape:
                    OnShapeResized(AreaRect);
                    curAction = Action.Nothing;
                    break;
                default:
                    break;
            }

            curAction = Action.Nothing;
            LeCanvas.self.Canvas.Cursor = Cursors.Default;
            OnMouseReleased(e);
        }

        public override void Paint(object sender, Graphics g)
        {
            if (ShowBorder == true)
            {
                g.DrawRectangle(new Pen(new SolidBrush(BorderColor), BorderWidth), Boundary);
            }

            if (Fill==true){
                g.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(
                        Boundary, FromColor, ToColor, LightAngle), Boundary);
            }
        }

        private Cursor GetMouseIcon(MouseEventArgs e)
        {
            Cursor ret = Cursors.Default;
            Position pos = CheckHotSpot(e);

            switch (pos)
            {
                case Position.Center:
                    ret = Cursors.Hand;
                    break;
                case Position.Corner:
                    ret = Cursors.Cross;
                    break;
            }
            return ret;
        }

        private void DecideUserAction(MouseEventArgs e)
        {
            Position pos = CheckHotSpot(e);

            switch (pos)
            {
                case Position.Center:
                    curAction = Action.AboutToMoveShape;
                    AreaRect = Boundary;
                    ptOrigin = new Point(e.X, e.Y);
                    OnMouseCaptured(e);
                    break;
                case Position.Corner:
                    curAction = Action.ResizeShape;
                    ptOrigin = BoundaryShape.GetOriginPoint(hotPoint, Boundary);
                    OnPrepareResizeShape(e);
                    break;
            }
        }

        private Position CheckHotSpot(MouseEventArgs e)
        {
            Point pt = e.Location;
            Point[] hots = RectTracker.GetPointsFromRect(Boundary);

            bool[] check = new bool[8];
            check[0] = CheckPointAt(hots[0], pt,PointAtPosition.TopLeft, ref PointAt, ref hotPoint);
            //check[1] = 
            CheckPointAt(hots[1], pt, PointAtPosition.TopMiddle, ref PointAt, ref hotPoint);
            check[2] = CheckPointAt(hots[2], pt, PointAtPosition.TopRight, ref PointAt, ref hotPoint);
            //check[3] = 
            CheckPointAt(hots[3], pt, PointAtPosition.RightMiddle, ref PointAt, ref hotPoint);
            check[4] = CheckPointAt(hots[4], pt, PointAtPosition.RightBottom, ref PointAt, ref hotPoint);
            //check[5] = 
            CheckPointAt(hots[5], pt, PointAtPosition.BottomMiddle, ref PointAt, ref hotPoint);
            check[6] = CheckPointAt(hots[6], pt, PointAtPosition.BottomLeft, ref PointAt, ref hotPoint);
            //check[7] = 
            CheckPointAt(hots[7], pt, PointAtPosition.LeftMidlle, ref PointAt, ref hotPoint);

            foreach (bool p in check)
            {
                if (p == true)
                {
                    return Position.Corner;
                }
            }

            if (Boundary.Contains(pt))
            {
                return Position.Center;
            }

            return Position.Nothing;
        }

        private bool CheckPointAt(Point point,Point mousePoint, PointAtPosition pointAtPosition, ref PointAtPosition PointAt, ref Point hotPoint)
        {
            bool ret = false;
            Rectangle rect = Common.GetHotSpot(point);
            rect = new Rectangle(Common.MovePoint(rect.Location, new Point(-5, -5)),
                new Size(rect.Width + 10, rect.Height + 10));

            if (rect.Contains(mousePoint))
            {
                PointAt = pointAtPosition;
                hotPoint = point;
                ret = true;
            }
            return ret;
        }

        public static Point GetOriginPoint(Point point, Rectangle rect)
        {
            Point ret = new Point();
            int dx, dy;
            dx = point.X - rect.X;
            dy = point.Y - rect.Y;
            if (dx == 0 && dy == 0)
            {
                ret = new Point(rect.X + rect.Width, rect.Y + rect.Height);
            }
            else if (dx == 0 && dy != 0)
            {
                ret = new Point(rect.X + rect.Width, rect.Y);
            }
            else if (dx != 0 && dy != 0)
            {
                ret = new Point(rect.X, rect.Y);
            }
            else if (dx != 0 && dy == 0)
            {
                ret = new Point(rect.X, rect.Y + rect.Height);
            }
            return ret;
        }

        #region raise events
        public delegate void ResizingShapeMoveHandler(object sender, Rectangle newRect,Rectangle oldRect);
        public event ResizingShapeMoveHandler ShapeResized;

        public delegate void ShapeMoveHandler(object sender, Point e);
        public event ShapeMoveHandler ShapeMoved;

        public delegate void ShapePrepareResizeHandler(MouseEventArgs e);
        public event ShapePrepareResizeHandler ShapePrepareResize;

        public void OnShapeMoved(Point dPoint)
        {
            MoveBorder(this, dPoint);
            if (ShapeMoved != null)
            {
                ShapeMoved(this, dPoint);
            }
            LeCanvas.self.Canvas.Invalidate();
        }

        private void OnShapeResized(Rectangle newRect)
        {
            if (ShapeResized != null)
            {
                ShapeResized(this, newRect, Boundary);
            }
        }

        public void MoveBorder(object sender, Point dPoint)
        {
            Point pt = Common.MovePoint(Boundary.Location, dPoint);
            Rectangle rect = new Rectangle(pt, Boundary.Size);

            Boundary = rect;
        }

        private void OnPrepareResizeShape(MouseEventArgs e)
        {
            if (ShapePrepareResize != null)
            {
                ShapePrepareResize(e); 
            }
        }

        #endregion

        #region mouse hover or leave
        public delegate void MouseCapturedHandler(object sender, MouseEventArgs e);
        public event MouseCapturedHandler MouseCaptured;

        public delegate void MouseReleasedHandler(object sender, MouseEventArgs e);
        public event MouseReleasedHandler MouseReleased;

        private void OnMouseCaptured(MouseEventArgs e)
        {
            if (MouseCaptured != null)
            {
                MouseCaptured(LeCanvas.self.Canvas, e);
            }
        }

        private void OnMouseReleased(MouseEventArgs e)
        {
            if (MouseReleased != null)
            {
                MouseReleased(LeCanvas.self.Canvas, e);
            }
        }
        #endregion

    }
}
