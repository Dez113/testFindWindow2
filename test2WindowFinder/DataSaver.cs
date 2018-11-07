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
        public delegate void DataSaving(ref List<IpParams> dict);
        public delegate void DataRestoring(ref List<IpParams> dict);
        public static DataSaving onSave;
        public static DataSaving onRestore;

        public static void Save(List<IpParams> dict)
        {
            //List<IpParams> dict = new List<IpParams>();
            //onSave(ref dict);

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
            Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(json);
            //Console.WriteLine(obj);
            List<IpParams> dict = obj.ToObject<List<IpParams>>();
            //Console.WriteLine(dict["contacts"]);
            //onRestore(ref dict);
            return dict;
        }
    }
}
