using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Y_TcpServer
{
    internal class StreamServerListener
    {
        private readonly TcpListener _tcpListener;
        private Socket _socket;
        private readonly Thread _waitForConnection;

        public const string EndConnectionCommand = "BYE";

        public event EventHandler<StringReadyEventArgs> NewStringRecieved;
        public event EventHandler<EventArgs> ConnectionClosed;

        public class StringReadyEventArgs : EventArgs
        {
            public string NewString { get; private set; }

            public StringReadyEventArgs(string s)
            {
                NewString = s;
            }
        }

        public StreamServerListener()
        {
            try
            {
                var ipAd = IPAddress.Parse("127.0.0.1");

                _tcpListener = new TcpListener(ipAd, 8341);

                // Start listeneting
                _tcpListener.Start();

                _waitForConnection = new Thread(WaitingForClient);
                _waitForConnection.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
            finally
            {
                Console.ReadLine(); //Pause
            }
        }

        public void CloseConnection()
        {
            _tcpListener.Stop();
            // Kill thread
            if (_waitForConnection.IsAlive)
            {
                _waitForConnection.Abort();
                _waitForConnection.Join(10);
            }
        }

        private void WaitingForClient()
        {
            try
            {
                // Accept will block until someone connects
                _socket = _tcpListener.AcceptSocket();

                ReadSocket();
            } catch(ThreadAbortException e)
            {
                try
                {
                    _socket.Close();
                } catch (Exception) {}
                Console.WriteLine(e.Message);
            }

        }

        private void ReadSocket()
        {
            
            try
            {
                // Will read until connection ends
                while (true)
                {
                    if (!_socket.Connected)
                        break;

                    var receive = new byte[2000];
                    int ret = _socket.Receive(receive, receive.Length, 0);
                    if (ret > 0)
                    {
                        string tmp = Encoding.ASCII.GetString(receive).Replace("\0", "");

                        if (tmp.Length > 0)
                        {
                            if(tmp == EndConnectionCommand)
                            {
                                // End connection here.
                                if (ConnectionClosed != null)
                                    ConnectionClosed.Invoke(this, new EventArgs());
                                break;
                            }
                            // Throw stuff recieved event!
                            NewStringRecieved.Invoke(this, new StringReadyEventArgs(tmp));
                        }
                    }
                }
                // Attempt to reuse socket
                 _socket.Disconnect(true);

            } catch(ThreadAbortException)
            {
                // Do nothing: Thread is ending
                return;
            }
            catch (Exception) { WaitingForClient(); }
            WaitingForClient();
        }
    }
}
