using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Minesweeper.Libraries
{
    [CreateAssetMenu(menuName = "Minesweeper/Board Library", fileName = "BoardLibrary")]
    public class BoardLibrary : ScriptableObject
    {
        [Serializable]
        public struct NumeratedTile
        {
            public Tile Tile;
            public int Number;
        }

        [Header("Faces")]
        [SerializeField] private Sprite smilingFace;
        [SerializeField] private Sprite coolFace;
        [SerializeField] private Sprite sadFace;
        
        [Header("Tiles")]
        [SerializeField] private Tile unknownTile;
        [SerializeField] private Tile emptyTile;
        [SerializeField] private Tile mineTile;
        [SerializeField] private Tile explodedTile;
        [SerializeField] private Tile flagTile;
    
        [Space, SerializeField] private NumeratedTile[] numeratedTiles;
        private Dictionary<int, Tile> numeratedTilesDictionary;

        public Sprite SmilingFace => smilingFace;
        public Sprite CoolFace => coolFace;
        public Sprite SadFace => sadFace;
        public Tile UnknownTile => unknownTile;
        public Tile EmptyTile => emptyTile;
        public Tile MineTile => mineTile;
        public Tile ExplodedTile => explodedTile;
        public Tile FlagTile => flagTile;

        public Dictionary<int, Tile> NumeratedTiles => 
            numeratedTilesDictionary ??= numeratedTiles.ToDictionary(tile => tile.Number, tile => tile.Tile);
    }
}