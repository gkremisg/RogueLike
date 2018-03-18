using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike
{
    class Tile
    {
        #region Fields

        // draw support
        Texture2D[] floorSprites;
        Texture2D dungeonExitSprite;
        Rectangle drawRectangle;

        // mark walls so player cant go through them
        bool isWall = false;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="floorSprite">floorSprite</param>
        /// <param name="dungeonExitSprite">sprite for dungeon exit</param>
        /// <param name="tilePosition">position of tile</param>
        /// <param name="rowNumber">row number for drawRectangle</param>
        /// <param name="colNumber">col number for drawRectangle</param>
        public Tile(Texture2D[] floorSprites,Texture2D dungeonExitSprite, Vector2 arrayPos, bool isWall)
        {
            this.floorSprites = floorSprites;
            this.dungeonExitSprite = dungeonExitSprite;
            this.drawRectangle = new Rectangle((int)(150 + arrayPos.Y * 50), 
                                               (int)(50+ arrayPos.X * 50), 
                                               50, 
                                               50);
            this.isWall = isWall;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// returns if a tile is wall
        /// </summary>
        public bool IsWall
        {
            get { return isWall; }
        }

        /// <summary>
        /// returns the rectangle of the tile
        /// </summary>
        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Draws sprite
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        /// <param name="isExit">true false if tile is exit</param>
        public void Draw(SpriteBatch spriteBatch, bool isExit)
        {
            if (!isExit)
                spriteBatch.Draw(ChooseCorrectSprite(), drawRectangle, Color.White);
            else
                spriteBatch.Draw(dungeonExitSprite, drawRectangle, Color.White);
        }

        /// <summary>
        /// Chooses sprite to draw based on if the tile is wall or not
        /// </summary>
        /// <returns></returns>
        private Texture2D ChooseCorrectSprite()
        {
            if (this.isWall)
            {
                return floorSprites[1];
            }
            else
            {
                return floorSprites[0];
            }
        }

        #endregion Methods
    }
}
