using System;
using System.IO;
using System.Security.Cryptography;
using Controllers.Data;
using UnityEngine;

namespace Data
{
    public static class SaveManager
    {
        private const string FileType = ".WordGamesSave";
        private static string SavePath => Application.persistentDataPath + "/save/";

        //security key
        private static byte[] key = new byte[16]
            {0x01, 0x11, 0x29, 0x00, 0x00, 0x00, 0x00, 0x11, 0x57, 0x67, 0x12, 0x03, 0x20, 0x50, 0x07, 0x02};
        //initialization vector
        private static byte[] iv = new byte[16]
            {0x03, 0x05, 0x07, 0x22, 0x16, 0x15, 0x00, 0x20, 0x22, 0x00, 0x15, 0x00, 0x00, 0x12, 0x30, 0x01};

        public static void SaveData<T>(T data, string fileName)
        {
            Directory.CreateDirectory(SavePath);

            Save(SavePath);

            void Save(string path)
            {
                var data2Save = EncryptString(JsonUtility.ToJson(data));
                File.WriteAllText(path + fileName + FileType, data2Save);
            }
        }

        public static T LoadData<T>(string fileName)
        {
            if (!SaveExist(fileName))
            {
                PlayerDataState playerData = new PlayerDataState();
                SaveData<PlayerDataState>(playerData, fileName);
            }
            Directory.CreateDirectory(SavePath);

            T dataToReturn;

            Load(SavePath);

            return dataToReturn;

            void Load(string path)
            {
                var file = File.ReadAllText(path + fileName + FileType);
                var json = DecryptString(file);

                dataToReturn = JsonUtility.FromJson<T>(json);
            }

        }

        public static bool SaveExist(string fileName) => File.Exists(SavePath + fileName + FileType);

        public static string EncryptString(string text)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (var aes = new AesManaged())
            {
                // Create encryptor    
                var encryptor = aes.CreateEncryptor(key, iv);
                // Create MemoryStream
                using (var ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (var sw = new StreamWriter(cs))
                            sw.Write(text);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptString(string text)
        {
            var bytes = Convert.FromBase64String(text);
            string plaintext = null;
            // Create AesManaged    
            using (var aes = new AesManaged())
            {
                // Create a decrypt    
                var decrypt = aes.CreateDecryptor(key, iv);
                // Create the streams used for decryption.    
                using (var ms = new MemoryStream(bytes))
                {
                    // Create crypto stream    
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (var reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}

//File Location C:\Users\Eren\AppData\LocalLow\DefaultCompany\WordGames\save