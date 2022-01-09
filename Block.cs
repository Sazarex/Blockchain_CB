using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Blockchain
{
    /// <summary>
    /// Блок данных
    /// </summary>
    [DataContract]
    public class Block//Блок
    {
        public int ID { get; private set; }// Идентификатор

        [DataMember]
        public string Data { get; private set; } //Данные

        [DataMember]
        public DateTime Created { get; private set; } //Дата создания

        [DataMember]
        public string Hash { get; private set; }//Хэш блока

        public string PreviousHash { get; private set; }//Предыдущий хеш

        public string User { get; private set; }//Имя пользователя

        public Block()//Конструктор генезис блока
        {
            ID = 1;
            Data = "Hello, World";
            Created = DateTime.Parse("03.01.2021 00:00:00.000");
            PreviousHash = "111111";
            User = "Admin";

            var data = GetData();
            Hash = GetHash(data);
        }

        public Block(string data,string user, Block block)//Конструктор блока
        {
            if(string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException("Пустой аргумент data", nameof(data));
            }

            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException("Пустой аргумент user", nameof(user));
            }

            if (block==null)
            {
                throw new ArgumentNullException("Пустой аргумент block", nameof(block));
            }

            Data = data;
            User = user;
            PreviousHash = block.Hash; //Хэш от Last-блока
            Created = DateTime.UtcNow;
            ID = block.ID + 1;


            var BlockData = GetData();
            Hash = GetHash(BlockData);

        }

        private string GetData()//Получение значимых данных
        {
             string result = "";

            result += Data;
            result += Created.ToString("dd.MM.yyyy HH:mm:ss:fff");
            result += PreviousHash;
            result += User;

            return result;
        }

        private string GetHash (string data)//Хэширование данных
        {
            var message = Encoding.ASCII.GetBytes(data);
            var HashString = new SHA256Managed();
            string hex = "";

            var HashValue = HashString.ComputeHash(message);
            foreach (byte x in HashValue)
            {
                hex += String.Format("{0:x2}", x);
            }


            return hex;
        }

        public override string ToString()
        {
            return Data;
        }

        /// <summary>
        /// Выполнить сериализацию объекта в JSON строку
        /// </summary>
        /// <returns></returns>
        public string Serialize()//Сериализация
        {

            var jsonSerializer = new DataContractJsonSerializer(typeof(Block));

            using (var ms = new MemoryStream())
            {
                jsonSerializer.WriteObject(ms, this);
                var result = Encoding.UTF8.GetString(ms.ToArray());
                return result;
            }

        }

        /// <summary>
        /// Выполнить десериализацию объекта Block из JSON строки.о
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Block Deserialize (string json)//Десериализация
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(Block));

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var result = (Block)jsonSerializer.ReadObject(ms);
                return result;
            }
        }
    }
}
