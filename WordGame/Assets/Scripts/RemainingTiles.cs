using System;
using System.Collections.Generic;
using System.Linq;
using Data;

public class DictionaryTileData
{
    public List<int> childrenID;
}

public class RemainingTiles
{
    private readonly List<TileData> _tiles; // Your list of tiles from the level.
    private readonly Dictionary<string, bool> _dictionary; // Your word dictionary.

    private Dictionary<string, DictionaryTileData>
        _allTilesWithChildren; // Is character locked or not, and the list of blocking tile IDs.

    private List<string> _validWords; // Store valid words.

    public RemainingTiles(LevelData currentLevel, List<TileData> tiles, Dictionary<string, bool> dictionary)
    {
        _tiles = tiles;
        _dictionary = dictionary;
        InitializeBlockedTiles(currentLevel);
    }

    private void InitializeBlockedTiles(LevelData currentLevel)
    {
        _allTilesWithChildren = new Dictionary<string, DictionaryTileData>();

        // Initialize characters with empty DictionaryTileData.
        foreach (var tile in _tiles)
        {
            DictionaryTileData tileData = new DictionaryTileData
            {
                childrenID = new List<int>(tile.children) // Create a copy of tile.children.
            };

            // Remove children IDs that don't exist in _tiles.
            tileData.childrenID.RemoveAll(childId => _tiles.Find(t => t.id == childId) == null);

            _allTilesWithChildren[tile.character] = tileData;
        }
    }

    public List<string> FindWords()
    {
        _validWords = new List<string>();

        foreach (var tile in _tiles)
        {
            if (tile.children.Count == 0 || tile.children.All(childId =>
                    _validWords.Any(word => word.Contains(_tiles.Find(t => t.id == childId).character))))
            {
                bool foundWord = FindWordsRecursive(tile.character);
                if (foundWord)
                {
                    // Return early if a valid word is found.
                    return _validWords;
                }
            }
        }

        return _validWords;
    }

    private bool FindWordsRecursive(string currentWord)
    {
        // Check if the current word is in the dictionary.
        if (_dictionary.ContainsKey(currentWord.ToLower()))
        {
            _validWords.Add(currentWord);
            return true; // Return true when a valid word is found.
        }

        foreach (var tile in _tiles)
        {
            string character = tile.character;
            DictionaryTileData tileData = _allTilesWithChildren[character];

            // Check if this character's children are unlocked.
            bool childrenUnlocked = tileData.childrenID.All(childId =>
                currentWord.Contains(_tiles.Find(t => t.id == childId).character));

            // If the character has no children or they are unlocked, add it to the word.
            if (!childrenUnlocked)
            {
                continue;
            }

            if (!currentWord.Contains(character))
            {
                if (FindWordsRecursive(currentWord + character))
                {
                    return true; // Return true when a valid word is found.
                }
            }
        }

        return false; // Return false if no valid word is found in this branch.
    }

}

