namespace RacingCarTuner.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.lblCarType = new System.Windows.Forms.Label();
            this.cmbCarType = new System.Windows.Forms.ComboBox();
            this.pnlCustomOptions = new System.Windows.Forms.Panel();
            this.lblEngine = new System.Windows.Forms.Label();
            this.chkEngine = new System.Windows.Forms.CheckBox();
            this.cmbEngine = new System.Windows.Forms.ComboBox();
            this.lblWheels = new System.Windows.Forms.Label();
            this.chkWheels = new System.Windows.Forms.CheckBox();
            this.cmbWheels = new System.Windows.Forms.ComboBox();
            this.lblArmor = new System.Windows.Forms.Label();
            this.chkArmor = new System.Windows.Forms.CheckBox();
            this.numArmor = new System.Windows.Forms.NumericUpDown();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.chkSpeed = new System.Windows.Forms.CheckBox();
            this.numSpeed = new System.Windows.Forms.NumericUpDown();
            this.btnBuild = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.pnlCustomOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArmor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCarType
            // 
            this.lblCarType.AutoSize = true;
            this.lblCarType.Location = new System.Drawing.Point(12, 15);
            this.lblCarType.Name = "lblCarType";
            this.lblCarType.Size = new System.Drawing.Size(80, 15);
            this.lblCarType.Text = "Тип автомобиля:";
            // 
            // cmbCarType
            // 
            this.cmbCarType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCarType.Location = new System.Drawing.Point(12, 33);
            this.cmbCarType.Name = "cmbCarType";
            this.cmbCarType.Size = new System.Drawing.Size(200, 23);
            // 
            // pnlCustomOptions
            // 
            this.pnlCustomOptions.Controls.Add(this.lblEngine);
            this.pnlCustomOptions.Controls.Add(this.chkEngine);
            this.pnlCustomOptions.Controls.Add(this.cmbEngine);
            this.pnlCustomOptions.Controls.Add(this.lblWheels);
            this.pnlCustomOptions.Controls.Add(this.chkWheels);
            this.pnlCustomOptions.Controls.Add(this.cmbWheels);
            this.pnlCustomOptions.Controls.Add(this.lblArmor);
            this.pnlCustomOptions.Controls.Add(this.chkArmor);
            this.pnlCustomOptions.Controls.Add(this.numArmor);
            this.pnlCustomOptions.Controls.Add(this.lblSpeed);
            this.pnlCustomOptions.Controls.Add(this.chkSpeed);
            this.pnlCustomOptions.Controls.Add(this.numSpeed);
            this.pnlCustomOptions.Location = new System.Drawing.Point(12, 65);
            this.pnlCustomOptions.Name = "pnlCustomOptions";
            this.pnlCustomOptions.Size = new System.Drawing.Size(200, 280);
            this.pnlCustomOptions.TabIndex = 0;
            // 
            // lblEngine
            // 
            this.lblEngine.AutoSize = true;
            this.lblEngine.Location = new System.Drawing.Point(3, 5);
            this.lblEngine.Name = "lblEngine";
            this.lblEngine.Size = new System.Drawing.Size(60, 15);
            this.lblEngine.Text = "Двигатель:";
            // 
            // chkEngine
            // 
            this.chkEngine.AutoSize = true;
            this.chkEngine.Location = new System.Drawing.Point(3, 23);
            this.chkEngine.Name = "chkEngine";
            this.chkEngine.Size = new System.Drawing.Size(100, 19);
            this.chkEngine.Text = "Установить";
            // 
            // cmbEngine
            // 
            this.cmbEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEngine.Enabled = false;
            this.cmbEngine.Location = new System.Drawing.Point(3, 45);
            this.cmbEngine.Name = "cmbEngine";
            this.cmbEngine.Size = new System.Drawing.Size(190, 23);
            // 
            // lblWheels
            // 
            this.lblWheels.AutoSize = true;
            this.lblWheels.Location = new System.Drawing.Point(3, 75);
            this.lblWheels.Name = "lblWheels";
            this.lblWheels.Size = new System.Drawing.Size(50, 15);
            this.lblWheels.Text = "Колёса:";
            // 
            // chkWheels
            // 
            this.chkWheels.AutoSize = true;
            this.chkWheels.Location = new System.Drawing.Point(3, 93);
            this.chkWheels.Name = "chkWheels";
            this.chkWheels.Size = new System.Drawing.Size(100, 19);
            this.chkWheels.Text = "Установить";
            // 
            // cmbWheels
            // 
            this.cmbWheels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWheels.Enabled = false;
            this.cmbWheels.Location = new System.Drawing.Point(3, 115);
            this.cmbWheels.Name = "cmbWheels";
            this.cmbWheels.Size = new System.Drawing.Size(190, 23);
            // 
            // lblArmor
            // 
            this.lblArmor.AutoSize = true;
            this.lblArmor.Location = new System.Drawing.Point(3, 145);
            this.lblArmor.Name = "lblArmor";
            this.lblArmor.Size = new System.Drawing.Size(50, 15);
            this.lblArmor.Text = "Броня:";
            // 
            // chkArmor
            // 
            this.chkArmor.AutoSize = true;
            this.chkArmor.Location = new System.Drawing.Point(3, 163);
            this.chkArmor.Name = "chkArmor";
            this.chkArmor.Size = new System.Drawing.Size(100, 19);
            this.chkArmor.Text = "Установить";
            // 
            // numArmor
            // 
            this.numArmor.Enabled = false;
            this.numArmor.Location = new System.Drawing.Point(3, 185);
            this.numArmor.Name = "numArmor";
            this.numArmor.Size = new System.Drawing.Size(190, 23);
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(3, 215);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(60, 15);
            this.lblSpeed.Text = "Скорость:";
            // 
            // chkSpeed
            // 
            this.chkSpeed.AutoSize = true;
            this.chkSpeed.Location = new System.Drawing.Point(3, 233);
            this.chkSpeed.Name = "chkSpeed";
            this.chkSpeed.Size = new System.Drawing.Size(100, 19);
            this.chkSpeed.Text = "Установить";
            // 
            // numSpeed
            // 
            this.numSpeed.Enabled = false;
            this.numSpeed.Location = new System.Drawing.Point(3, 255);
            this.numSpeed.Name = "numSpeed";
            this.numSpeed.Size = new System.Drawing.Size(190, 23);
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(12, 355);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(200, 30);
            this.btnBuild.Text = "Собрать автомобиль";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(12, 395);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(200, 150);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 560);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.pnlCustomOptions);
            this.Controls.Add(this.cmbCarType);
            this.Controls.Add(this.lblCarType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Racing Car Tuner (Без паттерна)";
            this.pnlCustomOptions.ResumeLayout(false);
            this.pnlCustomOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArmor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblCarType;
        private System.Windows.Forms.ComboBox cmbCarType;
        private System.Windows.Forms.Panel pnlCustomOptions;
        private System.Windows.Forms.Label lblEngine;
        private System.Windows.Forms.CheckBox chkEngine;
        private System.Windows.Forms.ComboBox cmbEngine;
        private System.Windows.Forms.Label lblWheels;
        private System.Windows.Forms.CheckBox chkWheels;
        private System.Windows.Forms.ComboBox cmbWheels;
        private System.Windows.Forms.Label lblArmor;
        private System.Windows.Forms.CheckBox chkArmor;
        private System.Windows.Forms.NumericUpDown numArmor;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.CheckBox chkSpeed;
        private System.Windows.Forms.NumericUpDown numSpeed;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.TextBox txtResult;
    }
}