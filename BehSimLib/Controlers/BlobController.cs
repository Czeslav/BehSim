using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BehSimLib.Blobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace BehSimLib.Controlers
{
    public class BlobController
    {
        public static Random random;
        public static Rectangle MouseRec;
        protected List<Blob> blobList;
        protected MouseState mouseState;


        public BlobController()
        {
            MouseRec = new Rectangle(0, 0, 1, 1);
            random = new Random();
            blobList = new List<Blob>();
        }

        public void AddBlob(Vector2 position)
        {
            blobList.Add(new Blob(position));
            
        }


        #region General methods
        /// <summary>
        /// Enter your blob type behaviur in this function
        /// </summary>
        public void Update()
        {
            //Enter behaviour below
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


            //checks mouse position
            mouseState = Mouse.GetState();
            MouseRec.X = mouseState.X;
            MouseRec.Y = mouseState.Y;
            for (int i = 0; i < blobList.Count; i++)
            {
                if (blobList[i].Rectangle.Intersects(MouseRec))
                {
                    blobList[i].SetHovered(true);
                    break;
                }
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
