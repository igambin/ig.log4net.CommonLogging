using System.Collections.Generic;
using System.Xml.Serialization;

namespace ig.log4net.Logging.Serializers
{
    /* 
     Licensed under the Apache License, Version 2.0

     http://www.apache.org/licenses/LICENSE-2.0
     */
    namespace Xml2CSharp
    {
        [XmlRoot(ElementName = "param")]
        public class Param
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlAttribute(AttributeName = "value")]
            public string Value { get; set; }
        }

        [XmlRoot(ElementName = "log4net")]
        public class Log4NetConfig
        {
            [XmlElement(ElementName = "param")]
            public List<Param> Param { get; set; }
        }
    }
}
