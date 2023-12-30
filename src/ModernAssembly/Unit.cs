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
    public class Unit : BlockScript
    {
        public int InputNum = 1;
        public int OutputNum = 1;

        public List<Port> Inputs = new List<Port>();
        public List<Port> Outputs = new List<Port>();

        public MSlider[] InputChannel;
        public MSlider[] InputSrcChannel;
        public MSlider[] OutputChannel;

        public virtual void InitInputPorts()
        {
            return;
        }   
        public virtual void InitOutputPorts()
        {
            return;
        }

        public virtual void UpdateUnit()
        {
            return;
        }

        public virtual int GetPortKey(bool IO, int index)
        {
            return 0;
        }

        public virtual void SaveInputSrcKey(int index, int key)
        {
            return;
        }

        public virtual void SaveInputKey(int index, int key)
        {
            return;
        }
        public virtual void SaveOutputKey(int index, int key)
        {
            return;
        }
    }
}
