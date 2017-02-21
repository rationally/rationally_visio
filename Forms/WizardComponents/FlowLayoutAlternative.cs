using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Windows.Forms;
using log4net;
using Rationally.Visio.EventHandlers.ClickEventHandlers;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.Forms.WizardComponents
{
    public sealed class FlowLayoutAlternative : GroupBox
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //private readonly int alternativeIndex;

        private AntiAliasedLabel alternativeIndexLabel;
        private AntiAliasedLabel alternativeTitleLabel;
        internal TextBox TextBoxAlternativeTitle;
        private AntiAliasedLabel alternativeStateLabel;
        private ComboBox alternativeStateDropdown;

        private AntiAliasedButton deleteAlternativeButton;

        public Alternative Alternative { get; private set; }
        public FlowLayoutAlternative()
        {
            Alternative = new Alternative("", ProjectSetupWizard.Instance.ModelCopy.AlternativeStateColors.Keys.First());
            ProjectSetupWizard.Instance.ModelCopy.Alternatives.Add(Alternative);
            Alternative.GenerateIdentifier(ProjectSetupWizard.Instance.ModelCopy.Alternatives.Count - 1);
            SuspendLayout();
            Init();
        }

        public FlowLayoutAlternative(Alternative alternative)
        {
            if (alternative == null)
            {
                throw new NullReferenceException("alternative is NULL; please use another constructor");
            }
            Alternative = alternative;

            SuspendLayout();
            Init();
        }

        public void UpdateData()
        {
            TextBoxAlternativeTitle.Text = Alternative.Title;
            alternativeStateDropdown.SelectedIndex = ProjectSetupWizard.Instance.ModelCopy.AlternativeStateColors.Keys.ToList().IndexOf(Alternative.Status);
        }

        public void UpdateModel()
        {
            Log.Debug("HasAlternative:" + (Alternative != null) + "|View has alternativescontainer:" + (Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is AlternativesContainer) != null));
            //should refer to model copy!
            Alternative.Status = alternativeStateDropdown.Text;
            Alternative.Title = TextBoxAlternativeTitle.Text;
        }

        private void RemoveAlternative(object sender, EventArgs e)
        {
            ProjectSetupWizard.Instance.TableLayoutMainContentAlternatives.Alternatives.Remove(this);
            ProjectSetupWizard.Instance.ModelCopy.Alternatives.Remove(Alternative);
            ProjectSetupWizard.Instance.TableLayoutMainContentAlternatives.UpdateRows();
        }

        public void Init()
        {
            // 
            // flowLayoutPanelAlternative1
            // 

            Dock = DockStyle.Top;
            Location = new Point(3, 3);
            Name = "flowLayoutPanelAlternative" + Alternative.Id;
            Size = new Size(714, 84);
            TabIndex = 0;

            alternativeIndexLabel = new AntiAliasedLabel();
            alternativeTitleLabel = new AntiAliasedLabel();
            TextBoxAlternativeTitle = new TextBox();
            alternativeStateLabel = new AntiAliasedLabel();
            alternativeStateDropdown = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, FormattingEnabled = true };
            deleteAlternativeButton = new AntiAliasedButton();

            Controls.Clear();
            Controls.Add(alternativeIndexLabel);
            Controls.Add(alternativeTitleLabel);
            Controls.Add(TextBoxAlternativeTitle);
            Controls.Add(alternativeStateLabel);
            Controls.Add(alternativeStateDropdown);
            Controls.Add(deleteAlternativeButton);
            // 
            // alternativeTitleLabel
            // 
            alternativeIndexLabel.AutoSize = true;
            alternativeIndexLabel.Location = new Point(3, 15);
            alternativeIndexLabel.Margin = new Padding(3, 10, 3, 0);
            alternativeIndexLabel.Name = "alternativeIndexLabel";
            alternativeIndexLabel.Size = new Size(10, 19);
            alternativeIndexLabel.TabIndex = 0;
            alternativeIndexLabel.Text = Alternative.IdentifierString;
            //
            // alternativeTitleLabel
            //
            alternativeTitleLabel.AutoSize = true;
            alternativeTitleLabel.Location = new Point(25, 17);
            alternativeTitleLabel.Margin = new Padding(3, 10, 3, 0);
            alternativeTitleLabel.Name = "alternativeTitleLabel";
            alternativeTitleLabel.Size = new Size(100, 19);
            alternativeTitleLabel.TabIndex = 1;
            alternativeTitleLabel.Text = "Title:";
            //
            // textBoxAlternativeTitle
            //
            TextBoxAlternativeTitle.Location = new Point(127, 15);
            TextBoxAlternativeTitle.Margin = new Padding(3, 6, 400, 3);
            TextBoxAlternativeTitle.Name = "textBoxAlternativeTitle";
            TextBoxAlternativeTitle.Size = new Size(350, 27);
            TextBoxAlternativeTitle.TabIndex = 2;
            //
            // alternativeStateLabel
            //
            alternativeStateLabel.AutoSize = true;
            alternativeStateLabel.Location = new Point(25, 52);
            alternativeStateLabel.Margin = new Padding(3, 10, 3, 0);
            alternativeStateLabel.Name = "alternativeStateLabel";
            alternativeStateLabel.Size = new Size(100, 19);
            alternativeStateLabel.TabIndex = 3;
            alternativeStateLabel.Text = "State:";
            //
            // alternativeStateDropdown
            //
            alternativeStateDropdown.FormattingEnabled = true;
            alternativeStateDropdown.Items.Clear();
            alternativeStateDropdown.Items.AddRange(ProjectSetupWizard.Instance.ModelCopy.AlternativeStateColors.Keys.ToArray());
            alternativeStateDropdown.SelectedIndex = 0;
            alternativeStateDropdown.Location = new Point(127, 50);
            alternativeStateDropdown.Margin = new Padding(3, 6, 3, 3);
            alternativeStateDropdown.Name = "alternativeStateDropdown";
            alternativeStateDropdown.Size = new Size(350, 27);
            alternativeStateDropdown.TabIndex = 4;
            //
            // deleteAlternativeButton
            //
            deleteAlternativeButton.Name = "deleteAlternativeButton";
            deleteAlternativeButton.UseVisualStyleBackColor = true;
            deleteAlternativeButton.Click += RemoveAlternative;
            deleteAlternativeButton.TabIndex = 5;
            deleteAlternativeButton.Location = new Point(600, 50);
            deleteAlternativeButton.Size = new Size(140, 27);
            deleteAlternativeButton.Margin = new Padding(3, 0, 3, 3);
            deleteAlternativeButton.Text = "Delete";
            ResumeLayout(false);
            PerformLayout();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            alternativeIndexLabel.Text = Alternative.IdentifierString;
            base.OnPaint(e);
        }
    }
}
