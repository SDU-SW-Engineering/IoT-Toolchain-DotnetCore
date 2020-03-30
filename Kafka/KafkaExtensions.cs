using System;

namespace BOSToolchain.Kafka {
    public static class KafkaExtensions {
        public static string GetBrokers(string revertToIfNotSet) {
            return GetEnvironmentVariable("kafka_brokers", revertToIfNotSet);
        }

        public static string GetBrokers() {
            return GetBrokers("localhost:9092");
        }

        public static string GetEnvironmentVariable(string environmentKey, string revertToIfNotSet) {
            string envVariable = Environment.GetEnvironmentVariable(environmentKey);
            if (string.IsNullOrEmpty(envVariable)) envVariable = revertToIfNotSet;
            return envVariable;
        }

        public static string GetEnvironmentVariable(string environmentKey) {
            string envVariable = Environment.GetEnvironmentVariable(environmentKey);
            if (string.IsNullOrEmpty(envVariable)) envVariable = "";
            return envVariable;
        }
    }
}