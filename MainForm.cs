using System;
using System.IO;
using System.Windows.Forms;

namespace FileCompressionApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnCompressArithmetic_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Пожалуйста, выберте верный файл", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string inputFile = txtFilePath.Text;
                string outputFile = inputFile + ".ac";

                ArithmeticCoder.EncodeFile(inputFile, outputFile);

                FileInfo originalFile = new FileInfo(inputFile);
                FileInfo compressedFile = new FileInfo(outputFile);

                double compressionRatio = (1 - (double)compressedFile.Length / originalFile.Length) * 100;

                MessageBox.Show($"Успешно!",
                    "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecompression_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text)
                || Path.GetExtension(txtFilePath.Text) != ".ac")
            {
                MessageBox.Show("Пожалуйста, выберте верный файл", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string inputFile = txtFilePath.Text;
                string outputFile = inputFile[..^2];

                ArithmeticCoder.DecodeFile(inputFile, outputFile);

                FileInfo originalFile = new FileInfo(inputFile);
                FileInfo compressedFile = new FileInfo(outputFile);

                double compressionRatio = (1 - (double)compressedFile.Length / originalFile.Length) * 100;

                MessageBox.Show($"Успешно!",
                    "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCompressLZW_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Пожалуйста, выберте верный файл", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string inputFile = txtFilePath.Text;
                string outputFile = inputFile + ".lzw";

                LZWCompressor.Compress(inputFile, outputFile);

                FileInfo originalFile = new FileInfo(inputFile);
                FileInfo compressedFile = new FileInfo(outputFile);

                double compressionRatio = (1 - (double)compressedFile.Length / originalFile.Length) * 100;

                MessageBox.Show($"Успешно!",
                    "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecompressLZW_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text)
                || Path.GetExtension(txtFilePath.Text) != ".lzw")
            {
                MessageBox.Show("Пожалуйста, выберте верный файл", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string inputFile = txtFilePath.Text;
                string outputFile = inputFile[..^3];

                LZWCompressor.Decompress(inputFile, outputFile);

                FileInfo originalFile = new FileInfo(inputFile);
                FileInfo compressedFile = new FileInfo(outputFile);

                double compressionRatio = (1 - (double)compressedFile.Length / originalFile.Length) * 100;

                MessageBox.Show($"Успешно!",
                    "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}