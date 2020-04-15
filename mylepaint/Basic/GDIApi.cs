using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LePaint.Basic
{
    internal class GDIApi
    {

        [StructLayout(LayoutKind.Explicit)]
        public struct WIN32Rect
        {
            [FieldOffset(0)]
            public int left;
            [FieldOffset(4)]
            public int top;
            [FieldOffset(8)]
            public int right;
            [FieldOffset(12)]
            public int bottom;
        }


        [DllImport("gdi32")]
        private static extern int GetClipBox(System.IntPtr hDC,
                                                ref WIN32Rect r);

        public static Rectangle GetViewableRect(Control control)
        {
            //! Get a graphics from the control, we need the HDC
            Graphics graphics = Graphics.FromHwnd(control.Handle);
            //! Get the hDC ( remember to call ReleaseHdc() when finished )
            IntPtr hDC = graphics.GetHdc();

            //! Create a rect to receive the viewable area of the control
            WIN32Rect r = new WIN32Rect();

            //! Call the Win32 method which recieves the viewable area
            GetClipBox(hDC, ref r);

            //! Convert that to a .NET Rectangle
            Rectangle rectangle = new Rectangle(r.left, r.top, r.right - r.left, r.bottom - r.top);

            //! Release the HDC (if you don't, the CLR throws an 
            //					 exception when it tries to Finalize/Dispose it)
            graphics.ReleaseHdc(hDC);
            //! Dispose of the graphics, we don' need it any more
            graphics.Dispose();
            graphics = null;

            return rectangle;
        }



    }

}