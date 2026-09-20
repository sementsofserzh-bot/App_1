namespace App_WinForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabTrainers;
        private System.Windows.Forms.TabPage tabAthletes;
        private System.Windows.Forms.TabPage tabRegistration;
        private System.Windows.Forms.TabPage tabBusiness;

        private System.Windows.Forms.DataGridView dgvTrainers;
        private System.Windows.Forms.Button btnAddTrainer;
        private System.Windows.Forms.Button btnEditTrainer;
        private System.Windows.Forms.Button btnDeleteTrainer;

        private System.Windows.Forms.DataGridView dgvAthletes;
        private System.Windows.Forms.Button btnAddAthlete;
        private System.Windows.Forms.Button btnEditAthlete;
        private System.Windows.Forms.Button btnDeleteAthlete;

        private System.Windows.Forms.ComboBox cmbRegAthlete;
        private System.Windows.Forms.ComboBox cmbRegTrainer;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label lblRegAthlete;
        private System.Windows.Forms.Label lblRegTrainer;

        private System.Windows.Forms.ComboBox cmbBizAthlete;
        private System.Windows.Forms.Label lblBizAthlete;
        private System.Windows.Forms.Button btnCalcTraining;
        private System.Windows.Forms.Button btnFilterTrainers;
        private System.Windows.Forms.Button btnShowRating;
        private System.Windows.Forms.TextBox txtBizResult;
        private System.Windows.Forms.DataGridView dgvBizTrainers;
        private System.Windows.Forms.Button btnRefresh;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabTrainers = new System.Windows.Forms.TabPage();
            this.dgvTrainers = new System.Windows.Forms.DataGridView();
            this.btnAddTrainer = new System.Windows.Forms.Button();
            this.btnEditTrainer = new System.Windows.Forms.Button();
            this.btnDeleteTrainer = new System.Windows.Forms.Button();

            this.tabAthletes = new System.Windows.Forms.TabPage();
            this.dgvAthletes = new System.Windows.Forms.DataGridView();
            this.btnAddAthlete = new System.Windows.Forms.Button();
            this.btnEditAthlete = new System.Windows.Forms.Button();
            this.btnDeleteAthlete = new System.Windows.Forms.Button();

            this.tabRegistration = new System.Windows.Forms.TabPage();
            this.lblRegAthlete = new System.Windows.Forms.Label();
            this.cmbRegAthlete = new System.Windows.Forms.ComboBox();
            this.lblRegTrainer = new System.Windows.Forms.Label();
            this.cmbRegTrainer = new System.Windows.Forms.ComboBox();
            this.btnRegister = new System.Windows.Forms.Button();

            this.tabBusiness = new System.Windows.Forms.TabPage();
            this.lblBizAthlete = new System.Windows.Forms.Label();
            this.cmbBizAthlete = new System.Windows.Forms.ComboBox();
            this.btnCalcTraining = new System.Windows.Forms.Button();
            this.btnFilterTrainers = new System.Windows.Forms.Button();
            this.btnShowRating = new System.Windows.Forms.Button();
            this.txtBizResult = new System.Windows.Forms.TextBox();
            this.dgvBizTrainers = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();

            this.tabControlMain.SuspendLayout();
            this.tabTrainers.SuspendLayout();
            this.tabAthletes.SuspendLayout();
            this.tabRegistration.SuspendLayout();
            this.tabBusiness.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrainers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAthletes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBizTrainers)).BeginInit();
            this.SuspendLayout();

            this.tabControlMain.Controls.Add(this.tabTrainers);
            this.tabControlMain.Controls.Add(this.tabAthletes);
            this.tabControlMain.Controls.Add(this.tabRegistration);
            this.tabControlMain.Controls.Add(this.tabBusiness);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(884, 511);

            this.tabTrainers.Controls.Add(this.dgvTrainers);
            this.tabTrainers.Controls.Add(this.btnAddTrainer);
            this.tabTrainers.Controls.Add(this.btnEditTrainer);
            this.tabTrainers.Controls.Add(this.btnDeleteTrainer);
            this.tabTrainers.Text = "Тренеры";

            this.dgvTrainers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrainers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrainers.Location = new System.Drawing.Point(12, 12);
            this.dgvTrainers.MultiSelect = false;
            this.dgvTrainers.Name = "dgvTrainers";
            this.dgvTrainers.ReadOnly = true;
            this.dgvTrainers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrainers.Size = new System.Drawing.Size(852, 390);

            this.btnAddTrainer.Location = new System.Drawing.Point(12, 415);
            this.btnAddTrainer.Size = new System.Drawing.Size(150, 40);
            this.btnAddTrainer.Text = "Добавить тренера";
            this.btnAddTrainer.Click += new System.EventHandler(this.btnAddTrainer_Click);

            this.btnEditTrainer.Location = new System.Drawing.Point(175, 415);
            this.btnEditTrainer.Size = new System.Drawing.Size(150, 40);
            this.btnEditTrainer.Text = "Редактировать";
            this.btnEditTrainer.Click += new System.EventHandler(this.btnEditTrainer_Click);

            this.btnDeleteTrainer.Location = new System.Drawing.Point(338, 415);
            this.btnDeleteTrainer.Size = new System.Drawing.Size(150, 40);
            this.btnDeleteTrainer.Text = "Удалить";
            this.btnDeleteTrainer.Click += new System.EventHandler(this.btnDeleteTrainer_Click);

            this.tabAthletes.Controls.Add(this.dgvAthletes);
            this.tabAthletes.Controls.Add(this.btnAddAthlete);
            this.tabAthletes.Controls.Add(this.btnEditAthlete);
            this.tabAthletes.Controls.Add(this.btnDeleteAthlete);
            this.tabAthletes.Text = "Атлеты";

            this.dgvAthletes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAthletes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAthletes.Location = new System.Drawing.Point(12, 12);
            this.dgvAthletes.MultiSelect = false;
            this.dgvAthletes.Name = "dgvAthletes";
            this.dgvAthletes.ReadOnly = true;
            this.dgvAthletes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAthletes.Size = new System.Drawing.Size(852, 390);

            this.btnAddAthlete.Location = new System.Drawing.Point(12, 415);
            this.btnAddAthlete.Size = new System.Drawing.Size(150, 40);
            this.btnAddAthlete.Text = "Добавить атлета";
            this.btnAddAthlete.Click += new System.EventHandler(this.btnAddAthlete_Click);

            this.btnEditAthlete.Location = new System.Drawing.Point(175, 415);
            this.btnEditAthlete.Size = new System.Drawing.Size(150, 40);
            this.btnEditAthlete.Text = "Редактировать";
            this.btnEditAthlete.Click += new System.EventHandler(this.btnEditAthlete_Click);

            this.btnDeleteAthlete.Location = new System.Drawing.Point(338, 415);
            this.btnDeleteAthlete.Size = new System.Drawing.Size(150, 40);
            this.btnDeleteAthlete.Text = "Удалить";
            this.btnDeleteAthlete.Click += new System.EventHandler(this.btnDeleteAthlete_Click);

            this.tabRegistration.Controls.Add(this.lblRegAthlete);
            this.tabRegistration.Controls.Add(this.cmbRegAthlete);
            this.tabRegistration.Controls.Add(this.lblRegTrainer);
            this.tabRegistration.Controls.Add(this.cmbRegTrainer);
            this.tabRegistration.Controls.Add(this.btnRegister);
            this.tabRegistration.Text = "Закрепление";

            this.lblRegAthlete.Location = new System.Drawing.Point(30, 40);
            this.lblRegAthlete.Text = "Выберите атлета:";
            this.lblRegAthlete.AutoSize = true;

            this.cmbRegAthlete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegAthlete.Location = new System.Drawing.Point(30, 65);
            this.cmbRegAthlete.Size = new System.Drawing.Size(300, 25);

            this.lblRegTrainer.Location = new System.Drawing.Point(30, 120);
            this.lblRegTrainer.Text = "Выберите тренера:";
            this.lblRegTrainer.AutoSize = true;

            this.cmbRegTrainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegTrainer.Location = new System.Drawing.Point(30, 145);
            this.cmbRegTrainer.Size = new System.Drawing.Size(300, 25);

            this.btnRegister.Location = new System.Drawing.Point(30, 200);
            this.btnRegister.Size = new System.Drawing.Size(300, 45);
            this.btnRegister.Text = "Зарегистрировать за тренером";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

            this.tabBusiness.Controls.Add(this.lblBizAthlete);
            this.tabBusiness.Controls.Add(this.cmbBizAthlete);
            this.tabBusiness.Controls.Add(this.btnCalcTraining);
            this.tabBusiness.Controls.Add(this.btnFilterTrainers);
            this.tabBusiness.Controls.Add(this.btnShowRating);
            this.tabBusiness.Controls.Add(this.txtBizResult);
            this.tabBusiness.Controls.Add(this.dgvBizTrainers);
            this.tabBusiness.Text = "Бизнес-функции";

            this.lblBizAthlete.Location = new System.Drawing.Point(20, 20);
            this.lblBizAthlete.Text = "Атлет:";
            this.lblBizAthlete.AutoSize = true;

            this.cmbBizAthlete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBizAthlete.Location = new System.Drawing.Point(70, 17);
            this.cmbBizAthlete.Size = new System.Drawing.Size(250, 25);

            this.btnCalcTraining.Location = new System.Drawing.Point(340, 14);
            this.btnCalcTraining.Size = new System.Drawing.Size(160, 30);
            this.btnCalcTraining.Text = "Подбор тренировки";
            this.btnCalcTraining.Click += new System.EventHandler(this.btnCalcTraining_Click);

            this.btnFilterTrainers.Location = new System.Drawing.Point(510, 14);
            this.btnFilterTrainers.Size = new System.Drawing.Size(160, 30);
            this.btnFilterTrainers.Text = "Подбор тренеров";
            this.btnFilterTrainers.Click += new System.EventHandler(this.btnFilterTrainers_Click);

            this.btnShowRating.Location = new System.Drawing.Point(680, 14);
            this.btnShowRating.Size = new System.Drawing.Size(170, 30);
            this.btnShowRating.Text = "Рейтинг тренеров";
            this.btnShowRating.Click += new System.EventHandler(this.btnShowRating_Click);

            this.txtBizResult.Location = new System.Drawing.Point(20, 60);
            this.txtBizResult.Multiline = true;
            this.txtBizResult.ReadOnly = true;
            this.txtBizResult.Size = new System.Drawing.Size(830, 60);

            this.dgvBizTrainers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBizTrainers.Location = new System.Drawing.Point(20, 135);
            this.dgvBizTrainers.ReadOnly = true;
            this.dgvBizTrainers.Size = new System.Drawing.Size(830, 320);

            this.ClientSize = new System.Drawing.Size(884, 511);
            this.Controls.Add(this.tabControlMain);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PRIME TIME — Система Управления Фитнес-Клубом";
            this.Load += new System.EventHandler(this.Form1_Load);

            this.tabControlMain.ResumeLayout(false);
            this.tabTrainers.ResumeLayout(false);
            this.tabAthletes.ResumeLayout(false);
            this.tabRegistration.ResumeLayout(false);
            this.tabBusiness.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrainers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAthletes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBizTrainers)).EndInit();
            this.ResumeLayout(false);
        }
    }
}