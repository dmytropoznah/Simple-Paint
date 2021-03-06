﻿using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic ;

using LePaint.Basic;

namespace LePaint.MainPart
{
    public class LeCanvas :LeShape,ICanvas
    {
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

        public Image BackGround
        {
            set
            {
                Canvas.BackgroundImage = value;
            }
        }


        EditMode curMode = EditMode.Nothing;

        ShapeAction curShapeAction = ShapeAction.Nothing;
        GroupAction curGroupAction = GroupAction.Nothing;

        private ArrayList selectedShapes;
        private LeMenu myMenu;
        private EditMode prevMode;

        #region constructor, paint
        
        public XMLShapes xmlShapes;

        public static LeCanvas self;

        public Control Canvas;

        public LeCanvas(Control canvas)
        {
            xmlShapes = new XMLShapes();
            Canvas = canvas;

            myMenu = new LeMenu();
            self = this;
        }

        #endregion

        public static LeShape SelectedShape = null;
        GroupShapes groupShapes = new GroupShapes();

        #region Mouse Actions
        public void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (SelectedShape != null)
            {
                if (SelectedShape.Boundary.Contains(e.Location))
                {
                    //myMenu.ShowSelectedProperties(SelectedShape);
                }
            }
            else
            {
                //myMenu.ShowSelectedProperties(this);
            }
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CheckRightMouseDown(e);
                return;
            }

            if (MouseOnShape(e.Location) == false)
            {
                if (myMenu.DrawShape == false)
                {
                    curMode = EditMode.GroupMode;
                }
            }
            else
            {
                curMode = EditMode.ShapeMode;
            }

            switch (curMode)
            {
                case EditMode.ShapeMode:
                    Canvas.Cursor = Cursors.Default;
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

        private void CheckRightMouseDown(MouseEventArgs e)
        {
            if (groupShapes.Count > 0)
            {
                if (groupShapes.Boundary.Contains(e.Location))
                {
                    myMenu.ShowEditShapesMenus(e);
                }
                else
                {
                    groupShapes = new GroupShapes();
                }
            }
            else
            {
                if (MouseOnShape(e.Location))
                {
                    myMenu.ShowEditShapesMenus(e);
                    curMode = EditMode.ShapeMode;
                }
                else
                {
                    myMenu.ShowDrawingShapeMenus(e);
                }
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (myMenu.DrawShape == true) curMode = EditMode.ShapeMode;

            switch (curMode)
            {
                case EditMode.ShapeMode:
                    Canvas.Cursor = Cursors.Default;
                    ShapeMouseMoveActions(e);
                    break;
                case EditMode.GroupMode:
                    GroupMouseMoveAction(e);
                    break;
            }
        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            switch (curMode)
            {
                case EditMode.ShapeMode:
                    Canvas.Cursor = Cursors.Default;
                    ShapeMouseUpActions(e);
                    break;
                case EditMode.GroupMode:
                    GroupMouseUpAction(e);
                    break;
            }
            Canvas.Invalidate();
        }

        public override void Paint(object sender, Graphics g) { }

        public void Paint(object sender, PaintEventArgs e)
        {
            foreach (LeShape shape in xmlShapes.GetList())
            {
                shape.Paint(sender, e.Graphics);
                if (shape.Selected)
                {
                    shape.DrawSelected(e);
                }
            }

            if (groupShapes.Count > 0)
            {
                groupShapes.Paint(sender, e);
            }
        }

        #endregion

        #region Groups Mode
        private void GroupMouseDownAction(MouseEventArgs e)
        {
            switch (curGroupAction)
            {
                case GroupAction.Drawing:
                    break;
                case GroupAction.MoveGroupShapes:
                    break;
                case GroupAction.Nothing:
                    if (groupShapes.Boundary.Contains(e.Location))
                    {
                        curGroupAction = GroupAction.MoveGroupShapes;
                        groupShapes.MouseDown(this, e);
                    }
                    else
                    {
                        ClearSelectedShape();
                        curGroupAction = GroupAction.Drawing;
                        ptOrigin = e.Location;
                        ptCurrent.X = -1;
                        AreaRect = new Rectangle();
                    }
                    break;
            }

        }
        private void GroupMouseMoveAction(MouseEventArgs e)
        {
            switch (curGroupAction)
            {
                case GroupAction.Drawing:
                    base.DrawMouseMove(e);
                    break;
                case GroupAction.MoveGroupShapes:
                    groupShapes.MouseMove(this, e);
                    break;
                case GroupAction.Nothing:
                    if (groupShapes.Boundary.Contains(e.Location))
                    {
                        Canvas.Cursor = Cursors.Hand;
                        groupShapes.MouseMove(this, e);
                    }
                    else
                        Canvas.Cursor = Cursors.Default;
                    break;
            }

        }

        private void GroupMouseUpAction(MouseEventArgs e)
        {
            switch (curGroupAction)
            {
                case GroupAction.Drawing:
                    groupShapes.SetSelectedShapes(AreaRect);
                    if (groupShapes.Count == 0)
                        groupShapes = new GroupShapes();

                    curGroupAction = GroupAction.Nothing;
                    base.DrawMouseMove(e);
                    break;
                case GroupAction.MoveGroupShapes:
                    groupShapes.Move();
                    groupShapes.MouseUp(this,e);
                    curGroupAction = GroupAction.Nothing;
                    break;
                case GroupAction.Nothing:
                    if (groupShapes.Boundary.Contains(e.Location))
                    {
                        Canvas.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        Canvas.Cursor = Cursors.Default;
                    }
                    break;
            }
        }

        private void OnShapeMoved(MouseEventArgs e)
        {
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Selected)
                {
                    Rectangle rect=Common.MoveRectangle(ptOrigin,e.Location,Boundary);
                    shape.Boundary=rect ;
                }
            }
        }

        private bool MouseOnShape(Point point)
        {
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Boundary.Contains(point))
                {
                    return true;
                }
                if (shape is ZoneShape)
                {
                    if ((shape as ZoneShape).TextField.Boundary.Contains(point))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Shapes Mode

        private void ShapeMouseDownActions(MouseEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Nothing:
                    if (MouseOnShape(e.Location) == false)
                    {
                        if (myMenu.DrawShape == true)
                        {
                            curShapeAction = ShapeAction.Drawing;
                            DrawMouseDown(e);
                            ClearSelectedShape();
                        }
                        OnSelectedShapeChanged(Canvas);
                    }
                    else
                    {
                        curShapeAction = ShapeAction.MovingShape;
                        selectedShapes = GetSelectedShape(e);
                        SendMouseAction(selectedShapes, e, MouseAction.Down);

                        SelectedShape = selectedShapes[0] as LeShape;

                        OnSelectedShapeChanged(selectedShapes[0]);
                    }
                    break;
                default:
                    break;
            }
        }

        public override void DrawMouseDown(MouseEventArgs e)
        {
            myMenu.CreateNewShape(e);
            myMenu.CurShape.Boundary = new Rectangle(); 
            myMenu.CurShape.DrawMouseDown(e);
        }

        private void ShapeMouseMoveActions(MouseEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Drawing:
                    myMenu.CurShape.DrawMouseMove(e); 
                    break;
                case ShapeAction.MovingShape:
                    SendMouseAction(selectedShapes, e, MouseAction.Move);
                    break;
                case ShapeAction.MoveGroupShapes:
                    break;
                case ShapeAction.Nothing:
                    SendMouseAction(GetSelectedShape(e), e, MouseAction.Move);
                    break;
            }
        }

        private void ShapeMouseUpActions(MouseEventArgs e)
        {
            switch (curShapeAction)
            {
                case ShapeAction.Drawing:
                    curShapeAction = ShapeAction.Nothing;
                    AddNewShape(e);
                    break;
                case ShapeAction.MovingShape:
                    SendMouseAction(selectedShapes, e, MouseAction.Up);
                    curShapeAction = ShapeAction.Nothing;
                    break;
            }
        }

        private void SendMouseAction(ArrayList arr, MouseEventArgs e, MouseAction act)
        {
            foreach (LeShape shape in arr)
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

        public virtual ArrayList GetMouseOnShapes(MouseEventArgs ex)
        {
            ArrayList ret = new ArrayList();

            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Boundary.Contains(ex.Location))
                {
                    ret.Add(shape);
                    break;
                }
            }
            return ret;
        }

        public void AddNewShape(MouseEventArgs e) {
            //if (myMenu.selectBack == false)
            {
                bool check=myMenu.CurShape.DrawMouseUp(e);
                if (check == true)
                {
                    xmlShapes.Add(myMenu.CurShape);
                }
            }
        }
        #endregion

        public void DeleteSelectedShapes()
        {
            List<LeShape> ret = new List<LeShape>();
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Selected) ret.Add(shape);
            }

            foreach (LeShape shape in ret)
            {
                xmlShapes.GetList().Remove(shape);
            }

            Canvas.Invalidate();
        }

        private void ClearSelectedShape()
        {
            foreach (LeShape shape in xmlShapes.GetList())
            {
                shape.Selected = false;
            }
        }

        private ArrayList GetSelectedShapes()
        {
            ArrayList ret = new ArrayList();

            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.Selected)
                    ret.Add(shape);
            }
            return ret;
        }

        private ArrayList GetSelectedShape(MouseEventArgs e)
        {
            ArrayList ret = new ArrayList();
            LeShape shape0=null;
            foreach (LeShape shape in xmlShapes.GetList())
            {
                if (shape.UpdateSelected(e.Location, ref shape0))
                {
                    ret.Add(shape0);
                    break;
                }
            }
            return ret;
        }

        public delegate void SelectedShapeChangedHandler(object p);
        public static event SelectedShapeChangedHandler SelectedShapeChanged;
        private void OnSelectedShapeChanged(object shape)
        {
            if (SelectedShapeChanged != null)
            {
                SelectedShapeChanged(shape);
            }
        }


        #region xml shapes handling
        protected void RemoveShape(LeShape ob)
        {
            xmlShapes.Remove(ob);
        }
        #endregion

        public void Save(String fileName)
        {
            if (fileName.ToUpper().EndsWith("XML"))
            {
                LeMenu.ExportToXML(fileName);
            }
            else
            {
                if (Canvas is PictureBox)
                {
                    PictureBox me = (Canvas as PictureBox);
                    
                    Image myImage = new System.Drawing.Bitmap(me.Width, me.Height);
                    Graphics g= System.Drawing.Graphics.FromImage(myImage);
                    g.FillRectangle(new SolidBrush(Color.White),0,0, me.Width, me.Height);
                    foreach (LeShape shape in xmlShapes.GetList())
                    {
                        shape.Paint(this, g);
                    }
                    me.Image = myImage ;
                    myImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
        
        public void Load(string fileName)
        {
            LeMenu.DeSerializeXML(fileName); 
        }
    }
}
