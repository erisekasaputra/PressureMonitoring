using PressureTest.Domains;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            var document = new ReportDocument(_exportData);

            document.GeneratePdfAndShow();
        }
    }
}
