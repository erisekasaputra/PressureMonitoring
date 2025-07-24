namespace PressureTest.Domains;

public class PLCRegisterData
{
    public string RegisterAddress { get; set; }
    public string RegisterArea { get; set; }
    public Int16 RegisterValue { get; set; }

    public PLCRegisterData()
    {
        RegisterValue = 0;
        RegisterAddress = string.Empty;
        RegisterArea = string.Empty;
    }

    public PLCRegisterData(
        string registerAddress,
        string registerArea,
        Int16 registerValue)
    {
        RegisterAddress = registerAddress;
        RegisterArea = registerArea;
        RegisterValue = registerValue;
    }
}
