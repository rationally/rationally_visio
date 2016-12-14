using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Rationally.Visio.Forms
{
    public partial class PleaseWait : Form
    {
        public PleaseWait()
        {
            ControlBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }
    }
}
