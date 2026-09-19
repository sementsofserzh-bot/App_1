using App_Model_Essence;
using App_Model_TestLogics;

namespace App_WinForms
{
    public partial class EmployeeForm : Form
    {
        private readonly ILogics logics;

        public EmployeeForm(ILogics logics)
        {
            InitializeComponent();
            this.logics = logics;

            FillEnums();
            RefreshAll();
        }

        private void FillEnums()
        {
            comboTrainerGender.Items.AddRange(Enum.GetValues<Gendre>().Cast<object>().ToArray());
            comboTrainerType.Items.AddRange(Enum.GetValues<TrainingType>().Cast<object>().ToArray());
            comboAthleteGender.Items.AddRange(Enum.GetValues<Gendre>().Cast<object>().ToArray());
            comboAthleteType.Items.AddRange(Enum.GetValues<TrainingType>().Cast<object>().ToArray());

            comboTrainerGender.SelectedIndex = 0;
            comboTrainerType.SelectedIndex = 0;
            comboAthleteGender.SelectedIndex = 0;
            comboAthleteType.SelectedIndex = 0;
        }

        private void RefreshAll()
        {
            RefreshTrainers();
            RefreshAthletes();
            RefreshTrainerAthleteCombos();
            RefreshAthleteTrainerCombo();
        }

        private void RefreshTrainers()
        {
            dataGridTrainers.Rows.Clear();

            foreach (Trainer trainer in logics.BD_Trainer)
            {
                dataGridTrainers.Rows.Add(
                    trainer.Id,
                    trainer.FullName,
                    trainer.Gendre,
                    trainer.TrainingType,
                    trainer.Age,
                    trainer.WorkExperience,
                    trainer.Athlete.Count);
            }
        }

        private void RefreshAthletes()
        {
            dataGridAthletes.Rows.Clear();

            foreach (Athlete athlete in logics.BD_Athlete)
            {
                string trainerName = athlete.trainer == null ? "Нет" : athlete.trainer.FullName;

                dataGridAthletes.Rows.Add(
                    athlete.Id,
                    athlete.FullName,
                    athlete.Gendre,
                    athlete.Age,
                    athlete.Height,
                    athlete.Weight,
                    athlete.TrainingType,
                    trainerName);
            }
        }

        private void RefreshTrainerAthleteCombos()
        {
            comboAttachTrainer.DataSource = null;
            comboAttachTrainer.DataSource = logics.BD_Trainer.ToList();
            comboAttachTrainer.DisplayMember = "FullName";

            comboAthleteForAttach.DataSource = null;
            comboAthleteForAttach.DataSource = logics.BD_Athlete.ToList();
            comboAthleteForAttach.DisplayMember = "FullName";
        }

        private void RefreshAthleteTrainerCombo()
        {
            comboAthleteTrainer.DataSource = null;
            List<Trainer> trainers = new List<Trainer>(logics.BD_Trainer);
            trainers.Insert(0, new Trainer { Id = 0, FullName = "Не менять тренера" });
            comboAthleteTrainer.DataSource = trainers;
            comboAthleteTrainer.DisplayMember = "FullName";
        }

        private Trainer? GetSelectedTrainer()
        {
            if (dataGridTrainers.SelectedRows.Count == 0)
                return null;

            int id = Convert.ToInt32(dataGridTrainers.SelectedRows[0].Cells[0].Value);
            return logics.CheckTrainer(id);
        }

        private Athlete? GetSelectedAthlete()
        {
            if (dataGridAthletes.SelectedRows.Count == 0)
                return null;

            int id = Convert.ToInt32(dataGridAthletes.SelectedRows[0].Cells[0].Value);
            return logics.CheckAthlete(id);
        }

        private void ClearTrainerFields()
        {
            textTrainerName.Clear();
            textTrainerAge.Clear();
            textTrainerExperience.Clear();
            comboTrainerGender.SelectedIndex = 0;
            comboTrainerType.SelectedIndex = 0;
        }

        private void ClearAthleteFields()
        {
            textAthleteName.Clear();
            textAthleteAge.Clear();
            textAthleteHeight.Clear();
            textAthleteWeight.Clear();
            comboAthleteGender.SelectedIndex = 0;
            comboAthleteType.SelectedIndex = 0;
        }

        private void dataGridTrainers_SelectionChanged(object sender, EventArgs e)
        {
            Trainer? trainer = GetSelectedTrainer();

            if (trainer == null)
                return;

            textTrainerName.Text = trainer.FullName;
            comboTrainerGender.SelectedItem = trainer.Gendre;
            comboTrainerType.SelectedItem = trainer.TrainingType;
            textTrainerAge.Text = trainer.Age.ToString();
            textTrainerExperience.Text = trainer.WorkExperience.ToString();
        }

        private void dataGridAthletes_SelectionChanged(object sender, EventArgs e)
        {
            Athlete? athlete = GetSelectedAthlete();

            if (athlete == null)
                return;

            textAthleteName.Text = athlete.FullName;
            comboAthleteGender.SelectedItem = athlete.Gendre;
            comboAthleteType.SelectedItem = athlete.TrainingType;
            textAthleteAge.Text = athlete.Age.ToString();
            textAthleteHeight.Text = athlete.Height.ToString();
            textAthleteWeight.Text = athlete.Weight.ToString();
        }

        private void buttonAddTrainer_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textTrainerAge.Text, out int age) || !int.TryParse(textTrainerExperience.Text, out int experience))
            {
                MessageBox.Show("Введите возраст и стаж числами.");
                return;
            }

            Trainer trainer = logics.AddTrainer(
                textTrainerName.Text,
                (Gendre)comboTrainerGender.SelectedItem!,
                (TrainingType)comboTrainerType.SelectedItem!,
                age,
                experience);

            RefreshAll();
            MessageBox.Show($"Тренер {trainer.FullName} успешно добавлен.");
            ClearTrainerFields();
        }

        private void buttonUpdateTrainer_Click(object sender, EventArgs e)
        {
            Trainer? trainer = GetSelectedTrainer();

            if (trainer == null)
            {
                MessageBox.Show("Выберите тренера.");
                return;
            }

            if (!int.TryParse(textTrainerAge.Text, out int age) || !int.TryParse(textTrainerExperience.Text, out int experience))
            {
                MessageBox.Show("Введите возраст и стаж числами.");
                return;
            }

            logics.UpdateInfoTrainer(
                trainer.Id,
                textTrainerName.Text,
                (Gendre)comboTrainerGender.SelectedItem!,
                (TrainingType)comboTrainerType.SelectedItem!,
                age,
                experience,
                null,
                null);

            RefreshAll();
            MessageBox.Show("Данные тренера изменены.");
        }

        private void buttonDeleteTrainer_Click(object sender, EventArgs e)
        {
            Trainer? trainer = GetSelectedTrainer();

            if (trainer == null)
            {
                MessageBox.Show("Выберите тренера.");
                return;
            }

            if (logics.RemoveTrainer(trainer.Id) == true)
            {
                RefreshAll();
                ClearTrainerFields();
                MessageBox.Show("Тренер удален.");
            }
        }

        private void buttonAddAthlete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textAthleteAge.Text, out int age) ||
                !int.TryParse(textAthleteHeight.Text, out int height) ||
                !int.TryParse(textAthleteWeight.Text, out int weight))
            {
                MessageBox.Show("Возраст, рост и вес должны быть числами.");
                return;
            }

            Athlete athlete = logics.AddAthlete(
                textAthleteName.Text,
                (Gendre)comboAthleteGender.SelectedItem!,
                (TrainingType)comboAthleteType.SelectedItem!,
                age,
                height,
                weight);

            RefreshAll();
            MessageBox.Show($"Атлет {athlete.FullName} успешно добавлен.");
            ClearAthleteFields();
        }

        private void buttonUpdateAthlete_Click(object sender, EventArgs e)
        {
            Athlete? athlete = GetSelectedAthlete();

            if (athlete == null)
            {
                MessageBox.Show("Выберите атлета.");
                return;
            }

            if (!int.TryParse(textAthleteAge.Text, out int age) ||
                !int.TryParse(textAthleteHeight.Text, out int height) ||
                !int.TryParse(textAthleteWeight.Text, out int weight))
            {
                MessageBox.Show("Возраст, рост и вес должны быть числами.");
                return;
            }

            Trainer? trainer = athlete.trainer;

            if (comboAthleteTrainer.SelectedItem is Trainer selectedTrainer && selectedTrainer.Id != 0)
                trainer = selectedTrainer;

            logics.UpdateInfoAthlete(
                athlete.Id,
                textAthleteName.Text,
                (Gendre)comboAthleteGender.SelectedItem!,
                age,
                height,
                weight,
                (TrainingType)comboAthleteType.SelectedItem!,
                trainer);

            RefreshAll();
            MessageBox.Show("Данные атлета изменены.");
        }

        private void buttonDeleteAthlete_Click(object sender, EventArgs e)
        {
            Athlete? athlete = GetSelectedAthlete();

            if (athlete == null)
            {
                MessageBox.Show("Выберите атлета.");
                return;
            }

            if (logics.RemoveAthlete(athlete.Id) == true)
            {
                RefreshAll();
                ClearAthleteFields();
                MessageBox.Show("Атлет удален.");
            }
        }

        private void buttonAttach_Click(object sender, EventArgs e)
        {
            Trainer? trainer = comboAttachTrainer.SelectedItem as Trainer;
            Athlete? athlete = comboAthleteForAttach.SelectedItem as Athlete;

            if (trainer == null || athlete == null)
            {
                MessageBox.Show("Выберите тренера и атлета.");
                return;
            }

            if (logics.Registration(trainer, athlete))
            {
                RefreshAll();
                MessageBox.Show("Атлет закреплен за тренером.");
            }
            else
            {
                MessageBox.Show("Не удалось закрепить атлета. Возможно, он уже закреплен.");
            }
        }

        private void buttonDetach_Click(object sender, EventArgs e)
        {
            Trainer? trainer = comboAttachTrainer.SelectedItem as Trainer;
            Athlete? athlete = comboAthleteForAttach.SelectedItem as Athlete;

            if (trainer == null || athlete == null)
            {
                MessageBox.Show("Выберите тренера и атлета.");
                return;
            }

            if (!trainer.Athlete.Contains(athlete))
            {
                MessageBox.Show("Этот атлет не закреплен за выбранным тренером.");
                return;
            }

            List<Athlete> deleteList = new List<Athlete> { athlete };
            logics.UpdateInfoTrainer(trainer.Id, null, null, null, null, null, deleteList, null);

            RefreshAll();
            MessageBox.Show("Атлет откреплен от тренера.");
        }
    }
}
