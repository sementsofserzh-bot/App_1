namespace App_WinForms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Label labelTitle;
        private Button buttonEmployee;
        private Button buttonUser;
        private Button buttonExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            labelTitle = new Label();
            buttonEmployee = new Button();
            buttonUser = new Button();
            buttonExit = new Button();
            SuspendLayout();

            // labelTitle
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            labelTitle.Location = new Point(115, 35);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(250, 37);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "PRIME TIME";

            // buttonEmployee
            buttonEmployee.Location = new Point(95, 100);
            buttonEmployee.Name = "buttonEmployee";
            buttonEmployee.Size = new Size(290, 45);
            buttonEmployee.TabIndex = 1;
            buttonEmployee.Text = "Сотрудник";
            buttonEmployee.UseVisualStyleBackColor = true;
            buttonEmployee.Click += buttonEmployee_Click;

            // buttonUser
            buttonUser.Location = new Point(95, 160);
            buttonUser.Name = "buttonUser";
            buttonUser.Size = new Size(290, 45);
            buttonUser.TabIndex = 2;
            buttonUser.Text = "Пользователь";
            buttonUser.UseVisualStyleBackColor = true;
            buttonUser.Click += buttonUser_Click;

            // buttonExit
            buttonExit.Location = new Point(95, 220);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(290, 45);
            buttonExit.TabIndex = 3;
            buttonExit.Text = "Выход";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += buttonExit_Click;

            // MainForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(480, 315);
            Controls.Add(buttonExit);
            Controls.Add(buttonUser);
            Controls.Add(buttonEmployee);
            Controls.Add(labelTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Prime Time";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
