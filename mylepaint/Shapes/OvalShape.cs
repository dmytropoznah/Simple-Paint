using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing ;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LePaint.MainPart;
using LePaint.Basic;

namespace LePaint.Shapes
{
    public class OvalShape:BoundaryShape  
    {
        public OvalShape(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            FromColor = Color.Red;
            ToColor = Color.White;
        }

        private OvalShape()
        {
            LeMenu.ShapeReloaded+=new LeMenu.ShapeReloadedHandler(LeMenu_ShapeReloaded); 
        }

        void LeMenu_ShapeReloaded(object sender)
        {
            CreateNewShape();
        }

        public override void Paint(object sender, Graphics g)
        {
            g.FillEllipse(new System.Drawing.Drawing2D.LinearGradientBrush(
                    Boundary, FromColor, ToColor, LightAngle, false), Boundary);
        }


        public override void DrawMouseDown(MouseEventArgs e)
        {
            ptOrigin = e.Location;
            ptCurrent.X = -1;
            ShowBorder = false;
            LeFromColor = new LeColor(Color.Black);

            int size = LeMenu.Size * 3;
            Boundary = new Rectangle(Common.MovePoint(e.Location, new Point(-size / 2, -size / 2)), new Size(size, size));
            base.ShapeResized += new ResizingShapeMoveHandler(OvalShape_ShapeResized);
            CreateNewShape();
        }

        void OvalShape_ShapeResized(object sender, Rectangle newRect, Rectangle oldRect)
        {
            Boundary = newRect;
        }

        private void CreateNewShape()
        {
            Rectangle rect = new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            Boundary = rect;
        }

        public override bool DrawMouseUp(MouseEventArgs e)
        {
            LeCanvas.self.Canvas.Invalidate();
            return true;
        }
    }
}
