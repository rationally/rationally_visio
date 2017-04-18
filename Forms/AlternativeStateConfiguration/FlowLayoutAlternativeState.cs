using System;
using System.Drawing;
using System.Windows.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.Forms.AlternativeStateConfiguration
{
    class FlowLayoutAlternativeState : FlowLayoutPanel
    {
        private Label labelStateTitle;
        public TextBox StateTextBox;
        private Button buttonPickStateColor;
        private Button removesStateButton;

        public string OldState { get;  }
        public string NewState => StateTextBox.Text;
        public Color Color { get; private set; }
        public int Index { get; set; }

        public FlowLayoutAlternativeState(int index)
        {
            OldState = null;
            Color = Constants.DefaultStateColor;
            Index = index;
            Init();
        }

        public FlowLayoutAlternativeState(AlternativeState state, int index)
        {
            OldState = state.GetName();
            Color = state.GetColor();
            Index = index;
            Init();
        }

        private void Init()
        {
            labelStateTitle = new Label();
            StateTextBox = new TextBox();
            buttonPickStateColor = new Button();
            removesStateButton = new Button();

            // 
            // flowLayoutPanel1
            // 
            Controls.Add(labelStateTitle);
            Controls.Add(StateTextBox);
            Controls.Add(buttonPickStateColor);
            Controls.Add(removesStateButton);
            Dock = DockStyle.Top;
            Location = new Point(3, 3);
            Name = "flowLayoutPanel1";
            Size = new Size(483, 40);
            Margin = new Padding(0,0,0,5);
            TabIndex = 0;
            // 
            // labelStateTitle
            // 
            labelStateTitle.AllowDrop = true;
            labelStateTitle.Anchor = AnchorStyles.Left;
            labelStateTitle.Location = new Point(3, 14);
            labelStateTitle.Margin = new Padding(3, 10, 3, 0);
            labelStateTitle.Name = "labelStateTitle";
            labelStateTitle.Size = new Size(35, 14);
            labelStateTitle.TabIndex = 0;
            labelStateTitle.Text = "State:";
            // 
            // textBox1
            // 
            StateTextBox.Location = new Point(44, 10);
            StateTextBox.Margin = new Padding(3, 10, 3, 3);
            StateTextBox.Name = "textBox1";
            StateTextBox.Size = new Size(194, 20);
            StateTextBox.TabIndex = 1;
            StateTextBox.Text = OldState ?? Constants.DefaultStateName;
            // 
            // textBoxStateColor
            // 
            buttonPickStateColor.BackColor = Color;
            buttonPickStateColor.Location = new Point(244, 10);
            buttonPickStateColor.Margin = new Padding(3, 10, 3, 3);
            buttonPickStateColor.Name = "buttonStateColor";
            buttonPickStateColor.Size = new Size(90, 20);
            buttonPickStateColor.TabIndex = 2;
            buttonPickStateColor.Text = "Pick Color";
            buttonPickStateColor.Click += ButtonPickStateColorClick;
            
            // 
            // removesStateButton
            // 
            removesStateButton.Location = new Point(475, 10);
            removesStateButton.Margin = new Padding(38, 10, 3, 3);
            removesStateButton.Name = "removesStateButton";
            removesStateButton.Size = new Size(75, 20);
            removesStateButton.TabIndex = 3;
            removesStateButton.Text = "Remove";
            removesStateButton.UseVisualStyleBackColor = true;
            removesStateButton.Click += RemovesStateButton_Click;
        }

        private void RemovesStateButton_Click(object sender, EventArgs e)
        {
            ((TableLayoutAlternativeStates)Parent).StateRows.Remove(this);
            ((TableLayoutAlternativeStates)Parent).UpdateRows();
        } 

        private void ButtonPickStateColorClick(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color = buttonPickStateColor.BackColor = colorDialog.Color;
            }
        }
    }
}
