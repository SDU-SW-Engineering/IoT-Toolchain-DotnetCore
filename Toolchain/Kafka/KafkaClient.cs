using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Toolchain.Kafka {
    public class KafkaClient {
        private string Brokers { get; set; }
        private string UniqueServiceId { get; set; }

        public KafkaClient(string uniqueServiceId, string brokers = null) {
            this.UniqueServiceId = uniqueServiceId;
            if (brokers == null) { this.Brokers = KafkaExtensions.GetBrokers(); }
            this.Brokers = brokers;
        }

        private void ProducerHandler(DeliveryReport<Null, string> report) {
            if (!report.Error.IsError) {
                Console.WriteLine($"Delivered message to {report.TopicPartitionOffset}");
            } else {
                Console.WriteLine($"Delivery Error: {report.Error.Reason}");
            }
        }

        public void SimpleProduce(string topic, List<Message<Null, string>> messages) {
            var producerConfig = new ProducerConfig { BootstrapServers = this.Brokers };

            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build()) {
                foreach (var message in messages) {
                    producer.Produce(topic, message, ProducerHandler);
                }
                // wait for up to 10 seconds for any inflight messages to be delivered.
                producer.Flush(TimeSpan.FromSeconds(1 + (messages.Count / 10)));
            }
        }

        public async Task SimpleProduceAsync(string topic, List<Message<Null, string>> messages) {
            var producerConfig = new ProducerConfig { BootstrapServers = this.Brokers };

            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build()) {
                foreach (var message in messages) {
                    try {
                        var deliveryReport = await producer.ProduceAsync(topic, message);
                        Console.WriteLine($"delivered to: {deliveryReport.TopicPartitionOffset}");
                    } catch (ProduceException<string, string> e) {
                        Console.WriteLine($"Failed to deliver message: {e.Message} [{e.Error.Code}]");
                    }
                }
                // wait for up to 10 seconds for any inflight messages to be delivered.
                producer.Flush(TimeSpan.FromSeconds(1 + (messages.Count / 10)));
            }
        }

        public void SimpleConsume(string topic, AutoOffsetReset offset, Action<ConsumeResult<Ignore, string>> messageHandler) {
            var consumerConfig = new ConsumerConfig {
                GroupId = this.UniqueServiceId,
                BootstrapServers = this.Brokers,
                AutoOffsetReset = offset
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build()) {
                consumer.Subscribe(topic);

                try {
                    while (true) {
                        try {
                            var consumeResult = consumer.Consume();
                            messageHandler(consumeResult);
                        } catch (ConsumeException e) {
                            Console.WriteLine($"Error occurred: {e.Error.Reason}");
                        }
                    }
                } catch (OperationCanceledException) {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    consumer.Close();
                }
            }
        }

        public ConsumeResult<Ignore, string> ManualConsume(string topic, AutoOffsetReset offset) {
            Console.WriteLine("Starting Manual Consume");

            var consumerConfig = new ConsumerConfig {
                GroupId = this.UniqueServiceId,
                BootstrapServers = this.Brokers,
                AutoOffsetReset = offset
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build()) {
                consumer.Subscribe(topic);

                try {
                    ConsumeResult<Ignore, string> consumeResult = consumer.Consume();
                    consumer.Close();
                    return consumeResult;
                } catch (OperationCanceledException) {
                    consumer.Close();
                    return null;
                }
            }
        }

        
    }
}