using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml.Serialization ;
using LePaint.MainPart;

namespace LePaint.Basic
{
    public abstract class BaseShape :LeSerializableShape , IShape
    {

        #region Properties

        #endregion

        #region Fields

        #endregion

        #region Constructor
        public BaseShape(Rectangle bound)
        {
        }
        protected BaseShape()
        {

        }
        #endregion

        #region Mouse movement events

        #endregion

        #region drawing part

        private void DrawShape(Graphics g)
        {
            /*
            if (path != null)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                if (leShape.Opaque == true)
                {
                    g.FillPath(new System.Drawing.Drawing2D.LinearGradientBrush(
                        path.GetBounds(),leShape.FromColor.ToColor() ,leShape.ToColor.ToColor(), 225), path);
                }
                else
                {
                    g.FillPath(new SolidBrush(Color.FromArgb(leShape.FromColor.A,leShape.FromColor.ToColor() )), path);
                    g.DrawPath(new Pen(leShape.BorderColor.ToColor(),leShape.BorderWidth), path);
                }
            }
            */
        }

        #endregion

        public virtual void MouseDown(object sender, MouseEventArgs e)
        {
        }

        public virtual void MouseMove(object sender, MouseEventArgs e)
        {
        }

        public virtual void MouseUp(object sender, MouseEventArgs e)
        {
        }

        public virtual void PrepareToDrawShape(MouseEventArgs e)
        {
        }

        public virtual void DrawTempShape(MouseEventArgs e)
        {
        }

        public virtual void AddNewShape(Rectangle e)
        {
        }

    }
}
