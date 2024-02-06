namespace Sokoban
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ShowLevelButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.Heading = new System.Windows.Forms.Label();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.RecordsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ShowLevelButton
            // 
            this.ShowLevelButton.Location = new System.Drawing.Point(93, 232);
            this.ShowLevelButton.Name = "ShowLevelButton";
            this.ShowLevelButton.Size = new System.Drawing.Size(345, 61);
            this.ShowLevelButton.TabIndex = 2;
            this.ShowLevelButton.Text = "Выбрать уровень";
            this.ShowLevelButton.UseVisualStyleBackColor = true;
            this.ShowLevelButton.Click += new System.EventHandler(this.ShowLevelButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(93, 412);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(345, 61);
            this.ExitButton.TabIndex = 4;
            this.ExitButton.Text = "Выход";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // Heading
            // 
            this.Heading.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Heading.Location = new System.Drawing.Point(93, 52);
            this.Heading.Name = "Heading";
            this.Heading.Size = new System.Drawing.Size(345, 61);
            this.Heading.TabIndex = 5;
            this.Heading.Text = "Сокобан";
            this.Heading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ContinueButton
            // 
            this.ContinueButton.Location = new System.Drawing.Point(93, 142);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(345, 61);
            this.ContinueButton.TabIndex = 6;
            this.ContinueButton.Text = "Продолжить";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // RecordsButton
            // 
            this.RecordsButton.Location = new System.Drawing.Point(93, 322);
            this.RecordsButton.Name = "RecordsButton";
            this.RecordsButton.Size = new System.Drawing.Size(345, 62);
            this.RecordsButton.TabIndex = 7;
            this.RecordsButton.Text = "Таблица Рекордов";
            this.RecordsButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(523, 468);
            this.Controls.Add(this.RecordsButton);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.Heading);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ShowLevelButton);
            this.Name = "Form1";
            this.Text = "Сокобан";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ShowLevelButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Label Heading;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.Button RecordsButton;
    }
}

