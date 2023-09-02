using Controllers.Data;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DeleteCurrentPlayerData))]
public class DeletePlayerDataEditor : Editor
{
    [MenuItem("Window/Delete Player Data")]
    public static void DeleteGameData()
    {
        DeleteCurrentPlayerData gameData = FindObjectOfType<DeleteCurrentPlayerData>();
        if (gameData != null)
        {
            gameData.DeletePlayerData();
        }
        else
        {
            Debug.LogWarning("DeleteCurrentGameData object not found in scene.");
        }
    }
}