using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

using LePaint.Basic;

namespace LePaint.MainPart
{
    public class BaseCanvas : ICanvas, IShape
    {
        #region Properties
        EditMode curMode = EditMode.Nothing;

        ShapeAction curShapeAction = ShapeAction.Nothing;
        //GroupAction curGroupAction = GroupAction.Nothing;

        protected BaseShape toAddShape;

        protected Point ptOriginal = new Point(0, 0);
        protected Point ptCurrent = new Point(0, 0);

        private ArrayList selectedShapes;
        protected Rectangle AreaRect;

        #endregion
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

        internal enum MouseAction
        {
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

        #region constructor, paint
        
        private XMLShapes xmlShapes;

        public static Control Canvas;
        public BaseCanvas(Control canvas)
        {
            xmlShapes = new XMLShapes();
            Canvas = canvas;
        }

        public virtual void Paint(object sender, PaintEventArgs e)
        {
            foreach (BaseShape shape in xmlShapes.ShapeList)
            {
                shape.Paint(sender, e);
            }
        }

        #endregion

        #region Menu part
        void OnShowProperty(object sender, EventArgs e)
        {
            //ShapeForm frm = new ShapeForm();
            //frm.Show();
        }

        void OnDelete(object sender, EventArgs e)
        {
            /*
            foreach (BaseShape shape in selectedShapes)
            {

                if (shape is AreaShape)
                {
                    if (MessageBox.Show("Do you really want to delete this " + (shape as AreaShape).TextField.leShape.Caption +
                        " ?",
                        "Shape Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        shapeList.Remove(shape);
                    }
                }
                else if (shape is AreaTextShape)
                {
                    if (MessageBox.Show("Do you really want to delete this caption related shape " + (shape as AreaTextShape).leShape.Caption +
                        " ?",
                        "Shape Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        shapeList.Remove((shape as AreaTextShape).Parent);
                    }
                }

            }
            parent.Invalidate();
             */ 
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
            foreach (BaseShape shape in xmlShapes.ShapeList)
            {
                if (shape.Selected == true) shape.Selected = false;
            }
        }

        private ArrayList CheckRealSelectedShape()
        {
            ArrayList ret = new ArrayList();

            foreach (BaseShape shape in xmlShapes.ShapeList)
            {
                if (shape.Selected == true)
                    ret.Add(shape);
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

        #region Mouse Actions
        public void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (curMode)
            {
                case EditMode.Nothing:
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
                if (CheckSelectedShape(e).Count > 0)
                {
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
        #endregion

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
                        PrepareToDrawShape(e);
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

        public virtual void PrepareToDrawShape(MouseEventArgs e)
        {
            //toAddShape = new BaseShape(parent, new Rectangle());
        }

        private void ShapeMouseMoveActions(MouseEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Drawing:
                    DrawTempShape(e);
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

        public virtual void DrawTempShape(MouseEventArgs e)
        {
            ptCurrent = e.Location;
            Common.MyDrawReversibleRectangle(parent, AreaRect);

            Rectangle old0 = AreaRect;
            AreaRect = Common.GetRectangle(ptOriginal, ptCurrent);
            Common.CheckForBoundary(parent, ref AreaRect, old0);

            Common.MyDrawReversibleRectangle(parent, AreaRect);
        }

        private void ShapeMouseUpActions(MouseEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Drawing:
                    curShapeAction = ShapeAction.Nothing;
                    AddNewShape(AreaRect);
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

        public virtual ArrayList CheckSelectedShape(MouseEventArgs ex)
        {
            ArrayList ret = new ArrayList();

            foreach (BaseShape shape in xmlShapes.ShapeList)
            {
                if (shape.Boundary.Contains(ex.Location))
                {
                    ret.Add(shape);
                    break;
                }
            }
            return ret;
        }

        public virtual void AddNewShape(Rectangle rect) { }
        #endregion

        #region xml shapes handling
        protected void RemoveShape(BaseShape ob)
        {
            xmlShapes.Remove(ob);  
        }
        protected void AddShape(BaseShape ob)
        {
            xmlShapes.Add(ob);
            if (ob is AreaShape)
            {
                LeSerializableShape friend=(ob as AreaShape).TextField;
                xmlShapes.Add(friend);
                ob.FriendIndex = friend.Index;
            }
        }
        #endregion

        public virtual void Save(String fileName)
        {
            ExportToXML(fileName); 
        }
        public virtual void Load(string fileName)
        {
            DeSerializeXML(fileName); 
        }

        protected int GetShapeCount()
        {
            return xmlShapes.ShapeList.Count;
        }



        protected List<LeSerializableShape> GetShapeList()
        {
            return xmlShapes.ShapeList;
        }

        void leShapeList_ShapeReloaded(object sender, XMLShapes shapes)
        {
            xmlShapes = shapes;
            parent.Invalidate();

            /*
            xmlShapes.ShapeList.Clear();
            System.Console.WriteLine("Shapes count= " + xmlShapes.ShapeList.Count);
            foreach (LeSerializableShape shape in xmlShapes.ShapeList)
            {
                System.Console.WriteLine("Shapes type= " + typeof(shape));
                if (shapes.LeShapeType == LeShapeType.Rect)
                {
                    AreaShape shape0 = new AreaShape(parent, shape.Boundary);
                    AddShape(shape0); 
                    shape0.leShape =shape;
                    System.Console.WriteLine("Added " + shape0.leShape.ToString()); 
                }
            }
            */
        }

        #region xml serialize handling

        public void ExportToXML(string fileName)
        {
            TextWriter w = new StreamWriter(fileName);
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(XMLShapes));
                s.Serialize(w, xmlShapes);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Serialize XML " + e.Message);
                System.Console.WriteLine("Serialize XML " + e.InnerException.Message);
            }
            finally
            {
                w.Close();
            }
        }

        public void DeSerializeXML(string fileName)
        {
            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(fileName);
                XmlTextReader xr = new XmlTextReader(sr);
                XmlSerializer xs = new XmlSerializer(typeof(XMLShapes));
                object c;
                if (xs.CanDeserialize(xr))
                {
                    try
                    {
                        XMLShapes ret = (XMLShapes)xs.Deserialize(xr);
                        System.Console.WriteLine("XMLShapes Created");
                        OnShapesLoaded(ret);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Open file " + e.InnerException.Message);
                    }
                    finally
                    {
                        /*
                        c = xs.Deserialize(xr);
                        Type t = this.GetType();
                        PropertyInfo[] properties = t.GetProperties();
                        foreach (PropertyInfo p in properties)
                        {
                            p.SetValue(this, p.GetValue(c, null), null);
                        }
                         */
                    }
                }
                xr.Close();
                sr.Close();
            }
        }

        public delegate void ShapesLoadedFromFileHandler(object sender, XMLShapes shapes);
        public event ShapesLoadedFromFileHandler ShapeReloaded;
        private void OnShapesLoaded(XMLShapes ret)
        {
            if (ShapeReloaded != null)
            {
                ShapeReloaded(this, ret);
            }

            xmlShapes = ret;
        }

        #endregion


    }
}
