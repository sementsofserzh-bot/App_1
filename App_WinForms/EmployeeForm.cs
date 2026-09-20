using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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
        /// <summary>
        /// Заполняет comboTrainerGender и comboAthleteGender значениями перечисления Gendre, а comboTrainerType и
        /// comboAthleteType — значениями TrainingType; при наличии элементов устанавливает первый элемент как
        /// выбранный.
        /// </summary>
        /// <remarks>Очищает существующие элементы перед заполнением. Использует Enum.GetValues и
        /// приведение к object для добавления в Items.</remarks>
        private void FillEnums()
        {
            comboTrainerGender.Items.Clear();
            comboTrainerType.Items.Clear();
            comboAthleteGender.Items.Clear();
            comboAthleteType.Items.Clear();

            comboTrainerGender.Items.AddRange(Enum.GetValues<Gendre>().Cast<object>().ToArray());
            comboTrainerType.Items.AddRange(Enum.GetValues<TrainingType>().Cast<object>().ToArray());
            comboAthleteGender.Items.AddRange(Enum.GetValues<Gendre>().Cast<object>().ToArray());
            comboAthleteType.Items.AddRange(Enum.GetValues<TrainingType>().Cast<object>().ToArray());

            if (comboTrainerGender.Items.Count > 0) comboTrainerGender.SelectedIndex = 0;
            if (comboTrainerType.Items.Count > 0) comboTrainerType.SelectedIndex = 0;
            if (comboAthleteGender.Items.Count > 0) comboAthleteGender.SelectedIndex = 0;
            if (comboAthleteType.Items.Count > 0) comboAthleteType.SelectedIndex = 0;
        }
        /// <summary>
        /// Обновляет тренеров, спортсменов и соответствующие связи между ними.
        /// </summary>
        /// <remarks>Выполняет операции последовательно в порядке: тренеры, спортсмены, комбинации
        /// тренер→спортсмен и комбинации спортсмен→тренер.</remarks>
        private void RefreshAll()
        {
            RefreshTrainers();
            RefreshAthletes();
            RefreshTrainerAthleteCombos();
            RefreshAthleteTrainerCombo();
        }
        /// <summary>
        /// Обновляет dataGridTrainers на основе коллекции logics.BD_Trainer: очищает таблицу, добавляет строки с Id,
        /// FullName, Gendre, TrainingType, Age, WorkExperience и списком закреплённых спортсменов, устанавливает
        /// подсказку для соответствующей ячейки.
        /// </summary>
        /// <remarks>Очищает Rows, включает перенос текста и авторазмер строк (AllCellsExceptHeaders).
        /// Формирует список закреплённых спортсменов как 'FullName (ID:Id)' через ', ' или использует 'Нет
        /// закрепленных' при отсутствии, добавляет строки и задаёт ToolTipText для ячейки с индексом 6.</remarks>
        private void RefreshTrainers()
        {
            dataGridTrainers.Rows.Clear();

            dataGridTrainers.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridTrainers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            foreach (Trainer trainer in logics.BD_Trainer)
            {
                string assignedAthletes = (trainer.Athlete != null && trainer.Athlete.Count > 0)
                    ? string.Join(", ", trainer.Athlete.Select(a => $"{a.FullName} (ID:{a.Id})"))
                    : "Нет закрепленных";

                int rowIndex = dataGridTrainers.Rows.Add(
                    trainer.Id,
                    trainer.FullName,
                    trainer.Gendre,
                    trainer.TrainingType,
                    trainer.Age,
                    trainer.WorkExperience,
                    assignedAthletes);

                dataGridTrainers.Rows[rowIndex].Cells[6].ToolTipText = assignedAthletes;
            }
        }
        /// <summary>
        /// Обновляет содержимое dataGridAthletes на основе коллекции logics.BD_Athlete.
        /// </summary>
        /// <remarks>Очищает существующие строки и добавляет по одной строке на каждого спортсмена с
        /// колонками Id, FullName, Gendre, Age, Height, Weight, TrainingType и отображением тренера в формате «ФИО
        /// (ID:ид)» или «Не прикреплен» при отсутствии тренера.</remarks>
        private void RefreshAthletes()
        {
            dataGridAthletes.Rows.Clear();

            foreach (Athlete athlete in logics.BD_Athlete)
            {
                string trainerName = (athlete.trainer == null)
                    ? "Не прикреплен"
                    : $"{athlete.trainer.FullName} (ID:{athlete.trainer.Id})";

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
        /// <summary>
        /// Обновляет DataSource и DisplayMember для comboAttachTrainer и comboAthleteForAttach, назначая списки
        /// тренеров и спортсменов из logics.
        /// </summary>
        /// <remarks>Сбрасывает DataSource перед повторным присвоением и использует ToList() для загрузки
        /// данных. Не выполняет проверок на null; вызывать из UI‑потока.</remarks>
        private void RefreshTrainerAthleteCombos()
        {
            comboAttachTrainer.DataSource = null;
            comboAttachTrainer.DataSource = logics.BD_Trainer.ToList();
            comboAttachTrainer.DisplayMember = "FullName";

            comboAthleteForAttach.DataSource = null;
            comboAthleteForAttach.DataSource = logics.BD_Athlete.ToList();
            comboAthleteForAttach.DisplayMember = "FullName";
        }
        /// <summary>
        /// Обновляет список тренеров в comboAthleteTrainer: формирует копию logics.BD_Trainer, вставляет в начало
        /// элемент «Не менять тренера» и задаёт DisplayMember 'FullName'.
        /// </summary>
        /// <remarks>Сбрасывает DataSource перед переназначением и использует новый List<Trainer> для
        /// вставки элемента на позицию 0; добавляемый тренер имеет Id = 0 и FullName = 'Не менять тренера'.</remarks>
        private void RefreshAthleteTrainerCombo()
        {
            comboAthleteTrainer.DataSource = null;
            List<Trainer> trainers = new List<Trainer>(logics.BD_Trainer);
            trainers.Insert(0, new Trainer { Id = 0, FullName = "Не менять тренера" });
            comboAthleteTrainer.DataSource = trainers;
            comboAthleteTrainer.DisplayMember = "FullName";
        }
        /// <summary>
        /// Возвращает выбранный в dataGridTrainers объект Trainer после проверки его идентификатора.
        /// </summary>
        /// <remarks>Может выбросить исключение, если значение ячейки идентификатора равно null или не
        /// может быть преобразовано в int.</remarks>
        /// <returns>Объект Trainer или null, если не выбрана ни одна строка или проверка идентификатора не прошла.</returns>
        private Trainer? GetSelectedTrainer()
        {
            if (dataGridTrainers.SelectedRows.Count == 0)
                return null;

            int id = Convert.ToInt32(dataGridTrainers.SelectedRows[0].Cells[0].Value);
            return logics.CheckTrainer(id);
        }
        /// <summary>
        /// Возвращает выбранного спортсмена из dataGridAthletes или null, если не выделена строка или спортсмен не
        /// найден.
        /// </summary>
        /// <remarks>Идентификатор берётся из первой ячейки первой выделенной строки и проверяется через
        /// logics.CheckAthlete(int).</remarks>
        /// <returns>Выбранный Athlete или null.</returns>
        private Athlete? GetSelectedAthlete()
        {
            if (dataGridAthletes.SelectedRows.Count == 0)
                return null;

            int id = Convert.ToInt32(dataGridAthletes.SelectedRows[0].Cells[0].Value);
            return logics.CheckAthlete(id);
        }
        /// <summary>
        /// Очищает текстовые поля имени, возраста и опыта тренера и при наличии элементов сбрасывает выбранный индекс
        /// комбобоксов пола и типа на 0.
        /// </summary>
        /// <remarks>Если комбобоксы не содержат элементов, их выбранный индекс остаётся
        /// неизменным.</remarks>
        private void ClearTrainerFields()
        {
            textTrainerName.Clear();
            textTrainerAge.Clear();
            textTrainerExperience.Clear();
            if (comboTrainerGender.Items.Count > 0) comboTrainerGender.SelectedIndex = 0;
            if (comboTrainerType.Items.Count > 0) comboTrainerType.SelectedIndex = 0;
        }
        /// <summary>
        /// Очищает поля имени, возраста, роста и веса спортсмена и при наличии элементов устанавливает SelectedIndex у
        /// комбобоксов пола и типа в 0.
        /// </summary>
        /// <remarks>Вызывать из UI‑потока; не выполняет валидацию и не изменяет внешние источники
        /// данных.</remarks>
        private void ClearAthleteFields()
        {
            textAthleteName.Clear();
            textAthleteAge.Clear();
            textAthleteHeight.Clear();
            textAthleteWeight.Clear();
            if (comboAthleteGender.Items.Count > 0) comboAthleteGender.SelectedIndex = 0;
            if (comboAthleteType.Items.Count > 0) comboAthleteType.SelectedIndex = 0;
        }
        /// <summary>
        /// Обновляет элементы управления формы значениями выбранного тренера из таблицы, заполняя имя, пол, тип
        /// тренировки, возраст и стаж работы.
        /// </summary>
        /// <remarks>При отсутствии выбранного тренера (GetSelectedTrainer возвращает null) поля остаются
        /// без изменений.</remarks>
        /// <param name="sender">Источник события, обычно компонент DataGrid, инициировавший изменение выбора.</param>
        /// <param name="e">Аргументы события SelectionChanged.</param>
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
        /// <summary>
        /// Обновляет элементы интерфейса — текстовые поля и комбобоксы — значениями выбранного спортсмена.
        /// </summary>
        /// <remarks>Если выбранный спортсмен равен null, обновление не выполняется; числовые значения
        /// приводятся к строкам для отображения.</remarks>
        /// <param name="sender">Источник события, обычно DataGrid, содержащий список спортсменов.</param>
        /// <param name="e">Аргументы события выбора.</param>
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
        /// <summary>
        /// Добавляет тренера на основании введённых данных: проверяет обязательные поля, валидирует и парсит возраст и
        /// стаж, вызывает логику добавления, обновляет интерфейс и отображает результат.
        /// </summary>
        /// <remarks>Отображает сообщения при ошибках валидации и при возникновении исключений; после
        /// успешного добавления обновляет представление и очищает поля ввода.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void buttonAddTrainer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textTrainerName.Text))
            {
                MessageBox.Show("Введите ФИО тренера.");
                return;
            }

            if (!int.TryParse(textTrainerAge.Text, out int age) || !int.TryParse(textTrainerExperience.Text, out int experience))
            {
                MessageBox.Show("Введите возраст и стаж числами.");
                return;
            }

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        /// <summary>
        /// Обновляет информацию выбранного тренера на основе значений полей формы: проверяет наличие выбранного тренера
        /// и корректность числовых полей возраста и стажа, вызывает логику обновления, обновляет представление и
        /// уведомляет пользователя об успехе или ошибке.
        /// </summary>
        /// <remarks>Выводит сообщения при отсутствии выбора тренера или некорректных входных данных;
        /// приводит выбранные элементы комбобоксов к enum Gendre и TrainingType; передаёт null для необязательных
        /// параметров и перехватывает исключения, отображая текст ошибки.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события Click.</param>
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

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        /// <summary>
        /// Обрабатывает нажатие кнопки удаления тренера.
        /// </summary>
        /// <remarks>Если тренер не выбран, отображает сообщение. При успешном удалении вызывает
        /// RefreshAll(), очищает поля тренера и отображает подтверждение.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// <summary>
        /// Обрабатывает событие клика по кнопке добавления атлета: проверяет вводимые поля, создаёт запись атлета через
        /// слой логики, обновляет интерфейс и отображает сообщения об успехе или ошибке.
        /// </summary>
        /// <remarks>Проверяет непустое поле имени и что возраст, рост и вес являются целыми числами; при
        /// некорректных данных отображает MessageBox и прекращает выполнение. При успешной валидации вызывает
        /// logics.AddAthlete, затем RefreshAll(), показывает подтверждающее сообщение и очищает поля ввода. Исключения
        /// при добавлении перехватываются и отображаются через MessageBox.</remarks>
        /// <param name="sender">Отправитель события.</param>
        /// <param name="e">Аргументы события.</param>
        private void buttonAddAthlete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textAthleteName.Text))
            {
                MessageBox.Show("Введите ФИО атлета.");
                return;
            }

            if (!int.TryParse(textAthleteAge.Text, out int age) ||
                !int.TryParse(textAthleteHeight.Text, out int height) ||
                !int.TryParse(textAthleteWeight.Text, out int weight))
            {
                MessageBox.Show("Возраст, рост и вес должны быть числами.");
                return;
            }

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        /// <summary>
        /// Обновляет информацию выбранного атлета по значениям полей формы и выбранного тренера.
        /// </summary>
        /// <remarks>Проверяет наличие выбранного атлета и валидирует возраст, рост и вес как целые числа;
        /// при наличии выбранного тренера использует его, иначе сохраняет текущего тренера атлета. Вызывает
        /// logics.UpdateInfoAthlete, обновляет представление и показывает сообщения об успехе или об ошибке.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        /// <summary>
        /// Удаляет выбранного атлета из хранилища и обновляет интерфейс.
        /// </summary>
        /// <remarks>Если атлет не выбран, отображается сообщение с просьбой выбрать атлета. При успешном
        /// удалении вызывается логика удаления, обновляются данные и очищаются поля формы, затем отображается
        /// подтверждение удаления.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
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
        /// <summary>
        /// Привязывает выбранного атлета к выбранному тренеру и уведомляет пользователя о результате.
        /// </summary>
        /// <remarks>Проверяет, что выбраны тренер и атлет, вызывает logics.Registration(trainer, athlete)
        /// для выполнения привязки, при успехе вызывает RefreshAll() и отображает информационное сообщение; при ошибке
        /// показывает сообщение об неудаче.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события клика.</param>
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
        /// <summary>
        /// Открепляет выбранного атлета от выбранного тренера.
        /// </summary>
        /// <remarks>Проверяет выбор тренера и атлета, снимает связь через logics.UpdateInfoTrainer,
        /// обновляет интерфейс и уведомляет пользователя через MessageBox.</remarks>
        /// <param name="sender">Отправитель события.</param>
        /// <param name="e">Аргументы события.</param>
        private void buttonDetach_Click(object sender, EventArgs e)
        {
            Trainer? trainer = comboAttachTrainer.SelectedItem as Trainer;
            Athlete? athlete = comboAthleteForAttach.SelectedItem as Athlete;

            if (trainer == null || athlete == null)
            {
                MessageBox.Show("Выберите тренера и атлета.");
                return;
            }

            if (trainer.Athlete == null || !trainer.Athlete.Contains(athlete))
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