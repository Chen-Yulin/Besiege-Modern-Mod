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

        public int newKey = 1;

        public int AddPort(Port port, int key) // return the actual mapper key
        {
            if (PortMap.ContainsKey(key))
            {
                if (PortMap[key] == port)
                {
                    Debug.Log("Duplicate port added to mapper");
                    return key;
                }
                
                Port originPort = PortMap[key];
                PortMap.Remove(key);

                // update the reference to the existing port
                foreach (var p in PortMap.Values)
                {
                    if (p.IO)
                    {
                        //output
                        p.UpdateDistKey(key, newKey);
                    }
                    else
                    {
                        //input
                        p.UpdateSrcKey(key, newKey);

                    }
                }

                // reallocate the key for the existing port
                PortMap.Add(newKey, originPort);
                if (originPort.IO)
                {
                    originPort.parentUnit.SaveOutputKey(originPort.Index, newKey);
                }
                else
                {
                    originPort.parentUnit.SaveInputKey(originPort.Index, newKey);
                }
                

                

                // Add the new port to the mapper
                PortMap.Add(key, port);
                //Debug.Log("Port key " + key + " already exists, adding port to key " + newKey);
                newKey++;
                return key;
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
