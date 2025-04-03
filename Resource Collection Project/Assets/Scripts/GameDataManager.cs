using System;
using Building;
using SaveLogic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [SerializeField] private AudioSource audioSource;
    
    private GameData _data;

    public int Sound
    {
        get => _data.sound;
        set
        {
            _data.sound = value;
            
            audioSource.mute = _data.sound != 1;
            
            SaveData();
        }
    }

    public int Resources(EResourceType type, int count = 0)
    {
        int countRes = type switch
        {
            EResourceType.Wood => _data.wood += count,
            EResourceType.Stone => _data.stone += count,
            EResourceType.Gold => _data.gold += count,
            EResourceType.Iron => _data.iron += count,
            EResourceType.Crystal => _data.crystal += count,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        
        GameEventManager.UpdateResourcesCountMethod(type, countRes);
        SaveData();
        return countRes;
    }
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool Init()
    {
        _data = SaveSystem.Load();
        audioSource.mute = _data.sound != 1;
        
        return true;
    }

    public void SaveData()
    {
        SaveSystem.Save(_data);
    }
}