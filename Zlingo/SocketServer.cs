using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Zlingo
{
    class SocketServer
    {
        private readonly int _port;
        public int Port { get { return _port; } }


        public StreamSocketListener listener;

        public int Testmodus = 0;
        public delegate void DataRecived(string data);
        public event DataRecived OnDataRecived;
        public List<SocketClient> DeelnemendeTeams = new List<SocketClient>();
        public List<SocketClient> Teams;
        private SocketClient client1;
        private SocketClient client2;
        private SocketClient client3;
        private SocketClient client4;
        private string Client_IP = "";
        public int Begonnen = 0; // Als het spel begint wordt Begonnen 1
        private int Aantal_Teams = 0;
        private PinsInit pin;
        public string data;
        public Spel nieuwspel;

        public int controleModus = 0; // 0 is uitgeschakelde modus, 1 is ingeschakeld (Om een woord goed te keuren)

        public SocketServer(int port, PinsInit pins) // Server biedt plaats voor 4 clients
        {
            Teams = new List<SocketClient>() { client1, client2, client3, client4 };
            pin = pins;
            _port = port;
            Star();
        }

        /// <summary>
        /// Wanneer er data binnenkomt start deze functie.
        /// Als de data een IP-adres bevat wordt er een nieuwe client(speler) aangemaakt.
        /// Zo niet, wordt de data (het antwoord) getoont en kan het gecontroleerd worden.
        /// </summary>
        /// <param name="bericht">Ontvangen string</param>
        public void Server_OnDataRecived(string bericht) 
        {
            data = bericht;
            
            IPAddress addr;
            if (IPAddress.TryParse(data, out addr))
            {
                if (data != Client_IP)
                {
                    Client_IP = data;
                    for (int i = 0; i < Teams.Count; i++)
                    {
                        if (Teams[i] == null)
                        {
                            Teams[i] = new SocketClient(Client_IP, 9000, Aantal_Teams);
                            DeelnemendeTeams.Add(Teams[i]);
                            i = Teams.Count;
                        }
                    }
                    Aantal_Teams += 1;
                    pin.lcd.ClearDisplay();
                    Task.Delay(5).Wait();
                    pin.lcd.Write("Team " + Aantal_Teams + " doet mee!");
                }
            }

            else
            {
                pin.lcd.ClearDisplay();
                Task.Delay(5).Wait();
                pin.lcd.SetCursorPosition(0, 0);
                pin.lcd.Write(nieuwspel.Speelwoord);
                pin.lcd.SetCursorPosition(1, 0);
                pin.lcd.Write((nieuwspel.teamnummer+1) + ": " + data);
                controleModus = 1;
                
            }
        }

        /// <summary>
        /// Als het antwoord geen Nederlands woord is gaat de beurt over.
        /// Als het aantal rondes is bereikt wordt de winnaar getoont en verstuurd naar alle teams.
        /// </summary>
        public void NietBestaandAntwoord() 
        {
            Teams[nieuwspel.teamnummer].Send("Wissel");
            nieuwspel.Wisselbeurt();
            if (nieuwspel.rondenummer != 10) Teams[nieuwspel.teamnummer].Send(nieuwspel.Showwoord);
            else
            {
                nieuwspel.Dataopslag();
                pin.lcd.SetCursorPosition(0, 0); pin.lcd.Write("En de winnaar is....");
                pin.lcd.SetCursorPosition(1, 0); pin.lcd.Write("Team " + (nieuwspel.TeamScores.IndexOf(nieuwspel.TeamScores.Max()) + 1) + ": " + nieuwspel.TeamScores.Max() + " punten!");
                for (int i = 0; i < DeelnemendeTeams.Count; i++) DeelnemendeTeams[i].Send(" Team" + (nieuwspel.TeamScores.IndexOf(nieuwspel.TeamScores.Max()) + 1) + nieuwspel.TeamScores.Max());
                Begonnen = 0;
            }
            controleModus = 0;
        }

        /// <summary>
        /// Het antwoord wordt gecontroleerd, waarna het juiste signaal wordt doorgestuurd.
        /// "Wissel" = 5 verkeerde antwoorden op rij, "WisselCorrect" = 3 woorden achter elkaar goed. "Correct" = 1 woord goed.
        /// Als het aantal rondes is bereikt wordt de winnaar getoont en verstuurd naar alle teams.
        /// </summary>
        public void CommunicationOrders()
        {
            pin.lcd.SetCursorPosition(1, 2);
            pin.lcd.Write("      ");
            pin.lcd.SetCursorPosition(1, 2);
            nieuwspel.Controle(data);
            if ((nieuwspel.teamnummer - 1) < 0 & nieuwspel.Wissel != 0)
            {
                if (nieuwspel.Wissel == 1) Teams[DeelnemendeTeams.Count - 1].Send("Wissel");
                else if (nieuwspel.Wissel == 2) Teams[DeelnemendeTeams.Count - 1].Send("WisselCorrect");
                nieuwspel.Wissel = 0;
            }
            else if (nieuwspel.Wissel != 0)
            {
                if (nieuwspel.Wissel == 1) Teams[nieuwspel.teamnummer - 1].Send("Wissel");
                else if (nieuwspel.Wissel == 2) Teams[nieuwspel.teamnummer - 1].Send("WisselCorrect"); 
                nieuwspel.Wissel = 0;
            }
            else if (nieuwspel.Correct == 1) { Teams[nieuwspel.teamnummer].Send("Correct"); nieuwspel.Correct = 0; }
            if (nieuwspel.rondenummer != 10) Teams[nieuwspel.teamnummer].Send(nieuwspel.Showwoord + "  " + nieuwspel.BijnaGoed);
            else 
            {
                nieuwspel.Dataopslag();
                pin.lcd.SetCursorPosition(0, 0); pin.lcd.Write("En de winnaar is....");
                pin.lcd.SetCursorPosition(1, 0); pin.lcd.Write("Team " + (nieuwspel.TeamScores.IndexOf(nieuwspel.TeamScores.Max()) + 1) + ": " + nieuwspel.TeamScores.Max() + " punten!");
                for (int i = 0; i < DeelnemendeTeams.Count; i++) DeelnemendeTeams[i].Send(" Team" + (nieuwspel.TeamScores.IndexOf(nieuwspel.TeamScores.Max()) + 1) + nieuwspel.TeamScores.Max());
                Begonnen = 0;
            }
            controleModus = 0;
        }


        public async void Star() // Een listener wordt aangemaakt om te wachten op data.
        {
            pin.lcd.Write("Wacht op teams..");
            listener = new StreamSocketListener();
            listener.ConnectionReceived += Listener_ConnectionReceived;

            await listener.BindServiceNameAsync(Port.ToString());
           
        }

        public void StartSpel() // Een nieuw spel wordt aangemaakt, waarna de beginnende speler een speelwoord krijgt toegestuurd.
        {
            if (Testmodus == 1) for (int i = 0; i < DeelnemendeTeams.Count; i++) DeelnemendeTeams[i].Send("Testrun");
            nieuwspel = new Spel(pin, Aantal_Teams);
            for (int y = 0; y < DeelnemendeTeams.Count; y++) DeelnemendeTeams[y].Send(Teams.IndexOf(Teams[y]).ToString()+"Nieuwspel");
            DeelnemendeTeams[nieuwspel.teamnummer].Send(nieuwspel.Showwoord);
        }
        
        /// <summary>
        /// Zodra de gegevens kloppen wordt de OnDataRecived method van hierboven aangeroepen met als parameter de verstuurde data.
        /// </summary>
        public async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            var reader = new DataReader(args.Socket.InputStream);
            try
            {
                while (true)
                {
                    uint sizeFieldCount = await reader.LoadAsync(sizeof(uint));
                    //if disconnected 
                    if (sizeFieldCount != sizeof(uint))
                        return;
                    uint stringLenght = reader.ReadUInt32();
                    uint actualStringLength = await reader.LoadAsync(stringLenght);
                    //if disconnected 
                    if (stringLenght != actualStringLength)
                        return;
                    if (OnDataRecived != null)
                        OnDataRecived(reader.ReadString(actualStringLength));

                }
            }
            catch (Exception ex)
            {
               
            }
        }
        
    }
}