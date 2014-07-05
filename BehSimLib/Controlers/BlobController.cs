using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BehSimLib.Blobs;
using Microsoft.Xna.Framework;


namespace BehSimLib.Controlers
{
    public class BlobController
    {
        public static Random random;

        protected List<Blob> blobList;


        public BlobController()
        {
            random = new Random();
            blobList = new List<Blob>();
        }

        public void AddBlob(Vector2 position)
        {
            blobList.Add(new Blob(position));
            
        }


        #region General methods
        public void Update()
        {
            foreach (var item in blobList)
            {
                
                if (item.BlobState == BlobState.None)
                {
                    item.Standby();
                }
                if (item.BlobState == BlobState.Moving
                    && item.SwitchAction())
                {
                    item.DoNothing();
                }
                if (item.BlobState == BlobState.Standby
                    && item.SwitchAction())
                {
                    item.Move();
                }
                

                item.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in blobList)
            {
                item.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
