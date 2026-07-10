using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public class SaveData
    {
        public int score;
    }
    
    public static void SaveGame(SaveData newSaveData)
    {
        SaveData saveData = GetGameData();
        if(newSaveData.score > saveData.score) PlayerPrefs.SetInt("Highscore", newSaveData.score);
    }

    public static SaveData GetGameData()
    {
        SaveData saveData = new SaveData(){
            score = PlayerPrefs.GetInt("Highscore", 0),
        };

        return saveData;
    }
}