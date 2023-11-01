namespace netlib.records;

public record UDPSenderOptions
{
    public int ReceiverPort { get; init; }
    public bool UseBroadcast { get; init; } = false;
}