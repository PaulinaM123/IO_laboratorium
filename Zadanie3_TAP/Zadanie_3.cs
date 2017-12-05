using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Zadanie3_TAP
{
    class Zadanie_3
    {
        public async Task<XmlDocument> Zadanie3(string address)
        {
            WebClient webClient = new WebClient(); ;
            var result = await webClient.DownloadStringTaskAsync(new Uri(address));
            XmlDocument myXML = new XmlDocument();
            myXML.LoadXml(result);

            return myXML;
        }
        static void Main(string[] args)
        {
            Zadanie_3 z = new Zadanie_3();

            var task = z.Zadanie3("http://www.feedforall.com/sample.xml");
            var xml = task.Result;

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                xml.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                Console.WriteLine(stringWriter.GetStringBuilder().ToString());
            }
        }

    }
}