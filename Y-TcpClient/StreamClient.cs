using System.Windows.Forms;

namespace Y_TcpClient
{
    class StreamClient
    {
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StreamingClient());
        }
    }
}
