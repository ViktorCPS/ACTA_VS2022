using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SiemensUI;

namespace SiemensTransfer
{
    public partial class SiemensDataTransfer : Form
    {
        public SiemensDataTransfer()
        {
            InitializeComponent();
        }       

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mappingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SiemensMapping mapping = new SiemensMapping();
            mapping.ShowDialog();
        }

        private void brezaDBSetupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BrezaDBConnSetup brezaDb = new BrezaDBConnSetup();
            brezaDb.ShowDialog();
        }

        private void ascoDBSetupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SiemensDBConnSetup asco = new SiemensDBConnSetup();
            asco.ShowDialog();
        }

        private void SiemensDataTransfer_Load(object sender, EventArgs e)
        {

        }
    }
}
