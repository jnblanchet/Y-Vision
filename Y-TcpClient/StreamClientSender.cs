using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Y_TcpClient
{
    public class StreamClientSender
    {
        private TcpClient _tcpSender;
        private readonly Mutex _socketLock;
        private readonly ASCIIEncoding _encoder;
        private Stream _stream;

        public const string EndConnectionCommand = "BYE";

        private static readonly IPAddress IpAd = IPAddress.Parse("127.0.0.1");
        private const int Port = 8341;

        private bool _ended;
        private bool _isReconnecting;

        public StreamClientSender()
        {
            _socketLock = new Mutex(false);
            _encoder = new ASCIIEncoding();
            _ended = false;
            try
            {
                _isReconnecting = true;
                new Thread(AttemptReconnect).Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        public void CloseConnection()
        {
            // Kill thread
            _ended = true;
            SendData(EndConnectionCommand);
            Thread.Sleep(100); // TODO: This should be a thread.join()
            _tcpSender.Close();
        }

        /// <summary>
        /// Returns true if socket is ready to send.
        /// </summary>
        public bool SendAsync(string data)
        {
            if (_tcpSender != null && _tcpSender.Connected)
            {
                var t = new Thread(SendData);
                t.Start(data);
                return true;
            }
            else
            {
                if (!_isReconnecting)
                    new Thread(AttemptReconnect).Start();
            }
            return false;
        }

        private void SendData(Object data)
        {
            try
            {
                _socketLock.WaitOne(20);
                var s = (string) data;

                byte[] ba = _encoder.GetBytes(s);
                if (_tcpSender.Connected)
                {
                    _stream.Write(ba, 0, ba.Length);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                try
                {
                    _socketLock.ReleaseMutex();
                }
                catch (Exception)
                {
                    // If the application is closing, the mutex release will probably crash
                }
            }
        }

        private void AttemptReconnect()
        {
            _isReconnecting = true;
            _tcpSender = new TcpClient();
            while (!_ended && !_tcpSender.Connected)
            {
                try
                {
                    _tcpSender.Connect(IpAd, Port);
                    _stream = _tcpSender.GetStream();
                }
                catch (Exception)
                {
                }
                Thread.Sleep(50);
            }
            _isReconnecting = false;
        }
    }
}
