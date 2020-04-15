using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml.Serialization;

using LePaint.Basic;
using LePaint.MainPart;

namespace LePaint.Shapes
{
    public class LeRectangle : BoundaryShape
    {
        public LeRectangle()
            : base()
        {
            LeMenu.ShapeReloaded += new LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded);
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            RegisterEvents();
        }
        public LeRectangle(Point pt)
            : base(pt)
        {
            ShowBorder = true;
        }

        public bool ShapeSizeOK(Point ptOrigin, Point ptCurrent)
        {
            Rectangle areaRect = Common.GetRectangle(ptOrigin, ptCurrent);

            if (areaRect.Width > 20 && areaRect.Height > 10)
            {
                return true;
            }
            else return false;
        }

        void OnMoveBorder(object sender, Point dPoint)
        {
            LeCanvas.self.Canvas.Invalidate();
        }

        void ResizeBorder(object sender, Rectangle newRect, Rectangle oldRect)
        {
            Boundary = newRect;
        }

        public override bool DrawMouseUp(MouseEventArgs e)
        {
            bool check = false;
            if (AreaRect.Width > LeMenu.Size && AreaRect.Height > LeMenu.Size) check = true;

            if (check == true)
            {
                Boundary = AreaRect;
                RegisterEvents();
            }
            else path = null;

            LeCanvas.self.Canvas.Invalidate();

            return check;
        }

        private void RegisterEvents()
        {
            base.ShapeMoved += new BoundaryShape.ShapeMoveHandler(OnMoveBorder);
            base.ShapeResized += new BoundaryShape.ResizingShapeMoveHandler(ResizeBorder);
        }

    }
}
