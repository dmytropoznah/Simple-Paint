using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LePaint.Others;

namespace LePaint
{

    public partial class FrmFire : Form
    {
        LeTools myTools;

        public FrmFire(LeTools tools)
        {
            InitializeComponent();

            myTools = tools;

            CheckScrollBar();

            panel1.Top = 0;
            panel1.Left = 0;
        }

        private void FrmFire_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            myTools.DrawOn(panel1);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            myTools.MouseDown(panel1,e.X,e.Y);
            panel1.Invalidate();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            myTools.MouseUp(panel1,e.X,e.Y );
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            panel1.Top =5 - vScrollBar1.Value * myTools.ToolHeight;
        }

        void CheckScrollBar(){
            int VSize;
            vScrollBar1.Minimum  = 0;
            VSize = (myTools.Size) / 2;

            panel1.Height = myTools.Size * myTools.ToolHeight;

            if (VSize < 4){
                vScrollBar1.Maximum  = 0;
            }
            else{
                vScrollBar1.Maximum = VSize*2;
            }
        }
    }

}
