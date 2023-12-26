using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modding.Modules;
using Modding;
using Modding.Blocks;
using UnityEngine;
using UnityEngine.Networking;
using Modding.Blocks;

namespace Modern
{
    public class WireManager : SingleInstance<WireManager>
    {
        public override string Name { get; } = "Wire Manager";

        public Dictionary<int, Port> PortMap = new Dictionary<int, Port>();

        public int newKey = 0;

        public int AddPort(Port port, int key) // return the actual mapper key
        {
            if (PortMap.ContainsKey(key))
            {
                PortMap.Add(newKey, port);
                //Debug.Log("Port key " + key + " already exists, adding port to key " + newKey);
                newKey++;
                return newKey - 1;
            }
            else
            {
                PortMap.Add(key, port);
                //Debug.Log("Port key " + key + " added to mapper");
                newKey = Mathf.Max(newKey, key + 1);
                return key;
            }
        }
        public void RemovePort(int key)
        {
            if (PortMap.ContainsKey(key))
            {
                PortMap.Remove(key);
                //Debug.Log("Port key " + key + " removed from mapper");
            }
        }

        public Port GetPort(int key)
        {
            if (PortMap.ContainsKey(key))
            {
                return PortMap[key];
            }
            else
            {
                return null;
            }
        }

    }
}
