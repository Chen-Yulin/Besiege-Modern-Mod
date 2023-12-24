using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    class ModController : SingleInstance<ModController>
    {
        public override string Name { get; } = "WW2NavalModController";

        private Rect windowRect = new Rect(15f, 100f, 280f, 50f);
        private readonly int windowID = ModUtility.GetWindowId();
        public bool windowHidden = false;
        private bool _wireConnector = false;

        public bool WireConnector
        {
            get
            {
                return _wireConnector;
            }
            set
            {
                _wireConnector = value;
                Connector.Instance.Enabled = value;
            }
        }

        private void Awake()
        {

        }

        public void Start()
        {
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    windowHidden = !windowHidden;
                }

            }
        }

        private void MACWindow(int windoID)
        {
            GUILayout.BeginVertical();
            {
                WireConnector = GUILayout.Toggle(WireConnector, "Use Wire Connector");
                GUILayout.Label("Press Ctrl+M to hide");
            }

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUI.DragWindow();

        }

        private void OnGUI()
        {
            if (!windowHidden && !StatMaster.hudHidden)
            {
                windowRect = GUILayout.Window(windowID, windowRect, new GUI.WindowFunction(MACWindow), "Modern Tool Box");
            }
        }

    }
}
