namespace FileCompressionApp
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            txtFilePath = new TextBox();
            btnSelectFile = new Button();
            btnCompressArithmetic = new Button();
            btnCompressLZW = new Button();
            btnDoubleCompress = new Button();
            label1 = new Label();
            btnDecompressArithmetic = new Button();
            btnDecompressLZW = new Button();
            SuspendLayout();
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(14, 47);
            txtFilePath.Margin = new Padding(4, 3, 4, 3);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.ReadOnly = true;
            txtFilePath.Size = new Size(419, 23);
            txtFilePath.TabIndex = 0;
            // 
            // btnSelectFile
            // 
            btnSelectFile.Location = new Point(441, 45);
            btnSelectFile.Margin = new Padding(4, 3, 4, 3);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new Size(88, 27);
            btnSelectFile.TabIndex = 1;
            btnSelectFile.Text = "Browse";
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += btnSelectFile_Click;
            // 
            // btnCompressArithmetic
            // 
            btnCompressArithmetic.Location = new Point(14, 92);
            btnCompressArithmetic.Margin = new Padding(4, 3, 4, 3);
            btnCompressArithmetic.Name = "btnCompressArithmetic";
            btnCompressArithmetic.Size = new Size(163, 40);
            btnCompressArithmetic.TabIndex = 2;
            btnCompressArithmetic.Text = "Arithmetic Compression";
            btnCompressArithmetic.UseVisualStyleBackColor = true;
            btnCompressArithmetic.Click += btnCompressArithmetic_Click;
            // 
            // btnCompressLZW
            // 
            btnCompressLZW.Location = new Point(184, 92);
            btnCompressLZW.Margin = new Padding(4, 3, 4, 3);
            btnCompressLZW.Name = "btnCompressLZW";
            btnCompressLZW.Size = new Size(163, 40);
            btnCompressLZW.TabIndex = 3;
            btnCompressLZW.Text = "LZW Compression";
            btnCompressLZW.UseVisualStyleBackColor = true;
            btnCompressLZW.Click += btnCompressLZW_Click;
            // 
            // btnDoubleCompress
            // 
            btnDoubleCompress.Location = new Point(355, 92);
            btnDoubleCompress.Margin = new Padding(4, 3, 4, 3);
            btnDoubleCompress.Name = "btnDoubleCompress";
            btnDoubleCompress.Size = new Size(174, 40);
            btnDoubleCompress.TabIndex = 4;
            btnDoubleCompress.Text = "Double Compression (AC+LZW)";
            btnDoubleCompress.UseVisualStyleBackColor = true;
            btnDoubleCompress.Click += btnDoubleCompress_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 29);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(28, 15);
            label1.TabIndex = 5;
            label1.Text = "File:";
            // 
            // btnDecompressArithmetic
            // 
            btnDecompressArithmetic.Location = new Point(13, 149);
            btnDecompressArithmetic.Margin = new Padding(4, 3, 4, 3);
            btnDecompressArithmetic.Name = "btnDecompressArithmetic";
            btnDecompressArithmetic.Size = new Size(163, 40);
            btnDecompressArithmetic.TabIndex = 6;
            btnDecompressArithmetic.Text = "Arithemtic Decompression";
            btnDecompressArithmetic.UseVisualStyleBackColor = true;
            btnDecompressArithmetic.Click += btnDecompression_Click;
            // 
            // btnDecompressLZW
            // 
            btnDecompressLZW.Location = new Point(184, 149);
            btnDecompressLZW.Name = "btnDecompressLZW";
            btnDecompressLZW.Size = new Size(163, 40);
            btnDecompressLZW.TabIndex = 7;
            btnDecompressLZW.Text = "LZW Decompression";
            btnDecompressLZW.UseVisualStyleBackColor = true;
            btnDecompressLZW.Click += btnDecompressLZW_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(542, 201);
            Controls.Add(btnDecompressLZW);
            Controls.Add(btnDecompressArithmetic);
            Controls.Add(label1);
            Controls.Add(btnDoubleCompress);
            Controls.Add(btnCompressLZW);
            Controls.Add(btnCompressArithmetic);
            Controls.Add(btnSelectFile);
            Controls.Add(txtFilePath);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "MainForm";
            Text = "File Compression Tool";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnCompressArithmetic;
        private System.Windows.Forms.Button btnCompressLZW;
        private System.Windows.Forms.Button btnDoubleCompress;
        private System.Windows.Forms.Label label1;
        private Button btnDecompressArithmetic;
        private Button btnDecompressLZW;
    }
}