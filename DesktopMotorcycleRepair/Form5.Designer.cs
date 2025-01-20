namespace DesktopMotorcycleRepair
{
    partial class Form5
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
            System.Windows.Forms.Label costLabel;
            System.Windows.Forms.Label serviceCodeLabel;
            System.Windows.Forms.Label serviceNameLabel;
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.motorcycleServicesDataGridView = new System.Windows.Forms.DataGridView();
            this.costNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.serviceCodeTextBox = new System.Windows.Forms.TextBox();
            this.serviceNameTextBox = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.motorcycleServicesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            costLabel = new System.Windows.Forms.Label();
            serviceCodeLabel = new System.Windows.Forms.Label();
            serviceNameLabel = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.motorcycleServicesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.motorcycleServicesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(18, 171);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(430, 84);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Process";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(costLabel);
            this.groupBox1.Controls.Add(this.costNumericUpDown);
            this.groupBox1.Controls.Add(serviceCodeLabel);
            this.groupBox1.Controls.Add(this.serviceCodeTextBox);
            this.groupBox1.Controls.Add(serviceNameLabel);
            this.groupBox1.Controls.Add(this.serviceNameTextBox);
            this.groupBox1.Location = new System.Drawing.Point(18, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 150);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // motorcycleServicesDataGridView
            // 
            this.motorcycleServicesDataGridView.AllowUserToAddRows = false;
            this.motorcycleServicesDataGridView.AllowUserToDeleteRows = false;
            this.motorcycleServicesDataGridView.AutoGenerateColumns = false;
            this.motorcycleServicesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.motorcycleServicesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.motorcycleServicesDataGridView.DataSource = this.motorcycleServicesBindingSource;
            this.motorcycleServicesDataGridView.Location = new System.Drawing.Point(18, 272);
            this.motorcycleServicesDataGridView.Name = "motorcycleServicesDataGridView";
            this.motorcycleServicesDataGridView.ReadOnly = true;
            this.motorcycleServicesDataGridView.Size = new System.Drawing.Size(430, 326);
            this.motorcycleServicesDataGridView.TabIndex = 8;
            this.motorcycleServicesDataGridView.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.motorcycleServicesDataGridView_RowHeaderMouseClick);
            // 
            // costLabel
            // 
            costLabel.AutoSize = true;
            costLabel.Location = new System.Drawing.Point(84, 101);
            costLabel.Name = "costLabel";
            costLabel.Size = new System.Drawing.Size(31, 13);
            costLabel.TabIndex = 0;
            costLabel.Text = "Cost:";
            // 
            // costNumericUpDown
            // 
            this.costNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource1, "Cost", true));
            this.costNumericUpDown.Location = new System.Drawing.Point(167, 101);
            this.costNumericUpDown.Name = "costNumericUpDown";
            this.costNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.costNumericUpDown.TabIndex = 1;
            // 
            // serviceCodeLabel
            // 
            serviceCodeLabel.AutoSize = true;
            serviceCodeLabel.Location = new System.Drawing.Point(84, 32);
            serviceCodeLabel.Name = "serviceCodeLabel";
            serviceCodeLabel.Size = new System.Drawing.Size(74, 13);
            serviceCodeLabel.TabIndex = 2;
            serviceCodeLabel.Text = "Service Code:";
            // 
            // serviceCodeTextBox
            // 
            this.serviceCodeTextBox.Location = new System.Drawing.Point(167, 29);
            this.serviceCodeTextBox.Name = "serviceCodeTextBox";
            this.serviceCodeTextBox.ReadOnly = true;
            this.serviceCodeTextBox.Size = new System.Drawing.Size(120, 20);
            this.serviceCodeTextBox.TabIndex = 3;
            // 
            // serviceNameLabel
            // 
            serviceNameLabel.AutoSize = true;
            serviceNameLabel.Location = new System.Drawing.Point(84, 69);
            serviceNameLabel.Name = "serviceNameLabel";
            serviceNameLabel.Size = new System.Drawing.Size(77, 13);
            serviceNameLabel.TabIndex = 4;
            serviceNameLabel.Text = "Service Name:";
            // 
            // serviceNameTextBox
            // 
            this.serviceNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "ServiceName", true));
            this.serviceNameTextBox.Location = new System.Drawing.Point(167, 66);
            this.serviceNameTextBox.Name = "serviceNameTextBox";
            this.serviceNameTextBox.Size = new System.Drawing.Size(178, 20);
            this.serviceNameTextBox.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ServiceCode";
            this.dataGridViewTextBoxColumn1.HeaderText = "ServiceCode";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ServiceName";
            this.dataGridViewTextBoxColumn2.HeaderText = "ServiceName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Cost";
            this.dataGridViewTextBoxColumn3.HeaderText = "Cost";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "DetailService";
            this.dataGridViewTextBoxColumn4.HeaderText = "DetailService";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // motorcycleServicesBindingSource
            // 
            this.motorcycleServicesBindingSource.DataSource = typeof(DesktopMotorcycleRepair.MotorcycleServices);
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(DesktopMotorcycleRepair.MotorcycleServices);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 610);
            this.Controls.Add(this.motorcycleServicesDataGridView);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form5";
            this.Text = "Services";
            this.Load += new System.EventHandler(this.Form5_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.motorcycleServicesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.motorcycleServicesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.BindingSource motorcycleServicesBindingSource;
        private System.Windows.Forms.DataGridView motorcycleServicesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.NumericUpDown costNumericUpDown;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TextBox serviceCodeTextBox;
        private System.Windows.Forms.TextBox serviceNameTextBox;
    }
}