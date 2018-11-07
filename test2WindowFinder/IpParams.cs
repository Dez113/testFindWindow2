using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test2WindowFinder
{
    
    public class IpParams
    {
        public string _name;
        public string _ip;
        public string _port;
        public string _sub;
        public string _gw;
        
        public IpParams (string Name, string Ip, string Port, string Subnet, string Gw)
        {
            this._name = Name;
            this._ip = Ip;
            this._port = Port;
            this._sub = Subnet;
            this._gw = Gw;
        } 
    }
}
