using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Papper
{
    public class PlcConnectionNotificationEventArgs : EventArgs
    {
        private readonly string _from;
        private readonly bool _isConnected;
        public PlcConnectionNotificationEventArgs(string from, bool connected)
        {
            _from = from;
            _isConnected = connected;
        }

        public string From { get { return _from; } }
        public bool IsConnected { get { return _isConnected; } }

    }

}
