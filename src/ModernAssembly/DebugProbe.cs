using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class DebugProbe : SingleInstance<DebugProbe>
    {
        public override string Name { get; } = "Debug Probe";

        public bool Enabled = false;

        public string debugMsg = null;

        public GUIStyle style;

        private bool style_init;

        public void Awake()
        {
        }
        public void Start()
        {
        }

        public void Update()
        {
            if (Enabled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Tool.RaycastAllSorted(ray, 50f);
                if (hits.Length > 0)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.collider.isTrigger)
                        {
                            continue;
                        }
                        Unit unit;
                        try
                        {
                            unit = hit.collider.transform.parent.parent.GetComponent<Unit>();
                        }
                        catch
                        {
                            break;
                        }
                        if (unit)
                        {
                            debugMsg = unit.DebugString();
                            break;
                        }
                        else
                        {
                            debugMsg = null;
                            break;
                        }
                    }
                }
            }
            else
            {
                debugMsg = null;
            }
        }

        public void OnGUI()
        {
            if (!style_init)
            {
                style_init = true;
                style = new GUIStyle(GUI.skin.box);
                style.alignment = TextAnchor.UpperLeft;
            }
            if (Enabled)
            {
                if (debugMsg != null)
                {
                    GUIContent content = new GUIContent(debugMsg);
                    float height = GUI.skin.textArea.CalcHeight(content, 150f);
                    Vector3 pos = Input.mousePosition;
                    GUI.Box(new Rect(pos.x + 50, Screen.height - pos.y + 50, 150, height), debugMsg, style);
                }
            }
        }
    }
}
