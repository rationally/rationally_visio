using System.Windows.Forms;
using Rationally.Visio.EventHandlers.ClickEventHandlers;
using Rationally.Visio.Model;

namespace Rationally.Visio.Forms.WizardComponents
{
    public sealed class FlowLayoutAlternative : FlowLayoutPanel
    {
        private readonly int alternativeIndex;

        private readonly Label alternativeIndexLabel;
        private readonly Label alternativeTitleLabel;
        private readonly TextBox textBoxAlternativeTitle;
        private readonly Label alternativeStateLabel;
        private readonly ComboBox alternativeStateDropdown;

        private Alternative Alternative { get; set; }
        public FlowLayoutAlternative(int alternativeIndex)
        {
            
            this.alternativeIndex = alternativeIndex;
            
            // 
            // flowLayoutPanelAlternative1
            // 
            
            Dock = DockStyle.Fill;
            Location = new System.Drawing.Point(3, 3);
            Name = "flowLayoutPanelAlternative"+this.alternativeIndex;
            Size = new System.Drawing.Size(754, 42);
            TabIndex = 0;
            alternativeIndexLabel = new Label();
            alternativeTitleLabel = new Label();
            textBoxAlternativeTitle = new TextBox();
            alternativeStateLabel = new Label();
            alternativeStateDropdown = new ComboBox {DropDownStyle = ComboBoxStyle.DropDownList, FormattingEnabled = true};
            //this.Acti += alternative_activated;
            SuspendLayout();
            Init();
        }

        public void UpdateData()
        {
            //connect to a model resource, if one is present for this row
            if (Globals.RationallyAddIn.Model.Alternatives.Count >= alternativeIndex)
            {
                Alternative = Globals.RationallyAddIn.Model.Alternatives[alternativeIndex - 1];//map to c-indexing
            }
            textBoxAlternativeTitle.Text = Alternative != null ? Alternative.Title : "";
            alternativeStateDropdown.SelectedIndex = Alternative != null ? Globals.RationallyAddIn.Model.AlternativeStates.IndexOf(Alternative.Status) : 0;
        }

        public void UpdateModel()
        {
            if (Alternative != null)
            {
                UpdateAlternativeHandler.Execute(AlternativeIndex-1, TextBoxAlternativeTitle.Text, AlternativeStateDropdown.SelectedItem.ToString());
            }
            else
            {
                if (!string.IsNullOrEmpty(textBoxAlternativeTitle.Text))
                {
                    Alternative newAlternative = new Alternative(TextBoxAlternativeTitle.Text, AlternativeStateDropdown.SelectedItem.ToString());
                    newAlternative.GenerateIdentifier(AlternativeIndex);
                    Globals.RationallyAddIn.View.Page = Globals.RationallyAddIn.Application.ActivePage;
                    Globals.RationallyAddIn.RebuildTree(Globals.RationallyAddIn.Application.ActiveDocument);
                    Globals.RationallyAddIn.Model.Alternatives.Add(newAlternative);
                    
                    
                    Globals.RationallyAddIn.View.AddAlternative(newAlternative);
                }
            }
        }

        private void Init()
        {
            Controls.Add(alternativeIndexLabel);
            Controls.Add(alternativeTitleLabel);
            Controls.Add(textBoxAlternativeTitle);
            Controls.Add(alternativeStateLabel);
            Controls.Add(alternativeStateDropdown);
            // 
            // alternativeTitleLabel
            // 
            alternativeIndexLabel.AutoSize = true;
            alternativeIndexLabel.Location = new System.Drawing.Point(3, 10);
            alternativeIndexLabel.Margin = new Padding(3, 10, 3, 0);
            alternativeIndexLabel.Name = "alternativeIndexLabel";
            alternativeIndexLabel.Size = new System.Drawing.Size(10, 19);
            alternativeIndexLabel.TabIndex = 0;
            alternativeIndexLabel.Text = alternativeIndex+":";
            // 
            // alternativeTitleLabel
            // 
            alternativeTitleLabel.AutoSize = true;
            alternativeTitleLabel.Location = new System.Drawing.Point(13, 10);
            alternativeTitleLabel.Margin = new Padding(3, 10, 3, 0);
            alternativeTitleLabel.Name = "alternativeTitleLabel";
            alternativeTitleLabel.Size = new System.Drawing.Size(42, 19);
            alternativeTitleLabel.TabIndex = 1;
            alternativeTitleLabel.Text = "Title:";
            // 
            // textBoxAlternativeTitle
            // 
            textBoxAlternativeTitle.Location = new System.Drawing.Point(61, 6);
            textBoxAlternativeTitle.Margin = new Padding(3, 6, 3, 3);
            textBoxAlternativeTitle.Name = "textBoxAlternativeTitle";
            textBoxAlternativeTitle.Size = new System.Drawing.Size(300, 27);
            textBoxAlternativeTitle.TabIndex = 2;
            
            // 
            // alternativeStateLabel
            // 
            alternativeStateLabel.AutoSize = true;
            alternativeStateLabel.Location = new System.Drawing.Point(414, 10);
            alternativeStateLabel.Margin = new Padding(50, 10, 3, 0);
            alternativeStateLabel.Name = "alternativeStateLabel";
            alternativeStateLabel.Size = new System.Drawing.Size(46, 19);
            alternativeStateLabel.TabIndex = 3;
            alternativeStateLabel.Text = "State:";
            // 
            // alternativeStateDropdown
            // 
            alternativeStateDropdown.FormattingEnabled = true;
            alternativeStateDropdown.Items.AddRange(Globals.RationallyAddIn.Model.AlternativeStates.ToArray());
            alternativeStateDropdown.SelectedIndex = 0;
            alternativeStateDropdown.Location = new System.Drawing.Point(466, 6);
            alternativeStateDropdown.Margin = new Padding(3, 6, 3, 3);
            alternativeStateDropdown.Name = "alternativeStateDropdown";
            alternativeStateDropdown.Size = new System.Drawing.Size(200, 27);
            alternativeStateDropdown.TabIndex = 4;


            ResumeLayout(false);
            PerformLayout();
        }
    }
}
