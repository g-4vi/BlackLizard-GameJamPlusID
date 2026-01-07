using UnityEngine;
using System;
using System.IO;

//class that handles the read/write of the data
public class FileDataHandler
{
    string dataDirPath = "";
    string dataFileName = "";

    bool useEncryption = false;
    readonly string encryptionKey = "d0_n0t_t0uch_1137blz";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileId)
    {
        if(profileId == null)
        {
            return null;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        GameData loadedData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                Debug.Log("Encryption: " + useEncryption);
                if(useEncryption)
                {
                    //decrypt the data
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data at path: {fullPath}\n {e}");
            }
        }
        return loadedData;

    }

    public void Save(GameData data, string profileId)
    {
        if(profileId==null) return;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        try
        {
            //Create directory if not exist yet
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);//convert data to json

            if(useEncryption)
            {
                //Encrypt the data
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer =  new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"Saving data failed at path: {fullPath} \n {e}");
        }
    }

    //XOR encryption
    string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
        }

        return modifiedData;
    }
}
