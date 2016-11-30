
using System.Drawing;
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentGeneral : TableLayoutPanel
    {
        private readonly FlowLayoutPanel flowLayoutGeneralAuthor;
        private readonly FlowLayoutPanel flowLayoutGeneralTopic;
        private readonly FlowLayoutPanel flowLayoutGeneralDate;
        private readonly FlowLayoutPanel flowLayoutGeneralVersion;
        private readonly AntiAliasedLabel authorLabel;
        private readonly AntiAliasedLabel topicLabel;
        private readonly AntiAliasedLabel dateLabel;
        private readonly AntiAliasedLabel versionLabel;
        public readonly DateTimePicker DateTimePickerCreationDate;
        public readonly TextBox TextDecisionTopic;
        public readonly TextBox TextAuthor;
        public readonly TextBox TextVersion;

        public TableLayoutMainContentGeneral()
        {
            flowLayoutGeneralTopic = new FlowLayoutPanel();
            topicLabel = new AntiAliasedLabel();
            TextDecisionTopic = new TextBox();

            flowLayoutGeneralAuthor = new FlowLayoutPanel();
            authorLabel = new AntiAliasedLabel();
            TextAuthor = new TextBox();

            flowLayoutGeneralDate = new FlowLayoutPanel();
            dateLabel = new AntiAliasedLabel();
            DateTimePickerCreationDate = new DateTimePicker();

            flowLayoutGeneralVersion = new FlowLayoutPanel();
            versionLabel = new AntiAliasedLabel();
            TextVersion = new TextBox();

            SuspendLayout();
            flowLayoutGeneralTopic.SuspendLayout();
            flowLayoutGeneralAuthor.SuspendLayout();
            flowLayoutGeneralDate.SuspendLayout();
            flowLayoutGeneralVersion.SuspendLayout();
            Init();
        }

        private void Init()
        {
            //
            // general information panel
            //
            BackColor = Color.WhiteSmoke;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Controls.Add(flowLayoutGeneralTopic, 0, 0);
            Controls.Add(flowLayoutGeneralAuthor, 0, 1);
            Controls.Add(flowLayoutGeneralDate, 0, 2);
            Controls.Add(flowLayoutGeneralVersion, 0, 3);
            Dock = DockStyle.Fill;
            Location = new Point(4, 4);
            Margin = new Padding(4);
            Name = "tableLayoutMainContentGeneral";
            RowCount = 4;
            RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            Size = new Size(760, 482);
            TabIndex = 20;
            // 
            // flowLayoutGeneralAuthor
            // 
            flowLayoutGeneralAuthor.Controls.Add(authorLabel);
            flowLayoutGeneralAuthor.Controls.Add(TextAuthor);
            flowLayoutGeneralAuthor.Dock = DockStyle.Fill;
            flowLayoutGeneralAuthor.Location = new Point(4, 52);
            flowLayoutGeneralAuthor.Margin = new Padding(4);
            flowLayoutGeneralAuthor.Name = "flowLayoutGeneralAuthor";
            flowLayoutGeneralAuthor.Size = new Size(752, 40);
            flowLayoutGeneralAuthor.TabIndex = 22;
            // 
            // label2
            // 
            authorLabel.AutoSize = true;
            authorLabel.Dock = DockStyle.Left;
            authorLabel.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            authorLabel.Location = new Point(5, 6);
            authorLabel.Margin = new Padding(5, 6, 5, 6);
            authorLabel.MinimumSize = new Size(100, 27);
            authorLabel.Name = "label2";
            authorLabel.Size = new Size(100, 27);
            authorLabel.TabIndex = 15;
            authorLabel.Text = "Author";
            authorLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textAuthor
            // 
            TextAuthor.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TextAuthor.Location = new Point(215, 6);
            TextAuthor.Margin = new Padding(5, 6, 5, 6);
            TextAuthor.Name = "textAuthor";
            TextAuthor.Size = new Size(600, 27);
            TextAuthor.TabIndex = 3;
            // 
            // flowLayoutGeneralTopic
            // 
            flowLayoutGeneralTopic.Controls.Add(topicLabel);
            flowLayoutGeneralTopic.Controls.Add(TextDecisionTopic);
            flowLayoutGeneralTopic.Dock = DockStyle.Fill;
            flowLayoutGeneralTopic.Location = new Point(4, 4);
            flowLayoutGeneralTopic.Margin = new Padding(4);
            flowLayoutGeneralTopic.Name = "flowLayoutGeneralTopic";
            flowLayoutGeneralTopic.Size = new Size(752, 40);
            flowLayoutGeneralTopic.TabIndex = 21;
            // 
            // label1
            // 
            topicLabel.AutoSize = true;
            topicLabel.Dock = DockStyle.Left;
            topicLabel.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            topicLabel.Location = new Point(5, 6);
            topicLabel.Margin = new Padding(5, 6, 5, 6);
            topicLabel.MinimumSize = new Size(100, 27);
            topicLabel.Name = "label1";
            topicLabel.Size = new Size(100, 27);
            topicLabel.TabIndex = 16;
            topicLabel.Text = "Topic";
            topicLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textDecisionTopic
            // 
            TextDecisionTopic.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TextDecisionTopic.Location = new Point(215, 6);
            TextDecisionTopic.Margin = new Padding(5, 6, 5, 6);
            TextDecisionTopic.Name = "textDecisionTopic";
            TextDecisionTopic.Size = new Size(600, 27);
            TextDecisionTopic.TabIndex = 2;
            // 
            // flowLayoutGeneralDate
            // 
            flowLayoutGeneralDate.Controls.Add(dateLabel);
            flowLayoutGeneralDate.Controls.Add(DateTimePickerCreationDate);
            flowLayoutGeneralDate.Dock = DockStyle.Fill;
            flowLayoutGeneralDate.Location = new Point(3, 99);
            flowLayoutGeneralDate.Name = "flowLayoutGeneralDate";
            flowLayoutGeneralDate.Size = new Size(754, 42);
            flowLayoutGeneralDate.TabIndex = 23;
            // 
            // label3
            // 
            dateLabel.AutoSize = true;
            dateLabel.Dock = DockStyle.Left;
            dateLabel.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateLabel.Location = new Point(5, 6);
            dateLabel.Margin = new Padding(5, 6, 5, 6);
            dateLabel.MinimumSize = new Size(100, 27);
            dateLabel.Name = "label3";
            dateLabel.Size = new Size(100, 27);
            dateLabel.TabIndex = 17;
            dateLabel.Text = "Date";
            dateLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dateTimePickerCreationDate
            // 
            DateTimePickerCreationDate.Location = new Point(216, 6);
            DateTimePickerCreationDate.Margin = new Padding(6, 6, 5, 6);
            DateTimePickerCreationDate.Name = "dateTimePickerCreationDate";
            DateTimePickerCreationDate.Size = new Size(600, 27);
            DateTimePickerCreationDate.TabIndex = 3;

            // 
            // flowLayoutGeneralVersion
            // 
            flowLayoutGeneralVersion.Controls.Add(versionLabel);
            flowLayoutGeneralVersion.Controls.Add(TextVersion);
            flowLayoutGeneralVersion.Dock = DockStyle.Fill;
            flowLayoutGeneralVersion.Location = new Point(4, 149);
            flowLayoutGeneralVersion.Margin = new Padding(4);
            flowLayoutGeneralVersion.Name = "flowLayoutGeneralVersion";
            flowLayoutGeneralVersion.Size = new Size(752, 40);
            flowLayoutGeneralVersion.TabIndex = 25;
            // 
            // versionlabel
            // 
            versionLabel.AutoSize = true;
            versionLabel.Dock = DockStyle.Left;
            versionLabel.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            versionLabel.Location = new Point(5, 6);
            versionLabel.Margin = new Padding(5, 6, 5, 6);
            versionLabel.MinimumSize = new Size(100, 27);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(100, 27);
            versionLabel.TabIndex = 16;
            versionLabel.Text = "Version";
            versionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textversion
            // 
            TextVersion.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TextVersion.Location = new Point(215, 6);
            TextVersion.Margin = new Padding(5, 6, 5, 6);
            TextVersion.Name = "textDecisionVersion";
            TextVersion.Size = new Size(600, 27);
            TextVersion.TabIndex = 2;


            flowLayoutGeneralAuthor.ResumeLayout(false);
            flowLayoutGeneralAuthor.PerformLayout();
            flowLayoutGeneralTopic.ResumeLayout(false);
            flowLayoutGeneralTopic.PerformLayout();
            flowLayoutGeneralDate.ResumeLayout(false);
            flowLayoutGeneralDate.PerformLayout();
            flowLayoutGeneralVersion.ResumeLayout(false);
            flowLayoutGeneralVersion.PerformLayout();
        }
    }
}
