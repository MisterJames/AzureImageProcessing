We would like to capture the logs from our system somewhere. 
 - there is no one place because we're using lots of services
 - we'd like to have some way to corrolate actions on one service with actions on another service 
 - for instnace if we trigger a DNS creation and it fails in the DNS service then we would like to know which request the failure was associated with
 - we can use a corrolation id to assicate messages but that doesn't solve where the logs go

Enter the ELK stack
	Elastic search
	Logstash
	Kibana
	
Let's start with log stash
 - log stash is like a sewage plant
 - no no, stay with me. 
 - contains a lot of pipes
 - message waste comes in and is cleaned and sorted and sometimes we add things to the messages
 - this is logstash. It can read messages from a wide variety of different sources
  - rabbit mq
  - http
  - websockets 
  - s3
  - github
  - files
  - syslog
  - ...
 - we can run arbitrary code to enhance the log messages things like 
  - convert IP addresses 
  - filter out events 
  - create new events from an existing event
  - remove personally identifying data or passwords or other configuration information
 - finally we can output the data from logstash to any one of a number of storage or notification tools
  - email
  - files
  - hipchat
  - slack 
  - cvs
  - elastic search