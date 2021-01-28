namespace PrizivToCSV {
    partial class Form1 {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.DownloadFileButton = new System.Windows.Forms.Button();
            this.CreateCSVButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // DownloadFileButton
            // 
            this.DownloadFileButton.Location = new System.Drawing.Point(12, 42);
            this.DownloadFileButton.Name = "DownloadFileButton";
            this.DownloadFileButton.Size = new System.Drawing.Size(114, 42);
            this.DownloadFileButton.TabIndex = 0;
            this.DownloadFileButton.Text = "Загрузить файл";
            this.DownloadFileButton.UseVisualStyleBackColor = true;
            this.DownloadFileButton.Click += new System.EventHandler(this.DownloadFileButton_Click);
            // 
            // CreateCSVButton
            // 
            this.CreateCSVButton.Location = new System.Drawing.Point(12, 135);
            this.CreateCSVButton.Name = "CreateCSVButton";
            this.CreateCSVButton.Size = new System.Drawing.Size(162, 48);
            this.CreateCSVButton.TabIndex = 1;
            this.CreateCSVButton.Text = "Преобразовать для srz";
            this.CreateCSVButton.UseVisualStyleBackColor = true;
            this.CreateCSVButton.Click += new System.EventHandler(this.CreateCSVButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 245);
            this.Controls.Add(this.CreateCSVButton);
            this.Controls.Add(this.DownloadFileButton);
            this.Name = "Form1";
            this.Text = "Призывники";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DownloadFileButton;
        private System.Windows.Forms.Button CreateCSVButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

