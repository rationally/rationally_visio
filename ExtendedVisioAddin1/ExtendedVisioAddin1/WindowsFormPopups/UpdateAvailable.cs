﻿using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Rationally.Visio.WindowsFormPopups
{
    public partial class UpdateAvailable : Form
    {
        public UpdateAvailable(Version current, Version New)
        {
            InitializeComponent();
            CurrVer.Text = current.ToString();
            NewVer.Text = New.ToString();
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Btn_Down_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://rationally.github.io/");
            Process.Start(sInfo);
            Close();
        }
    }
}
