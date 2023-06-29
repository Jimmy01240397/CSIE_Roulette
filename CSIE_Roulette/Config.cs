using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace CSIE_Roulette
{ 
    public class Config
    {
        public int SmallPrizeChance { get; set; }
        public List<string> BigPrizeTime { get; set; }
        private static Config _instance = null;
        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    var deserializer = new DeserializerBuilder().Build();
                    _instance = deserializer.Deserialize<Config>(File.ReadAllText("config.yaml"));
                }
                return _instance;
            }
        }

    }
}
