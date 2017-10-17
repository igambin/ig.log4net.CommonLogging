using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using ig.log4net.Extensions;
using ig.log4net.XmlTools;

namespace ig.log4net.Logging.Appenders.MailRecipientManagement
{

    // Convention: Add AppSettings with according strings
    //             following the pattern "Logging.RecipientGroups.<RecipientEnumName>"
    //             e. g. "Logging.RecipientGroups.Group1"
    public enum Recipients
    {
        Default,
        /* Examples
        Group1, 
        Group2,
        */
    }

    public static class RecipientsExtensions
    {
        public static List<MailRecipientGroup> GetRecipients(this Recipients recipientsEnum)
        {

            var configFileName = Path.Combine(typeof(Recipients).AssemblyDirectory(), "log4net.mailappender.recipients.xml");
            if (!File.Exists(configFileName))
            {
                CreateDefaultFile(configFileName);
            }

            var xml = File.ReadAllText(configFileName);
            var recipientGroups = Serialization.Deserialize<MailRecipientGroups>(xml);

            return recipientGroups.Recipients.Where(x => x.Name.Equals(recipientsEnum.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        private static void CreateDefaultFile(string configFileName)
        {
            var recipients = new List<MailRecipientGroup>
            {
                new MailRecipientGroup {
                    Name = "Default",
                    Recipients = new []{ "test@mydomain.xyz"},
                    SubjectPrefix = "DefaultPrefix "
                },
            };

            var example = Serialization.Serialize(recipients);

            try
            {
                using (var f = File.Create(configFileName))
                {
                    var bytes = example.ToByteArray();
                    f.Write(bytes, 0, bytes.Length);
                }
            }
            catch
            {
                throw new ConfigurationErrorsException(
                      $"Please make sure you have a file called 'log4net.mailappender.recipients.xml' in "
                    + $"your working directory.{Environment.NewLine}The file is required to contain at "
                    + $"least the 'Default'-MailRecipientGroup, e. g.:{Environment.NewLine}{example}");
            }
            
        }
    }
}
