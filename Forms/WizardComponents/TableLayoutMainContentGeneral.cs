
using System.Windows.Forms;

namespace Rationally.Visio.Forms.WizardComponents
{
    public class TableLayoutMainContentGeneral : TableLayoutPanel
    {
        private readonly FlowLayoutPanel flowLayoutGeneralAuthor;
        private readonly FlowLayoutPanel flowLayoutGeneralTopic;
        private readonly FlowLayoutPanel flowLayoutGeneralDate;
        private readonly Label authorLabel;
        private readonly Label topicLabel;
        private readonly Label dateLabel;
        public DateTimePicker DateTimePickerCreationDate;
        public readonly TextBox TextDecisionTopic;
        public readonly TextBox TextAuthor;

        public TableLayoutMainContentGeneral()
        {
            flowLayoutGeneralTopic = new FlowLayoutPanel();
            topicLabel = new Label();
            TextDecisionTopic = new TextBox();

            flowLayoutGeneralAuthor = new FlowLayoutPanel();
            authorLabel = new Label();
            TextAuthor = new TextBox();

            flowLayoutGeneralDate = new FlowLayoutPanel();
            dateLabel = new Label();
            DateTimePickerCreationDate = new DateTimePicker();

            SuspendLayout();
            flowLayoutGeneralTopic.SuspendLayout();
            flowLayoutGeneralAuthor.SuspendLayout();
            flowLayoutGeneralDate.SuspendLayout();
            Init();
        }

        private void Init()
        {
            //
            // general information panel
            //
            BackColor = System.Drawing.SystemColors.Control;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Controls.Add(flowLayoutGeneralTopic, 0, 0);
            Controls.Add(flowLayoutGeneralAuthor, 0, 1);
            Controls.Add(flowLayoutGeneralDate, 0, 2);
            Dock = DockStyle.Fill;
            Location = new System.Drawing.Point(4, 4);
            Margin = new Padding(4);
            Name = "tableLayoutMainContentGeneral";
            RowCount = 4;
            RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            Size = new System.Drawing.Size(760, 482);
            TabIndex = 20;
            // 
            // flowLayoutGeneralAuthor
            // 
            flowLayoutGeneralAuthor.Controls.Add(authorLabel);
            flowLayoutGeneralAuthor.Controls.Add(TextAuthor);
            flowLayoutGeneralAuthor.Dock = DockStyle.Fill;
            flowLayoutGeneralAuthor.Location = new System.Drawing.Point(4, 52);
            flowLayoutGeneralAuthor.Margin = new Padding(4);
            flowLayoutGeneralAuthor.Name = "flowLayoutGeneralAuthor";
            flowLayoutGeneralAuthor.Size = new System.Drawing.Size(752, 40);
            flowLayoutGeneralAuthor.TabIndex = 22;
            // 
            // label2
            // 
            authorLabel.AutoSize = true;
            authorLabel.Dock = DockStyle.Left;
            authorLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            authorLabel.Location = new System.Drawing.Point(5, 6);
            authorLabel.Margin = new Padding(5, 6, 5, 6);
            authorLabel.MinimumSize = new System.Drawing.Size(100, 27);
            authorLabel.Name = "label2";
            authorLabel.Size = new System.Drawing.Size(100, 27);
            authorLabel.TabIndex = 15;
            authorLabel.Text = "Author";
            authorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textAuthor
            // 
            TextAuthor.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TextAuthor.Location = new System.Drawing.Point(215, 6);
            TextAuthor.Margin = new Padding(5, 6, 5, 6);
            TextAuthor.Name = "textAuthor";
            TextAuthor.Size = new System.Drawing.Size(600, 27);
            TextAuthor.TabIndex = 3;
            // 
            // flowLayoutGeneralTopic
            // 
            flowLayoutGeneralTopic.Controls.Add(topicLabel);
            flowLayoutGeneralTopic.Controls.Add(TextDecisionTopic);
            flowLayoutGeneralTopic.Dock = DockStyle.Fill;
            flowLayoutGeneralTopic.Location = new System.Drawing.Point(4, 4);
            flowLayoutGeneralTopic.Margin = new Padding(4);
            flowLayoutGeneralTopic.Name = "flowLayoutGeneralTopic";
            flowLayoutGeneralTopic.Size = new System.Drawing.Size(752, 40);
            flowLayoutGeneralTopic.TabIndex = 21;
            // 
            // label1
            // 
            topicLabel.AutoSize = true;
            topicLabel.Dock = DockStyle.Left;
            topicLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            topicLabel.Location = new System.Drawing.Point(5, 6);
            topicLabel.Margin = new Padding(5, 6, 5, 6);
            topicLabel.MinimumSize = new System.Drawing.Size(100, 27);
            topicLabel.Name = "label1";
            topicLabel.Size = new System.Drawing.Size(100, 27);
            topicLabel.TabIndex = 16;
            topicLabel.Text = "Topic";
            topicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textDecisionTopic
            // 
            TextDecisionTopic.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TextDecisionTopic.Location = new System.Drawing.Point(215, 6);
            TextDecisionTopic.Margin = new Padding(5, 6, 5, 6);
            TextDecisionTopic.Name = "textDecisionTopic";
            TextDecisionTopic.Size = new System.Drawing.Size(600, 27);
            TextDecisionTopic.TabIndex = 2;
            // 
            // flowLayoutGeneralDate
            // 
            flowLayoutGeneralDate.Controls.Add(dateLabel);
            flowLayoutGeneralDate.Controls.Add(DateTimePickerCreationDate);
            flowLayoutGeneralDate.Dock = DockStyle.Fill;
            flowLayoutGeneralDate.Location = new System.Drawing.Point(3, 99);
            flowLayoutGeneralDate.Name = "flowLayoutGeneralDate";
            flowLayoutGeneralDate.Size = new System.Drawing.Size(754, 42);
            flowLayoutGeneralDate.TabIndex = 23;
            // 
            // label3
            // 
            dateLabel.AutoSize = true;
            dateLabel.Dock = DockStyle.Left;
            dateLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dateLabel.Location = new System.Drawing.Point(5, 6);
            dateLabel.Margin = new Padding(5, 6, 5, 6);
            dateLabel.MinimumSize = new System.Drawing.Size(100, 27);
            dateLabel.Name = "label3";
            dateLabel.Size = new System.Drawing.Size(100, 27);
            dateLabel.TabIndex = 17;
            dateLabel.Text = "Date";
            dateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimePickerCreationDate
            // 
            DateTimePickerCreationDate.Location = new System.Drawing.Point(216, 6);
            DateTimePickerCreationDate.Margin = new Padding(6, 6, 5, 6);
            DateTimePickerCreationDate.Name = "dateTimePickerCreationDate";
            DateTimePickerCreationDate.Size = new System.Drawing.Size(600, 27);
            DateTimePickerCreationDate.TabIndex = 3;

            flowLayoutGeneralAuthor.ResumeLayout(false);
            flowLayoutGeneralAuthor.PerformLayout();
            flowLayoutGeneralTopic.ResumeLayout(false);
            flowLayoutGeneralTopic.PerformLayout();
            flowLayoutGeneralDate.ResumeLayout(false);
            flowLayoutGeneralDate.PerformLayout();
            
        }
    }
}
