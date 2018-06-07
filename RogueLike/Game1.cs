using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RogueLike
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // sprites
        Texture2D wallSprite;
        Texture2D dungeonExitSprite;
        Texture2D playerSprite;
        Texture2D monsterSprite;
        Texture2D treasureSpriteSheet;
        Texture2D[] floorSprites;
        Rectangle collisionRectangle;

        public Random rand = new Random();
        Room[] rooms;
        Player pc;

        // game states
        public enum GameState {Start, Moving, Fighting, NextLevel, Exit, };
        public GameState currentState = GameState.Start;
        int currentRoom = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialization
            rooms = new Room[10];

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            floorSprites = new Texture2D[2];
            collisionRectangle = new Rectangle(0, 0, 50, 50);

            // Load content
            floorSprites[0] = Content.Load<Texture2D>(@"graphics\floorSprite");
            floorSprites[1] = Content.Load<Texture2D>(@"graphics\floorSprite1");
            wallSprite = Content.Load<Texture2D>(@"graphics\wallSprite");
            dungeonExitSprite = Content.Load<Texture2D>(@"graphics\dungeonExit");
            playerSprite = Content.Load<Texture2D>(@"graphics\playerSprite");
            monsterSprite = Content.Load<Texture2D>(@"graphics\monsterSprite");
            treasureSpriteSheet = Content.Load<Texture2D>(@"graphics\treasureSpriteSheet");           
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kb = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            if (pc != null)
                pc.Update(gameTime, kb, mouse, rooms[currentRoom].Tiles);

            switch (currentState)
            {
                case GameState.Start:
                    {
                        rooms[currentRoom] = new Room(floorSprites, dungeonExitSprite, treasureSpriteSheet, rand);

                        Vector2 pcPosition = new Vector2(150, 50);

                        pc = new Player(playerSprite, pcPosition, rand);
                        currentState = GameState.Moving;
                        break;
                    }
                case GameState.Moving:
                    {
                        if (pc.PlayerPosition.Y/50 - 1 == rooms[currentRoom].ExitPosition.X &&
                            pc.PlayerPosition.X/50 - 3 == rooms[currentRoom].ExitPosition.Y)
                            {
                                currentState = GameState.NextLevel;
                            }
                        break;
                    }
                case GameState.NextLevel:
                    {
                        if (currentRoom < 9)
                        {
                            rooms[currentRoom + 1] = new Room(floorSprites, dungeonExitSprite, treasureSpriteSheet, rand);
                            pc.PlayerPosition = new Vector2(150,50);
                            currentRoom++;
                            currentState = GameState.Moving;
                        }
                        else
                        {
                            currentState = GameState.Exit;
                        }
                        break;
                    }
                case GameState.Exit:
                    {
                        Exit();
                        break;
                    }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            rooms[currentRoom].Draw(spriteBatch);

            pc.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
