using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using App_Model_Essence;
using App_Model_TestLogics;

namespace App_WinForms
{
    public partial class Form1 : Form
    {
        private readonly ILogics _logics;

        public Form1()
        {
            InitializeComponent();
            _logics = new Logics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshAllData();
        }

        // Обновление всех списков и таблиц
        private void RefreshAllData()
        {
            RefreshTrainersGrid();
            RefreshAthletesGrid();
            RefreshComboBoxes();
        }

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
                // Игнорируем или выводим ошибку, если BD_Trainer равен null
            }
        }

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

        private void RefreshComboBoxes()
        {
            try
            {
                // Настройка выпадающих списков для регистрации
                cmbRegAthlete.DataSource = _logics.BD_Athlete?.ToList();
                cmbRegAthlete.DisplayMember = "FullName";
                cmbRegAthlete.ValueMember = "Id";

                cmbRegTrainer.DataSource = _logics.BD_Trainer?.ToList();
                cmbRegTrainer.DisplayMember = "FullName";
                cmbRegTrainer.ValueMember = "Id";

                // Настройка списков для бизнес-функций
                cmbBizAthlete.DataSource = _logics.BD_Athlete?.ToList();
                cmbBizAthlete.DisplayMember = "FullName";
                cmbBizAthlete.ValueMember = "Id";
            }
            catch (Exception ex)
            {
            }
        }

        // === ТРЕНЕРЫ ===
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

        // === АТЛЕТЫ ===
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

        // === ЗАКРЕПЛЕНИЕ ===
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

        // === БИЗНЕС-ФУНКЦИИ ===
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshAllData();
        }
    }
}