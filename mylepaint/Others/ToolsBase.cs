using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using LePaint.Basic;
namespace LePaint.Others
{
    public class ToolsBase
    {
        private const int singleToolSize = 565;
        private const int toolWidth = 64;
        protected const int toolHeight = 64;

        protected List<SingleTool> myTools = new List<SingleTool>();
        
        public ToolsBase(byte[] stream)
        {
            CreateFireShapes(stream );
        }

        private void CreateFireShapes(byte[] theData)
        {
            byte[] part = new byte[singleToolSize];
            myTools = new List<SingleTool>();
            int i = 0;
            while (true)
            {
                try
                {
                    Array.Copy(theData, i * singleToolSize, part, 0, singleToolSize);
                    object ob = (object)part;

                    SingleTool tool = new SingleTool();
                    tool.InitTool(part);
                    myTools.Add(tool);

                    i++;

                    //if (i >= 13) break;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Exception " + e.Message + " " + i);
                    break;
                }
            }

            System.Console.WriteLine("Total=" + i);
        }


        public static void DrawButton(Graphics g, int Left, int top, int width, int height,bool selected)
        {
            switch (selected)
            {
                case false://default
                    g.DrawLine(new Pen(Brushes.White), new Point(Left, top), new Point(Left + width, top));
                    g.DrawLine(new Pen(Brushes.White), new Point(Left, top), new Point(Left, top + height));
                    g.DrawLine(new Pen(Brushes.Black), new Point(Left + width, top), new Point(Left + width, top + height));
                    g.DrawLine(new Pen(Brushes.Black), new Point(Left, top + height), new Point(Left + width, top + height));
                    break;

                case true://push down
                    g.DrawLine(new Pen(Brushes.Blue), new Point(Left, top), new Point(Left + width, top));
                    g.DrawLine(new Pen(Brushes.Blue), new Point(Left, top), new Point(Left, top + height));
                    g.DrawLine(new Pen(Brushes.White), new Point(Left + width, top), new Point(Left + width, top + height));
                    g.DrawLine(new Pen(Brushes.White), new Point(Left, top + height), new Point(Left + width, top + height));
                    break;
            }
        }

        protected void RedrawTool(Graphics g, SingleTool data, int index)
        {
            int X;
            int Y;

            X = index % 2;
            Y = index / 2;

            Y = Y * 75 + 10;
            X = X * 75 + 10;

            Rectangle rect = new Rectangle();
            rect.X = X;// +32;
            rect.Y = Y;// +32;
            rect.Width = toolWidth;
            rect.Height = toolHeight;

            LeRect myRect = new LeRect(rect);
            data.Rect = new LeRect(rect);
            DrawButton(g, X, Y, toolWidth, toolHeight, data.Selected);
            data.Draw(g);
        }

    }

    
    public enum MouseAction
    {
        Normal,
        Down,
        Up
    }

}
