using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using netlib.eventargs;
using netlib.interfaces;
using netlib.records;

namespace netlib.udp;

internal class UDPReceiver(IOptions<UDPReceiverOptions> options, ILogger<UDPReceiver> logger) : IReceiver
{
    private bool exit = false;

    private Thread thread;

    private IPAddress iPAddress;

    private int port = options.Value.Port;

    private bool useBroadcast = options.Value.UseBroadcast;

    public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

    public IPAddress IPAddress
    {
        get
        {
            return this.iPAddress;
        }

        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(this.IPAddress), "The specified parameter must not be null.");
            }

            this.iPAddress = value;
        }
    }

    public void Start()
    {
        if (this.thread != null && this.thread.IsAlive)
        {
            throw new InvalidOperationException($"The {nameof(UDPReceiver)} can not be startet it is already running!");
        }

        this.thread = new Thread(this.Worker)
        {
            Name = $"{nameof(UDPReceiver)}"
        };

        this.exit = false;
        this.thread.Start();
        this.thread.IsBackground = false;
    }

    public void Stop()
    {
        if (this.thread == null || !this.thread.IsAlive)
        {
            throw new InvalidOperationException($"The {nameof(UDPReceiver)} can not " +
                $"be stopped it is not running yet!");
        }

        this.exit = true;
    }

    private async void Worker()
    {
        using UdpClient client = new(this.port);
        this.IPAddress = ((IPEndPoint)client.Client.LocalEndPoint).Address;

        client.EnableBroadcast = useBroadcast;

        while (!this.exit)
        {
            var broadcast = new IPEndPoint(IPAddress.Any, port);
            UdpReceiveResult result = await client.ReceiveAsync();
            byte[] datagram = result.Buffer;
            var sender = result.RemoteEndPoint;

            FireMessageReceived(new MessageReceivedEventArgs(datagram, sender));
        }
    }

    protected virtual void FireMessageReceived(MessageReceivedEventArgs arguments)
    {
        if (arguments is null)
        {
            throw new ArgumentNullException(nameof(arguments), "The specified parameter must not be null.");
        }

        this.OnMessageReceived?.Invoke(this, arguments);
    }
}