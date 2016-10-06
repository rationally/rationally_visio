using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Rationally.Visio.WindowsFormPopups
{
    public partial class ProjectSetupWizardOld : Form
    {
        private readonly List<Button> tabList;
        private List<Panel> tabPanels = new List<Panel>();

        private static readonly Color UnselectedButtonBackground = Color.FromArgb(220, 220, 220);
        private static readonly Color SelectedButtonBackground = Color.FromArgb(193, 206, 243);
        private static readonly Font UnselectedButtonFont = new Font("Calibri", (float) 10.0, FontStyle.Regular, GraphicsUnit.Point);
        private static readonly Font SelectedButtonFont = new Font("Calibri", (float)10.0, FontStyle.Bold, GraphicsUnit.Point);

        public ProjectSetupWizardOld()
        {

            InitializeComponent();
            tabList = new List<Button>() {overviewButton, consideredAlternativesButton, forcesAndConcernsButton, stakeholderButton, relatedButton};
            tabList.ForEach(b => b.Click += TabButton_Click);

        }

        private void TabButton_Click(object sender, EventArgs e)
        {
            Button button = (Button) sender;
            tabList.ForEach(b => b.BackColor = UnselectedButtonBackground);
            button.BackColor = SelectedButtonBackground;

            tabList.ForEach(b => b.Font = UnselectedButtonFont);
            button.Font = SelectedButtonFont;
            /*Graphics g = e.
            Brush _textBrush;

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = overviewButton.Bounds;

            Font fontBold = new Font("Calibri", (float)10.0, FontStyle.Bold, GraphicsUnit.Pixel);
            Font fontNormal = 

            Font fontToDrawIn;

            if (e. == DrawItemState.Selected)
            {
                Color hoverColor = Color.FromArgb(193, 206, 243);

                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Black);
                fontToDrawIn = fontBold;
                g.FillRectangle(new SolidBrush(hoverColor), e.Bounds);
                //_tabBounds.
                //g.FillRectangle(Brushes.Gray, e.Bounds.);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                fontToDrawIn = fontNormal;
                e.DrawBackground();
            }

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Near;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, fontToDrawIn, _textBrush, _tabBounds, new StringFormat(_stringFlags));*/
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
