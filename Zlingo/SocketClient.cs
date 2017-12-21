using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Zlingo
{
    class SocketClient
    {
        private int Aantal_Teams = 0;

        private readonly string _ip;
        private readonly int _port;
        private StreamSocket _socket;
        private DataWriter _writer;
        private DataReader _reader;
        
        public delegate void DataRecived(string data);
        public event DataRecived OnDataRecived;

        public string Ip { get { return _ip; } }
        public int Port { get { return _port; } }

        public SocketClient(string ip, int port, int teamaantal)
        {
            Aantal_Teams = teamaantal;
            _ip = ip;
            _port = port;
            Task.Run(() => Connect()).Wait();
            
        }

        public void Connect() // Maakt een nieuwe client(speler) aan op basis van ontvangen IP-adres
        {
            
            try
            {
                var hostName = new HostName(Ip);
                _socket = new StreamSocket();
                if (Aantal_Teams == 0)  _socket.ConnectAsync(hostName, Port.ToString());
                else  _socket.ConnectAsync(hostName, Port.ToString());
                _writer = new DataWriter(_socket.OutputStream);
            }
            catch (Exception ex)
            { }
        }

        public async void Send(string message) // Verstuurt een bericht
        {
            _writer.WriteUInt32(_writer.MeasureString(message));
            _writer.WriteString(message);

            try
            {
                await _writer.StoreAsync();
                await _writer.FlushAsync();
            }
            catch (Exception ex)
            { }
        }

        
    }
}
