
using Essence;
using Shared;

namespace App_WinForms
{
    /// <summary>
    /// Реализует графическое представление пользователя
    /// на основе Windows Forms и интерфейса IUserView.
    /// </summary>
    public partial class UserForm : Form, IUserView
    {
        public event EventHandler? RefreshAthletesRequested;
        public event EventHandler? RefreshTrainersRequested;
        public event EventHandler? AddAthleteRequested;
        public event EventHandler? RefreshRatingRequested;
        public event EventHandler? RegisterAthleteRequested;
        public event EventHandler? PersonalTrainingRequested;
        public event EventHandler? FilterTrainersRequested;
        public event EventHandler? SignUpFromFilterRequested;

        public AthleteInputDto NewAthlete { get; private set; } = new();

        public int SelectedRegistrationAthleteId =>
            (comboRegistrationAthlete.SelectedItem as AthleteDto)?.Id ?? 0;

        public int SelectedRegistrationTrainerId =>
            (comboRegistrationTrainer.SelectedItem as TrainerDto)?.Id ?? 0;

        public int SelectedPersonalAthleteId =>
            (comboPersonalAthlete.SelectedItem as AthleteDto)?.Id ?? 0;

        public int SelectedFilterAthleteId =>
            (comboFilterAthlete.SelectedItem as AthleteDto)?.Id ?? 0;

        public int SelectedFilteredTrainerId
        {
            get
            {
                if (dataGridFilter.SelectedRows.Count == 0)
                    return 0;

                object? value =
                    dataGridFilter.SelectedRows[0].Cells[0].Value;

                return value != null
                    ? Convert.ToInt32(value)
                    : 0;
            }
        }
        /// <summary>
        /// Инициализирует пользовательскую форму и настраивает элементы управления.
        /// </summary>
        public UserForm()
        {
            InitializeComponent();
            FillEnums();
            comboGender.SelectedIndexChanged +=
    comboGender_SelectedIndexChanged;

            dataGridFilter.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridFilter.MultiSelect = false;
        }

        private void FillEnums()
        {
            comboGender.Items.Clear();
            comboTrainingType.Items.Clear();

            comboGender.Items.AddRange(
                Enum.GetValues<Gendre>()
                    .Cast<object>()
                    .ToArray());

            if (comboGender.Items.Count > 0)
                comboGender.SelectedIndex = 0;

            UpdateTrainingTypesForSelectedGender();
        }
        private static TrainingType[] GetTrainingTypes(
    Gendre gender)
        {
            return Enum.GetValues<TrainingType>()
                .Where(t => t.ToString().StartsWith(
                    $"{gender}_",
                    StringComparison.Ordinal))
                .ToArray();
        }

        private void UpdateTrainingTypesForSelectedGender()
        {
            if (comboGender.SelectedItem is not Gendre gender)
                return;

            TrainingType[] types = GetTrainingTypes(gender);

            comboTrainingType.Items.Clear();

            comboTrainingType.Items.AddRange(
                types.Cast<object>().ToArray());

            if (types.Length > 0)
                comboTrainingType.SelectedIndex = 0;
        }

        private void comboGender_SelectedIndexChanged(
            object? sender, EventArgs e)
        {
            UpdateTrainingTypesForSelectedGender();
        }

        private static void BindCombo<T>(
            ComboBox combo,
            IReadOnlyList<T> items,
            Func<T, int> getId) where T : class
        {
            int selectedId = 0;

            if (combo.SelectedItem is T current)
                selectedId = getId(current);

            combo.DataSource = null;
            combo.DisplayMember = "FullName";
            combo.DataSource = items.ToList();

            if (selectedId > 0)
            {
                T? selected = items.FirstOrDefault(
                    x => getId(x) == selectedId);

                if (selected != null)
                    combo.SelectedItem = selected;
            }
        }

        public void ShowAthletes(IReadOnlyList<AthleteDto> athletes)
        {
            BindCombo(
                comboRegistrationAthlete,
                athletes,
                a => a.Id);

            BindCombo(
                comboPersonalAthlete,
                athletes,
                a => a.Id);

            BindCombo(
                comboFilterAthlete,
                athletes,
                a => a.Id);
        }

        public void ShowTrainers(IReadOnlyList<TrainerDto> trainers)
        {
            BindCombo(
                comboRegistrationTrainer,
                trainers,
                t => t.Id);
        }

        public void ShowRating(
            IReadOnlyList<TrainerRatingDto> rating)
        {
            dataGridRating.Rows.Clear();

            foreach (TrainerRatingDto item in rating)
            {
                TrainerDto trainer = item.Trainer;

                dataGridRating.Rows.Add(
                    item.Position,
                    trainer.Id,
                    trainer.FullName,
                    (Gendre)trainer.Gender,
                    trainer.Age,
                    trainer.WorkExperience,
                    item.AthleteCount,
                    (TrainingType)trainer.TrainingType);
            }
        }

        public void ShowPersonalTraining(string trainingPlan)
        {
            textPersonalResult.Text = trainingPlan;
        }

        public void ShowFilteredTrainers(
            IReadOnlyList<TrainerMatchDto> trainers)
        {
            dataGridFilter.Rows.Clear();

            foreach (TrainerMatchDto item in trainers)
            {
                TrainerDto trainer = item.Trainer;

                dataGridFilter.Rows.Add(
                    trainer.Id,
                    trainer.FullName,
                    (Gendre)trainer.Gender,
                    (TrainingType)trainer.TrainingType,
                    trainer.Age,
                    trainer.WorkExperience,
                    item.AthleteCount,
                    $"{item.MatchPercentage}%");
            }

            dataGridFilter.ClearSelection();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(
                message,
                "Информация",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void buttonAddAthlete_Click(
            object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textFullName.Text))
            {
                ShowError("Введите ФИО атлета.");
                return;
            }

            if (!int.TryParse(textAge.Text, out int age) ||
                !int.TryParse(textHeight.Text, out int height) ||
                !int.TryParse(textWeight.Text, out int weight))
            {
                ShowError(
                    "Возраст, рост и вес должны быть целыми числами.");
                return;
            }

            if (comboGender.SelectedItem is not Gendre gender ||
                comboTrainingType.SelectedItem is not TrainingType type)
            {
                ShowError("Выберите пол и тип тренировки.");
                return;
            }

            NewAthlete = new AthleteInputDto
            {
                FullName = textFullName.Text.Trim(),
                Gender = (int)gender,
                TrainingType = (int)type,
                Age = age,
                Height = height,
                Weight = weight
            };

            AddAthleteRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonRegistration_Click(
            object sender, EventArgs e)
        {
            if (SelectedRegistrationAthleteId <= 0 ||
                SelectedRegistrationTrainerId <= 0)
            {
                ShowError("Выберите атлета и тренера.");
                return;
            }

            RegisterAthleteRequested?.Invoke(
                this, EventArgs.Empty);
        }

        private void buttonPersonalTraining_Click(
            object sender, EventArgs e)
        {
            if (SelectedPersonalAthleteId <= 0)
            {
                ShowError("Сначала выберите или добавьте атлета.");
                return;
            }

            PersonalTrainingRequested?.Invoke(
                this, EventArgs.Empty);
        }

        private void buttonFilter_Click(
            object sender, EventArgs e)
        {
            if (SelectedFilterAthleteId <= 0)
            {
                ShowError("Выберите атлета.");
                return;
            }

            FilterTrainersRequested?.Invoke(
                this, EventArgs.Empty);
        }

        private void buttonSignUpFromFilter_Click(
            object sender, EventArgs e)
        {
            if (SelectedFilterAthleteId <= 0)
            {
                ShowError("Выберите атлета вверху страницы.");
                return;
            }

            if (SelectedFilteredTrainerId <= 0)
            {
                ShowError(
                    "Выберите тренера из таблицы подбора.");
                return;
            }

            SignUpFromFilterRequested?.Invoke(
                this, EventArgs.Empty);
        }

        private void buttonRefreshRating_Click(
            object sender, EventArgs e)
        {
            RefreshRatingRequested?.Invoke(
                this, EventArgs.Empty);
        }
    }
}
