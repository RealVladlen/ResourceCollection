using System.IO;
using UnityEngine;

namespace SaveLogic
{
    public static class SaveSystem
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        private const string SoundKey = "Sound";
        private const string WoodKey = "Wood";
        private const string StoneKey = "Stone";
        private const string GoldKey = "Gold";
        private const string IronKey = "Iron";
        private const string CrystalKey = "Crystal";

        public static void Save(GameData data)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                SaveToPlayerPrefs(data);
            }
            else
            {
                SaveToFile(data);
            }
        }

        public static GameData Load()
        {
            return Application.platform == RuntimePlatform.Android ? LoadFromPlayerPrefs() : LoadFromFile();
        }

        private static void SaveToPlayerPrefs(GameData data)
        {
            PlayerPrefs.SetInt(SoundKey, data.sound);
            PlayerPrefs.SetInt(WoodKey, data.wood);
            PlayerPrefs.SetInt(StoneKey, data.stone);
            PlayerPrefs.SetInt(GoldKey, data.gold);
            PlayerPrefs.SetInt(IronKey, data.iron);
            PlayerPrefs.SetInt(CrystalKey, data.crystal);
            PlayerPrefs.Save();
            Debug.Log("Game data saved to PlayerPrefs!");
        }

        private static GameData LoadFromPlayerPrefs()
        {
            return new GameData
            {
                sound = PlayerPrefs.GetInt(SoundKey, 1),
                wood = PlayerPrefs.GetInt(WoodKey, 0),
                stone = PlayerPrefs.GetInt(StoneKey, 0),
                gold = PlayerPrefs.GetInt(GoldKey, 0),
                iron = PlayerPrefs.GetInt(IronKey, 0),
                crystal = PlayerPrefs.GetInt(CrystalKey, 0)
            };
        }

        private static void SaveToFile(GameData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SavePath, json);
                Debug.Log("Game data saved to file: " + SavePath);
            }
            catch (IOException e)
            {
                Debug.LogError("File save failed: " + e.Message);
            }
        }

        private static GameData LoadFromFile()
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("Save file not found, creating new game data.");
                return new GameData();
            }

            try
            {
                string json = File.ReadAllText(SavePath);
                return JsonUtility.FromJson<GameData>(json);
            }
            catch (IOException e)
            {
                Debug.LogError("File load failed: " + e.Message);
                return new GameData();
            }
        }
    }
}
