using System;
using System.Collections;
using System.Collections.Generic;
using Toolchain;
using Toolchain.SAL;
using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Datasets;
using System.Reflection;
using System.IO;

namespace Toolchain.SAL {
    public class RDFStoreManager {
        private readonly Graph _graph;
        private readonly TripleStore _tripleStore;
        private readonly TurtleParser _turtleParser;
        private readonly CompressingTurtleWriter _turtleWriter;
        private readonly ISparqlQueryProcessor _processor;
        private readonly SparqlQueryParser _sqlQueryParser;

        public RDFStoreManager(RDFMode mode) {
            _graph = new Graph();
            _tripleStore = new TripleStore();
            _tripleStore.Add(_graph);
            _turtleParser = new TurtleParser();
            _processor = new LeviathanQueryProcessor(this._tripleStore);
            _sqlQueryParser = new SparqlQueryParser();
            _turtleWriter = new CompressingTurtleWriter { CompressionLevel = WriterCompressionLevel.High };

            switch (mode) {
                case RDFMode.SAL:

                    //doing weird shit to get the path of the library where the files are located
                    var pathToFile = (new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).GetLeftPart(UriPartial.Path);
                    var path = new Uri(pathToFile.Substring(0, pathToFile.LastIndexOf('/'))).AbsolutePath + "/Schemas/";

                    LoadTurtleFile(path + "Brick.ttl");
                    LoadTurtleFile(path + "schema.ttl");
                    LoadTurtleFile(path + "sal.ttl");
                    LoadTurtleFile(path + "sali.ttl");
                    break;
                case RDFMode.Generic:
                    break;
                default:
                    break;
            }
        }

        public void AddGraphFromString(string salDescription) {
            var reader = new StringReader(salDescription);
            _turtleParser.Load(_graph, reader);
            reader.Dispose();
        }

        public void LoadTurtleFile(string filePath) {
            _turtleParser.Load(_graph, filePath);
        }

        public void AddGraph(IGraph graph) {
            _graph.Assert(graph.Triples);
        }

        public SparqlResultSet QueryFromFile(string filePath) {
            SparqlQuery query = _sqlQueryParser.ParseFromFile(filePath);
            return (SparqlResultSet)_processor.ProcessQuery(query);
        }

        public SparqlResultSet QueryFromString(string query) {
            return (SparqlResultSet)_processor.ProcessQuery(_sqlQueryParser.ParseFromString(query));
        }

        public void WriteToTurtleFile(string filepath) {
            _turtleWriter.Save(_graph, filepath);
        }
    }

    public static class RDFStoreManagerExtensions {
        public static void SaveToTurtleFile(this IGraph graph, string filepath) {
            var writer = new CompressingTurtleWriter { CompressionLevel = WriterCompressionLevel.High };
            writer.Save(graph, filepath);
        }
    }

    public enum RDFMode {
        SAL, Generic
    }
}
