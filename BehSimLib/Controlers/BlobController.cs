using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
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
        public static Vector2 ScreenDim;
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
        /// Enter your blob type behaviur in this function DEFORE base.update
        /// </summary>
        public void Update()
        {
            //Enter behaviour below
            foreach (var item in blobList)
            {
                if (item.DoesNothing()
                    && item.PreviousBlobState == BlobState.None)
                {
                    item.Standby();
                }
                if (item.DoesNothing()
                    && item.PreviousBlobState == BlobState.Standby)
                {
                    item.Move();
                }
                if (item.DoesNothing()
                    && item.PreviousBlobState == BlobState.Moving)
                {
                    item.Standby();
                }
                
                //must be, prevents blob from falling off screen
                Vector2 dupa;
                if (item.IsCloseToBorder(out dupa))
                {
                    item.DoNothing();
                    item.MoveInOppositeDirection(dupa);
                }
                if (item.DoesNothing()
                    && item.PreviousBlobState == BlobState.MovingInOpositeDirection)
                {
                    item.Move();
                }
                //end of must be

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
