using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Modern
{
    public class AttachedExecuter : MonoBehaviour
    {
        public BlockBehaviour BB;

        public MToggle AsExecuter;

        protected bool _asExecuterChanged = false;

        public MToggle OnBoard;
        public MText Channel;

        protected bool _onBoardChanged;

        public bool onboard;

        public void AsExecuterChangeHandler(bool asExecuter)
        {
            if (!asExecuter)
            {
                OnBoard.DisplayInMapper = false;
                Channel.DisplayInMapper = false;
            }
            else
            {
                OnBoard.DisplayInMapper = true;
                Channel.DisplayInMapper = OnBoard.isDefaultValue;
            }
        }
        public void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
            SafeAwake();

            if (BB.isSimulating) { return; }
        }
        public void SafeAwake()
        {
            AsExecuter = BB.AddToggle("As Executer", "AsExecuter", "As Executer", false);
            OnBoard = BB.AddToggle("On Board", "OnBoard", "On Board", false);
            Channel = BB.AddText("Receive Channel", "Channel", "");
            AsExecuter.Toggled += (bool value) =>
            {
                _asExecuterChanged = true;
            };
            OnBoard.Toggled += (bool value) =>
            {
                _onBoardChanged = true;
            };

            // left for customization
            ExecuterSafeAwake();
        }
        public virtual void ExecuterBuildUpdate()
        {
            return;
        }
        public virtual void ExecuterSimulateFixedUpdate()
        {
            return;
        }
        public virtual void ExecuterSimulateUpdate()
        {

        }
        public virtual void ExecuterSimulateStart()
        {

        }
        public virtual void ExecuterSimulateStop()
        {

        }
        public virtual void ExecuterSafeAwake()
        {
            return;
        }
        public virtual void ExecuterUpdateOuput(Port Caller)
        {
            return;
        }
    }
}
