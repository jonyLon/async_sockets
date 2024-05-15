using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var listener = new TcpListener(IPAddress.Loopback, 8000);
        listener.Start();
        Console.WriteLine("Server is running...");

        while (true)
        {
            var client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");
            var stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string zipCode = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);

            string response = GetStreetsByZipCode(zipCode);
            byte[] data = System.Text.Encoding.ASCII.GetBytes(response);

            stream.Write(data, 0, data.Length);
            client.Close();
        }
    }

    static string GetStreetsByZipCode(string zipCode)
    {
        var doc = XDocument.Load(@"C:\Users\user\Desktop\ШАГ\current\homework\NP_sync_sockets-1\Server\Streets.xml");
        var streets = doc.Descendants("Street")
            .Where(st => st.Attribute("ZipCode").Value == zipCode)
            .Select(st => st.Value)
            .ToArray();

        return string.Join(", ", streets);
    }
}

