namespace App_WinForms
{
    partial class UserForm
    {
        private System.ComponentModel.IContainer? components = null;
        private TabControl tabControl;
        private TabPage tabAddAthlete;
        private TabPage tabRegistration;
        private TabPage tabPersonal;
        private TabPage tabFilter;
        private TabPage tabRating;
        private Label labelFullName;
        private Label labelGender;
        private Label labelTrainingType;
        private Label labelAge;
        private Label labelHeight;
        private Label labelWeight;
        private TextBox textFullName;
        private ComboBox comboGender;
        private ComboBox comboTrainingType;
        private TextBox textAge;
        private TextBox textHeight;
        private TextBox textWeight;
        private Button buttonAddAthlete;
        private Label labelRegistrationAthlete;
        private Label labelRegistrationTrainer;
        private ComboBox comboRegistrationAthlete;
        private ComboBox comboRegistrationTrainer;
        private Button buttonRegistration;
        private Label labelPersonalAthlete;
        private ComboBox comboPersonalAthlete;
        private Button buttonPersonalTraining;
        private TextBox textPersonalResult;
        private Label labelFilterAthlete;
        private ComboBox comboFilterAthlete;
        private Button buttonFilter;
        private DataGridView dataGridFilter;
        private Button buttonSignUpFromFilter;
        private Button buttonRefreshRating;
        private DataGridView dataGridRating;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tabControl = new TabControl();
            tabAddAthlete = new TabPage();
            tabRegistration = new TabPage();
            tabPersonal = new TabPage();
            tabFilter = new TabPage();
            tabRating = new TabPage();
            labelFullName = new Label();
            labelGender = new Label();
            labelTrainingType = new Label();
            labelAge = new Label();
            labelHeight = new Label();
            labelWeight = new Label();
            textFullName = new TextBox();
            comboGender = new ComboBox();
            comboTrainingType = new ComboBox();
            textAge = new TextBox();
            textHeight = new TextBox();
            textWeight = new TextBox();
            buttonAddAthlete = new Button();
            labelRegistrationAthlete = new Label();
            labelRegistrationTrainer = new Label();
            comboRegistrationAthlete = new ComboBox();
            comboRegistrationTrainer = new ComboBox();
            buttonRegistration = new Button();
            labelPersonalAthlete = new Label();
            comboPersonalAthlete = new ComboBox();
            buttonPersonalTraining = new Button();
            textPersonalResult = new TextBox();
            labelFilterAthlete = new Label();
            comboFilterAthlete = new ComboBox();
            buttonFilter = new Button();
            buttonSignUpFromFilter = new Button();
            dataGridFilter = new DataGridView();
            buttonRefreshRating = new Button();
            dataGridRating = new DataGridView();
            tabControl.SuspendLayout();
            tabAddAthlete.SuspendLayout();
            tabRegistration.SuspendLayout();
            tabPersonal.SuspendLayout();
            tabFilter.SuspendLayout();
            tabRating.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridFilter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridRating).BeginInit();
            SuspendLayout();

            tabControl.Controls.Add(tabAddAthlete);
            tabControl.Controls.Add(tabRegistration);
            tabControl.Controls.Add(tabPersonal);
            tabControl.Controls.Add(tabFilter);
            tabControl.Controls.Add(tabRating);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(900, 560);

            tabAddAthlete.Controls.Add(labelFullName);
            tabAddAthlete.Controls.Add(labelGender);
            tabAddAthlete.Controls.Add(labelTrainingType);
            tabAddAthlete.Controls.Add(labelAge);
            tabAddAthlete.Controls.Add(labelHeight);
            tabAddAthlete.Controls.Add(labelWeight);
            tabAddAthlete.Controls.Add(textFullName);
            tabAddAthlete.Controls.Add(comboGender);
            tabAddAthlete.Controls.Add(comboTrainingType);
            tabAddAthlete.Controls.Add(textAge);
            tabAddAthlete.Controls.Add(textHeight);
            tabAddAthlete.Controls.Add(textWeight);
            tabAddAthlete.Controls.Add(buttonAddAthlete);
            tabAddAthlete.Location = new Point(4, 24);
            tabAddAthlete.Padding = new Padding(3);
            tabAddAthlete.Size = new Size(892, 532);
            tabAddAthlete.Text = "Добавить атлета";
            tabAddAthlete.UseVisualStyleBackColor = true;

            labelFullName.AutoSize = true;
            labelFullName.Location = new Point(25, 30);
            labelFullName.Text = "ФИО:";
            textFullName.Location = new Point(150, 27);
            textFullName.Size = new Size(300, 23);

            labelGender.AutoSize = true;
            labelGender.Location = new Point(25, 75);
            labelGender.Text = "Пол:";
            comboGender.DropDownStyle = ComboBoxStyle.DropDownList;
            comboGender.Location = new Point(150, 72);
            comboGender.Size = new Size(300, 23);

            labelTrainingType.AutoSize = true;
            labelTrainingType.Location = new Point(25, 120);
            labelTrainingType.Text = "Тип тренировки:";
            comboTrainingType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTrainingType.Location = new Point(150, 117);
            comboTrainingType.Size = new Size(300, 23);

            labelAge.AutoSize = true;
            labelAge.Location = new Point(25, 165);
            labelAge.Text = "Возраст:";
            textAge.Location = new Point(150, 162);
            textAge.Size = new Size(150, 23);

            labelHeight.AutoSize = true;
            labelHeight.Location = new Point(25, 210);
            labelHeight.Text = "Рост:";
            textHeight.Location = new Point(150, 207);
            textHeight.Size = new Size(150, 23);

            labelWeight.AutoSize = true;
            labelWeight.Location = new Point(25, 255);
            labelWeight.Text = "Вес:";
            textWeight.Location = new Point(150, 252);
            textWeight.Size = new Size(150, 23);

            buttonAddAthlete.Location = new Point(150, 300);
            buttonAddAthlete.Size = new Size(300, 40);
            buttonAddAthlete.Text = "Добавить атлета";
            buttonAddAthlete.Click += buttonAddAthlete_Click;

            tabRegistration.Controls.Add(labelRegistrationAthlete);
            tabRegistration.Controls.Add(labelRegistrationTrainer);
            tabRegistration.Controls.Add(comboRegistrationAthlete);
            tabRegistration.Controls.Add(comboRegistrationTrainer);
            tabRegistration.Controls.Add(buttonRegistration);
            tabRegistration.Location = new Point(4, 24);
            tabRegistration.Padding = new Padding(3);
            tabRegistration.Size = new Size(892, 532);
            tabRegistration.Text = "Регистрация";
            tabRegistration.UseVisualStyleBackColor = true;

            labelRegistrationAthlete.AutoSize = true;
            labelRegistrationAthlete.Location = new Point(30, 45);
            labelRegistrationAthlete.Text = "Атлет:";
            comboRegistrationAthlete.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRegistrationAthlete.Location = new Point(150, 42);
            comboRegistrationAthlete.Size = new Size(320, 23);

            labelRegistrationTrainer.AutoSize = true;
            labelRegistrationTrainer.Location = new Point(30, 90);
            labelRegistrationTrainer.Text = "Тренер:";
            comboRegistrationTrainer.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRegistrationTrainer.Location = new Point(150, 87);
            comboRegistrationTrainer.Size = new Size(320, 23);

            buttonRegistration.Location = new Point(150, 135);
            buttonRegistration.Size = new Size(320, 40);
            buttonRegistration.Text = "Зарегистрировать";
            buttonRegistration.Click += buttonRegistration_Click;

            tabPersonal.Controls.Add(labelPersonalAthlete);
            tabPersonal.Controls.Add(comboPersonalAthlete);
            tabPersonal.Controls.Add(buttonPersonalTraining);
            tabPersonal.Controls.Add(textPersonalResult);
            tabPersonal.Location = new Point(4, 24);
            tabPersonal.Padding = new Padding(3);
            tabPersonal.Size = new Size(892, 532);
            tabPersonal.Text = "Персональная тренировка";
            tabPersonal.UseVisualStyleBackColor = true;

            labelPersonalAthlete.AutoSize = true;
            labelPersonalAthlete.Location = new Point(30, 30);
            labelPersonalAthlete.Text = "Атлет:";
            comboPersonalAthlete.DropDownStyle = ComboBoxStyle.DropDownList;
            comboPersonalAthlete.Location = new Point(150, 27);
            comboPersonalAthlete.Size = new Size(320, 23);

            buttonPersonalTraining.Location = new Point(150, 65);
            buttonPersonalTraining.Size = new Size(320, 40);
            buttonPersonalTraining.Text = "Подобрать тренировку";
            buttonPersonalTraining.Click += buttonPersonalTraining_Click;

            textPersonalResult.Location = new Point(30, 120);
            textPersonalResult.Size = new Size(830, 380);
            textPersonalResult.Multiline = true;
            textPersonalResult.ReadOnly = true;
            textPersonalResult.ScrollBars = ScrollBars.Vertical;
            textPersonalResult.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            textPersonalResult.BackColor = Color.White;
            textPersonalResult.Text = "Результат подбора отобразится здесь...";

            tabFilter.Controls.Add(labelFilterAthlete);
            tabFilter.Controls.Add(comboFilterAthlete);
            tabFilter.Controls.Add(buttonFilter);
            tabFilter.Controls.Add(dataGridFilter);
            tabFilter.Controls.Add(buttonSignUpFromFilter);
            tabFilter.Location = new Point(4, 24);
            tabFilter.Padding = new Padding(3);
            tabFilter.Size = new Size(892, 532);
            tabFilter.Text = "Подбор тренера";
            tabFilter.UseVisualStyleBackColor = true;

            labelFilterAthlete.AutoSize = true;
            labelFilterAthlete.Location = new Point(20, 20);
            labelFilterAthlete.Text = "Атлет:";
            comboFilterAthlete.DropDownStyle = ComboBoxStyle.DropDownList;
            comboFilterAthlete.Location = new Point(90, 17);
            comboFilterAthlete.Size = new Size(300, 23);

            buttonFilter.Location = new Point(410, 15);
            buttonFilter.Size = new Size(200, 30);
            buttonFilter.Text = "Рассчитать совместимость";
            buttonFilter.Click += buttonFilter_Click;

            dataGridFilter.AllowUserToAddRows = false;
            dataGridFilter.AllowUserToDeleteRows = false;
            dataGridFilter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridFilter.Location = new Point(20, 60);
            dataGridFilter.MultiSelect = false;
            dataGridFilter.ReadOnly = true;
            dataGridFilter.RowHeadersVisible = false;
            dataGridFilter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridFilter.Size = new Size(850, 390);
            dataGridFilter.Columns.Add("Id", "ID");
            dataGridFilter.Columns.Add("FullName", "ФИО Тренера");
            dataGridFilter.Columns.Add("Gender", "Пол");
            dataGridFilter.Columns.Add("TrainingType", "Специализация");
            dataGridFilter.Columns.Add("Age", "Возраст");
            dataGridFilter.Columns.Add("Experience", "Стаж");
            dataGridFilter.Columns.Add("Athletes", "Атлетов");
            dataGridFilter.Columns.Add("Match", "Совместимость");

            buttonSignUpFromFilter.Location = new Point(20, 465);
            buttonSignUpFromFilter.Size = new Size(850, 45);
            buttonSignUpFromFilter.Text = "Записаться к выбранному тренеру";
            buttonSignUpFromFilter.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            buttonSignUpFromFilter.BackColor = Color.LightGreen;
            buttonSignUpFromFilter.Click += buttonSignUpFromFilter_Click;

            tabRating.Controls.Add(buttonRefreshRating);
            tabRating.Controls.Add(dataGridRating);
            tabRating.Location = new Point(4, 24);
            tabRating.Padding = new Padding(3);
            tabRating.Size = new Size(892, 532);
            tabRating.Text = "Рейтинг тренеров";
            tabRating.UseVisualStyleBackColor = true;

            buttonRefreshRating.Location = new Point(20, 15);
            buttonRefreshRating.Size = new Size(180, 30);
            buttonRefreshRating.Text = "Обновить";
            buttonRefreshRating.Click += buttonRefreshRating_Click;

            dataGridRating.AllowUserToAddRows = false;
            dataGridRating.AllowUserToDeleteRows = false;
            dataGridRating.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridRating.Location = new Point(20, 60);
            dataGridRating.MultiSelect = false;
            dataGridRating.ReadOnly = true;
            dataGridRating.RowHeadersVisible = false;
            dataGridRating.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridRating.Size = new Size(850, 450);
            dataGridRating.Columns.Add("Number", "№");
            dataGridRating.Columns.Add("Id", "ID");
            dataGridRating.Columns.Add("FullName", "ФИО");
            dataGridRating.Columns.Add("Gender", "Пол");
            dataGridRating.Columns.Add("Age", "Возраст");
            dataGridRating.Columns.Add("Experience", "Стаж");
            dataGridRating.Columns.Add("Athletes", "Атлетов");
            dataGridRating.Columns.Add("TrainingType", "Тип");

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 560);
            Controls.Add(tabControl);
            Name = "UserForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Prime Time - Пользователь";

            ((System.ComponentModel.ISupportInitialize)dataGridFilter).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridRating).EndInit();
            tabRating.ResumeLayout(false);
            tabFilter.ResumeLayout(false);
            tabFilter.PerformLayout();
            tabPersonal.ResumeLayout(false);
            tabPersonal.PerformLayout();
            tabRegistration.ResumeLayout(false);
            tabRegistration.PerformLayout();
            tabAddAthlete.ResumeLayout(false);
            tabAddAthlete.PerformLayout();
            tabControl.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}