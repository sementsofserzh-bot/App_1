using System;
using System.Windows.Forms;
using App_Model_TestLogics;
using App_Model_Essence;

namespace App_WinForms
{
    public partial class MainForm : Form
    {
        private readonly ILogics logics;

        public MainForm(ILogics logics)
        {
            InitializeComponent();
            this.logics = logics;
        }
        
        /// <summary>
        /// Проверяет пароль и при успешной верификации открывает EmployeeForm модально; при неверном пароле отображает
        /// диалог с сообщением об ошибке.
        /// </summary>
        /// <remarks>Пароль проверяется методом CheckPassword; форма создаётся в блоке using и
        /// отображается через ShowDialog().</remarks>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void buttonEmployee_Click(object sender, EventArgs e)
        {
            if (CheckPassword("1234"))
            {
                using EmployeeForm form = new EmployeeForm(logics);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Неверный пароль. Доступ запрещен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Открывает модальную форму UserForm и ожидает её закрытия.
        /// </summary>
        /// <remarks>Форма создаётся в блоке using и автоматически освобождается после закрытия.</remarks>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события клика.</param>
        private void buttonUser_Click(object sender, EventArgs e)
        {
            using UserForm form = new UserForm(logics);
            form.ShowDialog();
        }
        /// <summary>
        /// Закрывает текущее окно (форму).
        /// </summary>
        /// <remarks>Вызов Close() инициирует события FormClosing и FormClosed.</remarks>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Отображает модальное окно запроса пароля и проверяет совпадение введённого текста с заданным правильным
        /// паролем.
        /// </summary>
        /// <remarks>Диалог основан на System.Windows.Forms; ввод в поле скрыт через
        /// UseSystemPasswordChar. Клавиша Enter соответствует кнопке ОК.</remarks>
        /// <param name="correctPassword">Правильный пароль для сравнения.</param>
        /// <returns>true, если пользователь подтвердил ввод (DialogResult.OK) и введённый пароль совпадает с правильным паролем;
        /// иначе false.</returns>
        private bool CheckPassword(string correctPassword)
        {
            using Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Авторизация",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label textLabel = new Label() { Left = 20, Top = 20, Text = "Введите пароль доступа:", AutoSize = true };
            TextBox textBox = new TextBox() { Left = 20, Top = 45, Width = 240, UseSystemPasswordChar = true };
            Button confirmation = new Button() { Text = "ОК", Left = 160, Top = 75, Width = 100, DialogResult = DialogResult.OK };

            prompt.AcceptButton = confirmation;

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);

            return prompt.ShowDialog() == DialogResult.OK && textBox.Text == correctPassword;
        }
    }
}