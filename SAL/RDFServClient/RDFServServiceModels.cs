using System.Collections.Generic;

namespace IoTToolchain.SAL.RDFServServiceModels {
    public class OwnedBy {
        public string legalName { get; set; }
    }

    public class HasServiceEndpoint {
        public OwnedBy ownedBy { get; set; }
        public string priority { get; set; }
    }

    public class HasKafkaRServiceEndpoint : HasServiceEndpoint {
        public List<Information> providesInformation { get; set; }
        public string read_topic { get; set; }
        public string write_topic { get; set; }
    }

    public class HasKafkaRWServiceEndpoint : HasServiceEndpoint {
        public List<Information> consumesInformation { get; set; }
        public List<Information> providesInformation { get; set; }
        public string read_topic { get; set; }
        public string write_topic { get; set; }
    }

    public class HasKafkaWServiceEndpoint : HasServiceEndpoint {
        public List<Information> consumesInformation { get; set; }
        public string read_topic { get; set; }
        public string write_topic { get; set; }
    }

    public class HasRestServiceEndpoint : HasServiceEndpoint {
        public List<Information> consumesInformation { get; set; }
        public List<Information> providesInformation { get; set; }
        public string url { get; set; }
        public string verb { get; set; }
    }

    public class Service {
        public List<HasKafkaRServiceEndpoint> hasKafkaRServiceEndpoint { get; set; }
        public List<HasKafkaRWServiceEndpoint> hasKafkaRWServiceEndpoint { get; set; }
        public List<HasKafkaWServiceEndpoint> hasKafkaWServiceEndpoint { get; set; }
        public List<HasRestServiceEndpoint> hasRestServiceEndpoint { get; set; }
        public string name { get; set; }
    }

    public class Information {
        public string hasLocation { get; set; }
        public string hasModality { get; set; }
        public string hasTemporalAspect { get; set; }
        public string hasUnit { get; set; }
        public string location { get; set; }
    }
}