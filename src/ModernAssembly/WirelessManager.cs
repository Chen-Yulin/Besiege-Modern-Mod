using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modern
{
    public class WirelessManager : SingleInstance<WirelessManager>
    {
        public override string Name { get; } = "Wireless Manager";

        public Dictionary<string, List<Port>> WirelessMapper = new Dictionary<string, List<Port>>();

        public void RegisterPort(string channel, Port port)
        {
            if (!WirelessMapper.ContainsKey(channel))
            {
                WirelessMapper.Add(channel, new List<Port>());
            }
            WirelessMapper[channel].Add(port);
        }
        public void UnregisterChannel(string channel)
        {
            if (WirelessMapper.ContainsKey(channel))
            {
                WirelessMapper.Remove(channel);
            }
        }
        public void PassData(string key, Data data)
        {
            if (WirelessMapper.ContainsKey(key))
            {
                foreach (var port in WirelessMapper[key])
                {
                    port.MyData = data;
                }
            }
        }
    }
}
