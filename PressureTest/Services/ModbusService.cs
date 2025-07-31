using FluentModbus; 
using PressureTest.Domains;
using PressureTest.Services.Interfaces;
using System.IO.Ports;
using System.Net;

namespace PressureTest.Services
{
    internal class ModbusService : IModbusService
    {
        private readonly ModbusRtuClient _modbus;
        private static string comport = string.Empty;
         
        public ModbusService()
        {
            _modbus = new(); // default station 1
        }

        public void Configure(string comPort, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            comport = comPort;
            _modbus.BaudRate = baudRate;
            _modbus.Parity = parity;
            _modbus.StopBits = stopBits; 
            _modbus.Connect(comPort, ModbusEndianness.BigEndian); 
        }

        /// <summary>
        /// function code e.g 0x03
        /// </summary>
        /// <param name="func"></param>
        /// <param name="startAddress"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException"></exception>
        public short[] ReadHoldingRegisters(byte func, ushort startAddress, ushort count)
        {
            //if (_port == null)
            //    throw new InvalidOperationException("Not configured");

            //if (!_port.IsOpen)
            //    _port.Open(); 

            //// Build Modbus RTU request: [slave][func=03][addr hi][addr lo][count hi][lo][crc lo][crc hi]
            //byte[] frame = new byte[8];
            //frame[0] = _slaveId;
            //frame[1] = func;
            //frame[2] = (byte)(startAddress >> 8);
            //frame[3] = (byte)(startAddress & 0xFF);
            //frame[4] = (byte)(count >> 8);
            //frame[5] = (byte)(count & 0xFF);
            //ushort crc = CRC16(frame, 0, 6);
            //frame[6] = (byte)(crc & 0xFF);
            //frame[7] = (byte)(crc >> 8);

            //_port.DiscardInBuffer();
            //_port.Write(frame, 0, frame.Length);

            //int respLen = 5 + 2 * count;
            //byte[] resp = new byte[respLen];
            //int read = _port.Read(resp, 0, respLen);
            //if (read != respLen)
            //    throw new IOException("Invalid response length");

            //// Verify CRC
            //ushort respCrc = CRC16(resp, 0, respLen - 2);
            //ushort recvCrc = (ushort)(resp[respLen - 2] | (resp[respLen - 1] << 8));
            //if (respCrc != recvCrc)
            //    throw new IOException("CRC error");

            //// Extract data
            //short[] values = new short[count];
            //for (int i = 0; i < count; i++)
            //{
            //    values[i] = (short)((resp[3 + i * 2] << 8) | resp[4 + i * 2]);
            //}
            //return values;

            return [];
        }
          
        public PLCRegisterData ReadRegister()
        {
            return new PLCRegisterData
            {
                RegisterAddress = "40001",
                RegisterArea = "HoldingRegister",
                RegisterValue = 1
            };
        }

        public void Pause()
        {
            if (_modbus is null)
            {
                return;
            }

            if (!_modbus.IsConnected)
            {
                return;
            }

            _modbus.Close(); 
        }

        public void Stop()
        { 
            if (_modbus is null)
            {
                return;
            }

            _modbus.Close();
        }

        /// <summary>
        /// Station e.g: 0,1,2
        /// Area e.g: M,D etc.
        /// Address e.g 0,1,2 etc.
        /// Word Count e.g 1 for 1 register.
        /// </summary> 
        /// <param name="station"></param>
        /// <param name="area"></param>
        /// <param name="address"></param>
        /// <param name="wordCount"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<PLCRegisterData> ReadRegister(string area, int address, int wordCount)
        {
            if (_modbus == null)
                throw new InvalidOperationException("Serial port is not configured.");

            if (!_modbus.IsConnected)
            { 
                _modbus.Connect(comport); 
            }

            if (!_modbus.IsConnected) 
            {
                throw new IOException("Serial port has not been open");
            } 

            var result = await _modbus.ReadHoldingRegistersAsync(1, (ushort)522, 4);

            var span = result.Span;

             
            ushort resulttotal = (ushort)((span[0] << 8) | span[1]);

            if (resulttotal < 0)
            { 
                resulttotal = 0;
            }
             
             
            return new PLCRegisterData
            {
                RegisterAddress = $"{area}{address}",
                RegisterArea = area,
                RegisterValue =(short)resulttotal
            };
        }

        public static float ConvertToFloat(Span<ushort> span, int startIndex = 0, ModbusEndian mode = ModbusEndian.BigEndian)
        {
            ushort reg1 = span[startIndex];
            ushort reg2 = span[startIndex + 1];

            byte[] bytes = new byte[4];

            switch (mode)
            {
                case ModbusEndian.BigEndian:
                    bytes[0] = (byte)(reg1 >> 8);
                    bytes[1] = (byte)(reg1 & 0xFF);
                    bytes[2] = (byte)(reg2 >> 8);
                    bytes[3] = (byte)(reg2 & 0xFF);
                    break;

                case ModbusEndian.LittleEndian:
                    bytes[0] = (byte)(reg2 & 0xFF);
                    bytes[1] = (byte)(reg2 >> 8);
                    bytes[2] = (byte)(reg1 & 0xFF);
                    bytes[3] = (byte)(reg1 >> 8);
                    break;

                case ModbusEndian.WordSwap:
                    bytes[0] = (byte)(reg2 >> 8);
                    bytes[1] = (byte)(reg2 & 0xFF);
                    bytes[2] = (byte)(reg1 >> 8);
                    bytes[3] = (byte)(reg1 & 0xFF);
                    break;

                case ModbusEndian.ByteWordSwap:
                    bytes[0] = (byte)(reg1 & 0xFF);
                    bytes[1] = (byte)(reg1 >> 8);
                    bytes[2] = (byte)(reg2 & 0xFF);
                    bytes[3] = (byte)(reg2 >> 8);
                    break;
            }

            return BitConverter.ToSingle(bytes, 0);
        }
    }

    
    
    public enum ModbusEndian
    {
        BigEndian,          // 12 34 56 78
        LittleEndian,       // 78 56 34 12
        WordSwap,           // 56 78 12 34
        ByteWordSwap        // 34 12 78 56
    }

}




