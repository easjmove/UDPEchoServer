using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPEchoServer
{
    class Program
    {
       public static void Main(string[] args)
        {
            Console.WriteLine("UDP Echo Server");

            //initialize the socket
            UdpClient socket = new UdpClient();
            //Binds is kinda like a TcpListener, we start listening for incoming UDP packets on port 65000
            socket.Client.Bind(new IPEndPoint(IPAddress.Any, 65000));

            //Keeps listening for packets forever, or until the console is closed
            while (true)
            {
                //define an Endpoint which we will use to store the Endpoint of the client
                IPEndPoint from = null;
                //Read data from a client, waits here until a client sends data
                byte[] data = socket.Receive(ref from);

                //we don't start another thread to handle the data, because the UDP protocol is not connection oriented
                //that means that one client doesn't block the server from receiving another packet
                //only in the case that the handling of the client takes some time, should we start another thread

                //Converts the bytes to a string using the UTF8 encoding, the encoding method should be the same on client and server
                string dataString = Encoding.UTF8.GetString(data);
                Console.WriteLine("Received from client: " + dataString + " - " + from.Address);

                //Adds move to the received string, if the client send a broadcast, to tell which server it came from
                dataString = "move: " + dataString;
                //converts the string back to a byte array using the same encoding as before (UTF8)
                byte[] toBeSend = Encoding.UTF8.GetBytes(dataString);
                //sends back the data to the clietn, using the Endpoint we stored earlier
                socket.Send(toBeSend, toBeSend.Length, from);
            }
        }
    }
}
