using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using App_Model_Essence;
using App_Model_TestLogics;
using App_Model_Logics;

namespace App_WinForms
{
    public partial class Form1 : Form
    {
        private readonly ILogics _logics;
        /// <summary>
        /// Создаёт экземпляр Form1, инициализирует компоненты формы и создаёт экземпляр Logics.
        /// </summary>
        /// <remarks>Вызов InitializeComponent настраивает элементы интерфейса; затем поле _logics
        /// инициализируется новым объектом Logics.</remarks>
        public Form1()
        {
            InitializeComponent();
            _logics = new Logics();
        }
        /// <summary>
        /// Инициализация при загрузке формы; обновляет все данные вызовом RefreshAllData.
        /// </summary>
        /// <remarks>Использует RefreshAllData для подготовки отображаемых данных.</remarks>
        /// <param name="sender">Источник события загрузки.</param>
        /// <param name="e">Аргументы события загрузки формы.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshAllData();
        }
        /// <summary>
        /// Обновляет данные интерфейса: перезагружает таблицы тренеров и атлетов и обновляет связанные элементы
        /// ComboBox.
        /// </summary>
        /// <remarks>Вызывать из UI‑потока. Операция синхронная и не выполняет длительных фоновых
        /// задач.</remarks>
        private void RefreshAllData()
        {
            RefreshTrainersGrid();
            RefreshAthletesGrid();
            RefreshComboBoxes();
        }
        /// <summary>
        /// Обновляет источник данных dgvTrainers, устанавливая проекцию записей из _logics.BD_Trainer с полями Id, ФИО,
        /// Пол, Специализация, Возраст, Стаж_Лет и количеством закреплённых атлетов.
        /// </summary>
        /// <remarks>Очищает DataSource перед присвоением. Если _logics.BD_Trainer равен null или при
        /// возникновении исключения — ошибка игнорируется. Должен вызываться из UI-потока.</remarks>
        private void RefreshTrainersGrid()
        {
            try
            {
                dgvTrainers.DataSource = null;
                var list = _logics.BD_Trainer?.Select(t => new
                {
                    t.Id,
                    ФИО = t.FullName,
                    Пол = t.Gendre,
                    Специализация = t.TrainingType,
                    Возраст = t.Age,
                    Стаж_Лет = t.WorkExperience,
                    Атлетов_Закреплено = t.Athlete?.Count ?? 0
                }).ToList();

                dgvTrainers.DataSource = list;
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Обновляет DataGridView dgvAthletes: формирует и присваивает источник данных со списком спортсменов и
        /// отображаемыми названиями столбцов.
        /// </summary>
        /// <remarks>Проекция выполняется из _logics.BD_Athlete в анонимный тип с русскими заголовками
        /// столбцов; при отсутствии данных DataSource предварительно сбрасывается в null. Исключения перехватываются и
        /// подавляются. Вызывать из UI‑потока.</remarks>
        private void RefreshAthletesGrid()
        {
            try
            {
                dgvAthletes.DataSource = null;
                var list = _logics.BD_Athlete?.Select(a => new
                {
                    a.Id,
                    ФИО = a.FullName,
                    Пол = a.Gendre,
                    Возраст = a.Age,
                    Рост = a.Height,
                    Вес = a.Weight,
                    Тип_Тренировки = a.TrainingType,
                    Тренер = a.trainer != null ? a.trainer.FullName : "Не прикреплен"
                }).ToList();

                dgvAthletes.DataSource = list;
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Обновляет DataSource, DisplayMember и ValueMember для cmbRegAthlete, cmbRegTrainer и cmbBizAthlete на основе
        /// коллекций в _logics.
        /// </summary>
        /// <remarks>Если коллекции отсутствуют, DataSource устанавливается в null. Исключения,
        /// возникающие при обновлении, подавляются.</remarks>
        private void RefreshComboBoxes()
        {
            try
            {
                cmbRegAthlete.DataSource = _logics.BD_Athlete?.ToList();
                cmbRegAthlete.DisplayMember = "FullName";
                cmbRegAthlete.ValueMember = "Id";

                cmbRegTrainer.DataSource = _logics.BD_Trainer?.ToList();
                cmbRegTrainer.DisplayMember = "FullName";
                cmbRegTrainer.ValueMember = "Id";

                cmbBizAthlete.DataSource = _logics.BD_Athlete?.ToList();
                cmbBizAthlete.DisplayMember = "FullName";
                cmbBizAthlete.ValueMember = "Id";
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Открывает модальную форму добавления/редактирования тренера и при подтверждении обновляет отображаемые
        /// данные.
        /// </summary>
        /// <remarks>Форма создаётся в блоке using и автоматически освобождается. При получении
        /// DialogResult.OK вызывается RefreshAllData().</remarks>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void btnAddTrainer_Click(object sender, EventArgs e)
        {
            using (var form = new FormTrainerEdit(_logics))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
        }
        /// <summary>
        /// Открывает форму редактирования выбранного тренера и обновляет отображаемые данные при успешном сохранении.
        /// </summary>
        /// <remarks>Если в таблице нет выбранного ряда, действие не выполняется. Получает идентификатор
        /// тренера, проверяет его через _logics.CheckTrainer и при наличии открывает FormTrainerEdit; при
        /// DialogResult.OK вызывает RefreshAllData(). Исключения логики отображаются через MessageBox.</remarks>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события клика.</param>
        private void btnEditTrainer_Click(object sender, EventArgs e)
        {
            if (dgvTrainers.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvTrainers.CurrentRow.Cells["Id"].Value);

            try
            {
                var trainer = _logics.CheckTrainer(id);
                if (trainer != null)
                {
                    using (var form = new FormTrainerEdit(_logics, trainer))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            RefreshAllData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка логики: {ex.Message}", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// Удаляет выбранного тренера после подтверждения пользователя и обновляет отображаемые данные.
        /// </summary>
        /// <remarks>Если ни одна строка не выбрана, операция прерывается. При подтверждении удаляет
        /// тренера по идентификатору, вызывает RefreshAllData и отображает информационное сообщение при возникновении
        /// исключения логики.</remarks>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void btnDeleteTrainer_Click(object sender, EventArgs e)
        {
            if (dgvTrainers.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvTrainers.CurrentRow.Cells["Id"].Value);

            if (MessageBox.Show("Удалить выбранного тренера?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _logics.RemoveTrainer(id);
                    RefreshAllData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка логики: {ex.Message}", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        /// <summary>
        /// Открывает форму добавления спортсмена и обновляет отображаемые данные при подтверждении.
        /// </summary>
        /// <remarks>Открывает FormAthleteEdit как модальное диалоговое окно; при возврате DialogResult.OK
        /// выполняется RefreshAllData().</remarks>
        /// <param name="sender">Источник события, обычно кнопка, вызвавшая обработчик.</param>
        /// <param name="e">Аргументы события.</param>
        private void btnAddAthlete_Click(object sender, EventArgs e)
        {
            using (var form = new FormAthleteEdit(_logics))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
        }
        /// <summary>
        /// Открывает модальную форму редактирования выбранного атлета и обновляет отображаемые данные при
        /// подтверждении.
        /// </summary>
        /// <remarks>Если в таблице нет выделенной строки, действие не выполняется. Получение сущности
        /// выполняется через _logics.CheckAthlete(id). Открывает FormAthleteEdit модально; при DialogResult.OK
        /// вызывается RefreshAllData(). Исключения логики отображаются пользователю в MessageBox.</remarks>
        /// <param name="sender">Источник события клика.</param>
        /// <param name="e">Аргументы события клика.</param>
        private void btnEditAthlete_Click(object sender, EventArgs e)
        {
            if (dgvAthletes.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvAthletes.CurrentRow.Cells["Id"].Value);

            try
            {
                var athlete = _logics.CheckAthlete(id);
                if (athlete != null)
                {
                    using (var form = new FormAthleteEdit(_logics, athlete))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            RefreshAllData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка логики: {ex.Message}", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// Удаляет выбранного атлета после подтверждения пользователя и обновляет отображаемые данные.
        /// </summary>
        /// <remarks>Если нет выбранной строки — операция отменяется. При подтверждении вызывает слой
        /// логики для удаления и обновляет данные; при ошибке отображает сообщение об ошибке.</remarks>
        /// <param name="sender">Источник события клика.</param>
        /// <param name="e">Аргументы события клика.</param>
        private void btnDeleteAthlete_Click(object sender, EventArgs e)
        {
            if (dgvAthletes.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvAthletes.CurrentRow.Cells["Id"].Value);

            if (MessageBox.Show("Удалить выбранного атлета?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _logics.RemoveAthlete(id);
                    RefreshAllData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка логики: {ex.Message}", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        /// <summary>
        /// Прикрепляет выбранного атлета к выбранному тренеру, отображает результат операции и при успехе обновляет
        /// данные интерфейса.
        /// </summary>
        /// <remarks>Проверяет выбор атлета и тренера и при отсутствии показывает предупреждение. Вызывает
        /// бизнес-логику для регистрации, отображает уведомления об успехе или ошибке и при успешной регистрации
        /// вызывает обновление данных. Перехватывает исключения и отображает сообщение об ошибке.</remarks>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события Click.</param>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            var athlete = cmbRegAthlete.SelectedItem as Athlete;
            var trainer = cmbRegTrainer.SelectedItem as Trainer;

            if (athlete == null || trainer == null)
            {
                MessageBox.Show("Выберите атлета и тренера!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool res = _logics.Registration(trainer, athlete);
                if (res)
                {
                    MessageBox.Show("Атлет успешно прикреплен к тренеру!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshAllData();
                }
                else
                {
                    MessageBox.Show("Не удалось зарегистрировать атлета.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка логики: {ex.Message}", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// Вычисляет персональную тренировку для выбранного спортсмена и отображает результат в txtBizResult; при
        /// ошибке показывает уведомление.
        /// </summary>
        /// <remarks>Если выбранный элемент не является Athlete, метод ничего не выполняет. Исключения
        /// перехватываются и отображаются в MessageBox.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события Click.</param>
        private void btnCalcTraining_Click(object sender, EventArgs e)
        {
            var athlete = cmbBizAthlete.SelectedItem as Athlete;
            if (athlete == null) return;

            try
            {
                string result = _logics.PersonalTraining(athlete);
                txtBizResult.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка логики: {ex.Message}", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// Фильтрует тренеров для выбранного спортсмена и обновляет dgvBizTrainers результатами поиска.
        /// </summary>
        /// <remarks>Если спортсмен не выбран, действие не выполняется. При ошибке бизнес‑логики
        /// отображается MessageBox с сообщением об ошибке.</remarks>
        /// <param name="sender">Источник события (контрол, инициировавший клик).</param>
        /// <param name="e">Аргументы события клика.</param>
        private void btnFilterTrainers_Click(object sender, EventArgs e)
        {
            var athlete = cmbBizAthlete.SelectedItem as Athlete;
            if (athlete == null) return;

            try
            {
                List<Trainer> matchedTrainers = _logics.PersonalFilterTrainers(athlete);
                dgvBizTrainers.DataSource = matchedTrainers?.Select(t => new
                {
                    t.Id,
                    ФИО = t.FullName,
                    Специализация = t.TrainingType,
                    Опыт = t.WorkExperience
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка логики: {ex.Message}", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// Отображает в dgvBizTrainers рейтинг тренеров, полученный из логики, проецируя данные в коллекцию с полями
        /// Место, Id, ФИО, Возраст, Опыт и Атлетов.
        /// </summary>
        /// <remarks>В случае ошибки логики отображает MessageBox с текстом ошибки. Подсчёт числа атлетов
        /// защищён от null с помощью null-conditional оператора.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события нажатия кнопки.</param>
        private void btnShowRating_Click(object sender, EventArgs e)
        {
            try
            {
                List<Trainer> rating = _logics.RateTrainers();
                dgvBizTrainers.DataSource = rating?.Select((t, index) => new
                {
                    Место = index + 1,
                    t.Id,
                    ФИО = t.FullName,
                    Возраст = t.Age,
                    Опыт = t.WorkExperience,
                    Атлетов = t.Athlete?.Count ?? 0
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка логики: {ex.Message}", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// Обновляет все данные, вызывая RefreshAllData.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshAllData();
        }
    }
}