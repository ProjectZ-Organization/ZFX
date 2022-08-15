using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZCPU
{
    class Network
    {
        public Network()
        {

        }
        /// <summary>
        /// Converts bytes to string
        /// </summary>
        /// <param name="bytes">The byte array to convert</param>
        /// <returns>The converted string</returns>
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
        /// <summary>
        /// Download the bytes of a website
        /// </summary>
        /// <param name="url">The URL</param>
        /// <returns>A byte[] of what the website contains</returns>
        public byte[] getBytes(string url)
        {
            if (!url.StartsWith("https://") || !url.StartsWith("http://"))
            {
                url = "https://" + url;
            }
            return new WebClient().DownloadData(url);
        }
        /// <summary>
        /// I honestly dont remember.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string get(string url)
        {
            return BytesToString(getBytes(url));
        }
    }
}
