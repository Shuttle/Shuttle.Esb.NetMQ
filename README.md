# Shuttle.Esb.NetMQ

NetMQ implementation for use with Shuttle.Esb.  Since NetMQ/ZeroMQ does not provide a queue as such this mechanism is used to *front* other queues or to make use of in-memory queues.

## NetMQQueue

The queue configuration is part of the specified uri, e.g.:

``` xml
    <inbox
      workQueueUri="netmq://host-ip:port/queue-name"
	  .
	  .
	  .
    />
```

| Segment / Argument | Default	| Description | Version Introduced |
| --- | --- | --- | --- |
| host-ip		 | none	| The IP address of the Shuttle.Esb.NetMQ.Server that will process all requests. | |
| port				 | none	| The port on which the Shuttle.Esb.NetMQ.Server will be listening for connections. | |
| queue-name | none | The queue name that needs to be accessed. | |

## Shuttle.Esb.NetMQ.Server

The server is required to listen to any requests from clients.  Since the server acts as a *go-between* for an actual queue you would need to specify the `uri` of the actual queue represented by a `name` using the application configuration file:

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="netmq" type="Shuttle.Esb.NetMQ.Server.NetMQSection, Shuttle.Esb.NetMQ.Server"/>
  </configSections>

  <netmq port="6565">
    <queues>
      <add name="queue-one" uri="rabbitmq://user:password@localhost/actual-queue-one" />
      <add name="queue-two" uri="msmq://./actual-queue-two" />
      <add name="queue-three" uri="memory://actual-queue-three" />
    </queues>
  </netmq>
</configuration>
```