namespace PressureTest.Domains;

public class ExportData
{
    public string ChartImagePath { get; set; } 

    public List<PLCRegisterData> RegisterValues { get; set; }

    public string? HeaderLogo { get; set; }

    public ExportData(string chartImagePath, List<PLCRegisterData> registerValues, string? headerLogo)
    {
        ChartImagePath = chartImagePath;
        RegisterValues = registerValues;

        if (!string.IsNullOrEmpty(headerLogo))
            HeaderLogo = headerLogo;
    }
}
