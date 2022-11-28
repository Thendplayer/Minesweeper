using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minesweeper.Libraries
{
    [CreateAssetMenu(menuName = "Minesweeper/Game Library", fileName = "GameLibrary")]
    public class GameLibrary : ScriptableObject
    {
        [Serializable]
        public struct LevelData
        {
            public string Id;
            public Vector2Int Size;
            public int MineCount;
            public float CameraSize;
        }

        [SerializeField] private LevelData[] levels;
        private Dictionary<string, LevelData> levelsDictionary;

        public LevelData GetLevel(string Id)
        {
            levelsDictionary ??= levels.ToDictionary(level => level.Id, level => level);
            return levelsDictionary[Id];
        }
    }
}