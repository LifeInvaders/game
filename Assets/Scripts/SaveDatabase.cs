using System.IO;
using Photon.Pun;
using Player;
using UnityEngine;

public class SaveDatabase : MonoBehaviour
{
    public int startMethod; //0 = save on launch, 1 = load on launch, nothing = call function manually

    void Start()
    {
        if (startMethod == 0)
            Save();
        if (startMethod == 1)
            Load();
    }
    
    public void Save()
    {
        string savePath = Application.persistentDataPath + "/SaveData";
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        using (Stream stream = File.Open(savePath + "/_save.dat", FileMode.Create)) 
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, PlayerDatabase.Instance);
            stream.Close();
        }
    }

    public void Load()
    {
        string savePath = Application.persistentDataPath + "/SaveData/_save.dat";
        if (!File.Exists(savePath)) return;
        using (Stream stream = File.Open(Application.persistentDataPath + "/SaveData/_save.dat", FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            PlayerDatabase.Instance = (PlayerDatabase) binaryFormatter.Deserialize(stream);
        }
    }
}