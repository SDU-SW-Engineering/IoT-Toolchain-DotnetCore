using System;
using System.Text.RegularExpressions;
using VDS.RDF;

namespace IoTToolchain {
    public static class RdfHelpers {
        public static IGraph Graph { get; set; }
        public static INode CreateEntity(string nameSpace, string name) {
            return Graph.CreateUriNode(UriFactory.Create(nameSpace + name));
        }

        public static INode CreateProperty(string value) {
            return Graph.CreateLiteralNode(value);
        }

        public static Triple CreateTriple(string subjectNamespace, string subjectName, string predicateNamespace, string predicateName, string objectNamespace, string objectName) {
            var subject = CreateEntity(subjectNamespace, subjectName);
            var predicate = CreateEntity(predicateNamespace, predicateName);
            var obj = CreateEntity(objectNamespace, objectName);

            return new Triple(subject, predicate, obj);
        }

        public static Triple CreateTriple(INode subject, string predicateNamespace, string predicateName, string objectNamespace, string objectName) {
            var predicate = CreateEntity(predicateNamespace, predicateName);
            var obj = CreateEntity(objectNamespace, objectName);

            return new Triple(subject, predicate, obj);
        }

        public static Triple CreateTriple(string subjectNamespace, string subjectName, string predicateNamespace, string predicateName, string value) {
            var subject = CreateEntity(subjectNamespace, subjectName);
            var predicate = CreateEntity(predicateNamespace, predicateName);
            var obj = CreateProperty(value);

            return new Triple(subject, predicate, obj);
        }

        public static Triple CreateTriple(INode subject, string predicateNamespace, string predicateName, string value) {
            var predicate = CreateEntity(predicateNamespace, predicateName);
            var obj = CreateProperty(value);

            return new Triple(subject, predicate, obj);
        }

        public static Triple CreateTriple(INode subject, INode predicate, string value) {
            return new Triple(subject, predicate, CreateProperty(value));
        }

        public static Triple CreateTriple(INode subject, INode predicate, INode obj) {
            return new Triple(subject, predicate, obj);
        }

        public static Triple CreateTriple(INode subject, INode predicate, string objectNamespace, string objectName) {
            return new Triple(subject, predicate, CreateEntity(objectNamespace, objectName));
        }
    }
}