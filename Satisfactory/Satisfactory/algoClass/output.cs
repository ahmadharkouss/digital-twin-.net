using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Satisfactory
{

    internal class Output
    {
        private string _outputFilepath = "";
        private double BestTime { get; set; }

        public List<string> Result = new();

        public Output() {}

        public Output(string filepath)
        {
            _outputFilepath = filepath;
        }

        public void SetBestTime(double best_time)
        { 
            BestTime = best_time;
        }

        public void FromProductList(IEnumerable<Product> listo)
        {
            foreach(Product pro in listo)
            {
                AddToList(pro.GetId());
            }
        }

        public void AddToList(string productId)
        {
            Result.Add(productId);
        }

        public void WriteToFile()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            XElement listo =  new XElement("list");
            foreach (string elm in Result)
            {
                listo.Add(new XElement(elm));
            }

            XDocument xdoc = new XDocument(
                new XComment($"Estimated best time in secondes = {BestTime}s"),
                listo
            );

            StringWriter sw = new StringWriter();
            XmlWriter xWrite = XmlWriter.Create(sw, settings);
            xdoc.Save(xWrite);
            xWrite.Close();
            
            // Save to Disk
            xdoc.Save(_outputFilepath);
        }
    }
}
