using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using LePaint.MainPart;

namespace LePaint.Basic
{
    internal struct Common
    {
        public const float PI = 3.14159265358979F;
        public const int MaxUndoNum = 50;
        public const int No_Click = 0x80;
        public const int ERASE_OP = 0x8000;
        public const int INIT_OP = 0x8000;

        public static Rectangle MoveRectangle(Point cur, Point org, Rectangle rc)
        {
            int dirX, dirY;
            if (cur.X < org.X) //move to left?
                dirX = -1;
            else dirX = 1;
            if (cur.Y < org.Y) //move to top?
                dirY = -1;
            else dirY = 1;

            rc.X = rc.X + dirX * Math.Abs(cur.X - org.X);
            rc.Y = rc.Y + dirY * Math.Abs(cur.Y - org.Y);
            return rc;
        }

        internal static Point MovePoint(Point rc, Point dPoint)
        {
            int dirX, dirY;
            if (dPoint.X < 0) //move to left?
                dirX = -1;
            else dirX = 1;
            if (dPoint.Y < 0) //move to top?
                dirY = -1;
            else dirY = 1;

            rc.X = rc.X + dirX * Math.Abs(dPoint.X);
            rc.Y = rc.Y + dirY * Math.Abs(dPoint.Y);

            return rc;
        }

        public static Rectangle GetRectangle(Point cur, Point org)
        {
            Rectangle rc0 = new Rectangle();
            rc0.Width = Math.Abs(cur.X - org.X);
            rc0.Height = Math.Abs(cur.Y - org.Y);

            if (cur.X < org.X) rc0.X = cur.X;
            else rc0.X = org.X;

            if (cur.Y < org.Y) rc0.Y = cur.Y;
            else rc0.Y = org.Y;
            return rc0;
        }


        public static bool DotInRect(int x, int y, Rectangle rc)
        {
            if (x > rc.Left && x < rc.Left + rc.Width &&
                y > rc.Top && y < rc.Top + rc.Height)
                return true;
            return false;
        }
        public static bool DotInBox(int X, int Y, int X0, int Y0, int X1, int Y1)
        {
            int Temp;
            if (X0 > X1)
            {
                Temp = X0;
                X0 = X1;
                X1 = Temp;
            }
            if (Y0 > Y1)
            {
                Temp = Y0;
                Y0 = Y1;
                Y1 = Temp;
            }

            if (X >= X0 && X <= X1 && Y >= Y0 && Y <= Y1)
            {
                return true;
            }
            return false;
        }

        public static void SwapXY(ref int X, ref int Y)
        {
            int Temp;
            Temp = X;
            X = Y;
            Y = Temp;
        }

        public static bool DotInLineH(int X, int Y, int X0, int Y0, int X1)
        {
            if (X0 > X1) SwapXY(ref X0, ref X1);
            if (X < X0 || X > X1) return false;
            if (Y < Y0 - 4 || Y > Y0 + 4) return false;
            return true;

        }

        public static bool DotInLineV(int X, int Y, int X0, int Y0, int Y1)
        {
            if (Y0 > Y1) SwapXY(ref Y0, ref Y1);
            if (Y < Y0 || Y > Y1) return false;
            if (X < X0 - 4 || X > X0 + 4) return false;
            return true;
        }

        public static void MyDrawReversibleRectangle(Control client, Rectangle rc)
        {
            Rectangle rc0 = client.RectangleToScreen(rc);
            ControlPaint.DrawReversibleFrame(rc0,
                Color.Black, FrameStyle.Dashed);
        }

        public static void EraseArea(Control client, ref Rectangle areaRect)
        {
            MyDrawReversibleRectangle(client, areaRect);
            areaRect.X = -1;
            areaRect.Y = -1;
            areaRect.Width = 0;
            areaRect.Height = 0;
        }


        internal static Rectangle GetHotSpot(Point p)
        {
            Rectangle ret = new Rectangle();

            ret.X = p.X - 3;
            ret.Y = p.Y - 3;
            ret.Width = 6;
            ret.Height =6;
            return ret;
        }

        internal static Rectangle GetRealRect(Control canvas, Rectangle AreaRect)
        {
            Rectangle newRect = canvas.RectangleToScreen(AreaRect);
            return newRect;
        }

        public static bool CheckForBoundary(Control canvas, ref Rectangle AreaRect, Rectangle old)
        {
            int x0, y0;
            int x1, y1;
            bool check = true;

            x0 = AreaRect.X;
            y0 = AreaRect.Y;

            x1 = AreaRect.X + AreaRect.Width;
            y1 = AreaRect.Y + AreaRect.Height;

            Rectangle toTest = GDIApi.GetViewableRect(canvas);

            if (x0 < toTest.X + 5)
            {
                AreaRect.X = toTest.X + 5;
                check = false;
            }
            if (y0 < toTest.Y + 5)
            {
                AreaRect.Y = toTest.Y + 5;
                check = false;
            }
            if (x1 > toTest.Width + toTest.X - 5)
            {
                AreaRect = old;
                check = false;
            }
            if (y1 > toTest.Height + toTest.Y - 5)
            {
                AreaRect = old;
                check = false;
            }
            return check;
        }


        internal static System.Collections.ArrayList GetPointsFromRect(Rectangle rect)
        {
            System.Collections.ArrayList points = new System.Collections.ArrayList();

            points.Add(new Point(rect.X, rect.Y));
            points.Add(new Point(rect.X + rect.Width, rect.Y));
            points.Add(new Point(rect.X + rect.Width, rect.Y + rect.Height));
            points.Add(new Point(rect.X, rect.Y + rect.Height));

            return points;
        }

        internal static void MakeSquare(ref Rectangle rect)
        {
            if (rect.Width > rect.Height)
            {
                rect.Y = rect.Y - (rect.Width - rect.Height);
                rect.Height = rect.Width;
            }
            else
            {
                rect.X = rect.X - (rect.Height - rect.Width);
                rect.Width = rect.Height;
            }
            Common.CheckForBoundary(LeCanvas.self.Canvas, ref rect, new Rectangle());
        }

        internal static int GetLength(Point ptEnd, Point ptStart)
        {
            int dx, dy;
            dx = ptEnd.X - ptStart.X;
            dy = ptStart.Y - ptEnd.Y;

            dx = (int)Math.Pow(dx, 2);
            dy = (int)Math.Pow(dy, 2);

            int length = (int)Math.Sqrt(dx + dy);
            return length;
        }

        internal static Rectangle Convert(RectangleF rect)
        {
            Rectangle ret = new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

            return ret;
        }

        public static int GetAngle(Point startPoint, Point endPoint)
        {
            int dx = (endPoint.X - startPoint.X);
            int dy = (endPoint.Y - startPoint.Y);
            double val = Math.Atan2(dy, dx);

            int ret = (int)(180 * val / Math.PI);
            return ret;
        }


        internal static bool PointCloseToPoints(List<Point> Points, Point point, ref Point ptOrigin)
        {
            bool check = false;
            if (Points.Count > 1)
            {
                Point ptPrevious = Points.Last<Point> ();
                foreach (Point p in Points)
                {
                    Rectangle bound = new Rectangle(p.X - 10, p.Y - 10, 20, 20);
                    if (bound.Contains(point))
                    {
                        check = true;
                        ptOrigin = ptPrevious;
                        break;
                    }
                    ptPrevious = p;
                }
            }
            return check;
        }

        internal static Point GetCentre(List<Point> Points)
        {
            Point ret = new Point();
            foreach (Point p in Points)
            {
                ret.X += p.X;
                ret.Y += p.Y;
            }
            ret.X = ret.X / Points.Count;
            ret.Y = ret.Y / Points.Count;

            return ret;
        }

        internal static List<Point> TurnPoints(List<Point> Points, Point centerPoint, Point originPoint, Point endPoint,float ratio)
        {
            int angle = GetAngle(centerPoint, endPoint);
            int oldAngle = GetAngle(centerPoint, originPoint);
            int dLength = GetLength(centerPoint, endPoint) - GetLength(centerPoint, originPoint);
            dLength =(int)( dLength / ratio);

            Point[] pt = new Point[Points.Count];

            int i = 0;
            foreach (Point p in Points)
            {
                pt[i++] = Common.TurnPoint(centerPoint, p, (angle-oldAngle),dLength);
            }

            List<Point> ret = new List<Point>();
            ret.AddRange(pt);
            return ret;
        }

        internal static Point TurnPoint(Point orgin, Point p, int angle,int dLength)
        {
            int radius = Common.GetLength(orgin, p) + dLength;

            int newAngle = Common.GetAngle(orgin, p); 
            Point ret = new Point();
            ret.X = (int)(orgin.X + radius * Math.Sin((newAngle+ angle) * Math.PI / 180));
            ret.Y = (int)(orgin.Y - radius * Math.Cos ((newAngle + angle) * Math.PI / 180));
            return ret;
        }

        internal static Point GetMidPoint(Point pt1, Point pt2)
        {
            int dx = (pt2.X - pt1.X)/2;
            int dy = (pt2.Y - pt1.Y)/2;
            Point ret = new Point();
            ret.X  = pt1.X + dx;
            ret.Y = pt1.Y + dy;
            return ret;
        }
    }
}
