using System.Net;

namespace netlib.eventargs;

internal class MessageReceivedEventArgs
{
    private byte[] message;
    private IPEndPoint sender;
    public MessageReceivedEventArgs(byte[] message, IPEndPoint sender)
    {
        this.Message = message;
        this.Sender = sender;
    }

    public byte[] Message
    {
        get
        {
            return this.message;
        }

        private set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(this.Message), "The specified parameter must not be null.");
            }

            this.message = value;
        }
    }

    public IPEndPoint Sender
    {
        get
        {
            return this.sender;
        }

        private set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(this.Sender), "The specified parameter must not be null.");
            }

            this.sender = value;
        }
    }
}