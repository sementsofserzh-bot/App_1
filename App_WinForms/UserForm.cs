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
        /// <summary>
        /// Заполняет comboGender и comboTrainingType значениями перечислений Gendre и TrainingType и устанавливает
        /// первый элемент как выбранный при наличии элементов.
        /// </summary>
        /// <remarks>Получает значения перечислений через Enum.GetValues и добавляет их в коллекции Items;
        /// если коллекции не пусты, присваивает SelectedIndex = 0.</remarks>
        private void FillEnums()
        {
            comboGender.Items.Clear();
            comboTrainingType.Items.Clear();

            comboGender.Items.AddRange(Enum.GetValues<Gendre>().Cast<object>().ToArray());
            comboTrainingType.Items.AddRange(Enum.GetValues<TrainingType>().Cast<object>().ToArray());

            if (comboGender.Items.Count > 0) comboGender.SelectedIndex = 0;
            if (comboTrainingType.Items.Count > 0) comboTrainingType.SelectedIndex = 0;
        }
        /// <summary>
        /// Обновляет источники данных и свойства отображения нескольких ComboBox: назначает актуальные списки
        /// спортсменов и тренеров и устанавливает DisplayMember = "FullName".
        /// </summary>
        /// <remarks>Сбрасывает DataSource в null перед повторным присвоением и материализует коллекции
        /// вызовом ToList(), чтобы подгрузить актуальные данные; обновляет comboRegistrationAthlete,
        /// comboRegistrationTrainer, comboPersonalAthlete и comboFilterAthlete.</remarks>
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
        /// <summary>
        /// Обновляет dataGridRating: очищает все строки и заполняет их рейтингом тренеров, полученным из
        /// logics.RateTrainers(), добавляя порядковый номер, идентификатор, ФИО, пол, возраст, стаж, число подопечных и
        /// тип тренировки.
        /// </summary>
        /// <remarks>Должен вызываться из UI‑потока. Порядок и содержимое строк зависят от
        /// logics.RateTrainers(). При отсутствии списка спортсменов учитывается null-safe подсчёт
        /// (Trainer.Athlete?.Count ?? 0).</remarks>
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
        /// <summary>
        /// Обрабатывает нажатие кнопки добавления атлета: проверяет непустое ФИО, парсит возраст, рост и вес, вызывает
        /// логический слой для создания атлета, обновляет списки и очищает поля ввода, отображает результат или ошибку.
        /// </summary>
        /// <remarks>Выполняет валидацию: ФИО не пустое; возраст, рост и вес — целые числа. Приводит
        /// выбранные элементы комбобоксов к перечислениям Gendre и TrainingType, вызывает logics.AddAthlete, обновляет
        /// комбобоксы, показывает сообщение об успешном добавлении и очищает поля; при исключении отображает сообщение
        /// с текстом ошибки.</remarks>
        /// <param name="sender">Источник события (обычно элемент управления, инициировавший клик).</param>
        /// <param name="e">Аргументы события клика.</param>
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

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        /// <summary>
        /// Регистрирует выбранного атлета у выбранного тренера, обновляет элементы управления и отображает сообщение о
        /// результате.
        /// </summary>
        /// <remarks>Если не выбран атлет или тренер — отображает предупреждение. При успешной регистрации
        /// обновляет комбобоксы и рейтинг; при неуспехе — показывает сообщение о возможной причине.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// <summary>
        /// Запускает персональную тренировку для выбранного атлета, при отсутствии выбора отображает предупреждение и
        /// выводит результат в textPersonalResult.
        /// </summary>
        /// <remarks>Берёт выбранного атлета из comboPersonalAthlete и вызывает logics.PersonalTraining;
        /// результат помещается в textPersonalResult. Если атлет не выбран, показывается сообщение с просьбой выбрать
        /// или добавить атлета.</remarks>
        /// <param name="sender">Источник события, инициировавший клик.</param>
        /// <param name="e">Аргументы события Click.</param>
        private void buttonPersonalTraining_Click(object sender, EventArgs e)
        {
            Athlete? athlete = comboPersonalAthlete.SelectedItem as Athlete;

            if (athlete == null)
            {
                MessageBox.Show("Сначала выберите или добавьте атлета.");
                return;
            }

            textPersonalResult.Text = logics.PersonalTraining(athlete);
        }
        /// <summary>
        /// Фильтрует тренеров по выбранному атлету и отображает их в таблице, отсортированных по степени совпадения.
        /// </summary>
        /// <remarks>Если атлет не выбран, отображает сообщение и прекращает выполнение. Очищает
        /// dataGridFilter, получает ранжированный список тренеров через logics.PersonalFilterTrainers(athlete),
        /// вычисляет процент совпадения через logics.CalculateMatchPercentage и добавляет строки с данными тренера и
        /// процентом в таблицу.</remarks>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Athlete? athlete = comboFilterAthlete.SelectedItem as Athlete;

            if (athlete == null)
            {
                MessageBox.Show("Выберите атлета.");
                return;
            }

            dataGridFilter.Rows.Clear();

            var rankedTrainers = logics.PersonalFilterTrainers(athlete);

            foreach (Trainer trainer in rankedTrainers)
            {
                int matchPercent = logics.CalculateMatchPercentage(trainer, athlete);

                dataGridFilter.Rows.Add(
                    trainer.Id,
                    trainer.FullName,
                    trainer.Gendre,
                    trainer.TrainingType,
                    trainer.Age,
                    trainer.WorkExperience,
                    trainer.Athlete?.Count ?? 0,
                    $"{matchPercent}%");
            }
        }
        /// <summary>
        /// Записывает выбранного атлета к выделенному тренеру из таблицы подбора, выполняя проверки и обновляя
        /// интерфейс.
        /// </summary>
        /// <remarks>Проверяет наличие выбранного атлета и выделенной строки таблицы; получает тренера по
        /// идентификатору через логики; при успешной регистрации обновляет комбобоксы, рейтинг и таблицу подбора,
        /// вычисляет процент совместимости и отображает сообщение об успехе; при ошибках отображает соответствующие
        /// MessageBox.</remarks>
        /// <param name="sender">Источник события клика (обычно кнопка).</param>
        /// <param name="e">Аргументы события клика.</param>
        private void buttonSignUpFromFilter_Click(object sender, EventArgs e)
        {
            Athlete? athlete = comboFilterAthlete.SelectedItem as Athlete;

            if (athlete == null)
            {
                MessageBox.Show("Выберите атлета вверху страницы.");
                return;
            }

            if (dataGridFilter.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите тренера из таблицы, к которому хотите записаться.");
                return;
            }

            int trainerId = Convert.ToInt32(dataGridFilter.SelectedRows[0].Cells[0].Value);
            Trainer? trainer = logics.CheckTrainer(trainerId);

            if (trainer == null)
            {
                MessageBox.Show("Тренер не найден.");
                return;
            }

            if (logics.Registration(trainer, athlete))
            {
                RefreshCombos();
                RefreshRating();
                buttonFilter_Click(sender, e);

                int matchPercent = logics.CalculateMatchPercentage(trainer, athlete);
                MessageBox.Show($"Поздравляем! Атлет {athlete.FullName} успешно записан к тренеру {trainer.FullName}!\nСовместимость: {matchPercent}%.", "Успешная запись", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Не удалось записаться. Возможно, атлет уже закреплен за этим тренером.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// Обновляет отображаемый рейтинг, вызывая RefreshRating.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события Click.</param>
        private void buttonRefreshRating_Click(object sender, EventArgs e)
        {
            RefreshRating();
        }
    }
}