using HslCommunication.Core.IMessage;
using PressureTest.Domains;
using PressureTest.Services.Interfaces;
using ScottPlot;
using ScottPlot.AxisPanels;
using ScottPlot.Plottables;
using System.IO.Ports;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace PressureTest
{
    public partial class Form1 : Form
    {
        private JsonSerializerOptions _options = new() { WriteIndented = true };

        private MonitoringState _monitoringState = MonitoringState.Stop;
        private readonly IPLCReadWorker _plcWorker;
        private readonly IModbusService _modbusService;
        private List<PLCRegisterData> _registerDatas = new();
        private DateTime _startTime;

        private FileSystemWatcher? watcher;
        private readonly string targetFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Values");

        private bool _hasError = false;
        private string _errorMessage = string.Empty;
        private bool isPaused = false;

        public Form1(IPLCReadWorker plcWorker, IModbusService modbusService)
        {
            InitializeComponent();
            _plcWorker = plcWorker;
            _plcWorker.OnDataReceived = OnPLCDataReceived;
            _plcWorker.OnErrorRaised = OnErrorRaised;
            _modbusService = modbusService;


            _plcWorker.Stop();
            SetStateMonitoring(MonitoringState.Default);

            ChartSensor.Plot.XLabel("Time (Minutes)");
            ChartSensor.Plot.YLabel("Pressure (psi)");
           
            ChartSensor.Refresh();

            // disable interactivity by default
            ChartSensor.UserInputProcessor.Disable();

            // create two loggers and add them to the plot
            Logger1 = ChartSensor.Plot.Add.DataLogger();

            // use the right axis (already there by default) for the first logger
            LeftAxis axis1 = (LeftAxis)ChartSensor.Plot.Axes.Left;
            Logger1.Axes.YAxis = axis1;
            axis1.Color(Logger1.Color);
          
            Logger1.LineWidth = 2;
            
            ChartSensor.Plot.Grid.XAxisStyle.IsVisible = false;
            ChartSensor.Plot.Grid.YAxisStyle.IsVisible = false;


            ScottPlot.TickGenerators.NumericAutomatic tickGenX = new();
            tickGenX.TickDensity = 0.2;
            ChartSensor.Plot.Axes.Bottom.TickGenerator = tickGenX;

            ScottPlot.TickGenerators.NumericAutomatic tickGenY = new();
            tickGenY.TickDensity = 0.5;
            ChartSensor.Plot.Axes.Left.TickGenerator = tickGenY;


            ChartSensor.Plot.Axes.Left.TickLabelStyle.FontSize = 30;
            ChartSensor.Plot.Axes.Right.TickLabelStyle.FontSize = 30;
            ChartSensor.Plot.Axes.Bottom.TickLabelStyle.FontSize = 30;

            ChartSensor.Plot.Axes.Left.Label.FontSize = 30;
            ChartSensor.Plot.Axes.Bottom.Label.FontSize = 30;
            ChartSensor.Plot.Axes.Right.Label.FontSize = 30;




            ResetMonitoring();

            UpdatePlotTimer.Tick += (s, e) =>
            {
                if (Logger1.HasNewData)
                    ChartSensor.Refresh();
            };

            InitializeDataGridView();
            InitializeFileSystemWatcher();
            LoadFiles();
        }

        readonly System.Windows.Forms.Timer UpdatePlotTimer = new() { Interval = 100, Enabled = true };

        readonly DataLogger Logger1;

        private void OnPLCDataReceived(PLCRegisterData data)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => OnPLCDataReceived(data)));
                    return;
                }

                double elapsedMinutes = (DateTime.Now - _startTime).TotalMilliseconds / 60000.0;
                double value = data.RegisterValue;
                Logger1.Add(elapsedMinutes, value);
                _registerDatas.Add(data);
                LabelPressureValue.Text = $"{value:F2} psi";

                ResetError();
            }
            catch (Exception ex)
            {
                SetError(ex.Message, ex.InnerException?.Message ?? string.Empty);
            }
        }

        private static double ConvertFromPsiToBar(double psi)
        {
            return psi * 0.0689476;
        }

        private void SetError(string message, string innerMessage = "")
        {
            LabelError.Text = $"{message} {(string.IsNullOrEmpty(innerMessage) ? "" : $"[{innerMessage}]")}";
            LabelError.ForeColor = System.Drawing.Color.Red;
        }

        private void ResetError()
        {
            LabelError.Text = $"";
            LabelError.ForeColor = System.Drawing.Color.Black;
        }

        private void OnErrorRaised(string errorMessage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnErrorRaised(errorMessage)));
                return;
            }


            SetError(errorMessage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            { 
                SetStateMonitoring(isPaused ? MonitoringState.Resume : MonitoringState.Running);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    string.IsNullOrEmpty(ex.InnerException?.Message) ? "Error" : ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SetStateMonitoring(MonitoringState.Pause);
                isPaused = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    string.IsNullOrEmpty(ex.InnerException?.Message) ? "Error" : ex.Message);
            }
        }

        private void SetStateMonitoring(MonitoringState state)
        {
            _monitoringState = state;

            if (state is MonitoringState.Running or MonitoringState.Resume)
            {
                if (state is MonitoringState.Running)
                {
                    _startTime = DateTime.Now;
                }

                var address = Properties.Settings.Default.COM_ADDRESS.Trim();
                try
                { 
                    if (string.IsNullOrEmpty(address))
                    {
                        var formComSetting = new FormCOMSetting();
                        formComSetting.ShowDialog();
                        return;
                    }

                    _modbusService.Configure(address, 19200, Parity.None, 8, StopBits.Two);
                    _plcWorker.Start();

                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
                catch (IOException) // Wrong port address
                {
                    MessageBox.Show($"Could not open port {address}");
                }
                catch (InvalidOperationException) // e.g Open when it is already opened
                {
                    MessageBox.Show($"Could not open port {address}");
                }
                catch (Exception)
                {
                    MessageBox.Show($"Could not open port {address}");
                }
            }  
            else if (state is MonitoringState.Stop)
            {
                _plcWorker.Stop();
                _modbusService.Stop();
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            else if (state is MonitoringState.Pause)
            {
                _plcWorker.Pause();
                _modbusService.Pause();
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
            }
            else if (state is MonitoringState.Default)
            {
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void ResetMonitoring()
        {
            _registerDatas.Clear();
            Logger1.Clear(); 
            _registerDatas.Add(new PLCRegisterData());
            _registerDatas.Add(new PLCRegisterData());

            LabelPressureValue.Text = "0 psi";

            ChartSensor.Refresh();
        }

        private void SaveSensorData()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Plot plot = ChartSensor.Plot; 


            string plotFolder = Path.Combine(baseDir, "Plots");

            if (!Directory.Exists(plotFolder))
                Directory.CreateDirectory(plotFolder);

            var plotId = Guid.NewGuid();

            var fileName = $"{plotId}.png";

            string plotPath = Path.Combine(plotFolder, fileName);

            plot.SavePng(plotPath, 1920, 1080);




            string dataFolder = Path.Combine(baseDir, "Values");

            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            string filePath = Path.Combine(dataFolder, $"R_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{Guid.NewGuid()}.json"); 

            var data = new ExportData(fileName, _registerDatas, "");

            string json = JsonSerializer.Serialize(data, _options);

            File.WriteAllText(filePath, json);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetStateMonitoring(MonitoringState.Stop);
            SaveSensorData();
            ResetMonitoring();
            ResetError();
            isPaused = false;
        }


        private void cOMSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormCOMSetting();
            form.ShowDialog();
        }







        private void InitializeDataGridView()
        {
            button4.Enabled = false;
            DGV_Exported.Columns.Clear();
            DGV_Exported.Columns.Add("FileName", "File Name");

            foreach (DataGridViewColumn col in DGV_Exported.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void InitializeFileSystemWatcher()
        {
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            watcher = new FileSystemWatcher(targetFolder)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                IncludeSubdirectories = false,
                EnableRaisingEvents = true
            };

            watcher.Created += OnFileAdded;
            watcher.Deleted += OnFileDeleted;
            watcher.Renamed += OnFileRenamed;
            watcher.Changed += OnFileChanged;
        }

        private void OnFileAdded(object sender, FileSystemEventArgs e)
        {
            BeginInvoke(new Action(LoadFiles));
        }

        private void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            BeginInvoke(new Action(LoadFiles));
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            BeginInvoke(new Action(LoadFiles));
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            BeginInvoke(new Action(LoadFiles));
        }

        private void LoadFiles()
        {
            try
            {
                var files = new DirectoryInfo(targetFolder)
                       .GetFiles()
                       .OrderByDescending(f => f.LastWriteTime)
                       .Take(100)
                       .ToList();

                DGV_Exported.Rows.Clear();

                foreach (var file in files)
                {
                    if (file is not null)
                        DGV_Exported.Rows.Add(file.Name);
                }

                DGV_Exported.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading files: " + ex.Message);
            }
        }

        private void DGV_Exported_SelectionChanged(object sender, EventArgs e)
        {
            button4.Enabled = DGV_Exported.SelectedCells.Count > 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (DGV_Exported.CurrentCell != null)
            {
                var value = DGV_Exported.CurrentCell.Value?.ToString();

                if (value is null)
                {
                    MessageBox.Show("Table data is empty", "Information");
                    return;
                }

                var pathExportedJson = Path.Combine(targetFolder, value);
            
                if (!File.Exists(pathExportedJson))
                {
                    MessageBox.Show($"File {pathExportedJson} is not found", "Warning");
                    return;
                }


                using var exportOptions = new ExportOptions(pathExportedJson);

                if (!exportOptions.IsDisposed)
                {
                    exportOptions.ShowDialog();
                }
            }
        }
    }
}
