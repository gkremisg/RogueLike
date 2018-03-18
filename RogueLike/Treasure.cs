using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueLike
{
    class Treasure
    {
        #region Fields

        Texture2D spriteSheet;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;
        Point position;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="spriteSheet">sprite sheet</param>
        /// <param name="position">treasure position</param>
        public Treasure(Texture2D spriteSheet, Point position)
        {
            this.spriteSheet = spriteSheet;
            this.drawRectangle = new Rectangle((position.X + 3) * 50 + spriteSheet.Width / 3,
                                               (position.Y + 1) * 50 + spriteSheet.Height / 3,
                                               30, 30);
            this.sourceRectangle = new Rectangle(0, 0, 15, 15);
            this.position = position;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Get the location of the treasure
        /// </summary>
        public Point Location
        {
            get { return position; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Draws the treasure
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, drawRectangle, sourceRectangle, Color.White);
        }

        #endregion Methods
    }
}
