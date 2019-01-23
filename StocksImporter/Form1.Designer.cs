﻿namespace StocksImporter
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.mLstStocks = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.mLstEODData = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.mListErrors = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(623, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(497, 235);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.mLstStocks);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(489, 209);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Stocks";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // mLstStocks
            // 
            this.mLstStocks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mLstStocks.FormattingEnabled = true;
            this.mLstStocks.Location = new System.Drawing.Point(3, 3);
            this.mLstStocks.Name = "mLstStocks";
            this.mLstStocks.Size = new System.Drawing.Size(483, 199);
            this.mLstStocks.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.mLstEODData);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(489, 209);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "EOD Data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // mLstEODData
            // 
            this.mLstEODData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mLstEODData.FormattingEnabled = true;
            this.mLstEODData.Location = new System.Drawing.Point(3, 3);
            this.mLstEODData.Name = "mLstEODData";
            this.mLstEODData.Size = new System.Drawing.Size(483, 199);
            this.mLstEODData.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.mListErrors);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(489, 209);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Errors";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // mListErrors
            // 
            this.mListErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mListErrors.FormattingEnabled = true;
            this.mListErrors.Location = new System.Drawing.Point(3, 3);
            this.mListErrors.Name = "mListErrors";
            this.mListErrors.Size = new System.Drawing.Size(483, 199);
            this.mListErrors.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(623, 160);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 259);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox mLstStocks;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox mLstEODData;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox mListErrors;
    }
}

