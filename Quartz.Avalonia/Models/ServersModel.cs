using QuartzAvalonia.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzAvalonia.Models
{
    public class ServersModel
    {
        public Server Server { get; set; }
        public int Index { get; private set; }

        public ServersModel(Server server, int index)
        {
            Server = server;
            Index = index;
        }
    }
}
