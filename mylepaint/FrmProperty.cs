using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LePaint.MainPart;

namespace LePaint
{
    public partial class FrmProperty : Form
    {
        private static FrmProperty self;
        public FrmProperty(Object ob)
        {
            self = this;
            InitializeComponent();
            propertyGrid1.SelectedObject = ob;
            this.Disposed += new EventHandler(FrmProperty_Disposed);

            LeCanvas.SelectedShapeChanged += new LeCanvas.SelectedShapeChangedHandler(LeCanvas_SelectedShapeChanged);
        }

        void FrmProperty_Disposed(object sender, EventArgs e)
        {
            Form1.self.ClearPropertySelected();
            LeCanvas.SelectedShapeChanged -= new LeCanvas.SelectedShapeChangedHandler(LeCanvas_SelectedShapeChanged);
        }

        void LeCanvas_SelectedShapeChanged(object p)
        {
            propertyGrid1.SelectedObject = p;
        }
    }
}
