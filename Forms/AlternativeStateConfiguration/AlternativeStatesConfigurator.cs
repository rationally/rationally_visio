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
        private static AlternativeStatesConfigurator instance;
        public static AlternativeStatesConfigurator Instance
        {
            get
            {
                if (instance?.IsDisposed ?? true)
                {
                    instance = new AlternativeStatesConfigurator();
                }
                return instance;
            }
        }

        private AlternativeStatesConfigurator()
        {
            InitializeComponent();
        }

        private void addStateButton_Click(object sender, EventArgs e) => tableLayoutStateContent.AddRow();

        private void saveButton_Click(object sender, EventArgs e)
        {
            tableLayoutStateContent.Save();
            
        }

        private void CancelButton_click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
