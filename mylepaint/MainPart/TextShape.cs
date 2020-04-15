using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using LePaint.Basic;

namespace LePaint.MainPart
{
    public class TextShape : BoundaryShape
    {
        string caption = string.Empty;
        public string Caption
        {
            set { caption = value; }
            get { return caption; }
        }

        private LeFont textFont;
        public LeFont TextFont
        {
            set { textFont = value; }
            get { return textFont; }
        }

        private LeColor textColor;
        public LeColor TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        private int textSize;
        public int TextSize
        {
            get { return textSize; }
            set { textSize = value; }
        }

        private LeSerializableShape parent;
        public TextShape(string caption, Rectangle rect, LeSerializableShape parent)
            : base(rect)
        {
            this.parent = parent;
            Initialize(caption, rect);
            CalculateBoundary();
        }
        private TextShape()
        {
        }

        private void Initialize(string caption, Rectangle rect)
        {
            Caption = caption;
            TextSize = 15;
            Opaque = true;
            TextColor = new LeColor(Color.Red);
            TextFont = new LeFont(new Font("Tahoma", 15));
            Boundary = rect;
        }

        private void CalculateBoundary()
        {
            Font font = TextFont.ToFont();
            SizeF size = BaseCanvas.Canvas.CreateGraphics().MeasureString(Caption, font);
            Rectangle rect = new Rectangle(Boundary.X - 10, Boundary.Y + 10, (int)size.Width + 5, (int)size.Height + 5);

            Boundary = rect;
        }

        public override void Paint(object sender, PaintEventArgs e)
        {
            if (ShowBorder == true)
            {
                base.Paint(sender, e);
            }
            DrawMySelf(e.Graphics);
        }

        private void DrawMySelf(Graphics g)
        {
            if (Caption.Length > 0)
            {
                g.DrawString(Caption, TextFont.ToFont()
                    , new SolidBrush(TextColor.ToColor()), Boundary.Location.X + 3, Boundary.Location.Y + 3);
            }
        }

    }
}
