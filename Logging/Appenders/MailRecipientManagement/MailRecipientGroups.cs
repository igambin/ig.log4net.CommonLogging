using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ig.log4net.Logging.Appenders.MailRecipientManagement
{
    [XmlRoot]
    [Serializable]
    public class MailRecipientGroups
    {
        [XmlElement("MailRecipientGroup")]
        public List<MailRecipientGroup> Recipients { get; set; }
    }


}
