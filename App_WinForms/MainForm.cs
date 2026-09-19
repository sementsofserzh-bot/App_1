using App_Model_TestLogics;

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
            using EmployeeForm form = new EmployeeForm(logics);
            form.ShowDialog();
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
    }
}
