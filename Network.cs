using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZFX
{
    class Network
    {
        public Network()
        {

        }
        public string BytesToString(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        public byte[] getBytes(string url)
        {
            if (!url.StartsWith("https://") || !url.StartsWith("http://"))
            {
                url = "https://" + url;
            }
            return new WebClient().DownloadData(url);
        }
        public string get(string url)
        {
            return BytesToString(getBytes(url));
        }
    }
}
