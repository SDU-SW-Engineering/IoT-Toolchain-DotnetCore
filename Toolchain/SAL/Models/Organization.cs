using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;
using System;
using VDS.RDF.Nodes;
using Newtonsoft.Json;
using Toolchain.SAL.RDFServServiceModels;

namespace Toolchain.SAL {
    public class Organization {
        [JsonProperty(PropertyName = "legalName")]
        public string LegalName { get; set; }

        public void AddTriples(IGraph graph, INode parent, INode predicate) {
            if (graph == null) { throw new ArgumentNullException(nameof(graph)); }
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            graph.Assert(RdfHelpers.CreateTriple(
                RdfNS.SALModel, "organization_sdu",
                RdfNS.RDFUri, "type",
                RdfNS.SchemaOrgUri, "Organization"
            ));
            graph.Assert(RdfHelpers.CreateTriple(
                RdfNS.SALModel, "organization_sdu",
                RdfNS.SchemaOrgUri, "legalName",
                this.LegalName
            ));
            graph.Assert(RdfHelpers.CreateTriple(
                parent,
                predicate,
                RdfNS.SALModel, "organization_sdu"
            ));
        }

        internal void GenerateRDFServObject(RDFServServiceModels.HasServiceEndpoint endpoint) {
            endpoint.ownedBy = new OwnedBy {
                legalName = LegalName
            };
        }
    }
}