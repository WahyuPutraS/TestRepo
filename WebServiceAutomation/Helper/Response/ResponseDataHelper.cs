using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebServiceAutomation.Model.XmlModel;

namespace WebServiceAutomation.Helper.ResponseData
{
    public class ResponseDataHelper
    {

        /**
         ResponseDataHekper.DeserializeJsonResponse<Laptop>(responseData)
         public static Laptop DeserializeJsonResponse<T>(string responseData) where Laptop : class
        {
            return JsonConvert.DeserializeObject<Laptop>(responseData);

        }

        ResponseDataHelper.DeserializeJsonResponse<LaptopHybrid>(responseData)
        public static LaptopHybrid DeserializeJsonResponse<LaptopHybrid>(string responseData) where Laptop : class
        {
            return JsonConvert.DeserializeObject<LaptopHybrid>(responseData);

        }
        ResponseDataHelper.DeserializeJsonResponse<LaptopHybridv2>(responseData)
        public static LaptopHybridv2 DeserializeJsonResponse<LaptopHybridv2>(string responseData) where Laptop : class
        {
            return JsonConvert.DeserializeObject<LaptopHybridv2>(responseData);

        }

        */
        public static T DeserializeJsonResponse<T>(string responseData) where T : class
        {
            return JsonConvert.DeserializeObject<T>(responseData);
        }

        /**
         ResponseDataHelper.DeserializeXmlResponse<Laptop>(responseData)
          public static Laptop DeserializeXmlResponse<Laptop>(string responseData) where Laptop : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Laptop));
            TextReader textReader = new StringReader(responseData);
            return (Laptop)xmlSerializer.Deserialize(textReader);
        }
         ResponseDataHelper.DeserializeXmlResponse<LaptopHybrid>(responseData)

         public static LaptopHybrid DeserializeXmlResponse<LaptopHybrid>(string responseData) where LaptopHybrid : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LaptopHybrid));
            TextReader textReader = new StringReader(responseData);
            return (LaptopHybrid)xmlSerializer.Deserialize(textReader);
        }
         
         */

        public static T DeserializeXmlResponse<T>(string responseData) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            TextReader textReader = new StringReader(responseData);
            return (T)xmlSerializer.Deserialize(textReader);
        }
    }
}
