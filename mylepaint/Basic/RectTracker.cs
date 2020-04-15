using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using LePaint.MainPart;
namespace LePaint.Basic
{
    internal class RectTracker
    {
        #region Fields
        private int offset = 2;
        #endregion

        #region Constructor
        public RectTracker()
        {
        }
        #endregion

        #region Public Methods
        public void DrawSelectionGhostRect(Rectangle rc, Bitmap bm)
        {
            Graphics g = Graphics.FromImage(bm);
            ControlPaint.DrawFocusRectangle(g, rc, Color.Yellow, Color.Transparent);

            g.Dispose();
        }

        public void DrawSelectionTrackers(Rectangle rc)
        {
            //InitTrackerRects(rc);
            //this.BaseCanvas.Canvas = BaseCanvas.Canvas;

            //foreach (Rectangle rect in hotSpots)
            {
              //  ControlPaint.DrawReversibleFrame(rect, Color.Black, FrameStyle.Thick);
            }
        }

        #endregion

        public static Point[] GetPointsFromRect(Rectangle rect)
        {
            Point[] hots = new Point[8];
            hots[0] = rect.Location;
            hots[1] = Common.MovePoint(hots[0], new Point(rect.Width / 2, 0));
            hots[2] = Common.MovePoint(hots[0], new Point(rect.Width, 0));
            hots[3] = Common.MovePoint(hots[0], new Point(rect.Width, rect.Height / 2));
            hots[4] = Common.MovePoint(hots[0], new Point(rect.Width, rect.Height));
            hots[5] = Common.MovePoint(hots[0], new Point(rect.Width / 2, rect.Height));
            hots[6] = Common.MovePoint(hots[0], new Point(0, rect.Height));
            hots[7] = Common.MovePoint(hots[0], new Point(0, rect.Height / 2));

            return hots;
        }

        internal static void DrawPoints(PaintEventArgs e, Rectangle rect)
        {
            ArrayList toDraw = new ArrayList(GetPointsFromRect(rect));

            ArrayList hotSpots = new ArrayList();
            foreach (Point newPt in toDraw)
            {
                Rectangle rect0 = Common.GetHotSpot(newPt);
                hotSpots.Add(rect0);
            }

            //canvas.CreateGraphics().DrawRectangle(new Pen(new SolidBrush(Color.Blue),2),rect);
            e.Graphics.DrawRectangles(new Pen(new SolidBrush(Color.RoyalBlue),1),
                (Rectangle[])hotSpots.ToArray(typeof(Rectangle)));

            //e.Graphics.DrawRectangles((new SolidBrush(Color.Blue),
            //    (Rectangle[])hotSpots.ToArray(typeof(Rectangle)));
 
        }
    }
}

