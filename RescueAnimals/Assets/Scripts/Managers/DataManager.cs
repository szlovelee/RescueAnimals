using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Component.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace EnumTypes
{
    public class DataManager : MonoBehaviour
    {
        private const string FileName = "player.json";
        private static DataManager _instance;
        public bool IsWrite { get; private set; } = false;

        public static DataManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var go = new GameObject
                {
                    name = nameof(DataManager)
                };
                _instance = go.AddComponent<DataManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != this && _instance != null)
            {
                Destroy(gameObject);
            }
        }

        public SaveData LoadPlayerInfo(AnimalData animalData)
        {
            SaveData saveData;
            try
            {
                var parent = Application.persistentDataPath;
                var filePath = Path.Combine(parent, FileName);
                var json = File.ReadAllText(filePath);
                saveData = JsonUtility.FromJson<SaveData>(json);
                for (int i = 0; i < saveData.ReinforceSaveData.Count; i++)
                {
                    if (i >= animalData.AnimalReinforceData.Count) break;
                    animalData.AnimalReinforceData[i].reinforceLevel = saveData.ReinforceSaveData[i].reinforceLevel;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                saveData = new SaveData(
                    level: 1,
                    exp: 0,
                    gold: 0,
                    atk: 2,
                    new(),
                    new());
            }

            return saveData;
        }

        public void SavePlayer(Player player, AnimalData animalData, List<Rank> rankings)
        {
            var reinforce = animalData
                .AnimalReinforceData
                .Select(data => new ReinforceSaveData(data.animalType, data.reinforceLevel))
                .ToList();

            IsWrite = true;
            var parent = Application.persistentDataPath;
            var filePath = Path.Combine(parent, FileName);
            SaveData saveData = new SaveData(
                player.level,
                player.exp,
                player.gold,
                player.atk,
                reinforceSaveData: reinforce,
                rankSystemData: rankings
            );
            var json = JsonUtility.ToJson(saveData);
            File.WriteAllText(filePath, json);
            IsWrite = false;
            Debug.Log(json);
        }

        public void SavePlayer(SaveData saveData)
        {
            var parent = Application.persistentDataPath;
            var filePath = Path.Combine(parent, FileName);

            var json = JsonUtility.ToJson(saveData);
            File.WriteAllText(filePath, json);
            IsWrite = false;
            Debug.Log(filePath);
        }
    }
}