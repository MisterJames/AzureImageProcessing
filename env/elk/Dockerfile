FROM sebp/elk 
MAINTAINER Simon Timms
ENV REFRESHED_AT 2016-02-23

####### insert our key and crt file ########
COPY logstash-forwarder.key /etc/pki/tls/private/logstash-forwarder.key 
COPY logstash-forwarder.crt /etc/pki/tls/certs/logstash-forwarder.crt
COPY logstash-beats.key /etc/pki/tls/private/logstash-beats.key 
COPY logstash-beats.crt /etc/pki/tls/certs/logstash-beats.crt

####### beats input should use the json codec ######
COPY 02-beats-input.conf /etc/logstash/conf.d/02-beats-input.conf

####### start ###########

EXPOSE 5601 9200 9300 5000
VOLUME /var/lib/elasticsearch

CMD [ "/usr/local/bin/start.sh" ]
