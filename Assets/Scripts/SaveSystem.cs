using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public class SaveData
    {
        public int highscore;

        public SaveData(int score){
            highscore = score;
        }
    }
    
    public static void SaveGame(SaveData newSaveData)
    {
        SaveData saveData = GetGameData();
        if(newSaveData.highscore > saveData.highscore) PlayerPrefs.SetInt("Highscore", newSaveData.highscore);
    }

    public static SaveData GetGameData()
    {
        SaveData saveData = new SaveData(PlayerPrefs.GetInt("Highscore", 0));
        return saveData;
    }
}