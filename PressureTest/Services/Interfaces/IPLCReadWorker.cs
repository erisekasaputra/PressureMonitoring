using PressureTest.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureTest.Services.Interfaces
{
    public interface IPLCReadWorker
    {
        void Start();
        void Stop();
        void Pause();
        Action<PLCRegisterData>? OnDataReceived { get; set; } 
    }
}
