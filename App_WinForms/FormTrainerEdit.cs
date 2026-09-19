using System;
using System.Windows.Forms;
using App_Model_Essence;
using App_Model_TestLogics;

namespace App_WinForms
{
    public class FormTrainerEdit : Form
    {
        private readonly ILogics _logics;
        private readonly Trainer _trainerToEdit;

        private TextBox txtFullName;
        private ComboBox cmbGender;
        private ComboBox cmbTrainingType;
        private NumericUpDown numAge;
        private NumericUpDown numExperience;
        private Button btnSave;
        private Button btnCancel;

        public FormTrainerEdit(ILogics logics, Trainer trainer = null)
        {
            _logics = logics;
            _trainerToEdit = trainer;
            InitializeUI();

            if (_trainerToEdit != null)
            {
                this.Text = "Редактирование тренера";
                txtFullName.Text = _trainerToEdit.FullName;
                cmbGender.SelectedItem = _trainerToEdit.Gendre;
                cmbTrainingType.SelectedItem = _trainerToEdit.TrainingType;
                numAge.Value = _trainerToEdit.Age;
                numExperience.Value = _trainerToEdit.WorkExperience;
            }
            else
            {
                this.Text = "Добавление тренера";
            }
        }

        private void InitializeUI()
        {
            this.Size = new System.Drawing.Size(350, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblName = new Label { Text = "ФИО:", Location = new System.Drawing.Point(20, 20), AutoSize = true };
            txtFullName = new TextBox { Location = new System.Drawing.Point(130, 17), Width = 170 };

            var lblGender = new Label { Text = "Пол:", Location = new System.Drawing.Point(20, 55), AutoSize = true };
            cmbGender = new ComboBox { Location = new System.Drawing.Point(130, 52), Width = 170, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbGender.DataSource = Enum.GetValues(typeof(Gendre));

            var lblType = new Label { Text = "Тип:", Location = new System.Drawing.Point(20, 90), AutoSize = true };
            cmbTrainingType = new ComboBox { Location = new System.Drawing.Point(130, 87), Width = 170, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTrainingType.DataSource = Enum.GetValues(typeof(TrainingType));

            var lblAge = new Label { Text = "Возраст:", Location = new System.Drawing.Point(20, 125), AutoSize = true };
            numAge = new NumericUpDown { Location = new System.Drawing.Point(130, 122), Width = 170, Minimum = 18, Maximum = 100, Value = 25 };

            var lblExp = new Label { Text = "Стаж:", Location = new System.Drawing.Point(20, 160), AutoSize = true };
            numExperience = new NumericUpDown { Location = new System.Drawing.Point(130, 157), Width = 170, Minimum = 0, Maximum = 80, Value = 3 };

            btnSave = new Button { Text = "Сохранить", Location = new System.Drawing.Point(40, 210), Width = 110, Height = 35 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(170, 210), Width = 110, Height = 35 };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblName, txtFullName, lblGender, cmbGender, lblType, cmbTrainingType, lblAge, numAge, lblExp, numExperience, btnSave, btnCancel });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_trainerToEdit == null)
                {
                    _logics.AddTrainer(
                        txtFullName.Text,
                        (Gendre)cmbGender.SelectedItem,
                        (TrainingType)cmbTrainingType.SelectedItem,
                        (int)numAge.Value,
                        (int)numExperience.Value
                    );
                }
                else
                {
                    _logics.UpdateInfoTrainer(
                        _trainerToEdit.Id,
                        txtFullName.Text,
                        (Gendre)cmbGender.SelectedItem,
                        (TrainingType)cmbTrainingType.SelectedItem,
                        (int)numAge.Value,
                        (int)numExperience.Value,
                        null, null
                    );
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка логики: {ex.Message}", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}