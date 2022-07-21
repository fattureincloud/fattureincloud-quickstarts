namespace WinFormsQuickstart
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.syncButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.saveClient = new System.Windows.Forms.Button();
            this.clientEmailTextBox = new System.Windows.Forms.TextBox();
            this.clientVatNumberTextBox = new System.Windows.Forms.TextBox();
            this.clientTaxCodeTextBox = new System.Windows.Forms.TextBox();
            this.clientNameTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 426);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.syncButton);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "List Clients";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // syncButton
            // 
            this.syncButton.Location = new System.Drawing.Point(658, 345);
            this.syncButton.Name = "syncButton";
            this.syncButton.Size = new System.Drawing.Size(75, 23);
            this.syncButton.TabIndex = 1;
            this.syncButton.Text = "Sync";
            this.syncButton.UseVisualStyleBackColor = true;
            this.syncButton.Click += new System.EventHandler(this.syncButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(615, 386);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.saveClient);
            this.tabPage2.Controls.Add(this.clientEmailTextBox);
            this.tabPage2.Controls.Add(this.clientVatNumberTextBox);
            this.tabPage2.Controls.Add(this.clientTaxCodeTextBox);
            this.tabPage2.Controls.Add(this.clientNameTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 398);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Create Client";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // saveClient
            // 
            this.saveClient.Location = new System.Drawing.Point(350, 276);
            this.saveClient.Name = "saveClient";
            this.saveClient.Size = new System.Drawing.Size(99, 45);
            this.saveClient.TabIndex = 4;
            this.saveClient.Text = "Save Client";
            this.saveClient.UseVisualStyleBackColor = true;
            this.saveClient.Click += new System.EventHandler(this.saveClient_Click);
            // 
            // clientEmailTextBox
            // 
            this.clientEmailTextBox.Location = new System.Drawing.Point(309, 235);
            this.clientEmailTextBox.Name = "clientEmailTextBox";
            this.clientEmailTextBox.PlaceholderText = "Email";
            this.clientEmailTextBox.Size = new System.Drawing.Size(190, 23);
            this.clientEmailTextBox.TabIndex = 3;
            // 
            // clientVatNumberTextBox
            // 
            this.clientVatNumberTextBox.Location = new System.Drawing.Point(309, 186);
            this.clientVatNumberTextBox.Name = "clientVatNumberTextBox";
            this.clientVatNumberTextBox.PlaceholderText = "Vat Number";
            this.clientVatNumberTextBox.Size = new System.Drawing.Size(190, 23);
            this.clientVatNumberTextBox.TabIndex = 2;
            // 
            // clientTaxCodeTextBox
            // 
            this.clientTaxCodeTextBox.Location = new System.Drawing.Point(309, 134);
            this.clientTaxCodeTextBox.Name = "clientTaxCodeTextBox";
            this.clientTaxCodeTextBox.PlaceholderText = "Tax Code";
            this.clientTaxCodeTextBox.Size = new System.Drawing.Size(190, 23);
            this.clientTaxCodeTextBox.TabIndex = 1;
            // 
            // clientNameTextBox
            // 
            this.clientNameTextBox.Location = new System.Drawing.Point(309, 88);
            this.clientNameTextBox.Name = "clientNameTextBox";
            this.clientNameTextBox.PlaceholderText = "Name";
            this.clientNameTextBox.Size = new System.Drawing.Size(190, 23);
            this.clientNameTextBox.TabIndex = 0;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form";
            this.Text = "WinFormQuickstart";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button syncButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox clientEmailTextBox;
        private System.Windows.Forms.TextBox clientVatNumberTextBox;
        private System.Windows.Forms.TextBox clientTaxCodeTextBox;
        private System.Windows.Forms.TextBox clientNameTextBox;
        private System.Windows.Forms.Button saveClient;
    }
}
