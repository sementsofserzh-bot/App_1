using System;
using System.Linq;
using System.Windows.Forms;
using App_Model_Essence;
using App_Model_TestLogics;

namespace App_WinForms
{
    public partial class UserForm : Form
    {
        private readonly ILogics logics;

        public UserForm(ILogics logics)
        {
            InitializeComponent();
            this.logics = logics;
            FillEnums();
            RefreshCombos();
            RefreshRating();
        }

        private void FillEnums()
        {
            comboGender.Items.Clear();
            comboTrainingType.Items.Clear();

            comboGender.Items.AddRange(Enum.GetValues<Gendre>().Cast<object>().ToArray());
            comboTrainingType.Items.AddRange(Enum.GetValues<TrainingType>().Cast<object>().ToArray());

            if (comboGender.Items.Count > 0) comboGender.SelectedIndex = 0;
            if (comboTrainingType.Items.Count > 0) comboTrainingType.SelectedIndex = 0;
        }

        private void RefreshCombos()
        {
            comboRegistrationAthlete.DataSource = null;
            comboRegistrationAthlete.DataSource = logics.BD_Athlete.ToList();
            comboRegistrationAthlete.DisplayMember = "FullName";

            comboRegistrationTrainer.DataSource = null;
            comboRegistrationTrainer.DataSource = logics.BD_Trainer.ToList();
            comboRegistrationTrainer.DisplayMember = "FullName";

            comboPersonalAthlete.DataSource = null;
            comboPersonalAthlete.DataSource = logics.BD_Athlete.ToList();
            comboPersonalAthlete.DisplayMember = "FullName";

            comboFilterAthlete.DataSource = null;
            comboFilterAthlete.DataSource = logics.BD_Athlete.ToList();
            comboFilterAthlete.DisplayMember = "FullName";
        }

        private void RefreshRating()
        {
            dataGridRating.Rows.Clear();

            int number = 1;

            foreach (Trainer trainer in logics.RateTrainers())
            {
                dataGridRating.Rows.Add(
                    number,
                    trainer.Id,
                    trainer.FullName,
                    trainer.Gendre,
                    trainer.Age,
                    trainer.WorkExperience,
                    trainer.Athlete?.Count ?? 0,
                    trainer.TrainingType);

                number++;
            }
        }

        private void buttonAddAthlete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textFullName.Text))
            {
                MessageBox.Show("Введите ФИО атлета.");
                return;
            }

            if (!int.TryParse(textAge.Text, out int age) ||
                !int.TryParse(textHeight.Text, out int height) ||
                !int.TryParse(textWeight.Text, out int weight))
            {
                MessageBox.Show("Возраст, рост и вес должны быть числами.");
                return;
            }

            Athlete athlete = logics.AddAthlete(
                textFullName.Text,
                (Gendre)comboGender.SelectedItem!,
                (TrainingType)comboTrainingType.SelectedItem!,
                age,
                height,
                weight);

            RefreshCombos();
            MessageBox.Show($"Атлет {athlete.FullName} успешно добавлен.");

            textFullName.Clear();
            textAge.Clear();
            textHeight.Clear();
            textWeight.Clear();
        }

        private void buttonRegistration_Click(object sender, EventArgs e)
        {
            Athlete? athlete = comboRegistrationAthlete.SelectedItem as Athlete;
            Trainer? trainer = comboRegistrationTrainer.SelectedItem as Trainer;

            if (athlete == null || trainer == null)
            {
                MessageBox.Show("Выберите атлета и тренера.");
                return;
            }

            if (logics.Registration(trainer, athlete))
            {
                RefreshCombos();
                RefreshRating();
                MessageBox.Show("Атлет успешно зарегистрирован за тренером.");
            }
            else
            {
                MessageBox.Show("Регистрация не выполнена. Возможно, атлет уже закреплен за этим тренером.");
            }
        }

        private void buttonPersonalTraining_Click(object sender, EventArgs e)
        {
            Athlete? athlete = comboPersonalAthlete.SelectedItem as Athlete;

            if (athlete == null)
            {
                MessageBox.Show("Сначала выберите или добавьте атлета.");
                return;
            }

            labelPersonalResult.Text = logics.PersonalTraining(athlete);
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Athlete? athlete = comboFilterAthlete.SelectedItem as Athlete;

            if (athlete == null)
            {
                MessageBox.Show("Выберите атлета.");
                return;
            }

            dataGridFilter.Rows.Clear();

            var filteredTrainers = logics.PersonalFilterTrainers(athlete);

            foreach (Trainer trainer in filteredTrainers)
            {
                dataGridFilter.Rows.Add(
                    trainer.Id,
                    trainer.FullName,
                    trainer.Gendre,
                    trainer.TrainingType,
                    trainer.Age,
                    trainer.WorkExperience,
                    trainer.Athlete?.Count ?? 0);
            }

            if (dataGridFilter.Rows.Count == 0)
            {
                MessageBox.Show("Подходящих тренеров по данному направлению не найдено.");
            }
        }

        private void buttonRefreshRating_Click(object sender, EventArgs e)
        {
            RefreshRating();
        }
    }
}