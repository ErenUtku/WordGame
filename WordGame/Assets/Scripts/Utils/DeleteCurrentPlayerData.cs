using System.IO;
using Controllers.Data;
using UnityEngine;

namespace Utils
{
    public class DeleteCurrentPlayerData : MonoBehaviour
    {
        public void DeletePlayerData()
        {
            // Check if the file exists
            if (SaveManager.SaveExist("SavePlayerDataState"))
            {
                // Delete the file
                
                //HighScores
                PlayerPrefs.DeleteAll();
                
                //LevelData
                File.Delete(Application.persistentDataPath+"/save/"+ "SavePlayerDataState" + ".WordGamesSave");
                Debug.Log("GameData has been deleted.");

                // Create a new empty GameData object
                PlayerDataState emptyData = new PlayerDataState();
                // Save the empty GameData to the same file to replace the deleted data
                SaveManager.SaveData(emptyData, "SavePlayerDataState");
            }
            else
            {
                Debug.Log("No GameData file found.");
            }
        }
    }
}