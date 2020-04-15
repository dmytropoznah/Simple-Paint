using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Xml.Serialization ;

using LePaint.Basic;
using LePaint.MainPart;

namespace LePaint.Shapes
{
    public class TextShape : BoundaryShape
    {
        #region properties
        string caption = string.Empty;
        public string Caption
        {
            set { caption = value;
            LeCanvas.self.Canvas.Invalidate();
            }
            get { return caption; }
        }

        private LeFont textFont;
        public LeFont LeTextFont
        {
            set { textFont = value;
            }
            get { return textFont; }
        }

        [XmlIgnore]
        public Font TextFont
        {
            get { return textFont.ToFont(); }
            set
            {
                textFont = new LeFont(value);
                LeCanvas.self.Canvas.Invalidate();
            }
        }
        private LeColor textColor;
        public LeColor LeTextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        [XmlIgnore]
        public Color TextColor
        {
            get { return textColor.ToColor(); }
            set
            {
                textColor = new LeColor(value);
                LeCanvas.self.Canvas.Invalidate();
            }
        }

        public int TextSize
        {
            get { return (int)textFont.Size; }
            set { textFont.Size = value;
            LeCanvas.self.Canvas.Invalidate();
            }
        }
        #endregion

        private LeShape parent;
        public TextShape(string caption, Point pt, LeShape parent)
            : base(pt)
        {
            this.parent = parent;
            Initialize(caption, pt);
            InitBoundary();
        }

        private TextShape() {
        }

        public TextShape(Point pt)
            :base(pt)
        {
            TextFont = new Font("Times New Roman", 40);
            TextSize = 40;
            Initialize("TXT", new Point(pt.X+5,pt.Y+5)  );
            InitBoundary();
            ShowBorder = false;
            TextColor = Color.YellowGreen;
        }

        private void Initialize(string caption, Point pt)
        {
            Caption = caption;
            TextSize = 15;
            LeTextColor = new LeColor(Color.Red);
            LeTextFont = new LeFont(new Font("Tahoma", 10));
            Boundary = new Rectangle(pt, new Size(0, 0));
        }

        private void InitBoundary()
        {
            Font font = TextFont;
            SizeF size = LeCanvas.self.Canvas.CreateGraphics().MeasureString(Caption, font);
            Rectangle rect = new Rectangle(Boundary.X - 10, Boundary.Y + 10, (int)size.Width + 5, (int)size.Height + 5);

            Boundary = rect;
        }

        private void CalculateNewBoundary()
        {
            SizeF size = LeCanvas.self.Canvas.CreateGraphics().MeasureString(Caption, TextFont);
            Rectangle rect = new Rectangle(Boundary.X, Boundary.Y, (int)size.Width+5 , (int)size.Height+5 );
            Boundary = rect;
        }

        public override void DrawMouseDown(MouseEventArgs e)
        {
            ptOrigin = e.Location;
            ptCurrent.X = -1;
            Initialize("test",e.Location);
            InitBoundary();
            ShowBorder = true;
        }

        public override void DrawMouseMove(MouseEventArgs e)
        {
        }

        public override bool DrawMouseUp(MouseEventArgs e)
        {
            LeCanvas.self.Canvas.Invalidate();
            return true;
        }


        public override void Paint(object sender, Graphics g)
        {
            CalculateNewBoundary();
            if (ShowBorder == true)
            {
                base.Paint(sender, g);
            }
            DrawMySelf(g);
        }

        private void DrawMySelf(Graphics g)
        {
            if (Caption.Length > 0)
            {
                g.DrawString(Caption, LeTextFont.ToFont()
                    , new SolidBrush(LeTextColor.ToColor()), Boundary.Location.X + 3, Boundary.Location.Y + 3);
            }
        }

    }
}
