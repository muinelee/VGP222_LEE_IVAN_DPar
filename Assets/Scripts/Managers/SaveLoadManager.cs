using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{    
    public class PlayerCreditsData
    {
        public int totalCredits;

        public PlayerCreditsData()
        {
            // INTENTIONALLY LEFT BLANK
        }

        public PlayerCreditsData(int credits)
        {
            totalCredits = credits;
        }
    }

    public void SaveCredits(int credits)
    {
        PlayerCreditsData data = new PlayerCreditsData(credits);
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerCreditsData));
        FileStream stream = new FileStream(Application.persistentDataPath + "/playerData.xml", FileMode.Create);
        serializer.Serialize(stream, data);
        stream.Close();
    }

    public PlayerCreditsData LoadCredits()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerCreditsData));
            FileStream stream = new FileStream(Application.persistentDataPath + "/playerData.xml", FileMode.Open);
            PlayerCreditsData data = serializer.Deserialize(stream) as PlayerCreditsData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
    
    // Reset Credits
    public void ResetCredits()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.xml"))
        {
            File.Delete(Application.persistentDataPath + "/playerData.xml");
        }
    }
}
