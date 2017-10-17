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