
using Essence;
using Shared;

namespace App_WinForms
{
    /// <summary>
    /// Реализует графическое представление сотрудника
    /// на основе Windows Forms и интерфейса IEmployeeView.
    /// </summary>
    public partial class EmployeeForm : Form, IEmployeeView
    {
        public event EventHandler? AddTrainerRequested;
        public event EventHandler? AddAthleteRequested;
        public event EventHandler? RefreshDataRequested;
        public event EventHandler? UpdateTrainerRequested;
        public event EventHandler? DeleteTrainerRequested;
        public event EventHandler? UpdateAthleteRequested;
        public event EventHandler? DeleteAthleteRequested;
        public event EventHandler? AttachAthleteRequested;
        public event EventHandler? DetachAthleteRequested;

        public TrainerInputDto NewTrainer { get; private set; } = new();
        public AthleteInputDto NewAthlete { get; private set; } = new();
        public TrainerEditDto EditedTrainer { get; private set; } = new();
        public AthleteEditDto EditedAthlete { get; private set; } = new();

        public TrainerAthleteDto SelectedTrainerAthlete
        { get; private set; } = new();

        public int SelectedTrainerId => GetSelectedTrainer()?.Id ?? 0;
        public int SelectedAthleteId => GetSelectedAthlete()?.Id ?? 0;

        private IReadOnlyList<TrainerDto> _trainers =
            Array.Empty<TrainerDto>();

        private IReadOnlyList<AthleteDto> _athletes =
            Array.Empty<AthleteDto>();

        private bool _refreshing;
        /// <summary>
        /// Инициализирует форму сотрудника и настраивает элементы управления.
        /// </summary>
        public EmployeeForm()
        {
            InitializeComponent();
            FillEnums();
            comboAthleteGender.SelectedIndexChanged +=
    comboAthleteGender_SelectedIndexChanged;

            dataGridTrainers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridAthletes.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridTrainers.MultiSelect = false;
            dataGridAthletes.MultiSelect = false;
        }

        private void FillEnums()
        {
            comboTrainerGender.Items.Clear();
            comboTrainerType.Items.Clear();
            comboAthleteGender.Items.Clear();
            comboAthleteType.Items.Clear();

            comboTrainerGender.Items.AddRange(
                Enum.GetValues<Gendre>().Cast<object>().ToArray());

            comboTrainerType.Items.AddRange(
                Enum.GetValues<TrainingType>().Cast<object>().ToArray());

            comboAthleteGender.Items.AddRange(
                Enum.GetValues<Gendre>().Cast<object>().ToArray());

            

            if (comboTrainerGender.Items.Count > 0)
                comboTrainerGender.SelectedIndex = 0;

            if (comboTrainerType.Items.Count > 0)
                comboTrainerType.SelectedIndex = 0;

            if (comboAthleteGender.Items.Count > 0)
                comboAthleteGender.SelectedIndex = 0;
            UpdateAthleteTrainingTypes();

            if (comboAthleteType.Items.Count > 0)
                comboAthleteType.SelectedIndex = 0;
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

        private void UpdateAthleteTrainingTypes(
            TrainingType? preferredType = null)
        {
            if (comboAthleteGender.SelectedItem is not Gendre gender)
                return;

            TrainingType[] types = GetTrainingTypes(gender);

            comboAthleteType.Items.Clear();

            comboAthleteType.Items.AddRange(
                types.Cast<object>().ToArray());

            if (preferredType.HasValue &&
                types.Contains(preferredType.Value))
            {
                comboAthleteType.SelectedItem = preferredType.Value;
            }
            else if (types.Length > 0)
            {
                comboAthleteType.SelectedIndex = 0;
            }
        }

        private void comboAthleteGender_SelectedIndexChanged(
            object? sender, EventArgs e)
        {
            if (_refreshing)
                return;

            UpdateAthleteTrainingTypes();
        }
        private static void BindCombo<T>(
            ComboBox combo,
            IReadOnlyList<T> items,
            Func<T, int> getId)
            where T : class
        {
            int previousId = combo.SelectedItem is T selected
                ? getId(selected)
                : 0;

            combo.DataSource = null;
            combo.DisplayMember = "FullName";
            combo.DataSource = items.ToList();

            if (previousId != 0)
            {
                T? previous = items.FirstOrDefault(
                    x => getId(x) == previousId);

                if (previous != null)
                    combo.SelectedItem = previous;
            }
        }

        public void ShowTrainers(
            IReadOnlyList<TrainerDto> trainers)
        {
            _trainers = trainers;

            int previousId = SelectedTrainerId;

            _refreshing = true;

            try
            {
                dataGridTrainers.Rows.Clear();

                dataGridTrainers.DefaultCellStyle.WrapMode =
                    DataGridViewTriState.True;

                dataGridTrainers.AutoSizeRowsMode =
                    DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

                foreach (TrainerDto trainer in trainers)
                {
                    string assignedAthletes =
                        trainer.Athletes.Count > 0
                            ? string.Join(", ",
                                trainer.Athletes.Select(a =>
                                    $"{a.FullName} (ID:{a.Id})"))
                            : "Нет закрепленных";

                    int rowIndex = dataGridTrainers.Rows.Add(
                        trainer.Id,
                        trainer.FullName,
                        (Gendre)trainer.Gender,
                        (TrainingType)trainer.TrainingType,
                        trainer.Age,
                        trainer.WorkExperience,
                        assignedAthletes);

                    dataGridTrainers.Rows[rowIndex]
                        .Cells[6].ToolTipText = assignedAthletes;
                }

                dataGridTrainers.ClearSelection();

                foreach (DataGridViewRow row in dataGridTrainers.Rows)
                {
                    if (Convert.ToInt32(row.Cells[0].Value)
                        == previousId)
                    {
                        row.Selected = true;
                        break;
                    }
                }

                BindCombo(
                    comboAttachTrainer,
                    trainers,
                    t => t.Id);

                var trainerOptions = new List<TrainerDto>
                {
                    new TrainerDto
                    {
                        Id = 0,
                        FullName = "Не менять тренера"
                    }
                };

                trainerOptions.AddRange(trainers);

                BindCombo(
                    comboAthleteTrainer,
                    trainerOptions,
                    t => t.Id);

                // По умолчанию изменение атлета не меняет тренера.
                comboAthleteTrainer.SelectedIndex = 0;
            }
            finally
            {
                _refreshing = false;
            }

            dataGridTrainers_SelectionChanged(this, EventArgs.Empty);
        }

        public void ShowAthletes(
            IReadOnlyList<AthleteDto> athletes)
        {
            _athletes = athletes;

            int previousId = SelectedAthleteId;

            _refreshing = true;

            try
            {
                dataGridAthletes.Rows.Clear();

                foreach (AthleteDto athlete in athletes)
                {
                    string trainerName =
                        athlete.TrainerId.HasValue
                            ? $"{athlete.TrainerName} " +
                              $"(ID:{athlete.TrainerId.Value})"
                            : "Не прикреплен";

                    dataGridAthletes.Rows.Add(
                        athlete.Id,
                        athlete.FullName,
                        (Gendre)athlete.Gender,
                        athlete.Age,
                        athlete.Height,
                        athlete.Weight,
                        (TrainingType)athlete.TrainingType,
                        trainerName);
                }

                dataGridAthletes.ClearSelection();

                foreach (DataGridViewRow row in dataGridAthletes.Rows)
                {
                    if (Convert.ToInt32(row.Cells[0].Value)
                        == previousId)
                    {
                        row.Selected = true;
                        break;
                    }
                }

                BindCombo(
                    comboAthleteForAttach,
                    athletes,
                    a => a.Id);
            }
            finally
            {
                _refreshing = false;
            }

            dataGridAthletes_SelectionChanged(this, EventArgs.Empty);
        }

        private TrainerDto? GetSelectedTrainer()
        {
            if (dataGridTrainers.SelectedRows.Count == 0)
                return null;

            object? value =
                dataGridTrainers.SelectedRows[0].Cells[0].Value;

            if (value == null)
                return null;

            int id = Convert.ToInt32(value);

            return _trainers.FirstOrDefault(t => t.Id == id);
        }

        private AthleteDto? GetSelectedAthlete()
        {
            if (dataGridAthletes.SelectedRows.Count == 0)
                return null;

            object? value =
                dataGridAthletes.SelectedRows[0].Cells[0].Value;

            if (value == null)
                return null;

            int id = Convert.ToInt32(value);

            return _athletes.FirstOrDefault(a => a.Id == id);
        }

        private void dataGridTrainers_SelectionChanged(
            object sender, EventArgs e)
        {
            if (_refreshing)
                return;

            TrainerDto? trainer = GetSelectedTrainer();

            if (trainer == null)
                return;

            textTrainerName.Text = trainer.FullName;
            comboTrainerGender.SelectedItem =
                (Gendre)trainer.Gender;

            comboTrainerType.SelectedItem =
                (TrainingType)trainer.TrainingType;

            textTrainerAge.Text = trainer.Age.ToString();
            textTrainerExperience.Text =
                trainer.WorkExperience.ToString();
        }

        private void dataGridAthletes_SelectionChanged(
    object sender, EventArgs e)
        {
            if (_refreshing)
                return;

            AthleteDto? athlete = GetSelectedAthlete();

            if (athlete == null)
                return;

            _refreshing = true;

            try
            {
                textAthleteName.Text = athlete.FullName;

                comboAthleteGender.SelectedItem =
                    (Gendre)athlete.Gender;

                UpdateAthleteTrainingTypes(
                    (TrainingType)athlete.TrainingType);

                textAthleteAge.Text = athlete.Age.ToString();
                textAthleteHeight.Text = athlete.Height.ToString();
                textAthleteWeight.Text = athlete.Weight.ToString();

                comboAthleteTrainer.SelectedIndex = 0;
            }
            finally
            {
                _refreshing = false;
            }
        }

        private void ClearTrainerFields()
        {
            textTrainerName.Clear();
            textTrainerAge.Clear();
            textTrainerExperience.Clear();

            if (comboTrainerGender.Items.Count > 0)
                comboTrainerGender.SelectedIndex = 0;

            if (comboTrainerType.Items.Count > 0)
                comboTrainerType.SelectedIndex = 0;
        }

        private void ClearAthleteFields()
        {
            textAthleteName.Clear();
            textAthleteAge.Clear();
            textAthleteHeight.Clear();
            textAthleteWeight.Clear();

            if (comboAthleteGender.Items.Count > 0)
                comboAthleteGender.SelectedIndex = 0;

            if (comboAthleteType.Items.Count > 0)
                comboAthleteType.SelectedIndex = 0;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(
                this,
                message,
                "Информация",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(
                this,
                message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private static bool ReadNumber(
            TextBox textBox,
            string fieldName,
            out int result)
        {
            if (int.TryParse(textBox.Text, out result))
                return true;

            MessageBox.Show(
                $"{fieldName}: введите целое число.");

            return false;
        }

        private void buttonAddTrainer_Click(
            object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textTrainerName.Text))
            {
                ShowError("Введите ФИО тренера.");
                return;
            }

            if (!ReadNumber(textTrainerAge, "Возраст", out int age) ||
                !ReadNumber(
                    textTrainerExperience,
                    "Стаж",
                    out int experience))
            {
                return;
            }

            if (comboTrainerGender.SelectedItem is not Gendre gender ||
                comboTrainerType.SelectedItem is not TrainingType type)
            {
                ShowError("Выберите пол и тип тренировки.");
                return;
            }

            NewTrainer = new TrainerInputDto
            {
                FullName = textTrainerName.Text.Trim(),
                Gender = (int)gender,
                TrainingType = (int)type,
                Age = age,
                WorkExperience = experience
            };

            AddTrainerRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonUpdateTrainer_Click(
            object sender, EventArgs e)
        {
            TrainerDto? trainer = GetSelectedTrainer();

            if (trainer == null)
            {
                ShowError("Выберите тренера.");
                return;
            }

            if (!ReadNumber(textTrainerAge, "Возраст", out int age) ||
                !ReadNumber(
                    textTrainerExperience,
                    "Стаж",
                    out int experience))
            {
                return;
            }

            if (comboTrainerGender.SelectedItem is not Gendre gender ||
                comboTrainerType.SelectedItem is not TrainingType type)
            {
                ShowError("Выберите пол и тип тренировки.");
                return;
            }

            EditedTrainer = new TrainerEditDto
            {
                Id = trainer.Id,
                FullName = textTrainerName.Text.Trim(),
                Gender = (int)gender,
                TrainingType = (int)type,
                Age = age,
                WorkExperience = experience
            };

            UpdateTrainerRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonDeleteTrainer_Click(
            object sender, EventArgs e)
        {
            if (SelectedTrainerId <= 0)
            {
                ShowError("Выберите тренера.");
                return;
            }

            if (MessageBox.Show(
                    this,
                    "Удалить выбранного тренера?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)
                != DialogResult.Yes)
            {
                return;
            }

            DeleteTrainerRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonAddAthlete_Click(
            object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textAthleteName.Text))
            {
                ShowError("Введите ФИО атлета.");
                return;
            }

            if (!ReadNumber(
                    textAthleteAge, "Возраст", out int age) ||
                !ReadNumber(
                    textAthleteHeight, "Рост", out int height) ||
                !ReadNumber(
                    textAthleteWeight, "Вес", out int weight))
            {
                return;
            }

            if (comboAthleteGender.SelectedItem is not Gendre gender ||
                comboAthleteType.SelectedItem is not TrainingType type)
            {
                ShowError("Выберите пол и тип тренировки.");
                return;
            }

            NewAthlete = new AthleteInputDto
            {
                FullName = textAthleteName.Text.Trim(),
                Gender = (int)gender,
                TrainingType = (int)type,
                Age = age,
                Height = height,
                Weight = weight
            };

            AddAthleteRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonUpdateAthlete_Click(
            object sender, EventArgs e)
        {
            AthleteDto? athlete = GetSelectedAthlete();

            if (athlete == null)
            {
                ShowError("Выберите атлета.");
                return;
            }

            if (!ReadNumber(
                    textAthleteAge, "Возраст", out int age) ||
                !ReadNumber(
                    textAthleteHeight, "Рост", out int height) ||
                !ReadNumber(
                    textAthleteWeight, "Вес", out int weight))
            {
                return;
            }

            if (comboAthleteGender.SelectedItem is not Gendre gender ||
                comboAthleteType.SelectedItem is not TrainingType type)
            {
                ShowError("Выберите пол и тип тренировки.");
                return;
            }

            int? trainerId = null;

            if (comboAthleteTrainer.SelectedItem is TrainerDto trainer &&
                trainer.Id > 0)
            {
                trainerId = trainer.Id;
            }

            EditedAthlete = new AthleteEditDto
            {
                Id = athlete.Id,
                FullName = textAthleteName.Text.Trim(),
                Gender = (int)gender,
                TrainingType = (int)type,
                Age = age,
                Height = height,
                Weight = weight,
                TrainerId = trainerId
            };

            UpdateAthleteRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonDeleteAthlete_Click(
            object sender, EventArgs e)
        {
            if (SelectedAthleteId <= 0)
            {
                ShowError("Выберите атлета.");
                return;
            }

            if (MessageBox.Show(
                    this,
                    "Удалить выбранного атлета?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)
                != DialogResult.Yes)
            {
                return;
            }

            DeleteAthleteRequested?.Invoke(this, EventArgs.Empty);
        }

        private bool PrepareTrainerAthleteSelection()
        {
            if (comboAttachTrainer.SelectedItem is not TrainerDto trainer ||
                comboAthleteForAttach.SelectedItem is not AthleteDto athlete)
            {
                ShowError("Выберите тренера и атлета.");
                return false;
            }

            SelectedTrainerAthlete = new TrainerAthleteDto
            {
                TrainerId = trainer.Id,
                AthleteId = athlete.Id
            };

            return true;
        }

        private void buttonAttach_Click(
            object sender, EventArgs e)
        {
            if (!PrepareTrainerAthleteSelection())
                return;

            AttachAthleteRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonDetach_Click(
            object sender, EventArgs e)
        {
            if (!PrepareTrainerAthleteSelection())
                return;

            if (MessageBox.Show(
                    this,
                    "Открепить атлета от тренера?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)
                != DialogResult.Yes)
            {
                return;
            }

            DetachAthleteRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
