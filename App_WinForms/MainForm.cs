
using App_Composition;
using App_Model;
using Ninject;

namespace App_WinForms
{
    /// <summary>
    /// Представляет главное окно приложения для выбора роли пользователя
    /// и запуска соответствующего интерфейса.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly IKernel _kernel;
        /// <summary>
        /// Инициализирует главное окно и получает контейнер зависимостей Ninject.
        /// </summary>
        /// <param name="kernel">Контейнер зависимостей приложения.</param>
        public MainForm(IKernel kernel)
        {
            InitializeComponent();
            _kernel = kernel ?? throw new ArgumentNullException(
                nameof(kernel));
        }

        private void buttonEmployee_Click(object sender, EventArgs e)
        {
            if (!CheckPassword("1234"))
            {
                MessageBox.Show(
                    "Неверный пароль. Доступ запрещён.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            using EmployeeForm form = new EmployeeForm();

            var presenter =
                ApplicationServices.CreateEmployeePresenter(
                    _kernel, form);

            presenter.Initialize();

            form.ShowDialog(this);

            GC.KeepAlive(presenter);
        }

        private void buttonUser_Click(object sender, EventArgs e)
        {
            using UserForm form = new UserForm();

            var presenter =
                ApplicationServices.CreateUserPresenter(
                    _kernel, form);

            presenter.Initialize();

            form.ShowDialog(this);

            GC.KeepAlive(presenter);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool CheckPassword(string correctPassword)
        {
            using Form prompt = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Авторизация",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label textLabel = new Label
            {
                Left = 20,
                Top = 20,
                Text = "Введите пароль доступа:",
                AutoSize = true
            };

            TextBox textBox = new TextBox
            {
                Left = 20,
                Top = 45,
                Width = 240,
                UseSystemPasswordChar = true
            };

            Button confirmation = new Button
            {
                Text = "ОК",
                Left = 160,
                Top = 75,
                Width = 100,
                DialogResult = DialogResult.OK
            };

            prompt.AcceptButton = confirmation;

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);

            return prompt.ShowDialog(this) == DialogResult.OK
                && textBox.Text == correctPassword;
        }
    }
}
