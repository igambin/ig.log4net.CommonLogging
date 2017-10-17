=====================================================
===       ig.log4net.Logging Wrapper ReadMe       ===
=====================================================

After installation of the nuget-Package, ensure that 
the advanced properties of both files

- log4net.config 
- log4net.mailappender.recipients.xml

are configured as follows

- Build Action            : Content
- Copy to Output Directory: Copy Always

or else the log4net-configuration will not be copied
to the build-target-directory.



If you use the MailAppender for sending out emails, you can
configure 2 settings in the log4net.config file:

  <!-- Any log message with at least the set log-level will
       cause a notification to email recipients
    -->
  <param name="MailNotificationLevel" value="Warn" />

  <!-- If this setting is true, any log message containing 
       an exception will cause a notification to email
       recipients
    -->
  <param name="AlwaysNotifyByMailOnException" value="true|false" />


  
Additionally it is possible to configure your app|web.config 
in your application to use the following appSettings:

<appSettings>
    <!-- logging will add StackTraces of the "LOG"-call (where was the log message written) -->
    <add key="Logging.EvaluateStackTraces" value="true|false" />
</appSettings>



