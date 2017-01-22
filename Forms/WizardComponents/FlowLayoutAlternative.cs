using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Microsoft.VisualBasic.Logging;
using Rationally.Visio.EventHandlers.ClickEventHandlers;
using Rationally.Visio.Logger;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.Forms.WizardComponents
{
    public sealed class FlowLayoutAlternative : FlowLayoutPanel
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int alternativeIndex;

        private readonly AntiAliasedLabel alternativeIndexLabel;
        private readonly AntiAliasedLabel alternativeTitleLabel;
        internal readonly TextBox TextBoxAlternativeTitle;
        private readonly AntiAliasedLabel alternativeStateLabel;
        private readonly ComboBox alternativeStateDropdown;

        public Alternative Alternative { get; private set; }
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

            alternativeIndexLabel = new AntiAliasedLabel();
            alternativeTitleLabel = new AntiAliasedLabel();
            TextBoxAlternativeTitle = new TextBox();
            alternativeStateLabel = new AntiAliasedLabel();
            alternativeStateDropdown = new ComboBox {DropDownStyle = ComboBoxStyle.DropDownList, FormattingEnabled = true};
            //this.Acti += alternative_activated;
            SuspendLayout();
            Init();
        }

        public void UpdateData()
        {
            //connect to a model resource, if one is present for this row
            Alternative = Globals.RationallyAddIn.Model.Alternatives.Count >= alternativeIndex ? Globals.RationallyAddIn.Model.Alternatives[alternativeIndex - 1] : null;
            TextBoxAlternativeTitle.Text = Alternative != null ? Alternative.Title : "";
            alternativeStateDropdown.SelectedIndex = Alternative != null ? Globals.RationallyAddIn.Model.AlternativeStates.IndexOf(Alternative.Status) : 0;
        }

        public void UpdateModel()
        {
            TempFileLogger.Log("HasAlternative:" + (Alternative != null) + "|View has alternativescontainer:" + (Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is AlternativesContainer) != null));
            if (Alternative != null)
            {
                UpdateAlternativeHandler.Execute(alternativeIndex-1, TextBoxAlternativeTitle.Text, alternativeStateDropdown.SelectedItem.ToString());
            }
            else
            {
                if (!string.IsNullOrEmpty(TextBoxAlternativeTitle.Text))
                {
                    (Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is AlternativesContainer) as AlternativesContainer)?.AddAlternative(TextBoxAlternativeTitle.Text, alternativeStateDropdown.SelectedItem.ToString());
                }
            }
        }

        private void Init()
        {
            Controls.Add(alternativeIndexLabel);
            Controls.Add(alternativeTitleLabel);
            Controls.Add(TextBoxAlternativeTitle);
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
            TextBoxAlternativeTitle.Location = new System.Drawing.Point(61, 6);
            TextBoxAlternativeTitle.Margin = new Padding(3, 6, 3, 3);
            TextBoxAlternativeTitle.Name = "textBoxAlternativeTitle";
            TextBoxAlternativeTitle.Size = new System.Drawing.Size(300, 27);
            TextBoxAlternativeTitle.TabIndex = 2;
            
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
