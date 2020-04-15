using System;
using System.Windows.Forms;
using LePaint.MainPart;
using LePaint.Basic;

namespace LePaint
{
    public partial class Form1 : Form
    {
        LeCanvas  curCanvas;
        public static Form1 self; 
        public Form1()
        {
            InitializeComponent();
            curCanvas = new LeCanvas (pictureBox1);
            self = this;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            curCanvas.MouseDown(sender, e);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            curCanvas.MouseMove(sender, e);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            curCanvas.MouseUp(sender, e);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            curCanvas.Paint(sender, e);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "XML File (*.xml) |*.xml|JPEG File (*.jpg) |*.jpg";
            DialogResult ret= saveFileDialog1.ShowDialog();
            if (ret == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                curCanvas.Save(fileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML File (*.xml) |*.xml";
            DialogResult ret = openFileDialog1.ShowDialog();
            if (ret == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                curCanvas.Load(fileName);

                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            curCanvas.MouseDoubleClick(sender, e); 
        }

        private void aboutLePaintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckRegistry();
            //toolWnd.Parent = this;
            //propWnd.Parent = this;

            menuTool_Click(this, new EventArgs());
            menuProp_Click(this, new EventArgs());
        }

        private void CheckRegistry()
        {
            ModifyRegistry myRegistry = new ModifyRegistry();
            string ret = myRegistry.Read("Installation Date");

            if (ret == null)
            {
                myRegistry.Write("Installation Date", System.DateTime.Now.ToLongDateString());
                myRegistry.Write("Year", System.DateTime.Now.Year.ToString());
                myRegistry.Write("Month", System.DateTime.Now.Month.ToString());
                myRegistry.Write("Day", System.DateTime.Now.Day.ToString());
            }
            else
            {
                int year = int.Parse(myRegistry.Read("YEAR"));
                int month = int.Parse(myRegistry.Read("MONTH"));
                int day = int.Parse(myRegistry.Read("DAY"));
                System.DateTime newdate = new DateTime(year, month, day);
                System.TimeSpan day0 = System.DateTime.Now.Subtract(newdate);
                if (day0.Days  > 120)
                {
                      //MessageBox.Show (@"You have exceeded the 120 days trial period
                      //      Please contact mylepaint@yahoo.com for a copy"); 
                      //Application.Exit();
                }
            }
        }

        private void menuTool_Click(object sender, EventArgs e)
        {
            if (menuTool.Checked == true)
            {
                if (toolWnd == null)
                {
                    toolWnd = new FrmTool();
                }
                toolWnd.Hide();
                menuTool.Checked = false;
            }
            else
            {
                if (toolWnd==null){
                    toolWnd = new FrmTool();    
                }
                toolWnd.Show();
                menuTool.Checked = true;
            }
        }
        FrmTool toolWnd = new FrmTool();
        FrmProperty propWnd = new FrmProperty(null);

        private void menuProp_Click(object sender, EventArgs e)
        {
            if (menuProp.Checked == true)
            {
                propWnd.Hide();
                menuProp.Checked = false;
            }
            else
            {
                if (propWnd == null)
                {
                    propWnd = new FrmProperty(null);
                }
                propWnd.Show();
                menuProp.Checked = true;
            }
        }


        internal void ClearToolSelected()
        {
            menuTool.Checked = false;
            toolWnd = null;
        }

        internal void ClearPropertySelected()
        {
            menuProp.Checked = false;
            propWnd = null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void fireIndustriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void powerIndustriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}
