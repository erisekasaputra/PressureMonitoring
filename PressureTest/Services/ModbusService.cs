using PressureTest.Domains;
using PressureTest.Services.Interfaces;
using System.IO.Ports; 

namespace PressureTest.Services
{
    internal class ModbusService : IModbusService
    { 
        private readonly Random random = new();
        private SerialPort? _port;
        private byte _slaveId;
        private TaskCompletionSource<string>? _responseTcs;
        private readonly object _lock = new();

        public void Configure(string comPort, int baudRate, Parity parity, int dataBits, StopBits stopBits, byte slaveId)
        {
            _slaveId = slaveId;
            _port = new SerialPort(comPort, baudRate, parity, dataBits, stopBits)
            {
                ReadTimeout = 1000,
                WriteTimeout = 1000
            };
            _port.DataReceived += OnDataReceived;
            _port.Open();
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
            if (_port == null)
                throw new InvalidOperationException("Not configured");

            if (!_port.IsOpen)
                _port.Open(); 

            // Build Modbus RTU request: [slave][func=03][addr hi][addr lo][count hi][lo][crc lo][crc hi]
            byte[] frame = new byte[8];
            frame[0] = _slaveId;
            frame[1] = func;
            frame[2] = (byte)(startAddress >> 8);
            frame[3] = (byte)(startAddress & 0xFF);
            frame[4] = (byte)(count >> 8);
            frame[5] = (byte)(count & 0xFF);
            ushort crc = CRC16(frame, 0, 6);
            frame[6] = (byte)(crc & 0xFF);
            frame[7] = (byte)(crc >> 8);

            _port.DiscardInBuffer();
            _port.Write(frame, 0, frame.Length);

            int respLen = 5 + 2 * count;
            byte[] resp = new byte[respLen];
            int read = _port.Read(resp, 0, respLen);
            if (read != respLen)
                throw new IOException("Invalid response length");

            // Verify CRC
            ushort respCrc = CRC16(resp, 0, respLen - 2);
            ushort recvCrc = (ushort)(resp[respLen - 2] | (resp[respLen - 1] << 8));
            if (respCrc != recvCrc)
                throw new IOException("CRC error");

            // Extract data
            short[] values = new short[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = (short)((resp[3 + i * 2] << 8) | resp[4 + i * 2]);
            }
            return values;
        }
          
        public PLCRegisterData ReadRegister()
        {
            return new PLCRegisterData
            {
                RegisterAddress = "40001",
                RegisterArea = "HoldingRegister",
                RegisterValue = (short)(random.Next(0, 100))
            };
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
        public async Task<PLCRegisterData> ReadRegister(int station, string area, int address, int wordCount)
        {
            if (_port == null)
                throw new InvalidOperationException("Serial port is not configured.");

            if (!_port.IsOpen)
            {
                _port.Open();
            }

            string command = $"@{station:D2}R{area}{address:D4}{wordCount:D2}\r";

            lock (_lock)
            {
                _responseTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
                _port.DiscardInBuffer();
                _port.Write(command);
            }

            using var timeoutCts = new CancellationTokenSource(1000); // 1 detik timeout
            using (timeoutCts.Token.Register(() => _responseTcs.TrySetCanceled()))
            {
                string response = await _responseTcs.Task;
                string hex = response[..4]; // Ambil 4 digit pertama (misalnya "0032")
                short value = Convert.ToInt16(hex, 16);

                return new PLCRegisterData
                { 
                    RegisterAddress = $"{area}{address}",
                    RegisterArea = area,
                    RegisterValue = value
                };
            }
        }

        private static ushort CRC16(byte[] buf, int start, int len)
        {
            ushort crc = 0xFFFF;
            for (int pos = start; pos < start + len; pos++)
            {
                crc ^= buf[pos];
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x0001) != 0) crc = (ushort)((crc >> 1) ^ 0xA001);
                    else crc >>= 1;
                }
            }
            return crc;
        }

        private void OnDataReceived(object? sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (_port == null)
                    throw new InvalidOperationException("Port is not configured.");

                string data = _port.ReadExisting();
                lock (_lock)
                {
                    _responseTcs?.TrySetResult(data);
                }
            }
            catch
            {
                _responseTcs?.TrySetException(new IOException("Failed reading response."));
            }
        }
    }
}
