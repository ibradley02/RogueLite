using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Input;
using Duality.Resources;
using Duality.Plugins.Tilemaps;
using Duality.Plugins.Tilemaps.Properties;
using Duality.Components.Physics;

namespace RogueLite
{
    [RequiredComponent(typeof(Transform))]
    [RequiredComponent(typeof(RigidBody))]
    public class Player : Component, ICmpUpdatable, ICmpInitializable
    {
        Tilemap tilesGameBoard;
        Camera camera;

        private CharacterController character;

        public CharacterController Character
        {
            get { return this.character; }
            set { this.character = value; }
        }

        private Point2 GetTilePosFromWorldPos(Vector2 worldPos)
        {
            float gridMinX = tilesGameBoard.GameObj.Transform.Pos.X - (tilesGameBoard.Size.X * ((Tileset)tilesGameBoard.Tileset).TileSize.X) / 2;
            float gridMinY = tilesGameBoard.GameObj.Transform.Pos.Y - (tilesGameBoard.Size.Y * ((Tileset)tilesGameBoard.Tileset).TileSize.Y) / 2;

            int xTileIndex = (int)((worldPos.X - gridMinX) / ((Tileset)tilesGameBoard.Tileset).TileSize.X);
            int yTileIndex = (int)((worldPos.Y - gridMinY) / ((Tileset)tilesGameBoard.Tileset).TileSize.Y);

            return new Point2(xTileIndex, yTileIndex);
        }
        void ICmpInitializable.OnInit(InitContext context)
        {
            if (context == InitContext.Activate)
            {
                //Get a reference the to the game board
                tilesGameBoard = Scene.Current.FindComponent<Tilemap>(true);
                camera = Scene.Current.FindComponent<Camera>(true);
            }
        }

        void ICmpUpdatable.OnUpdate()
        {
            Transform transform = this.GameObj.Transform;
            RigidBody body = this.GameObj.GetComponent<RigidBody>();

            if (DualityApp.Mouse.ButtonHit(MouseButton.Left))
            {
                Vector2 tileIndex = GetTilePosFromWorldPos(camera.GetSpaceCoord(DualityApp.Mouse.Pos).Xy);
                Tile currentTile = tilesGameBoard.Tiles[(int)transform.Pos.X, (int)transform.Pos.Y];
                Tile clickedTile = tilesGameBoard.Tiles[(int)tileIndex.X, (int)tileIndex.Y];

                while (transform.Pos.Xy != tileIndex)
                {
                    transform.Pos.X += tileIndex.X / 2;
                    transform.Pos.Y += tileIndex.Y / 2;
                }

            }

        }

        void ICmpInitializable.OnShutdown(ShutdownContext context)
        {
        }
    }
}
