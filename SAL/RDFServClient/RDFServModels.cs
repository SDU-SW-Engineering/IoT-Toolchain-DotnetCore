using System;
using Confluent.Kafka;
using Newtonsoft.Json;
using BOSToolchain.SAL;

namespace BOSToolchain.SAL {
    public class QueryRequest : RDFServMessage {
        [JsonProperty(PropertyName = "query")]
        public string Query { get; set; }

        [JsonProperty(PropertyName = "result-path")]
        public string ResultPath { get; set; }

        public override string AsJSON() {
            //return "{\"result-path\":\" " + ResultPath + "\", \"query\": \"" + Query.RemoveWhitespace() + "\"}";
            Query = Query.RemoveWhitespace();
            return JsonConvert.SerializeObject(this);
        }
    }

    public class QueryResponse : RDFServMessage {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
        [JsonProperty(PropertyName = "results")]
        public string Results { get; set; }

        public override string AsJSON() {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class AddServiceRequest : RDFServMessage {
        [JsonProperty(PropertyName = "service")]
        [JsonConverter(typeof(RawJsonConverter))]
        public string Service { get; set; }

        [JsonProperty(PropertyName = "result-path")]
        public string ResultPath { get; set; }


        public override string AsJSON() {
            Service = Service.RemoveWhitespace();
            return JsonConvert.SerializeObject(this);
        }
    }

    public class AddServiceResponse : RDFServMessage {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
        [JsonProperty(PropertyName = "results")]
        public string Results { get; set; }

        public override string AsJSON() {
            return JsonConvert.SerializeObject(this);
        }
    }

    public abstract class RDFServMessage : IJSONAble {
        public Message<Null, string> AsMessage() {
            return new Message<Null, string> {
                Value = AsJSON()
            };
        }

        public abstract string AsJSON();
    }

    public interface IJSONAble {
        string AsJSON();
    }
}