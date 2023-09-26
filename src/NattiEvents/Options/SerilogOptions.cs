namespace NattiEvents.Options;

public class SerilogOptions
{
    public const string Serilog = "Serilog";
    public bool EnableConsole { get; set; }
    public bool EnableSeq { get; set; }
}
