﻿using System;
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
        protected Rectangle rectangle;
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
        protected BlobState previousBlobState = BlobState.None;
        /// <summary>
        /// Number of frames lasted since last state change
        /// </summary>
        protected int stateTimer = 0;
        /// <summary>
        /// Number of frames blob can do what he does, setted when blobState changes
        /// </summary>
        protected int currentActionTimer = 0;

        protected bool hovered;
        protected InfoBox infoBox;
        /// <summary>
        /// Collection of InfoBoxRows, every row in it will be shown in InfoBox when blob will be hovered
        /// </summary>
        protected List<InfoBoxRow> infoBoxRows;
#endregion

#region Constructors
        /// <summary>
        /// Creates new blob at specified position
        /// </summary>
        /// <param name="position"></param>
        public Blob(Vector2 position)
        {
            this.position = position;
            infoBox = new InfoBox();
            rectangle = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 2, (int)(scale * texture.Width)*2, (int)(scale * texture.Height)*2);
            infoBoxRows = new List<InfoBoxRow>();
        }
#endregion

#region Protected functions
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
        /// <summary>
        /// Returns blob velocity
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
        }
        /// <summary>
        /// Returns rectangle, which is a bit bigger than blob
        /// </summary>
        public Rectangle Rectangle
        {
            get { return rectangle; }
        }
        /// <summary>
        /// Sets private $hovered variable to given
        /// </summary>
        /// <param name="hovered"></param>
        public void SetHovered(bool hovered)
        {
            this.hovered = hovered;
        }
        /// <summary>
        /// Returns BlobState enum showing what is currently this blob doing
        /// </summary>
        public BlobState BlobState
        {
            get { return blobState; }
        }
        /// <summary>
        /// returns what blob just finished doing
        /// </summary>
        public BlobState PreviousBlobState
        {
            get { return previousBlobState; }
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
        /// <summary>
        /// Checks if blob is close to screen border
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>True if is, Vector2 is set as direction to closest border point</returns>
        public bool IsCloseToBorder(out Vector2 direction)
        {
            if (position.X > 20
                && position.X < BlobController.ScreenDim.X - 20
                && position.Y > 20
                && position.Y < BlobController.ScreenDim.Y - 20)
            {
                direction = Vector2.Zero;
                return false;
            }
            else
            {
                direction = new Vector2();
                if (position.X < 20)
                {
                    direction.X = 0;
                    direction.Y = position.Y;
                }
                if (position.X > BlobController.ScreenDim.X - 20)
                {
                    direction.X = BlobController.ScreenDim.X;
                    direction.Y = position.Y;
                }
                if (position.Y < 20)
                {
                    direction.X = position.X;
                    direction.Y = 0;
                }
                if (position.Y > BlobController.ScreenDim.Y - 20)
                {
                    direction.X = position.X;
                    direction.Y = BlobController.ScreenDim.Y;
                }
                return true;
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
            hovered = false;
            stateTimer++;
            position += velocity;

            //checks if finished current action
            if (currentActionTimer == stateTimer)
            {
                stateTimer = 0;
                previousBlobState = blobState;
                blobState = Blobs.BlobState.None;
            }

            //checks if blob reached it's desinaion
            if(blobState == BlobState.MovingInDirection
                && (int)position.X == (int)destination.X
                && (int)position.Y == (int)destination.Y)
            {
                DoNothing();
            }//if

            //moves rectangle
            rectangle.X = (int)position.X - texture.Width / 2;
            rectangle.Y = (int)position.Y - texture.Width / 2;

            //updates values for info box
            infoBoxRows.Clear();
            infoBoxRows.Add(new InfoBoxRow("Position", position));
            infoBoxRows.Add(new InfoBoxRow("Velocity", velocity));
        }//update

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw FieldOfView
            Rectangle viewf = new Rectangle((int)position.X - viewRange, (int)position.Y - viewRange, viewRange * 2, viewRange * 2);
            spriteBatch.Draw(viewField, viewf, Color.White);
            //draw blob
            Vector2 origin = new Vector2(texture.Width /2 );
            spriteBatch.Draw(texture, position, null, color, 0, origin, scale, SpriteEffects.None, 0);
            //draw InfoBox
            if (hovered)
            {
                infoBox.Update(position, infoBoxRows);
                infoBox.Draw(spriteBatch);
            }
        }

#endregion
    }
}
