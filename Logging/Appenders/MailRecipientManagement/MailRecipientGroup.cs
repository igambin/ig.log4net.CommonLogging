using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ig.log4net.Logging.Appenders.MailRecipientManagement
{
    [Serializable]
    public class MailRecipientGroup
    {
        [XmlAttribute] public string Name { get; set; }
        [XmlAttribute] public string SubjectPrefix { get; set; }
        [XmlElement("Recipient")] public string[] Recipients { get; set; }

        [XmlIgnore] public List<string> RecipientList => Recipients.Where(x => !string.IsNullOrWhiteSpace(x.Trim())).ToList();

        public string PrefixSubject(string subject, int maxLength)
        {
            var result = $"{SubjectPrefix?.Trim() ?? ""} {subject}".Trim();
            return result.Length > maxLength ? result.Substring(0, maxLength - 3) + "..." : result;
        }
    }
}
