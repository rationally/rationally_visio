using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rationally.Visio.Model;

namespace Rationally.Visio.Forms.AlternativeStateConfiguration
{
    class FlowLayoutAlternativeState : FlowLayoutPanel
    {
        private System.Windows.Forms.Label labelStateTitle;
        public System.Windows.Forms.TextBox StateTextBox;
        private System.Windows.Forms.Button buttonPickStateColor;
        private System.Windows.Forms.Button removesStateButton;

        public string State { get; set; }
        public Color Color { get; set; }
        public int Index { get; set; }

        public FlowLayoutAlternativeState(string state, Color color, int index)
        {
            State = state;
            Color = color;
            Index = index;
            Init();
        }

        public FlowLayoutAlternativeState(AlternativeState state, int index)
        {
            State = state.State;
            Color = state.Color;
            Index = index;
            Init();
        }

        private void Init()
        {
            this.labelStateTitle = new System.Windows.Forms.Label();
            this.StateTextBox = new System.Windows.Forms.TextBox();
            this.buttonPickStateColor = new System.Windows.Forms.Button();
            this.removesStateButton = new System.Windows.Forms.Button();

            // 
            // flowLayoutPanel1
            // 
            Controls.Add(this.labelStateTitle);
            Controls.Add(this.StateTextBox);
            Controls.Add(this.buttonPickStateColor);
            Controls.Add(this.removesStateButton);
            Dock = System.Windows.Forms.DockStyle.Top;
            Location = new System.Drawing.Point(3, 3);
            Name = "flowLayoutPanel1";
            Size = new System.Drawing.Size(483, 40);
            TabIndex = 0;
            // 
            // labelStateTitle
            // 
            this.labelStateTitle.AllowDrop = true;
            this.labelStateTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelStateTitle.Location = new System.Drawing.Point(3, 14);
            this.labelStateTitle.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.labelStateTitle.Name = "labelStateTitle";
            this.labelStateTitle.Size = new System.Drawing.Size(35, 14);
            this.labelStateTitle.TabIndex = 0;
            this.labelStateTitle.Text = "State:";
            // 
            // textBox1
            // 
            this.StateTextBox.Location = new System.Drawing.Point(44, 10);
            this.StateTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.StateTextBox.Name = "textBox1";
            this.StateTextBox.Size = new System.Drawing.Size(194, 20);
            this.StateTextBox.TabIndex = 1;
            this.StateTextBox.Text = State;
            // 
            // textBoxStateColor
            // 
            this.buttonPickStateColor.BackColor = Color;
            this.buttonPickStateColor.Location = new System.Drawing.Point(244, 10);
            this.buttonPickStateColor.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.buttonPickStateColor.Name = "buttonStateColor";
            this.buttonPickStateColor.Size = new System.Drawing.Size(90, 20);
            this.buttonPickStateColor.TabIndex = 2;
            this.buttonPickStateColor.Text = "Pick Color";
            this.buttonPickStateColor.Click += ButtonPickStateColorClick;
            
            // 
            // removesStateButton
            // 
            this.removesStateButton.Location = new System.Drawing.Point(405, 10);
            this.removesStateButton.Margin = new System.Windows.Forms.Padding(68, 10, 3, 3);
            this.removesStateButton.Name = "removesStateButton";
            this.removesStateButton.Size = new System.Drawing.Size(75, 20);
            this.removesStateButton.TabIndex = 3;
            this.removesStateButton.Text = "Remove";
            this.removesStateButton.UseVisualStyleBackColor = true;
            this.removesStateButton.Click += RemovesStateButton_Click;
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
