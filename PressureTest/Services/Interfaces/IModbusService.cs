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
        void Configure(string comPort, int baudRate, Parity parity, int dataBits, StopBits stopBits, byte slaveId);
        short[] ReadHoldingRegisters(byte func, ushort startAddress, ushort count);
        PLCRegisterData ReadRegister();
        Task<PLCRegisterData> ReadRegister(int station, string area, int address, int wordCount);
    }
}
