namespace DesktopMotorcycleRepair
{
    partial class Form6
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
            System.Windows.Forms.Label mechanicCodeLabel;
            System.Windows.Forms.Label mechanicNameLabel;
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mechanicsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mechanicsDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mechanicCodeTextBox = new System.Windows.Forms.TextBox();
            this.mechanicNameTextBox = new System.Windows.Forms.TextBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            mechanicCodeLabel = new System.Windows.Forms.Label();
            mechanicNameLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mechanicsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mechanicsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(mechanicCodeLabel);
            this.groupBox1.Controls.Add(this.mechanicCodeTextBox);
            this.groupBox1.Controls.Add(mechanicNameLabel);
            this.groupBox1.Controls.Add(this.mechanicNameTextBox);
            this.groupBox1.Location = new System.Drawing.Point(19, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 150);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(228, 32);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(137, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Edit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(313, 32);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Cancel";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(43, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(19, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(430, 84);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Process";
            // 
            // mechanicsBindingSource
            // 
            this.mechanicsBindingSource.DataSource = typeof(DesktopMotorcycleRepair.Mechanics);
            // 
            // mechanicsDataGridView
            // 
            this.mechanicsDataGridView.AllowUserToAddRows = false;
            this.mechanicsDataGridView.AllowUserToDeleteRows = false;
            this.mechanicsDataGridView.AutoGenerateColumns = false;
            this.mechanicsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mechanicsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.mechanicsDataGridView.DataSource = this.mechanicsBindingSource;
            this.mechanicsDataGridView.Location = new System.Drawing.Point(19, 271);
            this.mechanicsDataGridView.Name = "mechanicsDataGridView";
            this.mechanicsDataGridView.ReadOnly = true;
            this.mechanicsDataGridView.Size = new System.Drawing.Size(430, 316);
            this.mechanicsDataGridView.TabIndex = 11;
            this.mechanicsDataGridView.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.mechanicsDataGridView_RowHeaderMouseClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "MechanicCode";
            this.dataGridViewTextBoxColumn1.HeaderText = "MechanicCode";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "MechanicName";
            this.dataGridViewTextBoxColumn2.HeaderText = "MechanicName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "TransactionService";
            this.dataGridViewTextBoxColumn3.HeaderText = "TransactionService";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // mechanicCodeLabel
            // 
            mechanicCodeLabel.AutoSize = true;
            mechanicCodeLabel.Location = new System.Drawing.Point(72, 55);
            mechanicCodeLabel.Name = "mechanicCodeLabel";
            mechanicCodeLabel.Size = new System.Drawing.Size(85, 13);
            mechanicCodeLabel.TabIndex = 0;
            mechanicCodeLabel.Text = "Mechanic Code:";
            // 
            // mechanicCodeTextBox
            // 
            this.mechanicCodeTextBox.Location = new System.Drawing.Point(166, 52);
            this.mechanicCodeTextBox.Name = "mechanicCodeTextBox";
            this.mechanicCodeTextBox.ReadOnly = true;
            this.mechanicCodeTextBox.Size = new System.Drawing.Size(153, 20);
            this.mechanicCodeTextBox.TabIndex = 1;
            // 
            // mechanicNameLabel
            // 
            mechanicNameLabel.AutoSize = true;
            mechanicNameLabel.Location = new System.Drawing.Point(72, 81);
            mechanicNameLabel.Name = "mechanicNameLabel";
            mechanicNameLabel.Size = new System.Drawing.Size(88, 13);
            mechanicNameLabel.TabIndex = 2;
            mechanicNameLabel.Text = "Mechanic Name:";
            // 
            // mechanicNameTextBox
            // 
            this.mechanicNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "MechanicName", true));
            this.mechanicNameTextBox.Location = new System.Drawing.Point(166, 78);
            this.mechanicNameTextBox.Name = "mechanicNameTextBox";
            this.mechanicNameTextBox.Size = new System.Drawing.Size(153, 20);
            this.mechanicNameTextBox.TabIndex = 3;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(DesktopMotorcycleRepair.Mechanics);
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 610);
            this.Controls.Add(this.mechanicsDataGridView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form6";
            this.Text = "Mechanics";
            this.Load += new System.EventHandler(this.Form6_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mechanicsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mechanicsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.BindingSource mechanicsBindingSource;
        private System.Windows.Forms.DataGridView mechanicsDataGridView;
        private System.Windows.Forms.TextBox mechanicCodeTextBox;
        private System.Windows.Forms.TextBox mechanicNameTextBox;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}