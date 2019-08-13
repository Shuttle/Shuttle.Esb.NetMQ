# Shuttle.Esb.NetMQ

NetMQ implementation for use with Shuttle.Esb.

## NetMQQueue

NetMQ does not provide 2-phase commit out-of-the-box.  Although implementing it is not too much effort the 2PC adds tremendous overhead (as it does for anything).  For this reason shuttle does not use 2PC with NetMQ.

Instead we rely on the [idempotence service]({{ site.baseurl }}/idempotence-service/index.html).

Since NetMQ talks directly to a queue on any server it is suggested that you use an outbox that specifies a local queue just in case the remote queue is not immediately available.

### Installation

If you need to install NetMQ you can <a target='_blank' href='https://www.rabbitmq.com/install-windows.html'>follow these instructions</a>.

### Configuration

The queue configuration is part of the specified uri, e.g.:

~~~ xml
    <inbox
      workQueueUri="rabbitmq://username:password@host:port/virtualhost/queue?prefetchCount=25&amp;durable=true&amp;persistent=true"
	  .
	  .
	  .
    />
~~~

| Segment / Argument | Default	| Description | Version Introduced |
| --- | --- | --- | --- |
| username:password	 | empty|	| |
| virtualhost		 | /	|	| |
| port				 | default	|	| |
| prefetchcount			 | 25		| Specifies the number of messages to prefetch from the queue. | |
| durable			 | true     | Determines whether the queue is durable.  Note: be very mindful of the possible consequences before setting to 'false'. | v3.5.0 |
| persistent			 | true     | Determines whether messages will be persisted.  Note: be very mindful of the possible consequences before setting to 'false'. | v3.5.3 |
| priority			 | empty     | Determines the number of priorities supported by the queue. | v10.0.10 |

In addition to this there is also a NetMQ specific section (defaults specified here):

~~~ xml
<configuration>
  <configSections>
    <section name='rabbitmq' type="Shuttle.Esb.NetMQ.NetMQSection, Shuttle.Esb.NetMQ"/>
  </configSections>
  
  <rabbitmq
	localQueueTimeoutMilliseconds="250"
	remoteQueueTimeoutMilliseconds="2000"
	connectionCloseTimeoutMilliseconds="1000"
	requestedHeartbeat="30"
	operationRetryCount="3"
  />
  .
  .
  .
<configuration>
~~~