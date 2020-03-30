using System;
using VDS.RDF;

namespace BOSToolchain.SAL {
    public static class RdfNS {
        public static string SALUri { get { return @"https://ontology.hviidnet.com/sal.ttl#"; } }
        public static string SALIUri { get { return @"https://ontology.hviidnet.com/sali.ttl#"; } }
        public static string SALModel { get { return @"https://ontology.hviidnet.com/salmodel.ttl#"; } }
        public static string SchemaOrgUri { get { return @"http://schema.org/version/latest/schema.ttl#"; } }
        public static string BrickUri { get { return @"http://brickschema.org/ttl/Brick.ttl#"; } }
        public static string OWLUri { get { return @"http://www.w3.org/2002/07/owl#"; } }
        public static string RDFUri { get { return @"http://www.w3.org/1999/02/22-rdf-syntax-ns#"; } }
        public static string XMLUri { get { return @"http://www.w3.org/XML/1998/namespace"; } }
        public static string XSDUri { get { return @"http://www.w3.org/2001/XMLSchema#"; } }
        public static string RDFSUri { get { return @"http://www.w3.org/2000/01/rdf-schema#"; } }

        public static Graph AddStandardSALNamespaces(this Graph graph) {
            graph.BaseUri = new Uri("https://ontology.hviidnet.com/sal.ttl");
            graph.NamespaceMap.AddNamespace("sal", new Uri(RdfNS.SALUri));
            graph.NamespaceMap.AddNamespace("sali", new Uri(RdfNS.SALIUri));
            graph.NamespaceMap.AddNamespace("owl", new Uri(RdfNS.OWLUri));
            graph.NamespaceMap.AddNamespace("rdf", new Uri(RdfNS.RDFUri));
            graph.NamespaceMap.AddNamespace("xml", new Uri(RdfNS.XMLUri));
            graph.NamespaceMap.AddNamespace("xsd", new Uri(RdfNS.XSDUri));
            graph.NamespaceMap.AddNamespace("rdfs", new Uri(RdfNS.RDFSUri));
            graph.NamespaceMap.AddNamespace("brick", new Uri(RdfNS.BrickUri));
            graph.NamespaceMap.AddNamespace("schema", new Uri(RdfNS.SchemaOrgUri));
            graph.NamespaceMap.AddNamespace("model", new Uri(RdfNS.SALModel));
            return graph;
        }
    }
}