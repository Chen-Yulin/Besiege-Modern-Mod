using Modding.Blocks;
using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modern
{
    class CustomBlockController : SingleInstance<CustomBlockController>
    {

        public override string Name { get; } = "Custom Block Controller";

        internal PlayerMachineInfo PMI;

        private void Awake()
        {
            Events.OnMachineLoaded += (pmi) => { PMI = pmi; };
            Events.OnBlockInit += AddSliders;

        }
        private void AddSliders(Block block)
        {

            BlockBehaviour blockbehaviour = block.BuildingBlock.InternalObject;
            AddSliders(blockbehaviour);
        }
        private void AddSliders(BlockBehaviour block)
        {
            switch (block.BlockID)
            {
                case (int)BlockType.SteeringHinge:
                    {
                        if (block.gameObject.GetComponent(typeof(HingeDriver)) == null)
                            block.gameObject.AddComponent(typeof(HingeDriver));
                        break;
                    }
                
                default:
                    {
                        break;
                    }
            }
        }
    }
}
