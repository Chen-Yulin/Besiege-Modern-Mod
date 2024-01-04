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
    public class Connector : SingleInstance<Connector>
    {
        public override string Name { get; } = "Wire Connector";

        public bool Enabled = false;

        


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
                RaycastHit[] hits = Tool.RaycastAllSorted(ray, 20f);
                bool getBoard = false;
                if (hits.Length > 0)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.collider.isTrigger)
                        {
                            continue;
                        }
                        Board board;
                        try
                        {
                            board = hit.collider.transform.parent.parent.GetComponent<Board>();
                        }
                        catch
                        {
                            break;
                        }
                        if (board)
                        {
                            board.Spotted = true;
                            getBoard = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
            }

            

        }
        
    }
}
