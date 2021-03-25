using System;
using System.Collections.Generic;
using System.Text;

namespace CursorBot.RiotSharpFunctions
{
    public class RiotApiKey
    {
        private static RiotApiKey _instance;
        private static readonly object _lock = new object();

        private RiotApiKey() { }

        public static RiotApiKey Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new RiotApiKey();
                    }
                    return _instance;
                }
            }
        }

        private string riotKey;
        public string RiotKey
        {
            get { return riotKey; }
            set { riotKey = value; }
        }

        private string version;
        public string Version
        {
            get { return version; }
            set { version = value; }
        }
    }
}
