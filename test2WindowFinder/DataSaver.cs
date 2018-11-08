using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace test2WindowFinder
{
    class DataSaver
    {
        public static void Save(List<IpParams> dict)
        {
            string json = JsonConvert.SerializeObject(dict);
            byte[] save_data = Encoding.UTF8.GetBytes(json);
            FileStream fs1 = new FileStream("saves.json", FileMode.Create);
            fs1.Write(save_data, 0, save_data.Length);
            fs1.Close();
        }
        public static List<IpParams> Restore()
        {
            StreamReader fs = new StreamReader("saves.json");
            string json = fs.ReadToEnd();
            fs.Close();
            List<IpParams> ips = JsonConvert.DeserializeObject<List<IpParams>>(json);
            return ips;
        }
    }
}
