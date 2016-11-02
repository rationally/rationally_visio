using System;
using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.EventHandlers;
using Rationally.Visio.EventHandlers.ClickEventHandlers;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class FlowLayoutAlternative : FlowLayoutPanel
    {
        public int AlternativeIndex;

        public Label AlternativeIndexLabel;
        public Label AlternativeTitleLabel;
        public TextBox TextBoxAlternativeTitle;
        public Label AlternativeStateLabel;
        public ComboBox AlternativeStateDropdown;

        public Alternative Alternative { get; set; }
        public FlowLayoutAlternative(int alternativeIndex)
        {
            
            AlternativeIndex = alternativeIndex;
            
            // 
            // flowLayoutPanelAlternative1
            // 
            
            Dock = DockStyle.Fill;
            Location = new System.Drawing.Point(3, 3);
            Name = "flowLayoutPanelAlternative"+AlternativeIndex;
            Size = new System.Drawing.Size(754, 42);
            TabIndex = 0;
            AlternativeIndexLabel = new Label();
            AlternativeTitleLabel = new Label();
            TextBoxAlternativeTitle = new TextBox();
            AlternativeStateLabel = new Label();
            AlternativeStateDropdown = new ComboBox {DropDownStyle = ComboBoxStyle.DropDownList, FormattingEnabled = true};
            //this.Acti += alternative_activated;
            SuspendLayout();
            Init();
        }

        public void UpdateData()
        {
            //connect to a model resource, if one is present for this row
            if (Globals.RationallyAddIn.Model.Alternatives.Count >= AlternativeIndex)
            {
                Alternative = Globals.RationallyAddIn.Model.Alternatives[AlternativeIndex - 1];//map to c-indexing
            }
            TextBoxAlternativeTitle.Text = Alternative != null ? Alternative.Title : "";
            AlternativeStateDropdown.SelectedIndex = Alternative != null ? Globals.RationallyAddIn.Model.AlternativeStates.IndexOf(Alternative.Status) : 0;
        }

        public void UpdateModel()
        {
            if (Alternative != null)
            {
                UpdateAlternativeHandler.Execute(Alternative, TextBoxAlternativeTitle.Text, AlternativeStateDropdown.SelectedText);
            }
            else
            {
                if (!String.IsNullOrEmpty(TextBoxAlternativeTitle.Text))
                {
                    Alternative alternative = new Alternative(TextBoxAlternativeTitle.Text,
                        AlternativeStateDropdown.SelectedText,
                        "", (char) (64 + AlternativeIndex) + ":", Alternative.HighestUniqueIdentifier == -1 ? 0 : Alternative.HighestUniqueIdentifier);
                    Globals.RationallyAddIn.View.Page = Globals.RationallyAddIn.Application.ActivePage;
                    Globals.RationallyAddIn.RebuildTree(Globals.RationallyAddIn.Application.ActiveDocument);
                    Globals.RationallyAddIn.View.AddAlternative(alternative);
                    RepaintHandler.Repaint(Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is AlternativesContainer));
                }
            }
        }

        private void Init()
        {
            Controls.Add(AlternativeIndexLabel);
            Controls.Add(AlternativeTitleLabel);
            Controls.Add(TextBoxAlternativeTitle);
            Controls.Add(AlternativeStateLabel);
            Controls.Add(AlternativeStateDropdown);
            // 
            // alternativeTitleLabel
            // 
            AlternativeIndexLabel.AutoSize = true;
            AlternativeIndexLabel.Location = new System.Drawing.Point(3, 10);
            AlternativeIndexLabel.Margin = new Padding(3, 10, 3, 0);
            AlternativeIndexLabel.Name = "alternativeIndexLabel";
            AlternativeIndexLabel.Size = new System.Drawing.Size(10, 19);
            AlternativeIndexLabel.TabIndex = 0;
            AlternativeIndexLabel.Text = AlternativeIndex+":";
            // 
            // alternativeTitleLabel
            // 
            AlternativeTitleLabel.AutoSize = true;
            AlternativeTitleLabel.Location = new System.Drawing.Point(13, 10);
            AlternativeTitleLabel.Margin = new Padding(3, 10, 3, 0);
            AlternativeTitleLabel.Name = "alternativeTitleLabel";
            AlternativeTitleLabel.Size = new System.Drawing.Size(42, 19);
            AlternativeTitleLabel.TabIndex = 1;
            AlternativeTitleLabel.Text = "Title:";
            // 
            // textBoxAlternativeTitle
            // 
            TextBoxAlternativeTitle.Location = new System.Drawing.Point(61, 6);
            TextBoxAlternativeTitle.Margin = new Padding(3, 6, 3, 3);
            TextBoxAlternativeTitle.Name = "textBoxAlternativeTitle";
            TextBoxAlternativeTitle.Size = new System.Drawing.Size(300, 27);
            TextBoxAlternativeTitle.TabIndex = 2;
            
            // 
            // alternativeStateLabel
            // 
            AlternativeStateLabel.AutoSize = true;
            AlternativeStateLabel.Location = new System.Drawing.Point(414, 10);
            AlternativeStateLabel.Margin = new Padding(50, 10, 3, 0);
            AlternativeStateLabel.Name = "alternativeStateLabel";
            AlternativeStateLabel.Size = new System.Drawing.Size(46, 19);
            AlternativeStateLabel.TabIndex = 3;
            AlternativeStateLabel.Text = "State:";
            // 
            // alternativeStateDropdown
            // 
            AlternativeStateDropdown.FormattingEnabled = true;
            AlternativeStateDropdown.Items.AddRange(Globals.RationallyAddIn.Model.AlternativeStates.ToArray());
            AlternativeStateDropdown.SelectedIndex = 0;
            AlternativeStateDropdown.Location = new System.Drawing.Point(466, 6);
            AlternativeStateDropdown.Margin = new Padding(3, 6, 3, 3);
            AlternativeStateDropdown.Name = "alternativeStateDropdown";
            AlternativeStateDropdown.Size = new System.Drawing.Size(200, 27);
            AlternativeStateDropdown.TabIndex = 4;


            ResumeLayout(false);
            PerformLayout();
        }
    }
}
