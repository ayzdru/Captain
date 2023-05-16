namespace CaptainDocker.Forms
{
    partial class DockerConnectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockerConnectionForm));
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelHeader = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBoxEnableAuthentication = new System.Windows.Forms.CheckBox();
            this.groupBoxAuthentication = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxBasicAuthenticationPassword = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxBasicAuthenticationUsername = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxCertificatePassword = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonCertificateBrowse = new System.Windows.Forms.Button();
            this.textBoxCertificateFilePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButtonAuthenticationBasicAuth = new System.Windows.Forms.RadioButton();
            this.radioButtonAuthenticationCertificate = new System.Windows.Forms.RadioButton();
            this.radioButtonAuthenticationLocal = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxEngineApiUrl = new System.Windows.Forms.TextBox();
            this.openFileDialogCertificateFile = new System.Windows.Forms.OpenFileDialog();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBoxAuthentication.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(632, 2);
            this.label5.TabIndex = 4;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.buttonCancel.Location = new System.Drawing.Point(491, 14);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(120, 35);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.buttonSave.Location = new System.Drawing.Point(367, 14);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(120, 35);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.buttonSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 542);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(632, 60);
            this.panel2.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 106);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 23);
            this.label3.TabIndex = 11;
            this.label3.Text = "Name:";
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(0, 78);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(632, 2);
            this.label6.TabIndex = 3;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(99, 48);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(96, 23);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description";
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelHeader.Location = new System.Drawing.Point(95, 11);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(308, 46);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Docker Connection";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(146, 104);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(465, 29);
            this.textBoxName.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.labelDescription);
            this.panel1.Controls.Add(this.labelHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 80);
            this.panel1.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::CaptainDocker.Properties.Resources.new_docker_connection;
            this.pictureBox1.Location = new System.Drawing.Point(13, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // checkBoxEnableAuthentication
            // 
            this.checkBoxEnableAuthentication.AutoSize = true;
            this.checkBoxEnableAuthentication.Location = new System.Drawing.Point(146, 166);
            this.checkBoxEnableAuthentication.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxEnableAuthentication.Name = "checkBoxEnableAuthentication";
            this.checkBoxEnableAuthentication.Size = new System.Drawing.Size(201, 27);
            this.checkBoxEnableAuthentication.TabIndex = 20;
            this.checkBoxEnableAuthentication.Text = "Enable Authentication";
            this.checkBoxEnableAuthentication.UseVisualStyleBackColor = true;
            this.checkBoxEnableAuthentication.CheckedChanged += new System.EventHandler(this.CheckBoxEnableAuthentication_CheckedChanged);
            // 
            // groupBoxAuthentication
            // 
            this.groupBoxAuthentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAuthentication.Controls.Add(this.groupBox3);
            this.groupBoxAuthentication.Controls.Add(this.groupBox4);
            this.groupBoxAuthentication.Controls.Add(this.radioButtonAuthenticationBasicAuth);
            this.groupBoxAuthentication.Controls.Add(this.radioButtonAuthenticationCertificate);
            this.groupBoxAuthentication.Controls.Add(this.radioButtonAuthenticationLocal);
            this.groupBoxAuthentication.Enabled = false;
            this.groupBoxAuthentication.Location = new System.Drawing.Point(146, 197);
            this.groupBoxAuthentication.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxAuthentication.Name = "groupBoxAuthentication";
            this.groupBoxAuthentication.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxAuthentication.Size = new System.Drawing.Size(465, 341);
            this.groupBoxAuthentication.TabIndex = 19;
            this.groupBoxAuthentication.TabStop = false;
            this.groupBoxAuthentication.Text = "Authentication";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBoxBasicAuthenticationPassword);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.textBoxBasicAuthenticationUsername);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(7, 227);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(455, 91);
            this.groupBox3.TabIndex = 33;
            this.groupBox3.TabStop = false;
            // 
            // textBoxBasicAuthenticationPassword
            // 
            this.textBoxBasicAuthenticationPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBasicAuthenticationPassword.Location = new System.Drawing.Point(104, 51);
            this.textBoxBasicAuthenticationPassword.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxBasicAuthenticationPassword.Name = "textBoxBasicAuthenticationPassword";
            this.textBoxBasicAuthenticationPassword.Size = new System.Drawing.Size(347, 29);
            this.textBoxBasicAuthenticationPassword.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 54);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 23);
            this.label9.TabIndex = 21;
            this.label9.Text = "Password";
            // 
            // textBoxBasicAuthenticationUsername
            // 
            this.textBoxBasicAuthenticationUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBasicAuthenticationUsername.Location = new System.Drawing.Point(104, 18);
            this.textBoxBasicAuthenticationUsername.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxBasicAuthenticationUsername.Name = "textBoxBasicAuthenticationUsername";
            this.textBoxBasicAuthenticationUsername.Size = new System.Drawing.Size(347, 29);
            this.textBoxBasicAuthenticationUsername.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 23);
            this.label8.TabIndex = 19;
            this.label8.Text = "UserName:";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.textBoxCertificatePassword);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.buttonCertificateBrowse);
            this.groupBox4.Controls.Add(this.textBoxCertificateFilePath);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(6, 78);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(455, 105);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            // 
            // textBoxCertificatePassword
            // 
            this.textBoxCertificatePassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCertificatePassword.Location = new System.Drawing.Point(168, 59);
            this.textBoxCertificatePassword.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxCertificatePassword.Name = "textBoxCertificatePassword";
            this.textBoxCertificatePassword.Size = new System.Drawing.Size(283, 29);
            this.textBoxCertificatePassword.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 62);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 23);
            this.label10.TabIndex = 20;
            this.label10.Text = "Password:";
            // 
            // buttonCertificateBrowse
            // 
            this.buttonCertificateBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCertificateBrowse.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.buttonCertificateBrowse.Location = new System.Drawing.Point(365, 23);
            this.buttonCertificateBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCertificateBrowse.Name = "buttonCertificateBrowse";
            this.buttonCertificateBrowse.Size = new System.Drawing.Size(86, 30);
            this.buttonCertificateBrowse.TabIndex = 19;
            this.buttonCertificateBrowse.Text = "Browse...";
            this.buttonCertificateBrowse.UseVisualStyleBackColor = true;
            this.buttonCertificateBrowse.Click += new System.EventHandler(this.buttonCertificateBrowse_Click);
            // 
            // textBoxCertificateFilePath
            // 
            this.textBoxCertificateFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCertificateFilePath.Location = new System.Drawing.Point(168, 25);
            this.textBoxCertificateFilePath.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxCertificateFilePath.Name = "textBoxCertificateFilePath";
            this.textBoxCertificateFilePath.Size = new System.Drawing.Size(193, 29);
            this.textBoxCertificateFilePath.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 25);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 23);
            this.label4.TabIndex = 17;
            this.label4.Text = "Certificate File Path:";
            // 
            // radioButtonAuthenticationBasicAuth
            // 
            this.radioButtonAuthenticationBasicAuth.AutoSize = true;
            this.radioButtonAuthenticationBasicAuth.Location = new System.Drawing.Point(7, 202);
            this.radioButtonAuthenticationBasicAuth.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAuthenticationBasicAuth.Name = "radioButtonAuthenticationBasicAuth";
            this.radioButtonAuthenticationBasicAuth.Size = new System.Drawing.Size(232, 27);
            this.radioButtonAuthenticationBasicAuth.TabIndex = 31;
            this.radioButtonAuthenticationBasicAuth.Text = "Basic HTTP Authentication";
            this.radioButtonAuthenticationBasicAuth.UseVisualStyleBackColor = true;
            // 
            // radioButtonAuthenticationCertificate
            // 
            this.radioButtonAuthenticationCertificate.AutoSize = true;
            this.radioButtonAuthenticationCertificate.Location = new System.Drawing.Point(7, 53);
            this.radioButtonAuthenticationCertificate.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAuthenticationCertificate.Name = "radioButtonAuthenticationCertificate";
            this.radioButtonAuthenticationCertificate.Size = new System.Drawing.Size(198, 27);
            this.radioButtonAuthenticationCertificate.TabIndex = 30;
            this.radioButtonAuthenticationCertificate.Text = "Certificate Credentials";
            this.radioButtonAuthenticationCertificate.UseVisualStyleBackColor = true;
            // 
            // radioButtonAuthenticationLocal
            // 
            this.radioButtonAuthenticationLocal.AutoSize = true;
            this.radioButtonAuthenticationLocal.Checked = true;
            this.radioButtonAuthenticationLocal.Location = new System.Drawing.Point(7, 22);
            this.radioButtonAuthenticationLocal.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAuthenticationLocal.Name = "radioButtonAuthenticationLocal";
            this.radioButtonAuthenticationLocal.Size = new System.Drawing.Size(301, 27);
            this.radioButtonAuthenticationLocal.TabIndex = 29;
            this.radioButtonAuthenticationLocal.TabStop = true;
            this.radioButtonAuthenticationLocal.Text = "Local (Docker for Windows or MAC)";
            this.radioButtonAuthenticationLocal.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 140);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 23);
            this.label7.TabIndex = 17;
            this.label7.Text = "Engine API URL:";
            // 
            // textBoxEngineApiUrl
            // 
            this.textBoxEngineApiUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEngineApiUrl.Location = new System.Drawing.Point(146, 137);
            this.textBoxEngineApiUrl.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEngineApiUrl.Name = "textBoxEngineApiUrl";
            this.textBoxEngineApiUrl.Size = new System.Drawing.Size(465, 29);
            this.textBoxEngineApiUrl.TabIndex = 18;
            // 
            // openFileDialogCertificateFile
            // 
            this.openFileDialogCertificateFile.DefaultExt = "pfx";
            this.openFileDialogCertificateFile.Filter = "Certificate File|*.pfx|All files|*.*";
            this.openFileDialogCertificateFile.FilterIndex = 0;
            // 
            // DockerConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 602);
            this.Controls.Add(this.checkBoxEnableAuthentication);
            this.Controls.Add(this.groupBoxAuthentication);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxEngineApiUrl);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DockerConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Docker Connection";
            this.Load += new System.EventHandler(this.DockerConnectionForm_Load);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBoxAuthentication.ResumeLayout(false);
            this.groupBoxAuthentication.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxEnableAuthentication;
        private System.Windows.Forms.GroupBox groupBoxAuthentication;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxBasicAuthenticationPassword;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxBasicAuthenticationUsername;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonCertificateBrowse;
        private System.Windows.Forms.TextBox textBoxCertificateFilePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButtonAuthenticationBasicAuth;
        private System.Windows.Forms.RadioButton radioButtonAuthenticationCertificate;
        private System.Windows.Forms.RadioButton radioButtonAuthenticationLocal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxEngineApiUrl;
        private System.Windows.Forms.TextBox textBoxCertificatePassword;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.OpenFileDialog openFileDialogCertificateFile;
    }
}