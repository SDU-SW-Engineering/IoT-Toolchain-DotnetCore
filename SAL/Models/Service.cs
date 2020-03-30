using System;
using System.Collections.Generic;
using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BOSToolchain.SAL {
    public class Service {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "hasServiceEndpoint")]
        public List<ServiceEndpoint> HasServiceEndpoint { get; set; }

        public IGraph GenerateGraph() {
            IGraph graph = new Graph().AddStandardSALNamespaces();
            RdfHelpers.Graph = graph;
            var thisNode = RdfHelpers.CreateEntity(RdfNS.SALModel, IdGenerator.NewId("Service"));

            //Defining type of this object
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.RDFUri, "type",
                RdfNS.SALUri, "Service"
            ));
            //Set Data property location
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "name",
                this.Name
            ));

            if (this.HasServiceEndpoint != null) {
                foreach (var serviceEndpoint in this.HasServiceEndpoint.FindAll(m => m is KafkaRServiceEndpoint).ToList()) {
                    serviceEndpoint.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "hasKafkaRServiceEndpoint"));
                }
                foreach (var serviceEndpoint in this.HasServiceEndpoint.FindAll(m => m is KafkaWServiceEndpoint).ToList()) {
                    serviceEndpoint.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "hasKafkaWServiceEndpoint"));
                }
                foreach (var serviceEndpoint in this.HasServiceEndpoint.FindAll(m => m is KafkaRWServiceEndpoint).ToList()) {
                    serviceEndpoint.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "hasKafkaRWServiceEndpoint"));
                }
                foreach (var serviceEndpoint in this.HasServiceEndpoint.FindAll(m => m is RestServiceEndpoint).ToList()) {
                    serviceEndpoint.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "hasRestServiceEndpoint"));
                }
            }

            return graph;
        }

        public string GenerateString() {
            return StringWriter.Write(GenerateGraph(), new CompressingTurtleWriter());
        }

        public RDFServServiceModels.Service GenerateRDFServObject() {
            var service = new RDFServServiceModels.Service {
                name = Name,
                hasKafkaRServiceEndpoint = new List<RDFServServiceModels.HasKafkaRServiceEndpoint>(),
                hasKafkaRWServiceEndpoint = new List<RDFServServiceModels.HasKafkaRWServiceEndpoint>(),
                hasKafkaWServiceEndpoint = new List<RDFServServiceModels.HasKafkaWServiceEndpoint>(),
                hasRestServiceEndpoint = new List<RDFServServiceModels.HasRestServiceEndpoint>()
            };

            foreach (var item in HasServiceEndpoint) {
                item.GenerateRDFServObject(service);
            }

            return service;
        }
        public string AsRdfservJSON() {
            return JsonConvert.SerializeObject(GenerateRDFServObject(), Formatting.Indented);
        }
    }
}