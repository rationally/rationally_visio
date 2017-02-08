using System;
using System.Windows.Forms;

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
            Close();
        }

        private void CancelButton_click(object sender, EventArgs e) => Close();
    }
}
