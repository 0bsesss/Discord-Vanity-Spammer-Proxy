using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DiscordURLSpammer
{
    public class ProxyClient
    {
        private string proxyIP;
        private int proxyPort;
        private string proxyUsername;
        private string proxyPassword;

        public ProxyClient(string proxyIP, int proxyPort, string proxyUsername, string proxyPassword)
        {
            this.proxyIP = proxyIP;
            this.proxyPort = proxyPort;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;
        }

        public void ConnectToProxy()
        {
            try
            {
                TcpClient tcpClient = new TcpClient(proxyIP, proxyPort);

                NetworkStream networkStream = tcpClient.GetStream();
                StreamWriter streamWriter = new StreamWriter(networkStream, Encoding.ASCII);
                StreamReader streamReader = new StreamReader(networkStream, Encoding.ASCII);

                string proxyAuthHeader = GetProxyAuthHeader(proxyUsername, proxyPassword);
                streamWriter.WriteLine(proxyAuthHeader);
                streamWriter.Flush();

                // Proxy bağlantısı kuruldu

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Proxy'e bağlanılamadı: " + ex.Message);
            }
        }

        private string GetProxyAuthHeader(string username, string password)
        {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
            return $"CONNECT {proxyIP}:{proxyPort} HTTP/1.1\r\nProxy-Authorization: Basic {credentials}\r\n\r\n";
        }
    }
}