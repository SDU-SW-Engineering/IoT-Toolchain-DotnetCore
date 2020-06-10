using System.Collections.Generic;
using System;
using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;
using Newtonsoft.Json;
using IoTToolchain.SAL.RDFServServiceModels;
using IoTToolchain.SAL;


namespace IoTToolchain.SAL {
    public abstract class ServiceEndpoint {
        [JsonProperty(PropertyName = "priority")]
        public int Priority { get; set; }

        [JsonProperty(PropertyName = "ownedBy")]
        public Organization OwnedBy { get; set; }

        public abstract void AddTriples(IGraph graph, INode parent, INode predicate);
        protected virtual void AddTriplesFromSubClass(IGraph graph, INode parent, INode thisNode) {
            if (graph == null) { throw new ArgumentNullException(nameof(graph)); }
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (thisNode == null) { throw new ArgumentNullException(nameof(thisNode)); }

            this.OwnedBy.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "ownedBy"));

            //set object property Priority
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "priority",
                this.Priority.ToString()
            ));
        }

        internal abstract void GenerateRDFServObject(RDFServServiceModels.Service service);
    }

    public class KafkaRWServiceEndpoint : ServiceEndpoint {
        [JsonProperty(PropertyName = "read_topic")]
        public string ReadTopic { get; set; }

        [JsonProperty(PropertyName = "write_topic")]
        public string WriteTopic { get; set; }

        [JsonProperty(PropertyName = "consumesInformation")]
        public List<Information> ConsumesInformation { get; set; }

        [JsonProperty(PropertyName = "providesInformation")]
        public List<Information> ProvidesInformation { get; set; }

        public override void AddTriples(IGraph graph, INode parent, INode predicate) {
            if (graph == null) { throw new ArgumentNullException(nameof(graph)); }
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            string name = IdGenerator.NewId("KafkaRWServiceEndpoint");
            var thisNode = RdfHelpers.CreateEntity(RdfNS.SALModel, name);

            //Defining type of this object
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.RDFUri, "type",
                RdfNS.SALUri, "KafkaRWServiceEndpoint"
            ));
            //Define relationship to parent
            graph.Assert(RdfHelpers.CreateTriple(
                parent,
                predicate,
                thisNode
            ));
            //run superclass stuff.
            base.AddTriplesFromSubClass(graph, parent, thisNode);

            //Set Data property read_topic
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "read_topic",
                this.ReadTopic
            ));

            //Set Data property write_topic
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "write_topic",
                this.WriteTopic
            ));

            foreach (var item in this.ConsumesInformation) {
                item.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "consumesInformation"));
            }

            foreach (var item in this.ProvidesInformation) {
                item.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "providesInformation"));
            }
        }

        internal override void GenerateRDFServObject(RDFServServiceModels.Service service) {
            var endpoint = new RDFServServiceModels.HasKafkaRWServiceEndpoint {
                priority = Priority.ToString(),
                read_topic = ReadTopic,
                write_topic = WriteTopic,
                consumesInformation = new List<RDFServServiceModels.Information>(),
                providesInformation = new List<RDFServServiceModels.Information>()

            };

            OwnedBy.GenerateRDFServObject(endpoint);

            foreach (var item in ConsumesInformation) {
                endpoint.consumesInformation.Add(item.GenerateRDFServObject());
            }

            foreach (var item in ProvidesInformation) {
                endpoint.providesInformation.Add(item.GenerateRDFServObject());
            }

            service.hasKafkaRWServiceEndpoint.Add(endpoint);
        }
    }

    public class KafkaRServiceEndpoint : ServiceEndpoint {
        [JsonProperty(PropertyName = "read_topic")]
        public string ReadTopic { get; set; }
        [JsonProperty(PropertyName = "providesInformation")]
        public List<Information> ProvidesInformation { get; set; }

        public override void AddTriples(IGraph graph, INode parent, INode predicate) {
            if (graph == null) { throw new ArgumentNullException(nameof(graph)); }
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            string name = IdGenerator.NewId("KafkaRServiceEndpoint");
            var thisNode = RdfHelpers.CreateEntity(RdfNS.SALModel, name);

            //Defining type of this object
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.RDFUri, "type",
                RdfNS.SALUri, "KafkaRServiceEndpoint"
            ));
            //Define relationship to parent
            graph.Assert(RdfHelpers.CreateTriple(
                parent,
                predicate,
                thisNode
            ));
            //run superclass stuff.
            base.AddTriplesFromSubClass(graph, parent, thisNode);

            //Set Data property read_topic
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "read_topic",
                this.ReadTopic
            ));

            foreach (var item in this.ProvidesInformation) {
                item.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "providesInformation"));
            }
        }

        internal override void GenerateRDFServObject(RDFServServiceModels.Service service) {
            var endpoint = new RDFServServiceModels.HasKafkaRServiceEndpoint {
                priority = Priority.ToString(),
                read_topic = ReadTopic,
                providesInformation = new List<RDFServServiceModels.Information>()
            };

            OwnedBy.GenerateRDFServObject(endpoint);

            foreach (var item in ProvidesInformation) {
                endpoint.providesInformation.Add(item.GenerateRDFServObject());
            }

            service.hasKafkaRServiceEndpoint.Add(endpoint);
        }
    }

    public class KafkaWServiceEndpoint : ServiceEndpoint {
        [JsonProperty(PropertyName = "write_topic")]
        public string WriteTopic { get; set; }

        [JsonProperty(PropertyName = "consumesInformation")]
        public List<Information> ConsumesInformation { get; set; }

        public override void AddTriples(IGraph graph, INode parent, INode predicate) {
            if (graph == null) { throw new ArgumentNullException(nameof(graph)); }
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            string name = IdGenerator.NewId("KafkaWServiceEndpoint");
            var thisNode = RdfHelpers.CreateEntity(RdfNS.SALModel, name);

            //Defining type of this object
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.RDFUri, "type",
                RdfNS.SALUri, "KafkaWServiceEndpoint"
            ));
            //Define relationship to parent
            graph.Assert(RdfHelpers.CreateTriple(
                parent,
                predicate,
                thisNode
            ));
            //run superclass stuff.
            base.AddTriplesFromSubClass(graph, parent, thisNode);

            //Set Data property write_topic
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "write_topic",
                this.WriteTopic
            ));

            foreach (var item in this.ConsumesInformation) {
                item.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "consumesInformation"));
            }
        }

        internal override void GenerateRDFServObject(RDFServServiceModels.Service service) {
            var endpoint = new RDFServServiceModels.HasKafkaWServiceEndpoint {
                priority = Priority.ToString(),
                write_topic = WriteTopic,
                consumesInformation = new List<RDFServServiceModels.Information>()
            };

            OwnedBy.GenerateRDFServObject(endpoint);

            foreach (var item in ConsumesInformation) {
                endpoint.consumesInformation.Add(item.GenerateRDFServObject());
            }

            service.hasKafkaWServiceEndpoint.Add(endpoint);
        }
    }

    public class RestServiceEndpoint : ServiceEndpoint {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "verb")]
        public HTTPVerb Verb { get; set; }

        [JsonProperty(PropertyName = "consumesInformation")]
        public List<Information> ConsumesInformation { get; set; }

        [JsonProperty(PropertyName = "providesInformation")]
        public List<Information> ProvidesInformation { get; set; }

        public override void AddTriples(IGraph graph, INode parent, INode predicate) {
            if (graph == null) { throw new ArgumentNullException(nameof(graph)); }
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            string name = IdGenerator.NewId("RestServiceEndpoint");
            var thisNode = RdfHelpers.CreateEntity(RdfNS.SALModel, name);

            //Defining type of this object
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.RDFUri, "type",
                RdfNS.SALUri, "RestServiceEndpoint"
            ));

            //Define relationship to parent
            graph.Assert(RdfHelpers.CreateTriple(
                parent,
                predicate,
                thisNode
            ));
            //run superclass stuff.
            base.AddTriplesFromSubClass(graph, parent, thisNode);

            //Set Data property url
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "url",
                this.Url
            ));

            //set object property TemporalAspect
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "verb",
                RdfNS.SALIUri, Enum.GetName(typeof(HTTPVerb), this.Verb).ToUpper()
            ));

            foreach (var item in this.ConsumesInformation) {
                item.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "consumesInformation"));
            }

            foreach (var item in this.ProvidesInformation) {
                item.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "providesInformation"));
            }
        }

        internal override void GenerateRDFServObject(RDFServServiceModels.Service service) {
            var endpoint = new RDFServServiceModels.HasRestServiceEndpoint {
                priority = Priority.ToString(),
                verb = Enum.GetName(typeof(HTTPVerb), this.Verb).ToUpper(),
                url = Url,
                consumesInformation = new List<RDFServServiceModels.Information>(),
                providesInformation = new List<RDFServServiceModels.Information>()
            };

            OwnedBy.GenerateRDFServObject(endpoint);

            foreach (var item in ConsumesInformation) {
                endpoint.consumesInformation.Add(item.GenerateRDFServObject());
            }

            foreach (var item in ProvidesInformation) {
                endpoint.providesInformation.Add(item.GenerateRDFServObject());
            }

            service.hasRestServiceEndpoint.Add(endpoint);
        }
    }
}