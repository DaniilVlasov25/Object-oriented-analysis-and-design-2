using System;
using System.Windows.Forms;
using RacingCarTuner.Models;

namespace RacingCarTuner.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeData();

            chkEngine.CheckedChanged += ChkEngine_CheckedChanged;
            chkWheels.CheckedChanged += ChkWheels_CheckedChanged;
            chkArmor.CheckedChanged += ChkArmor_CheckedChanged;
            chkSpeed.CheckedChanged += ChkSpeed_CheckedChanged;
        }
        private void InitializeData()
        {
            cmbCarType.Items.Clear();
            cmbCarType.Items.AddRange(new[] { "Спорт", "Внедорожник", "Седан" });
            cmbCarType.SelectedIndex = 0;

            cmbEngine.Items.Clear();
            cmbEngine.Items.AddRange(new[] { "V6 Турбо", "V8 Атмо", "Электро" });
            cmbEngine.SelectedIndex = 0;

            cmbWheels.Items.Clear();
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

            Engine engine = null;
            if (chkEngine.Checked)
            {
                string engineType = cmbEngine.SelectedItem.ToString();
                int power = 0;
                string engineName = "";

                if (carType == "Спорт")
                {
                    switch (engineType)
                    {
                        case "V8 Атмо":
                            power = 400;
                            engineName = "V8 Спорт";
                            break;
                        case "V6 Турбо":
                            power = 300;
                            engineName = "V6 Спорт";
                            break;
                        case "Электро":
                            power = 250;
                            engineName = "Электро Спорт";
                            break;
                    }
                }
                else if (carType == "Внедорожник")
                {
                    switch (engineType)
                    {
                        case "V8 Атмо":
                            power = 350;
                            engineName = "V8 Внедорожник";
                            break;
                        case "V6 Турбо":
                            power = 300;
                            engineName = "V6 Внедорожник";
                            break;
                        case "Электро":
                            power = 200;
                            engineName = "Электро Внедорожник";
                            break;
                    }
                }
                else
                {
                    switch (engineType)
                    {
                        case "V8 Атмо":
                            power = 400;
                            engineName = "V8 Спорт";
                            break;
                        case "V6 Турбо":
                            power = 300;
                            engineName = "V6 Спорт";
                            break;
                        case "Электро":
                            power = 250;
                            engineName = "Электро Спорт";
                            break;
                    }
                }

                engine = new Engine(power, engineName);
            }

            Wheels[] wheels = null;
            if (chkWheels.Checked)
            {
                string wheelType = cmbWheels.SelectedItem.ToString();
                int grip = 0;
                string wheelName = "";

                if (carType == "Спорт" || carType == "Седан")
                {
                    grip = wheelType == "Слик" ? 95 : 70;
                    wheelName = wheelType + " Спорт";
                }
                else
                {
                    grip = wheelType == "Грязь" ? 85 : 70;
                    wheelName = wheelType + " Внедорожник";
                }

                wheels = new Wheels[4];
                for (int i = 0; i < 4; i++)
                {
                    wheels[i] = new Wheels(wheelName, grip);
                }
            }

            int? armor = chkArmor.Checked ? (int?)numArmor.Value : null;
            int? speed = chkSpeed.Checked ? (int?)numSpeed.Value : null;

            car = new Car(carType, engine, wheels, armor, speed);

            txtResult.Text = car.GetSpecs();
        }
    }
}