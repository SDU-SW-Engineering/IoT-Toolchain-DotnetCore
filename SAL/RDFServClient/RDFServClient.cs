using System;
using System.Collections.Generic;
using System.Threading;
using IoTToolchain.Kafka;
using IoTToolchain.SAL.RDFServServiceModels;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace IoTToolchain.SAL {
    public class RDFServClient {
        private string ServerTopic { get; set; }
        private string QueryResultPath { get; set; }
        private string QueryPath { get; set; }
        private string AddServiceResultPath { get; set; }
        private string AddServicePath { get; set; }
        private KafkaClient Client { get; set; }

        public RDFServClient(string kafkaServer, string kafkaClientId, string serverTopic) {
            ServerTopic = serverTopic;
            Client = new KafkaClient(kafkaClientId, kafkaServer);
            QueryResultPath = ServerTopic + "_" + kafkaClientId + "_response_query";
            QueryPath = ServerTopic + "_query";
            AddServiceResultPath = ServerTopic + "_" + kafkaClientId + "_response_addservice";
            AddServicePath = ServerTopic + "_add-service";
        }

        public ConsumeResult<Ignore, string> Query(string query) {
            var message = new QueryRequest {
                ResultPath = QueryResultPath,
                Query = query,
            };
            Console.WriteLine(message.AsJSON());
            Client.SimpleProduce(QueryPath, message.AsMessage().AsMessages());

            return Client.ManualConsume(QueryResultPath, AutoOffsetReset.Earliest);
        }



        public bool AddService(Service service) {
            var message = new AddServiceRequest {
                ResultPath = AddServiceResultPath,
                Service = service.AsRdfservJSON(), //TODO: Fix parseing of service as it does not work with RDFServ
            };
            Console.WriteLine(message.AsJSON());
            Client.SimpleProduce(AddServicePath, message.AsMessage().AsMessages());

            try {
                var rawResult = Client.ManualConsume(AddServiceResultPath, AutoOffsetReset.Earliest);
                AddServiceResponse result = JsonConvert.DeserializeObject<AddServiceResponse>(rawResult.Message.Value);
                if (result.Success) {
                    return true;
                } else {
                    Console.WriteLine(rawResult.Message.Value);
                    return false;
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
