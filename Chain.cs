using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain
{
    public class Chain
    {
        public List<Block> Blocks { get; private set; }

        public Block Last { get; private set; }
               
        public void Add (string data, string user)
        {
            var block = new Block(data, user, Last);
            Blocks.Add(block);
            Last = block;
            Save(Last);
        }


        public Chain()
        {

            Blocks = LoadChainFromDB();

            if (Blocks.Count == 0)
            {
                var GenesisBlock = new Block();

                Blocks.Add(GenesisBlock);
                Last = GenesisBlock;
                Save(Last);
            }
            else
            {
                if (Check())
                {
                    Last = Blocks.Last();
                }
                else
                {
                    throw new Exception("Ошибка получения блоков из базы данных. Цепочка не прошла проверку на целостность.");
                }    
            }
           
        }

        public bool Check()//проверка корректности цепочки, тру - корректна
        {

                var genesisBlock = new Block();
                var previousHash = genesisBlock.Hash;

                foreach(var block in Blocks.Skip(1))
                {
                    var hash = block.PreviousHash;
                    
                    if (previousHash != hash)
                    {
                        return false;
                    }

                    previousHash = block.Hash;
                }
            

            return true;
        }

        private void Save(Block block)//Метод записи блока в базу данных
        {
            using (var db = new BlockchainContext())
            {
                db.Blocks.Add(block);
                db.SaveChanges();
            }
        }

        private List<Block> LoadChainFromDB()
        {
            List<Block> result;

            using (var db = new BlockchainContext())
            {
                var count = db.Blocks.Count();

                result = new List<Block>(count * 2);

                result.AddRange(db.Blocks);
            }

            return result;
        }

        private void Sync()
        {

        }

    }
}
