using PressureTest.Domains;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PressureTest
{
    public partial class ExportOptions : Form
    {
        private readonly string _valuesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Values");
        private readonly string _plotsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plots");

        private ExportData? _exportData { get; set; }

        private double GetMaxPressureDrop(List<PLCRegisterData> registerValues)
        {
            if (registerValues.Count < 2)
                return 0;

            double maxDrop = 0;

            for (int i = 1; i < registerValues.Count; i++)
            {
                double prevBar = registerValues[i - 1].RegisterValue;
                double currBar = registerValues[i].RegisterValue;

                double drop = prevBar - currBar;

                if (drop >= 0 && drop > maxDrop)
                {
                    maxDrop = drop;
                }
            }

            return maxDrop;
        }

        public ExportOptions(string fileName)
        {
            InitializeComponent();

            var filePath = Path.Combine(_valuesFolder, fileName);
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);

                try
                {
                    var exportData = JsonSerializer.Deserialize<ExportData>(jsonString);

                    if (exportData is null)
                    {
                        this.Close();
                        MessageBox.Show("Invalid data object", "Warning");
                        return;
                    }

                    _exportData = exportData;



                    Txt_PropertyValue_S1_4.Text = $"{exportData.RegisterValues.Max(d => d.RegisterValue)} psi";
                    Txt_PropertyValue_S1_5.Text = $"{GetMaxPressureDrop(exportData.RegisterValues)} psi";

                }
                catch (Exception)
                {
                    this.Close();
                    MessageBox.Show("Invalid data format", "Warning");
                    return;
                }
                return;
            }

            this.Close();
            MessageBox.Show("File not found", "Warning");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_exportData is null)
            {
                this.Close();
                MessageBox.Show("Invalid data object", "Warning");
                return;
            }



            if (CB_Section_1.Checked == true && string.IsNullOrEmpty(Txt_Section_1_Title.Text))
            {
                MessageBox.Show("Title section 1 could not be empty if you choose to activate it", "Information");
                return;
            }




            var listTileSection1 = new List<TitleContent>();
            var listTileSection2 = new List<TitleContent>();


            for (int h = 1; h <= 1; h++)
            {
                for (int i = 1; i <= 7; i++)
                {
                    var nameControl = this.Controls.Find($"Txt_PropertyName_S{h}_{i}", true).FirstOrDefault();
                    var valueControl = this.Controls.Find($"Txt_PropertyValue_S{h}_{i}", true).FirstOrDefault();

                    if (nameControl != null && valueControl != null)
                    {
                        string name = string.Empty;
                        string value = string.Empty;

                        // Ambil nama dari TextBox saja
                        if (nameControl is TextBox nameTb && !string.IsNullOrWhiteSpace(nameTb.Text))
                        {
                            name = nameTb.Text.Trim();

                            // Value bisa dari TextBox atau DateTimePicker
                            if (valueControl is TextBox valueTb)
                            {
                                value = valueTb.Text.Trim();
                            }
                            else if (valueControl is DateTimePicker dtPicker)
                            {
                                value = dtPicker.Value.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                            }

                            if (h == 1)
                                listTileSection1.Add(new TitleContent(name, value));
                            else if (h == 2)
                                listTileSection2.Add(new TitleContent(name, value));
                        }
                    }
                }
            }

             


            var document = new ReportDocument(
                _exportData,
                Txt_Section_1_Title.Text,
                string.Empty,
                listTileSection1,
                listTileSection2,
                Txt_PropertyValue_S1_8.Text.Trim());

            using SaveFileDialog saveFileDialog = new();

            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1; // Default to PDF files
            saveFileDialog.RestoreDirectory = true; // Remember last used directory
            saveFileDialog.FileName = $"PressureTestReport_{DateTime.Now:yyyyMMddHHmmss}.pdf"; // Default file name

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Generate the PDF and SAVE it to the path chosen by the user
                    document.GeneratePdf(filePath);

                    // --- Now, open the saved PDF with your preferred browser ---
                    string browserPath = "";

                    // Example for Google Chrome
                    browserPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                    if (!File.Exists(browserPath))
                    {
                        browserPath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                    }
                    // Add other browser paths if needed

                    if (File.Exists(browserPath))
                    {
                        Process.Start(browserPath, filePath); // Launch the selected browser with the PDF
                    }
                    else
                    {
                        // Fallback to default program
                        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                    }
                }
                catch (System.ComponentModel.Win32Exception winEx)
                {
                    MessageBox.Show(
                        $"Error opening PDF: {winEx.Message}\n\n" +
                        "Please ensure a default PDF viewer (or a browser that can open PDFs) is set on your system, or specify a valid browser path.",
                        "PDF Viewer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ExportOptions_Load(object sender, EventArgs e)
        {
            CB_Section_1.Checked = true;
        }
    }
}
