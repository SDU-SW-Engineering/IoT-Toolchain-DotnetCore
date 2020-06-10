using System;
using System.Collections;
using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;
using System.Collections.Generic;
using Newtonsoft.Json;
using Toolchain.SAL.RDFServServiceModels;

namespace Toolchain.SAL {
    public class Information {
        public string Location { get; set; }
        public Unit HasUnit { get; set; }
        public Modality HasModality { get; set; }
        public TemporalAspect HasTemporalAspect { get; set; }
        public Location HasLocation { get; set; }

        public void AddTriples(IGraph graph, INode parent, INode predicate) {
            if (graph == null) { throw new ArgumentNullException(nameof(graph)); }
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }

            string name = IdGenerator.NewId("Information");
            var thisNode = RdfHelpers.CreateEntity(RdfNS.SALModel, name);


            //Defining type of this object
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.RDFUri, "type",
                RdfNS.SALUri, "Information"
            ));
            //Define relationship to parent
            graph.Assert(RdfHelpers.CreateTriple(
                parent,
                predicate,
                thisNode
            ));
            //Set Data property location
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "location",
                this.Location
            ));
            //set object property Unit
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "hasUnit",
                RdfNS.SALIUri, Enum.GetName(typeof(Unit), this.HasUnit)
            ));
            //set object property Modality
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "hasModality",
                RdfNS.SALIUri, Enum.GetName(typeof(Modality), this.HasModality)
            ));
            //set object property TemporalAspect
            graph.Assert(RdfHelpers.CreateTriple(
                thisNode,
                RdfNS.SALUri, "hasTemporalAspect",
                RdfNS.SALIUri, Enum.GetName(typeof(TemporalAspect), this.HasTemporalAspect)
            ));

            this.HasLocation?.AddTriples(graph, thisNode, RdfHelpers.CreateEntity(RdfNS.SALUri, "hasLocation"));
        }

        internal RDFServServiceModels.Information GenerateRDFServObject() {
            return new RDFServServiceModels.Information {
                hasModality = Enum.GetName(typeof(Modality), HasModality),
                hasUnit = Enum.GetName(typeof(Unit), HasUnit),
                hasTemporalAspect = Enum.GetName(typeof(TemporalAspect), HasTemporalAspect),
                hasLocation = HasLocation.Name,
                location = Location
            };
        }
    }
}