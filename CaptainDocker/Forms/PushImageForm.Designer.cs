namespace CaptainDocker.Forms
{
    partial class PushImageForm
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
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonFinish = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxRegistry = new System.Windows.Forms.ComboBox();
            this.comboBoxDockerEngine = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxRepository = new System.Windows.Forms.ComboBox();
            this.comboBoxTag = new System.Windows.Forms.ComboBox();
            this.comboBoxImage = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(632, 2);
            this.label5.TabIndex = 4;
            this.label5.Text = "label5";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.buttonCancel.Location = new System.Drawing.Point(474, 9);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(148, 30);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonFinish
            // 
            this.buttonFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFinish.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.buttonFinish.Location = new System.Drawing.Point(321, 9);
            this.buttonFinish.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(148, 30);
            this.buttonFinish.TabIndex = 1;
            this.buttonFinish.Text = "Finish";
            this.buttonFinish.UseVisualStyleBackColor = true;
            this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.buttonFinish);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 317);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(632, 50);
            this.panel2.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 216);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "Tag:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "Repository:";
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(0, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(632, 2);
            this.label6.TabIndex = 3;
            this.label6.Text = "label6";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Red;
            this.pictureBox1.Location = new System.Drawing.Point(15, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 60);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(331, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(101, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Push Image";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 80);
            this.panel1.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 159);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 16);
            this.label7.TabIndex = 16;
            this.label7.Text = "Registry:";
            // 
            // comboBoxRegistry
            // 
            this.comboBoxRegistry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRegistry.DisplayMember = "Text";
            this.comboBoxRegistry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRegistry.FormattingEnabled = true;
            this.comboBoxRegistry.Location = new System.Drawing.Point(140, 157);
            this.comboBoxRegistry.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxRegistry.Name = "comboBoxRegistry";
            this.comboBoxRegistry.Size = new System.Drawing.Size(480, 24);
            this.comboBoxRegistry.TabIndex = 17;
            this.comboBoxRegistry.ValueMember = "Value";
            this.comboBoxRegistry.SelectedIndexChanged += new System.EventHandler(this.ComboBoxRegistry_SelectedIndexChanged);
            // 
            // comboBoxDockerEngine
            // 
            this.comboBoxDockerEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDockerEngine.DisplayMember = "Text";
            this.comboBoxDockerEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDockerEngine.FormattingEnabled = true;
            this.comboBoxDockerEngine.Location = new System.Drawing.Point(140, 101);
            this.comboBoxDockerEngine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxDockerEngine.Name = "comboBoxDockerEngine";
            this.comboBoxDockerEngine.Size = new System.Drawing.Size(480, 24);
            this.comboBoxDockerEngine.TabIndex = 21;
            this.comboBoxDockerEngine.ValueMember = "Value";
            this.comboBoxDockerEngine.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDockerEngine_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 16);
            this.label9.TabIndex = 20;
            this.label9.Text = "Docker Engine:";
            // 
            // comboBoxRepository
            // 
            this.comboBoxRepository.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRepository.DisplayMember = "Text";
            this.comboBoxRepository.FormattingEnabled = true;
            this.comboBoxRepository.Location = new System.Drawing.Point(140, 185);
            this.comboBoxRepository.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxRepository.Name = "comboBoxRepository";
            this.comboBoxRepository.Size = new System.Drawing.Size(480, 24);
            this.comboBoxRepository.TabIndex = 22;
            this.comboBoxRepository.ValueMember = "Value";
            this.comboBoxRepository.SelectedIndexChanged += new System.EventHandler(this.ComboBoxRepository_SelectedIndexChanged);
            // 
            // comboBoxTag
            // 
            this.comboBoxTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTag.DisplayMember = "Text";
            this.comboBoxTag.FormattingEnabled = true;
            this.comboBoxTag.Location = new System.Drawing.Point(140, 213);
            this.comboBoxTag.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxTag.Name = "comboBoxTag";
            this.comboBoxTag.Size = new System.Drawing.Size(480, 24);
            this.comboBoxTag.TabIndex = 23;
            this.comboBoxTag.ValueMember = "Value";
            // 
            // comboBoxImage
            // 
            this.comboBoxImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxImage.DisplayMember = "Text";
            this.comboBoxImage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxImage.FormattingEnabled = true;
            this.comboBoxImage.Location = new System.Drawing.Point(140, 129);
            this.comboBoxImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxImage.Name = "comboBoxImage";
            this.comboBoxImage.Size = new System.Drawing.Size(480, 24);
            this.comboBoxImage.TabIndex = 25;
            this.comboBoxImage.ValueMember = "Value";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 132);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 16);
            this.label8.TabIndex = 24;
            this.label8.Text = "Image:";
            // 
            // PushImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 367);
            this.Controls.Add(this.comboBoxImage);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBoxTag);
            this.Controls.Add(this.comboBoxRepository);
            this.Controls.Add(this.comboBoxDockerEngine);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBoxRegistry);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PushImageForm";
            this.Text = "PushImageForm";
            this.Load += new System.EventHandler(this.PushImageForm_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonFinish;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxRegistry;
        private System.Windows.Forms.ComboBox comboBoxDockerEngine;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxRepository;
        private System.Windows.Forms.ComboBox comboBoxTag;
        private System.Windows.Forms.ComboBox comboBoxImage;
        private System.Windows.Forms.Label label8;
    }
}