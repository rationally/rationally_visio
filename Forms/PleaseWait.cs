using System.Reflection;
using System.Windows.Forms;
using log4net;

namespace Rationally.Visio.Forms
{
    public partial class PleaseWait : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public PleaseWait()
        {
            ControlBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }
    }
}
