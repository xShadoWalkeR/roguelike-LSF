﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike {
    class Game {
        Random rnd;

        private int level = 1;
        private ConsoleKeyInfo key;

        // MyPlayer
        private Player myPlayer;
        // MyWorld
        private World myWorld;
        // MyGenerator
        private Generator myGenerator;
        // MyDrawing Draws the world
        private Draw myDrawing;

        public Game() {
            rnd = new Random();
            myPlayer = new Player(65);
            myWorld = new World(myPlayer, rnd);
            myGenerator = new Generator(level, myWorld, rnd);
            myDrawing = new Draw();
        }

        public void GenerateWorld() {
            do {
                // Update all explored tiles
                UpdateTiles();

                // Draw the world and the stats/legend side bar
                myDrawing.DrawWorld(myPlayer, myWorld, level);
                myDrawing.DrawStats(myPlayer, myWorld);
                myDrawing.DrawMessages(myPlayer, myWorld, myGenerator);

                // Read Key input and update player
                key = Console.ReadKey();

                UpdatePlayer();

                if (myWorld.myTiles[myPlayer.y, myPlayer.x].Exit) {
                    level++;
                    myWorld = new World(myPlayer, rnd);
                    myGenerator = new Generator(level, myWorld, rnd);
                }

            } while (myPlayer.Hp > 0);
        }

        private void UpdatePlayer() {
            Player passPlayer;
            passPlayer = myPlayer;
            // Remove the player from the current Tile
            myWorld.myTiles[myPlayer.y, myPlayer.x].RemoveAt(0);

            // Check witch key was pressed to update the player X or Y
            if (key.Key == ConsoleKey.W) {
                // Moves the Player North losing 1 hp
                if (myPlayer.y != 0) {
                    myPlayer.y--;
                    myPlayer.Hp--;
                    myPlayer.LastMove = "You Moved NORTH!";
                }
            } else if (key.Key == ConsoleKey.A) {
                // Moves the Player West losing 1 hp
                if (myPlayer.x != 0) {
                    myPlayer.x--;
                    myPlayer.Hp--;
                    myPlayer.LastMove = "You Moved WEST!";
                }
            } else if (key.Key == ConsoleKey.S) {
                // Moves the Player South losing 1 hp
                if (myPlayer.y != 7) {
                    myPlayer.y++;
                    myPlayer.Hp--;
                    myPlayer.LastMove = "You Moved SOUTH!";
                }
            } else if (key.Key == ConsoleKey.D) {
                // Moves the Player East losing 1 hp
                if (myPlayer.x != 7) {
                    myPlayer.x++;
                    myPlayer.Hp--;
                    myPlayer.LastMove = "You Moved EAST!";
                }
            }

            // Insert the player into the first position on the Tile list
            myWorld.myTiles[myPlayer.y, myPlayer.x].Insert(0, passPlayer);
        }

        /// <summary>
        /// Updates the tiles arround the player, setting them as explored
        /// </summary>
        private void UpdateTiles() {
            // Update explored tiles keeping in mind the limits of the grid
            if (myPlayer.y > 0) {
                myWorld.myTiles[myPlayer.y - 1, myPlayer.x].Explored = true;
            }
            if (myPlayer.y < myWorld.Rows - 1) {
                myWorld.myTiles[myPlayer.y + 1, myPlayer.x].Explored = true;
            }
            if (myPlayer.x > 0) {
                myWorld.myTiles[myPlayer.y, myPlayer.x - 1].Explored = true;
            }
            if (myPlayer.x < myWorld.Columns - 1) {
                myWorld.myTiles[myPlayer.y, myPlayer.x + 1].Explored = true;
            }
        }
    }
}