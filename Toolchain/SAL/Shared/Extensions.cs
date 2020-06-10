using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Confluent.Kafka;
using VDS.RDF;
using VDS.RDF.Writing;

namespace Toolchain.SAL {
    public static class IdGenerator {
        public static string NewId(string type) {
            var id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            return $"{type}_{id}";
        }
    }

    public static class KafkaHelper {
        public static List<Message<Null, string>> AsMessages(this Message<Null, string> message) {
            return new List<Message<Null, string>> { message };
        }
    }
}