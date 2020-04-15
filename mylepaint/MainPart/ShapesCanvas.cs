using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LePaint.Basic;
using System.Xml;

namespace LePaint.MainPart
{
    public class ShapesCanvas : BaseCanvas, ICanvas  
    {
        public ShapesCanvas(Control p)
            :base(p)
        {
        }

        /// <summary>
        /// only for XML serialize or deserialize use
        /// </summary>
        public override void AddNewShape(Rectangle newRect)
        {
            if (newRect.Width > 20 && newRect.Height > 10)
            {
                AreaShape shape = new AreaShape(newRect, base.GetShapeCount());

                base.AddShape(shape);
            }
            parent.Invalidate();
        }

        public override ArrayList CheckSelectedShape(MouseEventArgs ex)
        {
            ArrayList ret = new ArrayList();

            foreach (BaseShape shape in base.GetShapeList())
            {
                if (shape is AreaShape)
                {
                    AreaShape area = shape as AreaShape;
                    if (area.TextField.Boundary.Contains(ex.Location))
                    {
                        ret.Add(area.TextField);
                        break;
                    }
                }
                if (shape.Boundary.Contains(ex.Location))
                {
                    ret.Add(shape);
                    break;
                }
            }
            return ret;
        }

        /*
        #region enums
        internal enum ShapeAction
        {
            Nothing,
            MovingShape,
            Drawing,
            MoveGroupShapes,
            ResizingShape
        }

        internal enum GroupAction
        {
            Nothing,
            Drawing,
            MoveGroupShapes,
        }

        internal enum EditMode
        {
            Nothing,
            ShapeMode,
            GroupMode
        }

        internal enum MouseAction{
            Down,
            Up,
            Move
        }
        internal enum Position
        {
            TopLeft,
            RightTop,
            RightBottom,
            LeftBottom,
            Center
        }
        #endregion

        EditMode curMode = EditMode.Nothing;

        Point ptOriginal = new Point(0, 0);
        Point ptCurrent = new Point(0, 0);

        Rectangle AreaRect;

        Control parent;

        public static ZonesCanvas Self;

        ShapeAction curShapeAction = ShapeAction.Nothing;
        GroupAction curGroupAction = GroupAction.Nothing;

        ArrayList zoneList = new ArrayList();
        ArrayList deviceList = new ArrayList();

        #region no change part
        public ZonesCanvas(Control p)
        {
            parent = p;
            Self = this;
        }

        public void Paint(object sender, PaintEventArgs e)
        {
            foreach (ZoneShape zone in zoneList)
            {
                zone.Paint(sender,e);
                zone.TextField.Paint(sender, e); 
            }
        }

        #endregion

        #region Menu part
        void OnShowProperty(object sender, EventArgs e)
        {
            ZoneForm frm = new ZoneForm();
            frm.Show();
        }

        void OnDelete(object sender, EventArgs e)
        {
            foreach (BaseShape shape in selectedShapes)
            {
                
                if (shape is ZoneShape)
                {
                    if (MessageBox.Show("Do you really want to delete this " + (shape as ZoneShape).TextField.Caption +
                        " ?",
                        "Shape Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        zoneList.Remove(shape);
                    }
                }
                else if (shape is ZoneTextShape)
                {
                    if (MessageBox.Show("Do you really want to delete this caption related shape " +(shape as ZoneTextShape).Caption +
                        " ?",
                        "Shape Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        zoneList.Remove((shape as ZoneTextShape).Parent);
                    }
                }
                
            }
            parent.Invalidate(); 
        }

        void ShowShapeMenus(MouseEventArgs e)
        {
            selectedShapes = CheckRealSelectedShape();
 
            ContextMenu menu = new ContextMenu();
            ///to translate
            MenuItem item = new MenuItem("Delete");

            item.Click += new EventHandler(OnDelete);
            menu.MenuItems.Add(item);

            item = new MenuItem("Property");
            item.Click += new EventHandler(OnShowProperty);
            menu.MenuItems.Add(item);
 
            menu.Show(parent, new Point(e.X, e.Y));
        }

        private void ClearRealSelectedShape()
        {
            foreach (ZoneShape shape in zoneList)
            {
                if (shape.BorderShape.Selected == true) shape.BorderShape.Selected = false;
                if (shape.TextField.Selected == true) shape.TextField.Selected =false ;
            }
        }
        private ArrayList CheckRealSelectedShape()
        {
            ArrayList ret=new ArrayList ();
            
            foreach(ZoneShape shape in zoneList){
                if (shape.BorderShape.Selected == true)
                    ret.Add(shape);
                if (shape.TextField.Selected ==true){
                    ret.Add(shape.TextField);                     
                }
            }
            return ret;
        }

        private void ShowCanvasMenus(MouseEventArgs e)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem item;

            item = new MenuItem("Full Screen");
            item.RadioCheck = true;

            item.Click += delegate(object sender, EventArgs ex)
            {
                if (item.Checked == true) item.Checked = false;
                else item.Checked = true;
            };
            menu.MenuItems.Add(item);

            item = new MenuItem("Clear Shapes");
            item.Click += delegate(object sender, EventArgs ex)
            {
                //if (item.Checked == true) item.Checked = false;
                //else item.Checked = true;
            };
            menu.MenuItems.Add(item);

            menu.Show(parent, new Point(e.X, e.Y));
        }

        #endregion

        public void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (curMode)
            {
                case EditMode.Nothing :
                    curMode = EditMode.GroupMode;
                    parent.Cursor = Cursors.UpArrow;
                    break;
                case EditMode.GroupMode:
                    curMode = EditMode.ShapeMode;
                    parent.Cursor = Cursors.Default;
                    break;
                case EditMode.ShapeMode:
                    curMode = EditMode.Nothing;
                    parent.Cursor = Cursors.Default;
                    break;
            }
        }


        public void MouseDown(object sender, MouseEventArgs e)
        {
            ptOriginal = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                if (CheckSelectedShape(e).Count >0){
                    selectedShapes = CheckSelectedShape(e);
                    ShowShapeMenus(e);
                }
                else
                {
                    ShowCanvasMenus(e);
                }
                return;
            }
            switch (curMode)
            {
                case EditMode.ShapeMode:
                    parent.Cursor = Cursors.Default;
                    ShapeMouseDownActions(e);
                    break;
                case EditMode.GroupMode:
                    GroupMouseDownAction(e);
                    break;
                case EditMode.Nothing:
                    curMode = EditMode.ShapeMode;
                    ShapeMouseDownActions(e);
                    break;
            }
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            switch (curMode)
            {
                case EditMode.ShapeMode:
                    parent.Cursor = Cursors.Default;
                    ShapeMouseMoveActions(e);
                    break;
                case EditMode.GroupMode:
                    GroupMouseMoveAction(e);
                    break;
            }
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            switch (curMode)
            {
                case EditMode.ShapeMode:
                    parent.Cursor = Cursors.Default;
                    ShapeMouseUpActions(e);
                    break;
                case EditMode.GroupMode:
                    GroupMouseUpAction(e);
                    break;
            }
        }

        #region Groups Mode
        private void GroupMouseDownAction(MouseEventArgs e)
        {
        }
        private void GroupMouseMoveAction(MouseEventArgs e)
        {
        }
        private void GroupMouseUpAction(MouseEventArgs e)
        {
        }

        #endregion

        #region Shapes Mode

        private void ShapeMouseDownActions(MouseEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Nothing:
                    if (CheckSelectedShape(e).Count == 0)
                    {
                        curShapeAction = ShapeAction.Drawing;
                        ClearRealSelectedShape();
                    }
                    else
                    {
                        curShapeAction = ShapeAction.MovingShape;
                        selectedShapes = CheckSelectedShape(e);
                        SendMouseAction(selectedShapes, e, MouseAction.Down);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ShapeMouseMoveActions(MouseEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Drawing:
                    ptCurrent = new Point(e.X, e.Y);
                    Common.MyDrawReversibleRectangle(parent, AreaRect);

                    Rectangle old0 = AreaRect;
                    AreaRect = Common.GetRectangle(ptOriginal, ptCurrent);
                    Common.CheckForBoundary(parent, ref AreaRect, old0);

                    Common.MyDrawReversibleRectangle(parent, AreaRect);
                    break;
                case ShapeAction.MovingShape:
                    SendMouseAction(selectedShapes, e, MouseAction.Move);
                    break;
                case ShapeAction.MoveGroupShapes:
                    break;
                case ShapeAction.Nothing:
                    SendMouseAction(CheckSelectedShape(e), e, MouseAction.Move);
                    break;
            }
        }

        private void ShapeMouseUpActions(MouseEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Drawing:
                    AddNewShape(e);
                    curShapeAction = ShapeAction.Nothing;
                    break;
                case ShapeAction.MovingShape:
                    SendMouseAction(selectedShapes, e, MouseAction.Up);
                    curShapeAction = ShapeAction.Nothing;
                    break;
            }
        }

        private void SendMouseAction(ArrayList arr, MouseEventArgs e, MouseAction act)
        {
            foreach (BaseShape shape in arr)
            {
                switch (act)
                {
                    case MouseAction.Down:
                        shape.MouseDown(this, e);
                        break;
                    case MouseAction.Up:
                        shape.MouseUp(this, e);
                        break;
                    case MouseAction.Move:
                        shape.MouseMove(this, e);
                        break;
                }
            }
        }

        private ArrayList selectedShapes;
        private ArrayList CheckSelectedShape(MouseEventArgs ex)
        {
            ArrayList ret = new ArrayList();
            Point p = new Point(ex.X, ex.Y);

            foreach (ZoneShape shape in zoneList)
            {
                if (shape.TextField.Boundary.Contains(p)){
                    ret.Add(shape.TextField);
                    break;
                }
                if (shape.BorderShape.Boundary.Contains(p))
                {
                    ret.Add(shape);
                    break;
                }
            }
            return ret;
        }

        #endregion

        #region draw lines
        private void EraseMyLine(Point pt0, Point pt1)
        {
            Point p0 = parent.PointToScreen(pt0);
            Point p1 = parent.PointToScreen(pt1);

            ControlPaint.DrawReversibleLine(p0, p1, Color.Black);
        }

        private void DrawMyLine(Point pt0, ref Point pt1)
        {
            int dx0 = Math.Abs(pt1.X - pt0.X);
            int dy0 = Math.Abs(pt1.Y - pt0.Y);

            if (dx0 > 0)
            {
                float f = dy0 / dx0;
                if (f > 1)
                {
                    pt1.X = pt0.X;
                }
                else
                {
                    pt1.Y = pt0.Y;
                }
            }

            Point p0 = parent.PointToScreen(pt0);
            Point p1 = parent.PointToScreen(pt1);

            ControlPaint.DrawReversibleLine(p0, p1, Color.Black);
        }
        #endregion
         */
    }
}
