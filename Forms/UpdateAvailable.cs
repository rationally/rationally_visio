using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.RationallyConstants;

namespace Rationally.Visio.Forms
{
    public partial class UpdateAvailable : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public UpdateAvailable(Version current, Version New)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            CurrVer.Text = current.ToString();
            NewVer.Text = New.ToString();
        }

        private void Btn_Close_Click(object sender, EventArgs e) => Close();

        private void Btn_Down_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(Constants.RationallySite);
            Process.Start(sInfo);
            Close();
        }
    }
}
