namespace CaptainDocker.Forms
{
    partial class CreateContainerForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateContainerForm));
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxDockerEngine = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxImage = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxEntrypoint = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxEnvironment = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridViewExposedPorts = new System.Windows.Forms.DataGridView();
            this.ContainerPortColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PortTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.HostIpColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostPortColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkBoxPublishAllPorts = new System.Windows.Forms.CheckBox();
            this.groupBoxSpecifyExposedPorts = new System.Windows.Forms.GroupBox();
            this.checkBoxAttachToStdin = new System.Windows.Forms.CheckBox();
            this.checkBoxAttachToStdout = new System.Windows.Forms.CheckBox();
            this.checkBoxAttachToStderr = new System.Windows.Forms.CheckBox();
            this.textBoxCommand = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.contextMenuStripSpecifyExposedPorts = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeSpecifyExposedPortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBoxAutoRemove = new System.Windows.Forms.CheckBox();
            this.checkBoxStartContainerAfterCreated = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxRestartPolicyMaximumRetryCount = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBoxRestartPolicyFlag = new System.Windows.Forms.ComboBox();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExposedPorts)).BeginInit();
            this.groupBoxSpecifyExposedPorts.SuspendLayout();
            this.contextMenuStripSpecifyExposedPorts.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.buttonCancel.Location = new System.Drawing.Point(474, 10);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(148, 35);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonCreate
            // 
            this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreate.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.buttonCreate.Location = new System.Drawing.Point(321, 10);
            this.buttonCreate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(148, 35);
            this.buttonCreate.TabIndex = 1;
            this.buttonCreate.Text = "&Create";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.buttonCreate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 872);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(632, 58);
            this.panel2.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(0, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(632, 2);
            this.label6.TabIndex = 3;
            this.label6.Text = "label6";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(244, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Create a container with image.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(101, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "Create Container";
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
            this.panel1.Size = new System.Drawing.Size(632, 92);
            this.panel1.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::CaptainDocker.Properties.Resources.create_container;
            this.pictureBox1.Location = new System.Drawing.Point(15, 13);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(71, 69);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 23);
            this.label7.TabIndex = 16;
            this.label7.Text = "Name:";
            // 
            // comboBoxDockerEngine
            // 
            this.comboBoxDockerEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDockerEngine.DisplayMember = "Text";
            this.comboBoxDockerEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDockerEngine.FormattingEnabled = true;
            this.comboBoxDockerEngine.Location = new System.Drawing.Point(140, 116);
            this.comboBoxDockerEngine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxDockerEngine.Name = "comboBoxDockerEngine";
            this.comboBoxDockerEngine.Size = new System.Drawing.Size(480, 31);
            this.comboBoxDockerEngine.TabIndex = 21;
            this.comboBoxDockerEngine.ValueMember = "Value";
            this.comboBoxDockerEngine.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDockerEngine_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(124, 23);
            this.label9.TabIndex = 20;
            this.label9.Text = "Docker Engine:";
            // 
            // comboBoxImage
            // 
            this.comboBoxImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxImage.DisplayMember = "Text";
            this.comboBoxImage.FormattingEnabled = true;
            this.comboBoxImage.Location = new System.Drawing.Point(140, 153);
            this.comboBoxImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxImage.Name = "comboBoxImage";
            this.comboBoxImage.Size = new System.Drawing.Size(480, 31);
            this.comboBoxImage.TabIndex = 25;
            this.comboBoxImage.ValueMember = "Value";
            this.comboBoxImage.SelectedIndexChanged += new System.EventHandler(this.ComboBoxImage_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 23);
            this.label8.TabIndex = 24;
            this.label8.Text = "Image:";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(140, 191);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(482, 30);
            this.textBoxName.TabIndex = 26;
            // 
            // textBoxEntrypoint
            // 
            this.textBoxEntrypoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEntrypoint.Location = new System.Drawing.Point(140, 228);
            this.textBoxEntrypoint.Name = "textBoxEntrypoint";
            this.textBoxEntrypoint.Size = new System.Drawing.Size(482, 30);
            this.textBoxEntrypoint.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 231);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 23);
            this.label3.TabIndex = 27;
            this.label3.Text = "Entrypoint:";
            // 
            // textBoxEnvironment
            // 
            this.textBoxEnvironment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEnvironment.Location = new System.Drawing.Point(140, 264);
            this.textBoxEnvironment.Name = "textBoxEnvironment";
            this.textBoxEnvironment.Size = new System.Drawing.Size(482, 30);
            this.textBoxEnvironment.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 268);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 23);
            this.label4.TabIndex = 29;
            this.label4.Text = "Environment:";
            // 
            // dataGridViewExposedPorts
            // 
            this.dataGridViewExposedPorts.AllowUserToOrderColumns = true;
            this.dataGridViewExposedPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewExposedPorts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewExposedPorts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewExposedPorts.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewExposedPorts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewExposedPorts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExposedPorts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ContainerPortColumn,
            this.PortTypeColumn,
            this.HostIpColumn,
            this.HostPortColumn});
            this.dataGridViewExposedPorts.Location = new System.Drawing.Point(15, 38);
            this.dataGridViewExposedPorts.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewExposedPorts.MultiSelect = false;
            this.dataGridViewExposedPorts.Name = "dataGridViewExposedPorts";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewExposedPorts.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewExposedPorts.RowHeadersVisible = false;
            this.dataGridViewExposedPorts.RowHeadersWidth = 51;
            this.dataGridViewExposedPorts.RowTemplate.Height = 24;
            this.dataGridViewExposedPorts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewExposedPorts.Size = new System.Drawing.Size(590, 220);
            this.dataGridViewExposedPorts.TabIndex = 32;
            // 
            // ContainerPortColumn
            // 
            this.ContainerPortColumn.HeaderText = "Container Port";
            this.ContainerPortColumn.MinimumWidth = 6;
            this.ContainerPortColumn.Name = "ContainerPortColumn";
            this.ContainerPortColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PortTypeColumn
            // 
            this.PortTypeColumn.HeaderText = "Port Type";
            this.PortTypeColumn.Items.AddRange(new object[] {
            "tcp",
            "udp",
            "sctp"});
            this.PortTypeColumn.MinimumWidth = 6;
            this.PortTypeColumn.Name = "PortTypeColumn";
            // 
            // HostIpColumn
            // 
            this.HostIpColumn.HeaderText = "Host IP";
            this.HostIpColumn.MinimumWidth = 6;
            this.HostIpColumn.Name = "HostIpColumn";
            // 
            // HostPortColumn
            // 
            this.HostPortColumn.HeaderText = "Host Port";
            this.HostPortColumn.MinimumWidth = 6;
            this.HostPortColumn.Name = "HostPortColumn";
            // 
            // checkBoxPublishAllPorts
            // 
            this.checkBoxPublishAllPorts.AutoSize = true;
            this.checkBoxPublishAllPorts.Checked = true;
            this.checkBoxPublishAllPorts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPublishAllPorts.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.checkBoxPublishAllPorts.Location = new System.Drawing.Point(21, 511);
            this.checkBoxPublishAllPorts.Name = "checkBoxPublishAllPorts";
            this.checkBoxPublishAllPorts.Size = new System.Drawing.Size(339, 24);
            this.checkBoxPublishAllPorts.TabIndex = 33;
            this.checkBoxPublishAllPorts.Text = "Publish all exposed ports to the host interfaces";
            this.checkBoxPublishAllPorts.UseVisualStyleBackColor = true;
            // 
            // groupBoxSpecifyExposedPorts
            // 
            this.groupBoxSpecifyExposedPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSpecifyExposedPorts.Controls.Add(this.dataGridViewExposedPorts);
            this.groupBoxSpecifyExposedPorts.Location = new System.Drawing.Point(15, 571);
            this.groupBoxSpecifyExposedPorts.Name = "groupBoxSpecifyExposedPorts";
            this.groupBoxSpecifyExposedPorts.Size = new System.Drawing.Size(610, 263);
            this.groupBoxSpecifyExposedPorts.TabIndex = 34;
            this.groupBoxSpecifyExposedPorts.TabStop = false;
            this.groupBoxSpecifyExposedPorts.Text = "Specify Exposed Ports";
            // 
            // checkBoxAttachToStdin
            // 
            this.checkBoxAttachToStdin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAttachToStdin.AutoSize = true;
            this.checkBoxAttachToStdin.Location = new System.Drawing.Point(21, 840);
            this.checkBoxAttachToStdin.Name = "checkBoxAttachToStdin";
            this.checkBoxAttachToStdin.Size = new System.Drawing.Size(145, 27);
            this.checkBoxAttachToStdin.TabIndex = 36;
            this.checkBoxAttachToStdin.Text = "Attach to stdin";
            this.checkBoxAttachToStdin.UseVisualStyleBackColor = true;
            // 
            // checkBoxAttachToStdout
            // 
            this.checkBoxAttachToStdout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAttachToStdout.AutoSize = true;
            this.checkBoxAttachToStdout.Checked = true;
            this.checkBoxAttachToStdout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAttachToStdout.Location = new System.Drawing.Point(166, 840);
            this.checkBoxAttachToStdout.Name = "checkBoxAttachToStdout";
            this.checkBoxAttachToStdout.Size = new System.Drawing.Size(157, 27);
            this.checkBoxAttachToStdout.TabIndex = 37;
            this.checkBoxAttachToStdout.Text = "Attach to stdout";
            this.checkBoxAttachToStdout.UseVisualStyleBackColor = true;
            // 
            // checkBoxAttachToStderr
            // 
            this.checkBoxAttachToStderr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAttachToStderr.AutoSize = true;
            this.checkBoxAttachToStderr.Checked = true;
            this.checkBoxAttachToStderr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAttachToStderr.Location = new System.Drawing.Point(321, 840);
            this.checkBoxAttachToStderr.Name = "checkBoxAttachToStderr";
            this.checkBoxAttachToStderr.Size = new System.Drawing.Size(152, 27);
            this.checkBoxAttachToStderr.TabIndex = 38;
            this.checkBoxAttachToStderr.Text = "Attach to stderr";
            this.checkBoxAttachToStderr.UseVisualStyleBackColor = true;
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCommand.Location = new System.Drawing.Point(138, 317);
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.Size = new System.Drawing.Size(482, 30);
            this.textBoxCommand.TabIndex = 40;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 321);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 23);
            this.label11.TabIndex = 39;
            this.label11.Text = "Command:";
            // 
            // contextMenuStripSpecifyExposedPorts
            // 
            this.contextMenuStripSpecifyExposedPorts.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripSpecifyExposedPorts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSpecifyExposedPortsToolStripMenuItem});
            this.contextMenuStripSpecifyExposedPorts.Name = "contextMenuStripSpecifyExposedPorts";
            this.contextMenuStripSpecifyExposedPorts.Size = new System.Drawing.Size(133, 28);
            // 
            // removeSpecifyExposedPortsToolStripMenuItem
            // 
            this.removeSpecifyExposedPortsToolStripMenuItem.Name = "removeSpecifyExposedPortsToolStripMenuItem";
            this.removeSpecifyExposedPortsToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
            this.removeSpecifyExposedPortsToolStripMenuItem.Text = "Remove";
            this.removeSpecifyExposedPortsToolStripMenuItem.Click += new System.EventHandler(this.removeSpecifyExposedPortsToolStripMenuItem_Click);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label10.Location = new System.Drawing.Point(136, 297);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(181, 17);
            this.label10.TabIndex = 41;
            this.label10.Text = "Example: FOO=bar,BAZ=quux";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label12.Location = new System.Drawing.Point(139, 350);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(129, 17);
            this.label12.TabIndex = 42;
            this.label12.Text = "Example: install,quiet";
            // 
            // checkBoxAutoRemove
            // 
            this.checkBoxAutoRemove.AutoSize = true;
            this.checkBoxAutoRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.checkBoxAutoRemove.Location = new System.Drawing.Point(21, 481);
            this.checkBoxAutoRemove.Name = "checkBoxAutoRemove";
            this.checkBoxAutoRemove.Size = new System.Drawing.Size(469, 24);
            this.checkBoxAutoRemove.TabIndex = 43;
            this.checkBoxAutoRemove.Text = "Automatically remove the container when it exits (--rm command)";
            this.checkBoxAutoRemove.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartContainerAfterCreated
            // 
            this.checkBoxStartContainerAfterCreated.AutoSize = true;
            this.checkBoxStartContainerAfterCreated.Checked = true;
            this.checkBoxStartContainerAfterCreated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStartContainerAfterCreated.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.checkBoxStartContainerAfterCreated.Location = new System.Drawing.Point(21, 541);
            this.checkBoxStartContainerAfterCreated.Name = "checkBoxStartContainerAfterCreated";
            this.checkBoxStartContainerAfterCreated.Size = new System.Drawing.Size(372, 24);
            this.checkBoxStartContainerAfterCreated.TabIndex = 44;
            this.checkBoxStartContainerAfterCreated.Text = "Start container after created (docker run command)";
            this.checkBoxStartContainerAfterCreated.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxRestartPolicyMaximumRetryCount);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.comboBoxRestartPolicyFlag);
            this.groupBox1.Location = new System.Drawing.Point(15, 370);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(610, 105);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Restart Policy";
            // 
            // textBoxRestartPolicyMaximumRetryCount
            // 
            this.textBoxRestartPolicyMaximumRetryCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRestartPolicyMaximumRetryCount.Location = new System.Drawing.Point(124, 69);
            this.textBoxRestartPolicyMaximumRetryCount.Name = "textBoxRestartPolicyMaximumRetryCount";
            this.textBoxRestartPolicyMaximumRetryCount.Size = new System.Drawing.Size(482, 30);
            this.textBoxRestartPolicyMaximumRetryCount.TabIndex = 50;
            this.textBoxRestartPolicyMaximumRetryCount.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label14.Location = new System.Drawing.Point(12, 65);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 34);
            this.label14.TabIndex = 49;
            this.label14.Text = "Maximum \r\nRetry Count:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 32);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 23);
            this.label13.TabIndex = 48;
            this.label13.Text = "Flag:";
            // 
            // comboBoxRestartPolicyFlag
            // 
            this.comboBoxRestartPolicyFlag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRestartPolicyFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRestartPolicyFlag.FormattingEnabled = true;
            this.comboBoxRestartPolicyFlag.Items.AddRange(new object[] {
            "no",
            "on-failure",
            "always",
            "unless-stopped"});
            this.comboBoxRestartPolicyFlag.Location = new System.Drawing.Point(125, 29);
            this.comboBoxRestartPolicyFlag.Name = "comboBoxRestartPolicyFlag";
            this.comboBoxRestartPolicyFlag.Size = new System.Drawing.Size(479, 31);
            this.comboBoxRestartPolicyFlag.TabIndex = 47;
            // 
            // CreateContainerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 930);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxStartContainerAfterCreated);
            this.Controls.Add(this.checkBoxAutoRemove);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxCommand);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.checkBoxAttachToStderr);
            this.Controls.Add(this.checkBoxAttachToStdout);
            this.Controls.Add(this.checkBoxAttachToStdin);
            this.Controls.Add(this.groupBoxSpecifyExposedPorts);
            this.Controls.Add(this.checkBoxPublishAllPorts);
            this.Controls.Add(this.textBoxEnvironment);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxEntrypoint);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.comboBoxImage);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBoxDockerEngine);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CreateContainerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Container";
            this.Load += new System.EventHandler(this.CreateContainerForm_Load);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExposedPorts)).EndInit();
            this.groupBoxSpecifyExposedPorts.ResumeLayout(false);
            this.contextMenuStripSpecifyExposedPorts.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxDockerEngine;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxImage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxEntrypoint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxEnvironment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridViewExposedPorts;
        private System.Windows.Forms.CheckBox checkBoxPublishAllPorts;
        private System.Windows.Forms.GroupBox groupBoxSpecifyExposedPorts;
        private System.Windows.Forms.CheckBox checkBoxAttachToStdin;
        private System.Windows.Forms.CheckBox checkBoxAttachToStdout;
        private System.Windows.Forms.CheckBox checkBoxAttachToStderr;
        private System.Windows.Forms.TextBox textBoxCommand;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSpecifyExposedPorts;
        private System.Windows.Forms.ToolStripMenuItem removeSpecifyExposedPortsToolStripMenuItem;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBoxAutoRemove;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContainerPortColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn PortTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostIpColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostPortColumn;
        private System.Windows.Forms.CheckBox checkBoxStartContainerAfterCreated;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBoxRestartPolicyFlag;
        private System.Windows.Forms.TextBox textBoxRestartPolicyMaximumRetryCount;
        private System.Windows.Forms.Label label14;
    }
}