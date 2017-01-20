using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rationally.Visio.Forms.AlternativeStateConfiguration;

namespace Rationally.Visio.Forms
{
    public partial class AlternativeStatesConfigurator : Form
    {
        public AlternativeStatesConfigurator()
        {
            InitializeComponent();
        }

        private void addStateButton_Click(object sender, EventArgs e) => tableLayoutStateContent.AddRow();

        private void saveButton_Click(object sender, EventArgs e)
        {
            tableLayoutStateContent.Save();
        }
    }
}
