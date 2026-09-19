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

        private void buttonEmployee_Click(object sender, EventArgs e)
        {
            // Вызываем проверку пароля. Укажи нужный пароль вместо "admin"
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

        private void buttonUser_Click(object sender, EventArgs e)
        {
            using UserForm form = new UserForm(logics);
            form.ShowDialog();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Вспомогательный метод для программного создания окошка ввода пароля
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
            TextBox textBox = new TextBox() { Left = 20, Top = 45, Width = 240, UseSystemPasswordChar = true }; // Скрывает символы
            Button confirmation = new Button() { Text = "ОК", Left = 160, Top = 75, Width = 100, DialogResult = DialogResult.OK };

            // Если нажать Enter, сработает кнопка ОК
            prompt.AcceptButton = confirmation;

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);

            return prompt.ShowDialog() == DialogResult.OK && textBox.Text == correctPassword;
        }
    }
}