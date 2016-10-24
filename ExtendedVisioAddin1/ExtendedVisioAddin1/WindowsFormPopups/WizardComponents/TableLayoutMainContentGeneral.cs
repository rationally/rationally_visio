
using System.Windows.Forms;

namespace Rationally.Visio.WindowsFormPopups.WizardComponents
{
    public class TableLayoutMainContentGeneral : TableLayoutPanel
    {
        public FlowLayoutPanel FlowLayoutGeneralAuthor;
        public FlowLayoutPanel FlowLayoutGeneralTopic;
        public Label AuthorLabel;
        public TextBox TextAuthor;
        public Label TopicLabel;
        public TextBox TextDecisionTopic;
        public FlowLayoutPanel FlowLayoutGeneralDate;
        public Label DateLabel;
        public DateTimePicker DateTimePickerCreationDate;

        public TableLayoutMainContentGeneral()
        {
            FlowLayoutGeneralTopic = new FlowLayoutPanel();
            TopicLabel = new Label();
            TextDecisionTopic = new TextBox();

            FlowLayoutGeneralAuthor = new FlowLayoutPanel();
            AuthorLabel = new Label();
            TextAuthor = new TextBox();

            FlowLayoutGeneralDate = new FlowLayoutPanel();
            DateLabel = new Label();
            DateTimePickerCreationDate = new DateTimePicker();

            SuspendLayout();
            FlowLayoutGeneralTopic.SuspendLayout();
            FlowLayoutGeneralAuthor.SuspendLayout();
            FlowLayoutGeneralDate.SuspendLayout();
            Init();
        }

        public void Init()
        {
            //
            // general information panel
            //
            BackColor = System.Drawing.SystemColors.Control;
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Controls.Add(FlowLayoutGeneralTopic, 0, 0);
            Controls.Add(FlowLayoutGeneralAuthor, 0, 1);
            Controls.Add(FlowLayoutGeneralDate, 0, 2);
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
            TabIndex = 0;
            // 
            // flowLayoutGeneralAuthor
            // 
            FlowLayoutGeneralAuthor.Controls.Add(AuthorLabel);
            FlowLayoutGeneralAuthor.Controls.Add(TextAuthor);
            FlowLayoutGeneralAuthor.Dock = DockStyle.Fill;
            FlowLayoutGeneralAuthor.Location = new System.Drawing.Point(4, 52);
            FlowLayoutGeneralAuthor.Margin = new Padding(4);
            FlowLayoutGeneralAuthor.Name = "flowLayoutGeneralAuthor";
            FlowLayoutGeneralAuthor.Size = new System.Drawing.Size(752, 40);
            FlowLayoutGeneralAuthor.TabIndex = 21;
            // 
            // label2
            // 
            AuthorLabel.AutoSize = true;
            AuthorLabel.Dock = DockStyle.Left;
            AuthorLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            AuthorLabel.Location = new System.Drawing.Point(5, 6);
            AuthorLabel.Margin = new Padding(5, 6, 5, 6);
            AuthorLabel.MinimumSize = new System.Drawing.Size(200, 27);
            AuthorLabel.Name = "label2";
            AuthorLabel.Size = new System.Drawing.Size(200, 27);
            AuthorLabel.TabIndex = 15;
            AuthorLabel.Text = "Author";
            AuthorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textAuthor
            // 
            TextAuthor.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TextAuthor.Location = new System.Drawing.Point(215, 6);
            TextAuthor.Margin = new Padding(5, 6, 5, 6);
            TextAuthor.Name = "textAuthor";
            TextAuthor.Size = new System.Drawing.Size(500, 27);
            TextAuthor.TabIndex = 13;
            // 
            // flowLayoutGeneralTopic
            // 
            FlowLayoutGeneralTopic.Controls.Add(TopicLabel);
            FlowLayoutGeneralTopic.Controls.Add(TextDecisionTopic);
            FlowLayoutGeneralTopic.Dock = DockStyle.Fill;
            FlowLayoutGeneralTopic.Location = new System.Drawing.Point(4, 4);
            FlowLayoutGeneralTopic.Margin = new Padding(4);
            FlowLayoutGeneralTopic.Name = "flowLayoutGeneralTopic";
            FlowLayoutGeneralTopic.Size = new System.Drawing.Size(752, 40);
            FlowLayoutGeneralTopic.TabIndex = 22;
            // 
            // label1
            // 
            TopicLabel.AutoSize = true;
            TopicLabel.Dock = DockStyle.Left;
            TopicLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TopicLabel.Location = new System.Drawing.Point(5, 6);
            TopicLabel.Margin = new Padding(5, 6, 5, 6);
            TopicLabel.MinimumSize = new System.Drawing.Size(200, 27);
            TopicLabel.Name = "label1";
            TopicLabel.Size = new System.Drawing.Size(200, 27);
            TopicLabel.TabIndex = 16;
            TopicLabel.Text = "Topic";
            TopicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textDecisionTopic
            // 
            TextDecisionTopic.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TextDecisionTopic.Location = new System.Drawing.Point(215, 6);
            TextDecisionTopic.Margin = new Padding(5, 6, 5, 6);
            TextDecisionTopic.Name = "textDecisionTopic";
            TextDecisionTopic.Size = new System.Drawing.Size(500, 27);
            TextDecisionTopic.TabIndex = 17;
            // 
            // flowLayoutGeneralDate
            // 
            FlowLayoutGeneralDate.Controls.Add(DateLabel);
            FlowLayoutGeneralDate.Controls.Add(DateTimePickerCreationDate);
            FlowLayoutGeneralDate.Dock = DockStyle.Fill;
            FlowLayoutGeneralDate.Location = new System.Drawing.Point(3, 99);
            FlowLayoutGeneralDate.Name = "flowLayoutGeneralDate";
            FlowLayoutGeneralDate.Size = new System.Drawing.Size(754, 42);
            FlowLayoutGeneralDate.TabIndex = 23;
            // 
            // label3
            // 
            DateLabel.AutoSize = true;
            DateLabel.Dock = DockStyle.Left;
            DateLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            DateLabel.Location = new System.Drawing.Point(5, 6);
            DateLabel.Margin = new Padding(5, 6, 5, 6);
            DateLabel.MinimumSize = new System.Drawing.Size(200, 27);
            DateLabel.Name = "label3";
            DateLabel.Size = new System.Drawing.Size(200, 27);
            DateLabel.TabIndex = 17;
            DateLabel.Text = "Date";
            DateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateTimePickerCreationDate
            // 
            DateTimePickerCreationDate.Location = new System.Drawing.Point(216, 6);
            DateTimePickerCreationDate.Margin = new Padding(6, 6, 5, 6);
            DateTimePickerCreationDate.Name = "dateTimePickerCreationDate";
            DateTimePickerCreationDate.Size = new System.Drawing.Size(500, 27);
            DateTimePickerCreationDate.TabIndex = 18;

            FlowLayoutGeneralAuthor.ResumeLayout(false);
            FlowLayoutGeneralAuthor.PerformLayout();
            FlowLayoutGeneralTopic.ResumeLayout(false);
            FlowLayoutGeneralTopic.PerformLayout();
            FlowLayoutGeneralDate.ResumeLayout(false);
            FlowLayoutGeneralDate.PerformLayout();
            
        }
    }
}
