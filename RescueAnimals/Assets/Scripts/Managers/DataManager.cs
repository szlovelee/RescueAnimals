using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

                var go = new GameObject();
                go.name = nameof(DataManager);
                _instance = go.AddComponent<DataManager>();
                return _instance;
            }
        }

        public Player LoadPlayerInfo()
        {
            var parent = Application.persistentDataPath;
            var filePath = Path.Combine(parent, FileName);
            var json = File.ReadAllText(filePath);
            var ret = JsonUtility.FromJson<SaveData>(json);
            Debug.Log(json);
            return new Player();
        }

        public void SavePlayer(Player player)
        {
            IsWrite = true;
            var parent = Application.persistentDataPath;
            var filePath = Path.Combine(parent, FileName);
            SaveData saveData = new SaveData(
                1,
                0,
                1000,
                10,
                100,
                new Dictionary<AnimalType, int>()
            );
            var json = JsonUtility.ToJson(saveData);
            File.WriteAllText(filePath, json);
            IsWrite = false;
        }
    }
}