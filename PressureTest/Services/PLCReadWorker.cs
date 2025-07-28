using PressureTest.Domains;
using PressureTest.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureTest.Services
{
    internal class PLCReadWorker : IPLCReadWorker
    {
        private readonly BackgroundWorker _worker;
        private readonly IModbusService _modbusService;
        private bool _stopRequested = false;  

        public Action<PLCRegisterData>? OnDataReceived { get; set; }
        public Action<string>? OnErrorRaised { get; set; }

        public PLCReadWorker(IModbusService modbusService)
        {
            _modbusService = modbusService;

            _worker = new BackgroundWorker();
            _worker.DoWork += Worker_DoWork;
        }

        public void Start()
        {
            _stopRequested = false;
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();
        }

        public void Stop()
        {
            _stopRequested = true; 
        }

        public void Pause()
        { 
            _stopRequested = true;
        }

        private async void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            while (!_stopRequested)
            {
                try
                {
                    var data = _modbusService.ReadRegister("D", 100, 1);
                    OnDataReceived?.Invoke(data);

                    await Task.Delay(100);
                }
                catch (IOException ex)
                {
                    OnErrorRaised?.Invoke(ex.Message);
                }
            }
        } 
    }
}
