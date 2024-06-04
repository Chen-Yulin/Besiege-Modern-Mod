using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Modern
{
    class TempTextureManager : SingleInstance<TempTextureManager>
    {
        uint id = 0;
        public uint RegisterID()
        {
            //Debug.Log("register id:" + id);
            id++;
            return id;
        }

        public override string Name { get; } = "Temp Texture Manager";

        public Dictionary<uint, Texture2D> textures = new Dictionary<uint, Texture2D>();

        public void GetTextureCopy(Texture2D src, uint ID)
        {

            if (textures.ContainsKey(ID))
            {
                Destroy(textures[ID]);
                textures[ID] = new Texture2D(src.width, src.height, TextureFormat.ARGB32, false);
                textures[ID].hideFlags = HideFlags.HideAndDontSave;
                Graphics.CopyTexture(src, textures[ID]);
            }
            else
            {
                //Debug.Log("Create new Texture with" + ID.ToString());
                Texture2D img = new Texture2D(src.width, src.height, TextureFormat.ARGB32, false);
                img.hideFlags = HideFlags.HideAndDontSave;
                Graphics.CopyTexture(src, img);
                textures.Add(ID, img);
            }
        }

        public Texture2D GetTexture(uint ID)
        {
            if (textures.ContainsKey(ID))
            {
                return textures[ID];
            }
            else
            {
                return null;
            }
        }

    }
}
