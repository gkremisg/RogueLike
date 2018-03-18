using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace RogueLike
{
    /// <summary>
    /// a class that generates the characteristics of
    /// the starting area
    /// </summary>
    class Room
    {
        #region Fields

        Tile[][] tiles;

        Vector2 exitPos;
        List<Treasure> treasureList;
        List<Rectangle> listOfTileRectangles;
        List<Point> pathToExit;
        List<Point> pathToTreasures;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="floorSprites">floor sprites</param>
        /// <param name="dungeonExitSprite">dungeon exit sprite</param>
        /// <param name="treasureSprite">treasure sprite</param>
        /// <param name="rand">random generator</param>
        public Room(Texture2D[] floorSprites, Texture2D dungeonExitSprite, Texture2D treasureSprite, Random rand)
        {
            exitPos = new Vector2(rand.Next(3,10), rand.Next(3,10));
            pathToExit = new List<Point>();
            pathToTreasures = new List<Point>();
            treasureList = new List<Treasure>();

            GenerateTreasures(treasureSprite, rand);

            // CalculatePathToTreasures(rand);

            CalculatePathToExit(rand);

            InitializeRoom(floorSprites, dungeonExitSprite, rand);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// returns the tile array
        /// </summary>
        public Tile[][] Tiles
        {
            get { return tiles; }
        }

        /// <summary>
        /// returns a list of rectangles for collision checks
        /// </summary>
        public List<Rectangle> ListOfTileRectangles
        {
            get { return listOfTileRectangles; }
        }

        /// <summary>
        /// returns the position of the room exit
        /// </summary>
        public Vector2 ExitPosition
        {
            get { return exitPos; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Draw support
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    if (i == exitPos.X && j == exitPos.Y)
                        tiles[i][j].Draw(spriteBatch, true);
                    else
                        tiles[i][j].Draw(spriteBatch, false);
                }
            }

            //foreach (Treasure treasure in treasureList)
            //{
            //    treasure.Draw(spriteBatch);
            //}
        }

        /// <summary>
        /// generate treasure positions in room
        /// </summary>
        /// <param name="rand"></param>
        private void GenerateTreasures(Texture2D treasureSprite, Random rand)
        {
            int numberOfTreasures =rand.Next(2,4);

            for (int i = 0; i < numberOfTreasures; i++)
            {
                Point treasureLocation = new Point(rand.Next(0, 10), rand.Next(0, 10));
                while (treasureLocation == exitPos.ToPoint() || treasureLocation == Point.Zero)
                {
                    treasureLocation = new Point(rand.Next(0, 10), rand.Next(0, 10));
                }
                treasureList.Add(new Treasure(treasureSprite, treasureLocation));
            }
        }

        /// <summary>
        /// Generates a guarenteed path to exit
        /// </summary>
        private void CalculatePathToExit(Random rand)
        {
            Point exitPoint = new Point((int)exitPos.X, (int)exitPos.Y);
            Point pathPoint = new Point(0, 0);
            pathToExit.Add(pathPoint);

            while (pathPoint != exitPoint)
            {

                if (rand.Next(0, 100) <= 49 && pathPoint.X < exitPoint.X)
                {
                    Point tempPoint = new Point(pathPoint.X + 1, pathPoint.Y);
                    pathToExit.Add(tempPoint);
                    pathPoint.X += 1;
                }
                else if (rand.Next(0,100) >= 50 && pathPoint.Y < exitPoint.Y)
                {
                    Point tempPoint = new Point(pathPoint.X, pathPoint.Y + 1);
                    pathToExit.Add(tempPoint);
                    pathPoint.Y += 1;
                }
            }
        }

        /// <summary>
        /// ensures there is a path to all treasures
        /// </summary>
        /// <param name="rand"></param>
        //private void CalculatePathToTreasures(Random rand)
        //{
        //    Point pathPoint = new Point(0, 0);
        //    pathToTreasures.Add(pathPoint);

        //    foreach (Treasure treasure in treasureList)
        //    {
        //        Point treasurePoint = treasure.Location;

        //        while (pathPoint != treasurePoint)
        //        {

        //            if (rand.Next(0, 100) <= 49 && pathPoint.X < treasurePoint.X)
        //            {
        //                Point tempPoint = new Point(pathPoint.X + 1, pathPoint.Y);
        //                pathToTreasures.Add(tempPoint);
        //                pathPoint.X += 1;
        //            }
        //            else if (rand.Next(0, 100) >= 50 && pathPoint.Y < treasurePoint.Y)
        //            {
        //                Point tempPoint = new Point(pathPoint.X, pathPoint.Y + 1);
        //                pathToTreasures.Add(tempPoint);
        //                pathPoint.Y += 1;
        //            }
        //        }
        //        pathPoint.X = pathPoint.Y = 0;
        //    }
        //}

        /// <summary>
        /// Initialize room characteristics
        /// </summary>
        /// <param name="floorSprites">floorSprites</param>
        /// <param name="dungeonExitSprite">dungeonExitSprite</param>
        /// <param name="rand">random generator</param>
        private void InitializeRoom(Texture2D[] floorSprites, Texture2D dungeonExitSprite, Random rand)
        {
            listOfTileRectangles = new List<Rectangle>();

            tiles = new Tile[500 / 50][];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile[500 / 50];
            }

            Point temp = new Point(0, 0);

            List<int> walls = new List<int>();

            walls = ReservoirSampling(rand);

            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    Vector2 tilePos = new Vector2(i, j);
                    temp.X = i;
                    temp.Y = j;

                    // generate a wall with a 30% chance if its not 
                    // in the path to exit or a treasure
                    if (walls.Contains(i*10+j) && !pathToExit.Contains(temp))
                    {
                        tiles[i][j] = new Tile(floorSprites, dungeonExitSprite, tilePos, true);
                        listOfTileRectangles.Add(tiles[i][j].DrawRectangle);
                    }
                    else
                    {
                        tiles[i][j] = new Tile(floorSprites, dungeonExitSprite, tilePos, false);
                    }
                }
            }
        }

        /// <summary>
        /// select random tiles to be walls
        /// </summary>
        /// <param name="rand">random generator</param>
        /// <returns></returns>
        private List<int> ReservoirSampling(Random rand)
        {
            List<int> temp = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                if (i < 35)
                {
                    temp.Add(i);
                }
                else
                {
                    int l = rand.Next(0, i + 1);
                    if (l < 35)
                    {
                        temp[l] = i;
                    }
                }
            }
            return temp;
        }

        #endregion Methods
    }
}
