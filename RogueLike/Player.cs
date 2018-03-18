using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RogueLike
{
    class Player
    {

        #region Fields

        Texture2D characterSprite;
        Rectangle drawRectangle;

        Vector2 playerPosition;

        // character traits
        int baseDamage;
        int damageModifier;
        int hitPoints;
        int armor;

        // navigate tile array
        int tilePosX;
        int tilePosY;

        KeyboardState previousKbState;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="characterSprite"></param>
        /// <param name="startingPosition"></param>
        /// <param name="rand"></param>
        public Player(Texture2D characterSprite, Vector2 startingPosition, Random rand)
        {
            this.characterSprite = characterSprite;
            playerPosition = startingPosition;
            drawRectangle = new Rectangle((int)startingPosition.X,
                                          (int)startingPosition.Y,
                                          50,
                                          50);
            this.hitPoints = 100;
            this.baseDamage = 4;        // randomly deal between 1-4 dmg, increases with levels
            this.damageModifier = 1;    // multiply base damage, increases with levels
            this.armor = 5;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Modifies the character hit points
        /// </summary>
        public int HealthPoints
        {
            get { return hitPoints; }
            set
            {
                if (value < 0)
                {
                    hitPoints = 0;
                }
                else
                    hitPoints = value;
            }
        }

        /// <summary>
        /// modifies or return player position
        /// </summary>
        public Vector2 PlayerPosition
        {
            get { return playerPosition; }
            set
            {
                playerPosition = value;
                drawRectangle.X = (int)value.X;
                drawRectangle.Y = (int)value.Y;
            }
        }

        /// <summary>
        /// Modifies the base damage
        /// </summary>
        public int BaseDamage
        {
            set { baseDamage = value; }
        }

        /// <summary>
        /// Changes the damage modifier
        /// </summary>
        public int DamageModifier
        {
            set { damageModifier = value; }
        }

        /// <summary>
        /// returns player rectangle for collision checks
        /// </summary>
        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// updates player information
        /// </summary>
        /// <param name="gametime">gametime</param>
        /// <param name="kb">keyboard state</param>
        /// <param name="mouse">mouse state</param>
        public void Update(GameTime gametime, KeyboardState kb, MouseState mouse, Tile[][] tiles)
        {
            // player starting position is (150,50). To match it to the tile array (0,0)
            // we need to swap playerX and playerX, divide by 50 
            // and subtract (1,3).
            tilePosX = (int)(playerPosition.Y / 50) - 1;
            tilePosY = (int)(playerPosition.X / 50) - 3;

            if (kb.IsKeyUp(Keys.W) && previousKbState.IsKeyDown(Keys.W))
            {
                if (drawRectangle.Top > 50)
                {
                    // check tile above character for wall
                    if (!tiles[tilePosX - 1][tilePosY].IsWall)
                    {
                        playerPosition.Y -= 50;
                        drawRectangle.Y = (int)playerPosition.Y;
                    }
                }
                else
                {
                    playerPosition.Y = 50;
                    drawRectangle.Y = (int)playerPosition.Y;
                }
            }

            if (kb.IsKeyUp(Keys.S) && previousKbState.IsKeyDown(Keys.S))
            {
                if (drawRectangle.Bottom < 550)
                {
                    // check tile below character
                    if (!tiles[tilePosX + 1][tilePosY].IsWall)
                    {
                        playerPosition.Y += 50;
                        drawRectangle.Y = (int)playerPosition.Y;
                    }
                }
                else
                {
                    playerPosition.Y = 500;
                    drawRectangle.Y = (int)playerPosition.Y;
                }
            }

            if (kb.IsKeyUp(Keys.A) && previousKbState.IsKeyDown(Keys.A))
            {
                if (drawRectangle.Left > 150)
                {
                    // check tile left of character
                    if (!tiles[tilePosX][tilePosY - 1].IsWall)
                    {
                        playerPosition.X -= 50;
                        drawRectangle.X = (int)playerPosition.X;
                    }
                }
                else
                {
                    playerPosition.X = 150;
                    drawRectangle.X = (int)playerPosition.X;
                }
            }

            if (kb.IsKeyUp(Keys.D) && previousKbState.IsKeyDown(Keys.D))
            {
                // check tile right of character
                if (drawRectangle.Right < 650)
                {
                    if (!tiles[tilePosX][tilePosY + 1].IsWall)
                    {
                        playerPosition.X += 50;
                        drawRectangle.X = (int)playerPosition.X;
                    }
                }
                else
                {
                    playerPosition.X = 600;
                    drawRectangle.X = (int)playerPosition.X;
                }
            }

            previousKbState = kb;
        }

        /// <summary>
        /// draws player
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(characterSprite, drawRectangle, Color.White);
        }

        #endregion Methods

    }
}
