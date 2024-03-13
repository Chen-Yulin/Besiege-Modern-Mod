using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modern
{
    public class WirelessManager : SingleInstance<WirelessManager>
    {
        public override string Name { get; } = "Wireless Manager";

        public Dictionary<string, List<Port>> WirelessMapperPort = new Dictionary<string, List<Port>>();
        public Dictionary<string, List<Unit>> WirelessMapperUnit = new Dictionary<string, List<Unit>>();

        public void RegisterPort(string channel, Port port)
        {
            if (!WirelessMapperPort.ContainsKey(channel))
            {
                WirelessMapperPort.Add(channel, new List<Port>());
            }
            WirelessMapperPort[channel].Add(port);
        }
        public void RegisterUnit(string channel, Unit unit)
        {
            if (!WirelessMapperUnit.ContainsKey(channel))
            {
                WirelessMapperUnit.Add(channel, new List<Unit>());
            }
            WirelessMapperUnit[channel].Add(unit);
        }
        public void UnregisterChannel(string channel)
        {
            if (WirelessMapperPort.ContainsKey(channel))
            {
                WirelessMapperPort.Remove(channel);
                return;
            }
            if (WirelessMapperUnit.ContainsKey(channel))
            {
                WirelessMapperUnit.Remove(channel);
                return;
            }
        }
        public void PassData(string key, Data data)
        {
            if (WirelessMapperPort.ContainsKey(key))
            {
                foreach (var port in WirelessMapperPort[key])
                {
                    port.MyData = data;
                }
            }
            if (WirelessMapperUnit.ContainsKey(key))
            {
                foreach (var unit in WirelessMapperUnit[key])
                {
                    unit.WirelessReceiveData(data);
                }
            }
        }
    }
}
