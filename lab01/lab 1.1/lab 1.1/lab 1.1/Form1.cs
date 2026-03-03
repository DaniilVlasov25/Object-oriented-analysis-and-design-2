using System;
using System.Windows.Forms;
using RacingCarTuner.Models;

namespace RacingCarTuner.Forms
{
    public partial class MainForm : Form
    {
        private Player player;
        private Garage garage;

        public MainForm()
        {
            InitializeComponent();
            InitializeData();

            garage = new Garage();
            player = new Player(garage);

            chkEngine.CheckedChanged += ChkEngine_CheckedChanged;
            chkWheels.CheckedChanged += ChkWheels_CheckedChanged;
            chkArmor.CheckedChanged += ChkArmor_CheckedChanged;
            chkSpeed.CheckedChanged += ChkSpeed_CheckedChanged;
        }

        private void InitializeData()
        {
            cmbCarType.Items.AddRange(new[] { "Спорт", "Внедорожник", "Седан" });
            cmbCarType.SelectedIndex = 0;

            cmbEngine.Items.AddRange(new[] { "V6 Турбо", "V8 Атмо", "Электро" });
            cmbEngine.SelectedIndex = 0;

            cmbWheels.Items.AddRange(new[] { "Слик", "Грязь", "Универсал" });
            cmbWheels.SelectedIndex = 0;

            numArmor.Minimum = 0;
            numArmor.Maximum = 100;
            numArmor.Value = 50;

            numSpeed.Minimum = 100;
            numSpeed.Maximum = 400;
            numSpeed.Value = 200;

            chkEngine.Checked = false;
            chkWheels.Checked = false;
            chkArmor.Checked = false;
            chkSpeed.Checked = false;

            cmbEngine.Enabled = false;
            cmbWheels.Enabled = false;
            numArmor.Enabled = false;
            numSpeed.Enabled = false;

            pnlCustomOptions.Visible = true;
        }

        private void ChkEngine_CheckedChanged(object sender, EventArgs e)
        {
            cmbEngine.Enabled = chkEngine.Checked;
        }

        private void ChkWheels_CheckedChanged(object sender, EventArgs e)
        {
            cmbWheels.Enabled = chkWheels.Checked;
        }

        private void ChkArmor_CheckedChanged(object sender, EventArgs e)
        {
            numArmor.Enabled = chkArmor.Checked;
        }

        private void ChkSpeed_CheckedChanged(object sender, EventArgs e)
        {
            numSpeed.Enabled = chkSpeed.Checked;
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            string carType = cmbCarType.SelectedItem.ToString();
            Car car;

            string selectedEngine = chkEngine.Checked ? cmbEngine.SelectedItem.ToString() : null;
            string selectedWheels = chkWheels.Checked ? cmbWheels.SelectedItem.ToString() : null;
            int? selectedArmor = chkArmor.Checked ? (int?)numArmor.Value : null;
            int? selectedSpeed = chkSpeed.Checked ? (int?)numSpeed.Value : null;

            CarBuilder builder;
            if (carType == "Спорт")
            {
                builder = new SportsCarBuilder();
            }
            else if (carType == "Внедорожник")
            {
                builder = new OffroadCarBuilder();
            }
            else
            {
                builder = new SportsCarBuilder();
            }

            builder.Reset();
            builder.SetBody(carType);

            if (selectedEngine != null)
                builder.SetEngine(selectedEngine);

            if (selectedWheels != null)
                builder.SetWheels(selectedWheels);

            if (selectedArmor != null)
                builder.SetArmor(selectedArmor);

            if (selectedSpeed != null)
                builder.SetSpeed(selectedSpeed);

            car = builder.GetProduct();

            txtResult.Text = car.GetSpecs();
        }
    }
}