using PressureTest.Domains;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressureTest.Services.Interfaces
{
    public interface IModbusService
    {
        void Pause();
        void Stop();
        void Configure(string comPort, int baudRate, Parity parity, int dataBits, StopBits stopBits);
        short[] ReadHoldingRegisters(byte func, ushort startAddress, ushort count);
        PLCRegisterData ReadRegister();
        PLCRegisterData ReadRegister(string area, int address, int wordCount);
    }
}
