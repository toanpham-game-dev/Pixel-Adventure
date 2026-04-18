using Firebase.Firestore;
using System.Collections.Generic;

[FirestoreData]
public class CloudSaveData
{
    [FirestoreProperty] public int UnlockedLevel { get; set; }
    [FirestoreProperty] public Dictionary<string, int> LevelStars { get; set; }
    [FirestoreProperty] public string LastSaveTime { get; set; }
}