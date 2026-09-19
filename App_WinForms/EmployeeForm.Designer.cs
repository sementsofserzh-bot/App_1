namespace App_WinForms
{
    partial class EmployeeForm
    {
        private System.ComponentModel.IContainer? components = null;
        private TabControl tabControl;
        private TabPage tabTrainers;
        private TabPage tabAthletes;
        private DataGridView dataGridTrainers;
        private DataGridView dataGridAthletes;
        private Label labelTrainerName;
        private Label labelTrainerGender;
        private Label labelTrainerType;
        private Label labelTrainerAge;
        private Label labelTrainerExperience;
        private TextBox textTrainerName;
        private ComboBox comboTrainerGender;
        private ComboBox comboTrainerType;
        private TextBox textTrainerAge;
        private TextBox textTrainerExperience;
        private Button buttonAddTrainer;
        private Button buttonUpdateTrainer;
        private Button buttonDeleteTrainer;
        private Label labelAthleteName;
        private Label labelAthleteGender;
        private Label labelAthleteType;
        private Label labelAthleteAge;
        private Label labelAthleteHeight;
        private Label labelAthleteWeight;
        private Label labelAthleteTrainer;
        private TextBox textAthleteName;
        private ComboBox comboAthleteGender;
        private ComboBox comboAthleteType;
        private TextBox textAthleteAge;
        private TextBox textAthleteHeight;
        private TextBox textAthleteWeight;
        private ComboBox comboAthleteTrainer;
        private Button buttonAddAthlete;
        private Button buttonUpdateAthlete;
        private Button buttonDeleteAthlete;
        private Label labelAttachTrainer;
        private Label labelAttachAthlete;
        private ComboBox comboAttachTrainer;
        private ComboBox comboAthleteForAttach;
        private Button buttonAttach;
        private Button buttonDetach;

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
            tabTrainers = new TabPage();
            dataGridTrainers = new DataGridView();
            labelTrainerName = new Label();
            labelTrainerGender = new Label();
            labelTrainerType = new Label();
            labelTrainerAge = new Label();
            labelTrainerExperience = new Label();
            textTrainerName = new TextBox();
            comboTrainerGender = new ComboBox();
            comboTrainerType = new ComboBox();
            textTrainerAge = new TextBox();
            textTrainerExperience = new TextBox();
            buttonAddTrainer = new Button();
            buttonUpdateTrainer = new Button();
            buttonDeleteTrainer = new Button();
            labelAttachTrainer = new Label();
            labelAttachAthlete = new Label();
            comboAttachTrainer = new ComboBox();
            comboAthleteForAttach = new ComboBox();
            buttonAttach = new Button();
            buttonDetach = new Button();
            tabAthletes = new TabPage();
            dataGridAthletes = new DataGridView();
            labelAthleteName = new Label();
            labelAthleteGender = new Label();
            labelAthleteType = new Label();
            labelAthleteAge = new Label();
            labelAthleteHeight = new Label();
            labelAthleteWeight = new Label();
            labelAthleteTrainer = new Label();
            textAthleteName = new TextBox();
            comboAthleteGender = new ComboBox();
            comboAthleteType = new ComboBox();
            textAthleteAge = new TextBox();
            textAthleteHeight = new TextBox();
            textAthleteWeight = new TextBox();
            comboAthleteTrainer = new ComboBox();
            buttonAddAthlete = new Button();
            buttonUpdateAthlete = new Button();
            buttonDeleteAthlete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridTrainers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridAthletes).BeginInit();
            tabControl.SuspendLayout();
            tabTrainers.SuspendLayout();
            tabAthletes.SuspendLayout();
            SuspendLayout();

            // tabControl
            tabControl.Controls.Add(tabTrainers);
            tabControl.Controls.Add(tabAthletes);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1100, 680);
            tabControl.TabIndex = 0;

            // tabTrainers
            tabTrainers.Controls.Add(dataGridTrainers);
            tabTrainers.Controls.Add(labelTrainerName);
            tabTrainers.Controls.Add(labelTrainerGender);
            tabTrainers.Controls.Add(labelTrainerType);
            tabTrainers.Controls.Add(labelTrainerAge);
            tabTrainers.Controls.Add(labelTrainerExperience);
            tabTrainers.Controls.Add(textTrainerName);
            tabTrainers.Controls.Add(comboTrainerGender);
            tabTrainers.Controls.Add(comboTrainerType);
            tabTrainers.Controls.Add(textTrainerAge);
            tabTrainers.Controls.Add(textTrainerExperience);
            tabTrainers.Controls.Add(buttonAddTrainer);
            tabTrainers.Controls.Add(buttonUpdateTrainer);
            tabTrainers.Controls.Add(buttonDeleteTrainer);
            tabTrainers.Controls.Add(labelAttachTrainer);
            tabTrainers.Controls.Add(labelAttachAthlete);
            tabTrainers.Controls.Add(comboAttachTrainer);
            tabTrainers.Controls.Add(comboAthleteForAttach);
            tabTrainers.Controls.Add(buttonAttach);
            tabTrainers.Controls.Add(buttonDetach);
            tabTrainers.Location = new Point(4, 24);
            tabTrainers.Name = "tabTrainers";
            tabTrainers.Padding = new Padding(3);
            tabTrainers.Size = new Size(1092, 652);
            tabTrainers.TabIndex = 0;
            tabTrainers.Text = "Тренеры";
            tabTrainers.UseVisualStyleBackColor = true;

            // trainer labels/fields
            labelTrainerName.AutoSize = true;
            labelTrainerName.Location = new Point(12, 15);
            labelTrainerName.Text = "ФИО:";
            textTrainerName.Location = new Point(120, 12);
            textTrainerName.Size = new Size(220, 23);

            labelTrainerGender.AutoSize = true;
            labelTrainerGender.Location = new Point(12, 48);
            labelTrainerGender.Text = "Пол:";
            comboTrainerGender.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTrainerGender.Location = new Point(120, 45);
            comboTrainerGender.Size = new Size(220, 23);

            labelTrainerType.AutoSize = true;
            labelTrainerType.Location = new Point(12, 81);
            labelTrainerType.Text = "Тип тренировки:";
            comboTrainerType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTrainerType.Location = new Point(120, 78);
            comboTrainerType.Size = new Size(220, 23);

            labelTrainerAge.AutoSize = true;
            labelTrainerAge.Location = new Point(380, 15);
            labelTrainerAge.Text = "Возраст:";
            textTrainerAge.Location = new Point(470, 12);
            textTrainerAge.Size = new Size(100, 23);

            labelTrainerExperience.AutoSize = true;
            labelTrainerExperience.Location = new Point(380, 48);
            labelTrainerExperience.Text = "Стаж:";
            textTrainerExperience.Location = new Point(470, 45);
            textTrainerExperience.Size = new Size(100, 23);

            buttonAddTrainer.Location = new Point(610, 12);
            buttonAddTrainer.Size = new Size(130, 30);
            buttonAddTrainer.Text = "Добавить";
            buttonAddTrainer.Click += buttonAddTrainer_Click;

            buttonUpdateTrainer.Location = new Point(750, 12);
            buttonUpdateTrainer.Size = new Size(130, 30);
            buttonUpdateTrainer.Text = "Изменить";
            buttonUpdateTrainer.Click += buttonUpdateTrainer_Click;

            buttonDeleteTrainer.Location = new Point(890, 12);
            buttonDeleteTrainer.Size = new Size(130, 30);
            buttonDeleteTrainer.Text = "Удалить";
            buttonDeleteTrainer.Click += buttonDeleteTrainer_Click;

            // trainer/athlete attach controls
            labelAttachTrainer.AutoSize = true;
            labelAttachTrainer.Location = new Point(380, 84);
            labelAttachTrainer.Text = "Тренер:";
            comboAttachTrainer.DropDownStyle = ComboBoxStyle.DropDownList;
            comboAttachTrainer.Location = new Point(470, 81);
            comboAttachTrainer.Size = new Size(200, 23);

            labelAttachAthlete.AutoSize = true;
            labelAttachAthlete.Location = new Point(680, 84);
            labelAttachAthlete.Text = "Атлет:";
            comboAthleteForAttach.DropDownStyle = ComboBoxStyle.DropDownList;
            comboAthleteForAttach.Location = new Point(730, 81);
            comboAthleteForAttach.Size = new Size(190, 23);

            buttonAttach.Location = new Point(930, 76);
            buttonAttach.Size = new Size(75, 30);
            buttonAttach.Text = "+";
            buttonAttach.Click += buttonAttach_Click;

            buttonDetach.Location = new Point(1010, 76);
            buttonDetach.Size = new Size(65, 30);
            buttonDetach.Text = "-";
            buttonDetach.Click += buttonDetach_Click;

            // dataGridTrainers
            dataGridTrainers.AllowUserToAddRows = false;
            dataGridTrainers.AllowUserToDeleteRows = false;
            dataGridTrainers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridTrainers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridTrainers.Location = new Point(12, 125);
            dataGridTrainers.MultiSelect = false;
            dataGridTrainers.ReadOnly = true;
            dataGridTrainers.RowHeadersVisible = false;
            dataGridTrainers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridTrainers.Size = new Size(1060, 510);
            dataGridTrainers.SelectionChanged += dataGridTrainers_SelectionChanged;
            dataGridTrainers.Columns.Add("Id", "ID");
            dataGridTrainers.Columns.Add("FullName", "ФИО");
            dataGridTrainers.Columns.Add("Gender", "Пол");
            dataGridTrainers.Columns.Add("TrainingType", "Тип тренировки");
            dataGridTrainers.Columns.Add("Age", "Возраст");
            dataGridTrainers.Columns.Add("Experience", "Стаж");
            dataGridTrainers.Columns.Add("Athletes", "Атлетов");

            // tabAthletes
            tabAthletes.Controls.Add(dataGridAthletes);
            tabAthletes.Controls.Add(labelAthleteName);
            tabAthletes.Controls.Add(labelAthleteGender);
            tabAthletes.Controls.Add(labelAthleteType);
            tabAthletes.Controls.Add(labelAthleteAge);
            tabAthletes.Controls.Add(labelAthleteHeight);
            tabAthletes.Controls.Add(labelAthleteWeight);
            tabAthletes.Controls.Add(labelAthleteTrainer);
            tabAthletes.Controls.Add(textAthleteName);
            tabAthletes.Controls.Add(comboAthleteGender);
            tabAthletes.Controls.Add(comboAthleteType);
            tabAthletes.Controls.Add(textAthleteAge);
            tabAthletes.Controls.Add(textAthleteHeight);
            tabAthletes.Controls.Add(textAthleteWeight);
            tabAthletes.Controls.Add(comboAthleteTrainer);
            tabAthletes.Controls.Add(buttonAddAthlete);
            tabAthletes.Controls.Add(buttonUpdateAthlete);
            tabAthletes.Controls.Add(buttonDeleteAthlete);
            tabAthletes.Location = new Point(4, 24);
            tabAthletes.Name = "tabAthletes";
            tabAthletes.Padding = new Padding(3);
            tabAthletes.Size = new Size(1092, 652);
            tabAthletes.TabIndex = 1;
            tabAthletes.Text = "Атлеты";
            tabAthletes.UseVisualStyleBackColor = true;

            labelAthleteName.AutoSize = true;
            labelAthleteName.Location = new Point(12, 15);
            labelAthleteName.Text = "ФИО:";
            textAthleteName.Location = new Point(120, 12);
            textAthleteName.Size = new Size(220, 23);

            labelAthleteGender.AutoSize = true;
            labelAthleteGender.Location = new Point(12, 48);
            labelAthleteGender.Text = "Пол:";
            comboAthleteGender.DropDownStyle = ComboBoxStyle.DropDownList;
            comboAthleteGender.Location = new Point(120, 45);
            comboAthleteGender.Size = new Size(220, 23);

            labelAthleteType.AutoSize = true;
            labelAthleteType.Location = new Point(12, 81);
            labelAthleteType.Text = "Тип тренировки:";
            comboAthleteType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboAthleteType.Location = new Point(120, 78);
            comboAthleteType.Size = new Size(220, 23);

            labelAthleteAge.AutoSize = true;
            labelAthleteAge.Location = new Point(380, 15);
            labelAthleteAge.Text = "Возраст:";
            textAthleteAge.Location = new Point(450, 12);
            textAthleteAge.Size = new Size(85, 23);

            labelAthleteHeight.AutoSize = true;
            labelAthleteHeight.Location = new Point(560, 15);
            labelAthleteHeight.Text = "Рост:";
            textAthleteHeight.Location = new Point(610, 12);
            textAthleteHeight.Size = new Size(85, 23);

            labelAthleteWeight.AutoSize = true;
            labelAthleteWeight.Location = new Point(720, 15);
            labelAthleteWeight.Text = "Вес:";
            textAthleteWeight.Location = new Point(760, 12);
            textAthleteWeight.Size = new Size(85, 23);

            labelAthleteTrainer.AutoSize = true;
            labelAthleteTrainer.Location = new Point(380, 48);
            labelAthleteTrainer.Text = "Тренер:";
            comboAthleteTrainer.DropDownStyle = ComboBoxStyle.DropDownList;
            comboAthleteTrainer.Location = new Point(450, 45);
            comboAthleteTrainer.Size = new Size(250, 23);

            buttonAddAthlete.Location = new Point(720, 45);
            buttonAddAthlete.Size = new Size(120, 30);
            buttonAddAthlete.Text = "Добавить";
            buttonAddAthlete.Click += buttonAddAthlete_Click;

            buttonUpdateAthlete.Location = new Point(850, 45);
            buttonUpdateAthlete.Size = new Size(120, 30);
            buttonUpdateAthlete.Text = "Изменить";
            buttonUpdateAthlete.Click += buttonUpdateAthlete_Click;

            buttonDeleteAthlete.Location = new Point(720, 78);
            buttonDeleteAthlete.Size = new Size(250, 30);
            buttonDeleteAthlete.Text = "Удалить";
            buttonDeleteAthlete.Click += buttonDeleteAthlete_Click;

            dataGridAthletes.AllowUserToAddRows = false;
            dataGridAthletes.AllowUserToDeleteRows = false;
            dataGridAthletes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridAthletes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridAthletes.Location = new Point(12, 125);
            dataGridAthletes.MultiSelect = false;
            dataGridAthletes.ReadOnly = true;
            dataGridAthletes.RowHeadersVisible = false;
            dataGridAthletes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridAthletes.Size = new Size(1060, 510);
            dataGridAthletes.SelectionChanged += dataGridAthletes_SelectionChanged;
            dataGridAthletes.Columns.Add("Id", "ID");
            dataGridAthletes.Columns.Add("FullName", "ФИО");
            dataGridAthletes.Columns.Add("Gender", "Пол");
            dataGridAthletes.Columns.Add("Age", "Возраст");
            dataGridAthletes.Columns.Add("Height", "Рост");
            dataGridAthletes.Columns.Add("Weight", "Вес");
            dataGridAthletes.Columns.Add("TrainingType", "Тип тренировки");
            dataGridAthletes.Columns.Add("Trainer", "Тренер");

            // EmployeeForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 680);
            Controls.Add(tabControl);
            MinimumSize = new Size(900, 600);
            Name = "EmployeeForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Prime Time - Сотрудник";

            tabAthletes.ResumeLayout(false);
            tabAthletes.PerformLayout();
            tabTrainers.ResumeLayout(false);
            tabTrainers.PerformLayout();
            tabControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridAthletes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridTrainers).EndInit();
            ResumeLayout(false);
        }
    }
}
