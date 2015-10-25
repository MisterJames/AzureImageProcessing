FROM sebp/elk 
MAINTAINER Simon Timms
ENV REFRESHED_AT 2015-10-12

####### insert our key and crt file ########
COPY logstash-forwarder.key /etc/pki/tls/private/logstash-forwarder.key 
COPY logstash-forwarder.crt /etc/pki/tls/certs/logstash-forwarder.crt

####### start ###########

EXPOSE 5601 9200 9300 5000
VOLUME /var/lib/elasticsearch

CMD [ "/usr/local/bin/start.sh" ]
