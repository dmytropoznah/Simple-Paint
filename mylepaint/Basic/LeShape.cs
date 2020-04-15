using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using LePaint.MainPart;

namespace LePaint.Basic
{
    public enum ShapeStyle
    {
        Sqaure,
        Round,
        Circle,
        DashedRound
    }

    public struct LeColor
    {
        public int A;
        public int R;
        public int G;
        public int B;
        public LeColor(Color color)
        {
            this.A = color.A;
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }

        public static LeColor FromColor(Color color)
        {
            return new LeColor(color);
        }

        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }
    }

    public struct LeRect{
        public int X,Y,Width,Height;
        public LeRect(Rectangle rect){
            X =rect.X;
            Y=rect.Y;
            Width =rect.Width ;
            Height=rect.Height ;
        }
    }

    public struct LeFont
    {
        public float Size;
        public string Name;
        public FontStyle Style;

        public LeFont(Font font)
        {
            this.Size = font.Size;
            this.Name = font.Name;
            this.Style = font.Style;
        }
        public static LeFont FromFont(Font font)
        {
            return new LeFont(font);
        }
        public Font ToFont()
        {
            if (Name != null && Size > 1)
            {
                return new Font(Name, Size, Style);
            }
            else return new Font("Times New Roman", 5);
        }
    }

    public abstract class LeShape : IShape
    {
        #region xml serializable variables

        private bool showBorder = true;
        public bool ShowBorder
        {
            get { return showBorder; }
            set
            {
                showBorder = value;
                LeCanvas.self.Canvas.Invalidate();
            }
        }
        private LeColor borderColor = new LeColor(Color.Black);
        public LeColor LeBorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                LeCanvas.self.Canvas.Invalidate();
            }
        }

        [XmlIgnore]
        public Color BorderColor
        {
            get { return LeBorderColor.ToColor(); }
            set { LeBorderColor = new LeColor(value); }

        }
        private int borderWidth = 1;
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                LeCanvas.self.Canvas.Invalidate();
            }
        }

        private Rectangle bounds;
        [XmlIgnore]
        public Rectangle Boundary
        {
            set { bounds = value; 
            Rect =new LeRect(value); 
            }
            get { return bounds; }
        }

        public LeRect Rect
        {
            set
            {
                bounds = new Rectangle(value.X, value.Y, value.Width, value.Height);
            }
            get { return new LeRect(bounds); }
        }

        private LeColor fromColor = new LeColor(Color.BurlyWood);
        public LeColor LeFromColor
        {
            get { return fromColor; }
            set { fromColor = value; }
        }
        [XmlIgnore]
        public Color FromColor
        {
            get { return LeFromColor.ToColor(); }
            set
            {
                LeFromColor = new LeColor(value);
                LeCanvas.self.Canvas.Invalidate();
            }
        }
        private LeColor toColor = new LeColor(Color.White);
        public LeColor LeToColor
        {
            get { return toColor; }
            set { toColor = value; }
        }
        [XmlIgnore]
        public Color ToColor
        {
            get { return LeToColor.ToColor(); }
            set
            {
                LeToColor = new LeColor(value);
                LeCanvas.self.Canvas.Invalidate();
            }
        }

        private int angle=225;
        public int LightAngle
        {
            set { angle = value; }
            get { return angle; }
        }
        #endregion

        #region System Running variables
        private bool selected = false;
        [XmlIgnore]
        public bool Selected
        {
            set { selected = value; }
            get { return selected; }
        }

        protected GraphicsPath path = null;
        protected ArrayList objectsInPath;
        protected Point ptOrigin;
        protected Point ptCurrent;
        protected Point ptPrevious;

        protected Rectangle AreaRect;

        //protected bool shapeResizing;
        //protected BoundaryShape boundaryShape;

        protected Point centerPoint;
        protected bool isDrawingOK = false;

        #endregion

        public LeShape()
        {
            path = new GraphicsPath();
            objectsInPath = new ArrayList();
        }
        public LeShape(Point pt)
        {
            path = new GraphicsPath();
            objectsInPath = new ArrayList();
            AreaRect = new Rectangle(pt, new Size(0, 0));
            ptOrigin = pt;
            ptCurrent = pt;
            ptPrevious = pt;

            FromColor = Color.FromArgb(30, Color.Red);
            ToColor = Color.FromArgb(30, Color.White);
        }

        protected virtual void DrawMouseHoverShape()
        {
            if (path != null)
            {
                Graphics g = LeCanvas.self.Canvas.CreateGraphics();
                g.DrawPath(new Pen(Color.FromArgb(200, 200, 200)), path);
            }
        }

        public virtual void Paint(object sender, Graphics g)
        {
        }

        public virtual void MouseDown(object sender, MouseEventArgs e) { }
        public virtual void MouseMove(object sender, MouseEventArgs e) { }
        public virtual void MouseUp(object sender, MouseEventArgs e) { }

        public virtual void DrawMouseMove(MouseEventArgs e)
        {
            ptCurrent = e.Location;

            Common.MyDrawReversibleRectangle(LeCanvas.self.Canvas, AreaRect);

            Rectangle old0 = AreaRect;
            AreaRect = Common.GetRectangle(ptOrigin, ptCurrent);
            Common.CheckForBoundary(LeCanvas.self.Canvas, ref AreaRect, old0);

            Common.MyDrawReversibleRectangle(LeCanvas.self.Canvas, AreaRect);
        }

        public virtual void MoveReversiableBoundary(MouseEventArgs e)
        {
            Common.MyDrawReversibleRectangle(LeCanvas.self.Canvas, AreaRect);

            Rectangle old = AreaRect;
            AreaRect = Common.MoveRectangle(e.Location, ptPrevious, AreaRect);

            Common.CheckForBoundary(LeCanvas.self.Canvas, ref AreaRect, old);
            Common.MyDrawReversibleRectangle(LeCanvas.self.Canvas, AreaRect);

            ptPrevious = e.Location;
        }

        public virtual bool DrawMouseUp(MouseEventArgs e) { return false; }
        public virtual void DrawMouseDown(MouseEventArgs e) {
            ptOrigin = e.Location;
            ptCurrent.X = -1;
        }

        internal void DrawSelected(PaintEventArgs e)
        {
            RectTracker.DrawPoints(e, bounds);
        }

        public virtual bool UpdateSelected(Point point, ref LeShape shape0)
        {
            if (Boundary.Contains(point))
            {
                selected = true;
                shape0 = this;
            }
            else
            {
                selected = false;
            }

            return selected;
        }
    }
}
