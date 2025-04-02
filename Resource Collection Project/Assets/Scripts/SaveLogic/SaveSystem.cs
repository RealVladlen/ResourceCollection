using System.IO;
using UnityEngine;

namespace SaveLogic
{
    public static class SaveSystem
    {
        private static string _savePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        public static void Save(GameData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_savePath, json);
            Debug.Log("Game saved: " + _savePath);
        }

        public static GameData Load()
        {
            if (!File.Exists(_savePath))
            {
                Debug.LogWarning("Save file not found, creating new game data.");
                return new GameData();
            }
        
            string json = File.ReadAllText(_savePath);
            return JsonUtility.FromJson<GameData>(json);
        }
    }
}