using System.IO;
using FullSerializer;
using Player;
using UnityEngine;

public class SaveDatabase : MonoBehaviour
{
    public int startMethod; //0 = save on Start, 1 = load on Start, default = no Start method

    readonly int versionHash = 27836721;
    private static readonly fsSerializer Serializer = new fsSerializer();


    void Start()
    {
        if (startMethod == 0)
            Save();
        if (startMethod == 1)
            Load();
    }

    private void DeleteDeprecate()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SaveData"))
            return;
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/SaveData");
        foreach (FileInfo file in directoryInfo.EnumerateFiles())
        {
            if (file.Name != "_save_" + versionHash + ".dat")
                file.Delete();
        }
            
    }
    
    public void Save()
    {
        string savePath = Application.persistentDataPath + "/SaveData" ;
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        using (Stream stream = File.Open(savePath + "/_save_" + versionHash.ToString() + ".dat", FileMode.Create)) 
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, PlayerDatabase.Instance);
            stream.Close();
        }
    }

    public void Savejson()
    {
        fsData data;
        Serializer.TrySerialize(typeof(PlayerDatabase), PlayerDatabase.Instance, out data).AssertSuccessWithoutWarnings();
        string savePath = Application.persistentDataPath + "/SaveData" ;
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        StreamWriter file = new StreamWriter(savePath + "/_save_" + versionHash.ToString() + ".dat");
        file.Write(fsJsonPrinter.CompressedJson(data));
        file.Close();
    }   

    public void Load()
    {
        string savePath = Application.persistentDataPath + "/SaveData/_save_" + versionHash.ToString() + ".dat";
        if (!File.Exists(savePath)) return;
        using (Stream stream = File.Open(savePath, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            PlayerDatabase.Instance = (PlayerDatabase) binaryFormatter.Deserialize(stream);
        }
    }

    public void Loadjson()
    {
        string savePath = Application.persistentDataPath + "/SaveData/_save_" + versionHash.ToString() + ".dat";
        if (!File.Exists(savePath)) return;
        fsData json = fsJsonParser.Parse(File.ReadAllText(savePath));
        object deserialized = null;
        Serializer.TryDeserialize(json, typeof(PlayerDatabase), ref deserialized).AssertSuccessWithoutWarnings();
        PlayerDatabase.Instance = (PlayerDatabase) deserialized;
    }
}