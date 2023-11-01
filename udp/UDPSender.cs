using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using netlib.interfaces;
using netlib.records;

namespace netlib.udp;

public class UDPSender(IOptions<UDPSenderOptions> options, ILogger<UDPSender> logger) : ISender
{
    private UdpClient sender = new UdpClient();

    private bool useBroadcast = options.Value.UseBroadcast;

    private int port = options.Value.ReceiverPort;

    public void SendBroadcastMessage(byte[] message)
    {
        if (message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var receiver = new IPEndPoint(IPAddress.Broadcast, port);

        try
        {
            int sent = sender.Send(message, receiver);
        }
        catch (SocketException)
        {
            logger.LogError("Cannot connect to {address}/{port}", receiver.Address.ToString(), receiver.Port.ToString());
        }
    }

    public void SendMessage(byte[] message, IPEndPoint receiver)
    {
        if (message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (receiver is null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        try
        {
            sender.Send(message, receiver);
        }
        catch (SocketException)
        {
            logger.LogError("Cannot connect to {address}/{port}", receiver.Address.ToString(), receiver.Port.ToString());
        }
    }
}