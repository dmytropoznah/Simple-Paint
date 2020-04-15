using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using LePaint.Basic;
using LePaint.MainPart;

namespace LePaint.Others
{
    public class SingleTool:BoundaryShape
    {
        //定义存入文件的元件数据结构
        internal byte StrokeNum;
        internal long CompId;
        internal string CompName;//20 bytes
        
        internal CompData[] compData = new CompData[30];
        private const int CompSize=18;
        private const int NameSize = 20;

        public double Multi { get; set; }

        public static SingleTool SelectedTool;

        /// <summary>
        /// Tool's original size
        /// </summary>
        private int width;
        private int height;

        public SingleTool(Point pt)
            : base(pt)
        {
            ShowBorder = false;
            
        }

        public void Update(SingleTool from)
        {
            this.StrokeNum = from.StrokeNum;
            this.CompId = from.CompId;
            this.CompName = from.CompName;
            this.width = from.width;
            this.height = from.height;

            Array.Copy(from.compData, this.compData, 30);
        }

        public SingleTool() { }

        public void InitTool(byte[] data)
        {
            int i=0;
            int j = 0;

            Multi = 1;
            StrokeNum = data[i++];
            CompId = BitConverter.ToInt32(data, i);
            i+=4;

            CompName = BitConverter.ToString(data, i, NameSize);

            i += 20;

            for(j=0;j<30;j++){
                CompData da = new CompData();
                da.Init(data, i + j * CompSize, CompSize);
                compData[j] = da;
            }

            GetToolSize();

        }

        private void GetToolSize()
        {
            int left=0;
            int right = 0;
            int top = 0;
            int bottom = 0;

            for (int i = 0; i < 30; i++)
            {
                if (compData[i].X0 < left)
                {
                    left = compData[i].X0;
                }

                if (compData[i].X1 > right)
                {
                    right = compData[i].X1;
                }

                if (compData[i].Y0 < top)
                {
                    top = compData[i].Y0;
                }

                if (compData[i].Y1 > bottom)
                {
                    bottom = compData[i].Y1;
                }
            }

            width = Math.Abs(right - left);
            height = Math.Abs(bottom - top);
        }

        public static object RawDataToObject(ref byte[] rawData,
            Type overlayType)
        {
            
            object result = null;

            /*
            GCHandle pinnedRawData = GCHandle.Alloc(rawData,
                GCHandleType.Pinned);
            try
            {

                // Get the address of the data array
                IntPtr pinnedRawDataPtr =
                    pinnedRawData.AddrOfPinnedObject();

                // overlay the data type on top of the raw data
                result = Marshal.PtrToStructure(
                    pinnedRawDataPtr,
                    overlayType);
            }
            finally
            {
                // must explicitly release
                pinnedRawData.Free();
            }
            */
            return result;
        }


        internal void Draw(Graphics g)
        {
            if (StrokeNum < 1) return;

            for (int i = StrokeNum - 1; i > 0; i--)
            {
                DrawStroke(g, compData[i]);
            }
        }

        /// <summary>
        /// 绘制元件
        /// </summary>
        private void DrawStroke(Graphics Ob, CompData ObjectData)
        {
            int X0;
            int Y0;
            int X1;
            int Y1;

            int X;
            int Y;
            int width;
            int height;

            X = this.Rect.X + this.Rect.Width / 2;
            Y = this.Rect.Y + this.Rect.Height / 2;

            int size=Math.Min ( this.Rect.Width, this.Rect.Height);

            double dxRatio = GetNewPos(ObjectData.X0, size);
            double dyRatio = GetNewPos(ObjectData.Y0, size);

            X0 = (int)(dxRatio + X);
            Y0 = (int)(dyRatio + Y);

            dxRatio = GetNewPos(ObjectData.X1, size);
            dyRatio = GetNewPos(ObjectData.Y1, size);
            
            X1 = (int)(dxRatio + X);
            Y1 = (int)(dyRatio+ Y);

            width = Math.Abs(X1 - X0);
            height = Math.Abs(Y1 - Y0);

            Pen pen = new Pen(new SolidBrush(Color.Black));
            switch (ObjectData.Pentype)
            {
                case 1://点
                    int t = Multi > 1 ? (int)Multi : 1;
                    Ob.DrawEllipse(pen, new Rectangle(X0, Y0,t,t));
                    break;
                //线
                case 2:
                    Ob.DrawLine(pen, new Point(X0, Y0), new Point(X1, Y1));
                    break;
                //空心矩形
                case 6:
                    Ob.DrawRectangle(pen, new Rectangle(X0, Y0, width, height));
                    break;
                //'空心圆
                case 4:
                    Ob.DrawEllipse(pen, new Rectangle(X0, Y0, width, height));
                    break;
                //'实心矩形
                case 7:
                    Ob.DrawRectangle(pen, new Rectangle(X0, Y0, width, height));
                    Ob.FillRectangle(new SolidBrush(Color.Aquamarine), new Rectangle(X0, Y0, width, height));
                    //Ob.FillColor = QBColor(7)
                    break;
                //'实心圆
                case 5:
                    Ob.FillEllipse(new SolidBrush(Color.Beige), new Rectangle(X0, Y0, width, height));
                    Ob.DrawEllipse(pen, new Rectangle(X0, Y0, width, height));
                    break;
                //'圆弧
                case 3:
                    if (width > 0 && height > 0)
                    {
                        Ob.DrawArc(pen, new Rectangle(X0, Y0, width, height), 10, 30);
                    }
                    break;
            }
        }

        private double GetNewPos(int myVal, int reference)
        {
            double ret = 0;

            if (myVal != 0)
            {
                int temp = Math.Abs(myVal);
                int dir = (myVal / temp);
                Multi = (double)(reference / (Math.Max(width,height))+0.0001);
                ret = Multi * temp * dir;
            }

            return ret;
        }


        public override bool DrawMouseUp(MouseEventArgs e)
        {
            bool check = false;
            if (AreaRect.Width > LeMenu.Size && AreaRect.Height > LeMenu.Size) check = true;

            if (check == true)
            {
                Boundary = AreaRect;
            }
            else path = null;

            this.Selected = true;
            this.Update(SingleTool.SelectedTool);
            this.Multi = 1;
            return check;
        }

        public override void Paint(object sender, Graphics g)
        {
            Draw(g);
        }
    }

    struct CompData{
        internal byte Pentype;   //笔型
        internal int X0;         //坐标
        internal int Y0;
        internal int X1;
        internal int Y1;
        internal Single StartAngle;    //圆弧起始角
        internal Single EndAngle;      //圆弧终止角
        internal byte Belong;    //部位

        internal void Init(byte[] data, int i, int count)
        {
            Console.WriteLine(i);
            Pentype = data[i++];
            X0 = BitConverter.ToInt16(data, i);
            i += 2; ;
            Y0 = BitConverter.ToInt16(data, i);
            i += 2; ;
            X1 = BitConverter.ToInt16(data, i);
            i += 2; ;
            Y1 = BitConverter.ToInt16(data, i);
            i += 2;
            StartAngle = BitConverter.ToSingle(data, i);
            i += 4;
            EndAngle = BitConverter.ToSingle(data, i);
            i += 4;

            Belong = data[i];
        }

    }




}
