namespace netlib.records;

public record UDPReceiverOptions
{
    public int Port { get; init; }
    public bool UseBroadcast { get; init; } = false;
}