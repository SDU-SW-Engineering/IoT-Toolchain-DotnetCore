using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;
using System;
using Newtonsoft.Json;


namespace IoTToolchain.SAL {
    public enum Modality {
        Angle, CO2, Presence, Flow, Illuminance, Power, Pressure, Rain, Humidity, Temperature, Wind,
        AbsoluteTime, RelativeTime, PowerFlexibility, Performance, Energy, Certainty,
        ErroneousOccupancyCount, OccupancyCount,
        Count, //OCCURE SPECIFIC?!
        CameraCount, //OCCURE SPECIFIC?!
        Location, //OCCURE SPECIFIC?!
        SpatialGranularityModality, //OCCURE SPECIFIC?!
        TemporalAspectModality, //OCCURE SPECIFIC?!
        UUID //OCCURE SPECIFIC?!
    }

    public enum Unit {
        Boolean, Count, CubicMeters, CubicMetersPerHour, DegreeCelsius, DegreeFahrenheit, Degrees, Epoch,
        GigaByte, KiloByte, Hours, Hertz, Joules, JoulesPerCubicMeter, Kelvin, KiloJoulesPerSquareMeter,
        KiloMeters, KiloWatts, KiloWattHours, Lux, CubicMetersPerSecond, MilliAmperes, Minutes,
        MilliMeters, MilliSeconds, MetersPerSecond, MilliVolts, MilliWatts, MilliWattHours, Pascal,
        Percent, PartsPerMillion, RotationsPerMinute, Volts, Watts, Unitless, Time, DateTime, Date
    }

    public enum TemporalAspect {
        Prediction, RealTime, Archival, NotApplicable
    }

    public enum HTTPVerb {
        Get, Post, Update, Delete
    }
}