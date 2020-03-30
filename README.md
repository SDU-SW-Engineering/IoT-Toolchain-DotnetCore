# .Net Core tool chain for Kafka

This is a library that contains all the common components for the Kafka related functionality using .net core.

## Kafka Client Wrapper

Below an example is given of how to use this sub module:

```csharp
using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using ToolChainForKafka.Kafka;

namespace Example {
    class Program {
        static void Main(string[] args) {
            var client = new KafkaClientWrapper("MyServiceName", "tek-coordicy0b.tek.sdu.dk:9300");

            var t = new Thread(new ThreadStart(() => {
                client.SimpleConsume("test-topic", AutoOffsetReset.Earliest, (x) => {
                    Console.WriteLine($"Consumed message '{x.Value}' at: '{x.TopicPartitionOffset}'.");
                });
            }));
            t.IsBackground = true;
            t.Start();


            var messages = new List<Message<Null, string>>();

            for (int i = 0; i < 10; i++) {
                var json = @"{
                    'Name': 'SomeName',  
                    'Description': '" + i.ToString() + @"'  
                    }";
                var message = new Message<Null, string> { Value = json };
                messages.Add(message);
            }

            client.SimpleProduce("test-topic", messages);

            Thread.Sleep(TimeSpan.FromSeconds(20));
        }
    }
}
```

## SAL

This class allows using the SAL, and provides the abstractions needed to query and receive messages.

## SAL Model Generator

Use it like this:

```csharp
var manager2 = new RDFStoreManager(RDFStoreManagerMode.Generic);

var room182 = new Room { Name = "U182" };
var sdu = new Organization { LegalName = "SDU" };

var serviceDescription = new Service {
    Name = "Service1",
    HasServiceEndpoint = new List<ServiceEndpoint> {
        new KafkaRWServiceEndpoint{
            Priority = 1,
            ReadTopic = "SomeReadTopic",
            WriteTopic = "SomeWriteTopic",
            OwnedBy = sdu,
            ConsumesInformation = new List<Information> {
                new Information {
                    Location = "Temp",
                    HasModality = Modality.Temperature,
                    HasUnit = Unit.DegreeCelsius,
                    HasTemporalAspect = TemporalAspect.Prediction,
                    HasLocation = room182,
                },
                new Information {
                    Location = "Hum",
                    HasModality = Modality.Humidity,
                    HasUnit = Unit.Percent,
                    HasTemporalAspect = TemporalAspect.Prediction,
                    HasLocation = room182,
                },
            },
            ProvidesInformation = new List<Information> {
                new Information {
                    Location = "Time",
                    HasModality = Modality.AbsoluteTime,
                    HasUnit = Unit.DateTime,
                    HasTemporalAspect = TemporalAspect.Prediction,
                    HasLocation = room182,
                }
            }
        },
        new RestServiceEndpoint{
            Priority = 1,
            Url = "http://www.testing.dk/someresturl",
            Verb = HTTPVerb.Get,
            OwnedBy = sdu,
            ConsumesInformation = new List<Information> {},
            ProvidesInformation = new List<Information> {
                new Information {
                    Location = "time",
                    HasModality = Modality.AbsoluteTime,
                    HasUnit = Unit.DateTime,
                    HasTemporalAspect = TemporalAspect.Prediction,
                    HasLocation = room182,
                }
            }
        }
    },
};
manager2.WriteCustomGraphToTurtleFile(serviceDescription.GenerateGraph(), "serviceDescription.ttl");

```