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
        }

        private void FillEnums()
        {
            comboGender.Items.AddRange(Enum.GetValues<Gendre>().Cast<object>().ToArray());
            comboTrainingType.Items.AddRange(Enum.GetValues<TrainingType>().Cast<object>().ToArray());
            comboGender.SelectedIndex = 0;
            comboTrainingType.SelectedIndex = 0;
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
                    trainer.Athlete.Count,
                    trainer.TrainingType);

                number++;
            }
        }

        private void buttonAddAthlete_Click(object sender, EventArgs e)
        {
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
                MessageBox.Show("Регистрация не выполнена. Возможно, атлет уже закреплен за тренером.");
            }
        }

        private void buttonPersonalTraining_Click(object sender, EventArgs e)
        {
            Athlete? athlete = comboPersonalAthlete.SelectedItem as Athlete;

            if (athlete == null)
            {
                MessageBox.Show("Сначала добавьте атлета.");
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

            foreach (Trainer trainer in logics.PersonalFilterTrainers(athlete))
            {
                dataGridFilter.Rows.Add(
                    trainer.Id,
                    trainer.FullName,
                    trainer.Gendre,
                    trainer.TrainingType,
                    trainer.Age,
                    trainer.WorkExperience,
                    trainer.Athlete.Count);
            }

            if (dataGridFilter.Rows.Count == 0)
                MessageBox.Show("Подходящих тренеров не найдено.");
        }

        private void buttonRefreshRating_Click(object sender, EventArgs e)
        {
            RefreshRating();
        }
    }
}
