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
                MessageBox.Show("Please select a valid file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show($"Arithmetic compression completed!\n" +
                    $"Original size: {originalFile.Length} bytes\n" +
                    $"Compressed size: {compressedFile.Length} bytes\n" +
                    $"Compression ratio: {compressionRatio:F2}%",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during arithmetic compression: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecompression_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text)
                || Path.GetExtension(txtFilePath.Text) != ".ac")
            {
                MessageBox.Show("Please select a valid file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show($"Arithmetic decompression completed!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during arithmetic decompression: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCompressLZW_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Please select a valid file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show($"LZW compression completed!\n" +
                    $"Original size: {originalFile.Length} bytes\n" +
                    $"Compressed size: {compressedFile.Length} bytes\n" +
                    $"Compression ratio: {compressionRatio:F2}%",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during LZW compression: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecompressLZW_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text)
                || Path.GetExtension(txtFilePath.Text) != ".lzw")
            {
                MessageBox.Show("Please select a valid file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show($"LZW decompression completed!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during LZW decompression: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDoubleCompress_Click(object sender, EventArgs e)
        {
            /*if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Please select a valid file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string inputFile = txtFilePath.Text;
                string tempFile = inputFile + ".ac";
                string finalFile = inputFile + ".ac.lzw";

                // First compression - Arithmetic
                ArithmeticCoder acCompressor = new ArithmeticCoder();
                acCompressor.Compress(inputFile, tempFile);

                // Second compression - LZW
                LZWCompressor lzwCompressor = new LZWCompressor();
                lzwCompressor.Compress(tempFile, finalFile);

                FileInfo originalFile = new FileInfo(inputFile);
                FileInfo compressedFile = new FileInfo(finalFile);

                double compressionRatio = (1 - (double)compressedFile.Length / originalFile.Length) * 100;

                // Clean up temporary file
                if (File.Exists(tempFile))
                    File.Delete(tempFile);

                MessageBox.Show($"Double compression completed!\n" +
                    $"Original size: {originalFile.Length} bytes\n" +
                    $"Compressed size: {compressedFile.Length} bytes\n" +
                    $"Compression ratio: {compressionRatio:F2}%",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during double compression: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }
    }
}