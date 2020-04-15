using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using LePaint.Basic;

namespace LePaint.MainPart
{
    public class ArrowsCanvas : BaseCanvas, ICanvas
    {
        public ArrowsCanvas(Control canvas)
            :base(canvas)
        {
        }

        #region IShape Members

        public override void PrepareToDrawShape(MouseEventArgs e)
        {
            ptOriginal = e.Location;
            ptCurrent.X = -1;
            toAddShape = new ArrowShape(new Rectangle());
        }

        public override void DrawTempShape(MouseEventArgs e)
        {
            if (ptCurrent.X > 0)
            {
                (toAddShape as ArrowShape).DrawReversibleArrow(ptOriginal, ref ptCurrent);
            }

            ptCurrent = new Point(e.X, e.Y);
            (toAddShape as ArrowShape).DrawReversibleArrow(ptOriginal, ref ptCurrent);

        }

        public override void AddNewShape(Rectangle e)
        {
            if ((toAddShape as ArrowShape).TempDrawingOK() == true)
            {
                (toAddShape as ArrowShape).CreatePath();
                base.AddShape(toAddShape);
            }

            parent.Invalidate();
        }

        #endregion

    }
}
