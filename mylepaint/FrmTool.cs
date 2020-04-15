using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using LePaint.Basic;
using LePaint.MainPart ;
using LePaint;

namespace LePaint
{
    public partial class FrmTool : Form
    {
        List<LeShape> curTools = new List<LeShape>();
        ArrowShape btnCancel=new ArrowShape ();
        Rectangle selected = new Rectangle();

        public FrmTool()
        {
            InitializeComponent();

            this.Disposed += new EventHandler(FrmTool_Disposed);
        }

        void FrmTool_Disposed(object sender, EventArgs e)
        {
            Form1.self.ClearToolSelected();
        }

        private void Canvas_Click(object sender, EventArgs e)
        {

        }

        private void FrmTool_Load(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle();
            rect.X = 10; rect.Y = 10;
            rect.Size = new Size(LeMenu.Size * 2, LeMenu.Size * 2);

            btnCancel = new ArrowShape(rect.Location);
            btnCancel.Change();

            int i = 1;
            rect.X += rect.Width+5;
            foreach (Type type in LeMenu.shapeMenus.Keys)
            {
                ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(Point) });
                LeShape shape = constructor.Invoke(new object[] { rect.Location }) as LeShape;
                shape.Boundary = rect;
                curTools.Add(shape);

                rect.X += rect.Width+5;
                i++;
                if (i > 1)
                {
                    i = 0;
                    rect.X = 10;
                    rect.Y += rect.Height+5;
                }
            }
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            DrawTools(e);
            if (selected.X > 0)
            {
                LePaint.Others.ToolsBase.DrawButton(e.Graphics, selected.X, selected.Y,
                    selected.Width, selected.Height, true);
            }
        }

        private void DrawTools(PaintEventArgs e)
        {
            btnCancel.Paint(this, e.Graphics);
            foreach (LeShape shape in curTools)
            {
                shape.Paint(this, e.Graphics);

                LePaint.Others.ToolsBase.DrawButton(e.Graphics, shape.Rect.X, shape.Rect.Y,
                    shape.Rect.Width, shape.Rect.Height, false);
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            SetSelected(e.Location);
            Canvas.Invalidate(); 
        }

        private void SetSelected(Point point)
        {
            if (btnCancel.Boundary.Contains(point))
            {
                selected = btnCancel.Boundary;
                if (LeMenu.self != null)
                {
                    LeMenu.self.CurShape = null;
                    LeMenu.self.DrawShape = false;
                }
            }
            else
            {
                foreach (LeShape shape in curTools)
                {
                    if (shape.Boundary.Contains(point))
                    {
                        selected = shape.Boundary;
                        LeMenu.self.CurType = shape.GetType();
                        LeMenu.self.DrawShape = true;
                        break;
                    }
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
