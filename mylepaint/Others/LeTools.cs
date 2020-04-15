using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

using LePaint.MainPart;

namespace LePaint.Others
{
    public class LeTools : ToolsBase
    {
        public int Size { get { return myTools.Count; } }

        public int ToolHeight { get { return toolHeight; } }

        public LeTools(byte[] theData)
            : base(theData)
        {
        }

        /// <summary>
        ///重画元件箱 
        /// </summary>
        /// <param name="panel1"></param>
        internal void DrawOn(Panel panel1)
        {
            Graphics g = panel1.CreateGraphics();

            for (int i = 0; i < myTools.Count; i++)
            {
                RedrawTool(g, myTools[i], i);
            }
        }


        internal void MouseDown(Panel panel1, int x, int y)
        {
            foreach (SingleTool tool in myTools)
            {
                if (tool.Boundary.Contains(new Point(x, y)))
                {
                    tool.Selected = true;
                    LeMenu.self.CurType =typeof(SingleTool);
                    LeMenu.self.DrawShape = true;

                    SingleTool.SelectedTool = tool;
                }
                else
                {
                    tool.Selected = false;
                }
            }
        }

        internal void MouseUp(Panel panel1, int x, int y)
        {

        }

    }

}
