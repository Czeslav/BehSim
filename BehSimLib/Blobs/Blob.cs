using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BehSimLib.Controlers;

namespace BehSimLib.Blobs
{
    public enum BlobState { Standby, Moving, MovingInDirection, MovingInOpositeDirection, None };


    public class Blob
    {
#region Variables
        protected Vector2 position;
        protected Vector2 destination;
        protected Color color = Color.White;
        protected float rotation = 0;
        protected float speed = 1.5f;
        protected Vector2 velocity;

        protected float scale = 0.5f;
        protected int viewRange = 32;

        protected static Texture2D texture;
        protected static Texture2D viewField;
        protected static Texture2D textBox;

        protected BlobState blobState = BlobState.None;
        /// <summary>
        /// Number of frames lasted since last state change
        /// </summary>
        protected int stateTimer = 0;
        /// <summary>
        /// Number of frames blob can do what he does, setted when blobState changes
        /// </summary>
        protected int currentActionTimer = 0;
#endregion

#region Constructors
        /// <summary>
        /// Creates new blob at (30,30)
        /// </summary>
        public Blob()
        {
            position = new Vector2(30);
        }
        /// <summary>
        /// Creates new blob at specified position
        /// </summary>
        /// <param name="position"></param>
        public Blob(Vector2 position)
        {
            this.position = position;
        }
#endregion

#region Private functions

#endregion

#region Public Properties
        public static void SetTexture(Texture2D _texture)
        {
            texture = _texture;
        }
        public static void SetViewFieldTexture(Texture2D _texture)
        {
            viewField = _texture;
        }
        public BlobState BlobState
        {
            get { return blobState; }
        }
        /// <summary>
        /// Number of frames since last blobState change
        /// </summary>
        public int StateTimer
        {
            get { return stateTimer; }
        }
        /// <summary>
        /// Number of frames a blob can do his current action, setted at blobState change 
        /// </summary>
        public int CurrentActionTimer
        {
            get { return currentActionTimer; }
        }
        /// <summary>
        /// Checks if it's time to switch action
        /// </summary>
        /// <returns></returns>
        public bool SwitchAction()
        {
            if (currentActionTimer == stateTimer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks if blob does nothing and wants to do something
        /// </summary>
        /// <returns></returns>
        public bool DoesNothing()
        {
            if (blobState == BlobState.None)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
#endregion

#region Public Actions

        /// <summary>
        /// Moves blob in random direction. Duration 30-50 frames
        /// </summary>
        public void Move()
        {
            //rands angle, then calcuates and sets velocity and rands timer
            blobState = BlobState.Moving;
            stateTimer = 0;

            currentActionTimer = BlobController.random.Next(30,50);

            float degree = BlobController.random.Next(0, 360);
            rotation = (float)(Math.PI / 180.0f * degree);

            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)-Math.Cos(rotation) * speed;
        }
        /// <summary>
        /// Moves blob to destination, finishes when reaches given destination
        /// </summary>
        /// <param name="Destination"></param>
        public void MoveInDirection(Vector2 Destination)
        {
            //calculates distance to move in both diameters, then calculates velocity, and sets timer and velocity
            destination = Destination;
            Vector2 diference = new Vector2(Destination.X - position.X, Destination.Y - position.Y);
            float distance = (float)Math.Sqrt(Math.Pow(diference.X, 2) + Math.Pow(diference.Y, 2));

            int framesNeeded = (int)(distance / speed + 1);
            velocity.X = diference.X / framesNeeded;
            velocity.Y = diference.Y / framesNeeded;

            stateTimer = 0;
            currentActionTimer = framesNeeded;
            blobState = BlobState.MovingInDirection;

        }
        /// <summary>
        /// Runs away from given point, lasts 25 frames
        /// </summary>
        /// <param name="Direction"></param>
        public void MoveInOppositeDirection(Vector2 Point)
        {
            Vector2 diference = new Vector2(Point.X - position.X, Point.Y - position.Y);

            diference = Vector2.Negate(diference);
            float length = diference.Length();
            float steps = length / speed;

            velocity.X = diference.X / steps;
            velocity.Y = diference.Y / steps;

            stateTimer = 0;
            currentActionTimer = 25;
            blobState = BlobState.MovingInOpositeDirection;
        }
        /// <summary>
        /// Holds blob in current position. Duration 15-40 frames
        /// </summary>
        public void Standby()
        {
            //sets velocity to zero and rands timer
            velocity = Vector2.Zero;
            blobState = BlobState.Standby;
            stateTimer = 0;

            currentActionTimer = BlobController.random.Next(15,40);
        }
        /// <summary>
        /// Cancels everything blob does and sets $blobState to none
        /// </summary>
        public void DoNothing()
        {
            //sets velocity to zero and blobstate to none
            velocity = Vector2.Zero;
            blobState = BlobState.None;
        }

#endregion

#region General Methods
        public void Update()
        {
            stateTimer++;

            position += velocity;

            //checks if blob reached it's desinaion
            if(blobState == BlobState.MovingInDirection
                && (int)position.X == (int)destination.X
                && (int)position.Y == (int)destination.Y)
            {
                DoNothing();
            }
        }//update

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle viewf = new Rectangle((int)position.X - viewRange, (int)position.Y - viewRange, viewRange * 2, viewRange * 2);
            spriteBatch.Draw(viewField, viewf, Color.White);
            //spriteBatch.Draw(texture, rec, color);
            Vector2 origin = new Vector2(texture.Width * scale);
            spriteBatch.Draw(texture, position, null, color, 0, origin, scale, SpriteEffects.None, 0);
        }

#endregion
    }
}
