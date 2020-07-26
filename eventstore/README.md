
# run event store
EventStore.ClusterNode.exe -MemDb true -RunProjections All -ExtIp 192.168.0.130
EventStore.ClusterNode.exe -MemDb true -RunProjections All -ExtIp 192.168.0.130 -IntIp 192.168.0.130


# curl on windows cmd
curl.exe -i -H "Accept:application/vnd.eventstore.atom+json" "http://192.168.0.130:2113/streams/$all/head/backward/1" -u "admin:changeit"

# curl on windows ps
curl.exe -i -H 'Accept:application/vnd.eventstore.atom+json' 'http://192.168.0.130:2113/streams/$all/head/backward/1' -u 'admin:changeit'

# curl on linux
curl -i -H 'Accept:application/vnd.eventstore.atom+json' 'http://192.168.0.130:2113/streams/$all/head/backward/1' -u 'admin:changeit'

