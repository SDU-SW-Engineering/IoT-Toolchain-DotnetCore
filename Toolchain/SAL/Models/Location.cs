using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;
using System;
using Newtonsoft.Json;

namespace Toolchain.SAL {
    public class Location {
        public string Name { get; set; }

        public virtual void AddTriples(IGraph graph, INode parent, INode predicate) {
            if (graph == null) { throw new ArgumentNullException(nameof(graph)); }
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            //Defining type of this object            
            graph.Assert(RdfHelpers.CreateTriple(
                RdfNS.SALModel, Name,
                RdfNS.RDFUri, "type",
                RdfNS.BrickUri, "Location"
            ));
            //Define relationship to parent
            graph.Assert(RdfHelpers.CreateTriple(
                parent,
                predicate,
                RdfNS.SALModel, Name
            ));
        }
    }

    public class Room : Location {

        public override void AddTriples(IGraph graph, INode parent, INode predicate) {
            if (graph == null) { throw new ArgumentNullException(nameof(graph)); }
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            graph.Assert(RdfHelpers.CreateTriple(
                RdfNS.SALModel, Name,
                RdfNS.RDFUri, "type",
                RdfNS.BrickUri, "Room"
            ));
            graph.Assert(RdfHelpers.CreateTriple(
                parent,
                predicate,
                RdfNS.SALModel, Name
            ));
        }
    }
}