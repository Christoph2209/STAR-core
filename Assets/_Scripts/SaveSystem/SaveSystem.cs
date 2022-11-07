using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using File = UnityEngine.Windows.File;
//Coded by Christopher Soravilla
public class SaveSystem : MonoBehaviour
{
    //These are the private variables, in which we have the path directories and the filetype, just so we dont have to type .text all the time
    private const string FileType = ".txt";
    private static string SavePath => Application.persistentDataPath + "/Saves/";
    private static string BackupPath => Application.persistentDataPath + "/BackUp/";
    //This is a save counter, after 5 saves, it will make a backup.
    private static int SaveCount;
    //Save Data Function
    public static void SaveData<T>(T data, string fileName)
    {
        //Creating the directories in the folders
        Directory.CreateDirectory(SavePath);
        Directory.CreateDirectory(BackupPath);
        //If the savecount % 5 is 0, it will save to a backup.
        if (SaveCount % 5 == 0) Save(BackupPath);
        //Saves the files into the function
        Save(SavePath);
        //Adds to the save count
        SaveCount++;


        void Save(string path)
        {
            //While using the stream writer
            using (StreamWriter writer = new StreamWriter(path + fileName + FileType))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream();
                formatter.Serialize(memoryStream, data);
                string dataToSave = Convert.ToBase64String(memoryStream.ToArray());
                writer.WriteLine(dataToSave);
            }
        }
    }

    public static T LoadData<T>(string fileName)
    {
        Directory.CreateDirectory(SavePath);
        Directory.CreateDirectory(BackupPath);

        bool backUpNeeded = false;
        T dataToReturn;

        Load(SavePath);
        if (backUpNeeded) Load(BackupPath);

        return dataToReturn;


        void Load(string path)
        {
            using (StreamReader reader = new StreamReader(path + fileName + FileType))
            {
                BinaryFormatter fortmatter = new BinaryFormatter();
                string dataToLoad = reader.ReadToEnd();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(dataToLoad));
                try
                {
                    dataToReturn = (T)fortmatter.Deserialize(memoryStream);
                }
                catch
                {
                    backUpNeeded = true;
                    dataToReturn = default;
                }
            }
        }
    }

    public static bool SaveExists(String fileName)=>
        File.Exists(SavePath + fileName + FileType)
        || File.Exists(BackupPath + fileName + FileType);




}
