using System;
using System.Windows.Forms;
using App_Model_Essence;
using App_Model_TestLogics;

namespace App_WinForms
{
    public class FormAthleteEdit : Form
    {
        private readonly ILogics _logics;
        private readonly Athlete _athleteToEdit;

        private TextBox txtFullName;
        private ComboBox cmbGender;
        private ComboBox cmbTrainingType;
        private NumericUpDown numAge;
        private NumericUpDown numHeight;
        private NumericUpDown numWeight;
        private Button btnSave;
        private Button btnCancel;

        public FormAthleteEdit(ILogics logics, Athlete athlete = null)
        {
            _logics = logics;
            _athleteToEdit = athlete;
            InitializeUI();

            if (_athleteToEdit != null)
            {
                this.Text = "Редактирование атлета";
                txtFullName.Text = _athleteToEdit.FullName;
                cmbGender.SelectedItem = _athleteToEdit.Gendre;
                cmbTrainingType.SelectedItem = _athleteToEdit.TrainingType;
                numAge.Value = _athleteToEdit.Age;
                numHeight.Value = _athleteToEdit.Height > 0 ? _athleteToEdit.Height : 170;
                numWeight.Value = _athleteToEdit.Weight > 0 ? _athleteToEdit.Weight : 70;
            }
            else
            {
                this.Text = "Добавление атлета";
            }
        }

        private void InitializeUI()
        {
            this.Size = new System.Drawing.Size(350, 350);
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
            numAge = new NumericUpDown { Location = new System.Drawing.Point(130, 122), Width = 170, Minimum = 14, Maximum = 100, Value = 20 };

            var lblHeight = new Label { Text = "Рост (см):", Location = new System.Drawing.Point(20, 160), AutoSize = true };
            numHeight = new NumericUpDown { Location = new System.Drawing.Point(130, 157), Width = 170, Minimum = 100, Maximum = 250, Value = 175 };

            var lblWeight = new Label { Text = "Вес (кг):", Location = new System.Drawing.Point(20, 195), AutoSize = true };
            numWeight = new NumericUpDown { Location = new System.Drawing.Point(130, 192), Width = 170, Minimum = 30, Maximum = 300, Value = 70 };

            btnSave = new Button { Text = "Сохранить", Location = new System.Drawing.Point(40, 245), Width = 110, Height = 35 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(170, 245), Width = 110, Height = 35 };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblName, txtFullName, lblGender, cmbGender, lblType, cmbTrainingType, lblAge, numAge, lblHeight, numHeight, lblWeight, numWeight, btnSave, btnCancel });
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
                if (_athleteToEdit == null)
                {
                    _logics.AddAthlete(
                        txtFullName.Text,
                        (Gendre)cmbGender.SelectedItem,
                        (TrainingType)cmbTrainingType.SelectedItem,
                        (int)numAge.Value,
                        (int)numHeight.Value,
                        (int)numWeight.Value
                    );
                }
                else
                {
                    _logics.UpdateInfoAthlete(
                        _athleteToEdit.Id,
                        txtFullName.Text,
                        (Gendre)cmbGender.SelectedItem,
                        (int)numAge.Value,
                        (int)numHeight.Value,
                        (int)numWeight.Value,
                        (TrainingType)cmbTrainingType.SelectedItem,
                        null
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