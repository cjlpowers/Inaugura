using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Measurement
{
    /// <summary>
    /// A class which represents a unit of measure
    /// </summary>
    public class Unit
    {
        #region Internal Constructs
        /// <summary>
        /// Describes the system of measure which a unit is classified
        /// </summary>
        public enum UnitSystem
        {
            /// <summary>
            /// No Unit System
            /// </summary>
            None = 0,
            /// <summary>
            /// The metric system
            /// </summary>
            SI = 1,
            /// <summary>
            /// The imperial system
            /// </summary>
            English = 2
        }

        /// <summary>
        /// A class which represents the unit type
        /// </summary>
        public class UnitType
        {
            #region Types
            /// <summary>
            /// Length
            /// </summary>
            public static UnitType Length = new UnitType("Length", Unit.Meter);
            /// <summary>
            /// Area
            /// </summary>
            public static UnitType Area = new UnitType("Area", Unit.SquareMeter);
            /// <summary>
            /// Volume
            /// </summary>
            public static UnitType Volume = new UnitType("Volume", Unit.CubicMeter);
            /// <summary>
            /// Mass
            /// </summary>
            public static UnitType Mass = new UnitType("Mass", Unit.kg);
            /// <summary>
            /// Pressure
            /// </summary>
            public static UnitType Pressure = new UnitType("Pressure", Unit.Pa);
            /// <summary>
            /// Force
            /// </summary>
            public static UnitType Force = new UnitType("Force", Unit.N);
            /// <summary>
            /// Temperature
            /// </summary>
            public static UnitType Temperature = new UnitType("Temperature", Unit.Celsius);
            /// <summary>
            /// Energy
            /// </summary>
            public static UnitType Energy = new UnitType("Energy", Unit.J);
            /// <summary>
            /// Time
            /// </summary>
            public static UnitType Time = new UnitType("Time", Unit.Second);
            /// <summary>
            /// Power
            /// </summary>
            public static UnitType Power = new UnitType("Power", Unit.W);
            /// <summary>
            /// Power
            /// </summary>
            public static UnitType Illuminance = new UnitType("Illuminance", Unit.Lux);
            /// <summary>
            /// Secondary Floor Measurement Units
            /// </summary>
            public static UnitType SecondaryMeasurement = new UnitType("Secondary Measurement", null);
            #endregion

            #region Variables
            private string mName;
            private Unit mStdUnit;
            #endregion

            #region Properties
            /// <summary>
            /// The name
            /// </summary>
            public string Name
            {
                get
                {
                    return this.mName;
                }
            }

            /// <summary>
            /// The standard unit
            /// </summary>
            public Unit StandardUnit
            {
                get
                {
                    return this.mStdUnit;
                }
            }
            #endregion

            #region Methods
            private UnitType(string name, Unit standardUnit)
            {
                this.mName = name;
                this.mStdUnit = standardUnit;
            }
            #endregion
        }

        /// <summary>
        /// A class which defines a unit conversion
        /// </summary>
        public abstract class UnitConversion
        {
            #region Variables
            private Unit mUnit;
            #endregion

            #region Properties
            /// <summary>
            /// The unit resulting after conversion
            /// </summary>
            public Unit Unit
            {
                get
                {
                    return this.mUnit;
                }
            }
            #endregion

            #region Methods
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="unit">The unit resulting from the conversion</param>
            protected UnitConversion(Unit unit)
            {
                this.mUnit = unit;
            }

            /// <summary>
            /// Performs the unit conversion
            /// </summary>
            /// <param name="value">The value</param>
            /// <returns>The converted value</returns>
            public abstract double Convert(double value);

            /// <summary>
            /// Performs a unconversion
            /// </summary>
            /// <param name="value">The converted value</param>
            /// <returns>The  value</returns>
            public abstract double Unconvert(double value);
            #endregion
        }

        /// <summary>
        /// A class representing a linear unit conversion
        /// </summary>
        public class LinearUnitConversion : UnitConversion
        {
            #region Variables
            private double mFactor;
            private double mOffset;
            #endregion

            #region Properties
            /// <summary>
            /// The conversion factor
            /// </summary>
            public double Factor
            {
                get
                {
                    return this.mFactor;
                }
            }

            /// <summary>
            /// The offset
            /// </summary>
            public double Offset
            {
                get
                {
                    return this.mOffset;
                }
            }
            #endregion

            #region Methods
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="unit">The desired unit</param>
            /// <param name="factor">The conversion factor</param>            
            public LinearUnitConversion(Unit unit, double factor)
                : this(unit, factor, 0)
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="unit">The desired unit</param>
            /// <param name="factor">The conversion factor</param>            
            /// <param name="offset">The conversion offset</param>
            public LinearUnitConversion(Unit unit, double factor, double offset)
                : base(unit)
            {
                this.mFactor = factor;
                this.mOffset = offset;
            }
            #endregion

            /// <summary>
            /// Performs the unit conversion
            /// </summary>
            /// <param name="value">The value</param>
            /// <returns>The converted value</returns>
            public override double Convert(double value)
            {
                return value * this.Factor + this.Offset;
            }

            /// <summary>
            /// Performs a unconversion
            /// </summary>
            /// <param name="value">The converted value</param>
            /// <returns>The  value</returns>
            public override double Unconvert(double value)
            {
                return (value - this.Offset) / this.Factor;
            }
        }

        /// <summary>
        /// A class representing a collection of units
        /// </summary>
        public class UnitCollection : System.Collections.Generic.List<Unit>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public UnitCollection()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="list">The list of units</param>
            public UnitCollection(Unit[] list)
            {
                this.AddRange(list);
            }

            public UnitCollection(params System.Collections.IEnumerable[] args)
            {
                foreach (System.Collections.IEnumerable list in args)
                {
                    foreach (Unit unit in list)
                        this.Add(unit);
                }
            }

            #region Properties
            /// <summary>
            /// SI units
            /// </summary>
            public Unit[] SI
            {
                get
                {
                    List<Unit> list = new List<Unit>();
                    foreach (Unit unit in this)
                    {
                        if (unit.System == Unit.UnitSystem.SI)
                            list.Add(unit);
                    }
                    return list.ToArray();
                }
            }

            /// <summary>
            /// English units
            /// </summary>
            public Unit[] English
            {
                get
                {
                    List<Unit> list = new List<Unit>();
                    foreach (Unit unit in this)
                    {
                        if (unit.System == Unit.UnitSystem.English)
                            list.Add(unit);
                    }
                    return list.ToArray();
                }
            }
            #endregion
        }
        #endregion

        #region Variables
        private UnitType mType;
        private UnitConversion mStdUnitConversion;
        private UnitSystem mSystem;
        private string mName;
        private string mSymbol;
        #endregion

        #region Unit Defintions

        #region Length Units
        /// <summary>
        /// Meter
        /// </summary>
        public static Unit Meter = new Unit("Meter", "m", UnitType.Length, UnitSystem.SI, null);
        /// <summary>
        /// Kilometer
        /// </summary>
        public static Unit Kilometer = new Unit("Kilometer", "km", UnitType.Length, UnitSystem.SI, new LinearUnitConversion(Unit.Meter, 1000));
        /// <summary>
        /// Centimeter
        /// </summary>
        public static Unit Centimeter = new Unit("Centimeter", "cm", UnitType.Length, UnitSystem.SI, new LinearUnitConversion(Unit.Meter, 0.01));
        /// <summary>
        /// Millimeter
        /// </summary>
        public static Unit Millimeter = new Unit("Millimeter", "mm", UnitType.Length, UnitSystem.SI, new LinearUnitConversion(Unit.Meter, 0.001));
        /// <summary>
        /// Inch
        /// </summary>
        public static Unit Inch = new Unit("Inch", "in", UnitType.Length, UnitSystem.English, new LinearUnitConversion(Unit.Meter, 0.0254));
        /// <summary>
        /// Feet
        /// </summary>
        public static Unit Feet = new Unit("Feet", "ft", UnitType.Length, UnitSystem.English, new LinearUnitConversion(Unit.Meter, 0.3048));
        /// <summary>
        /// Yard
        /// </summary>
        public static Unit Yard = new Unit("Yard", "yd", UnitType.Length, UnitSystem.English, new LinearUnitConversion(Unit.Meter, 0.9144));
        /// <summary>
        /// All Length Units
        /// </summary>
        public static UnitCollection LengthUnits = new UnitCollection(new Unit[] { Kilometer, Meter, Centimeter, Millimeter, Feet, Yard });
        #endregion

        #region Area Units
        /// <summary>
        /// Square Meter
        /// </summary>
        public static Unit SquareMeter = new Unit("Square Meter", "m²", UnitType.Area, UnitSystem.SI, null);
        /// <summary>
        /// Square Feet
        /// </summary>
        public static Unit SquareFeet = new Unit("Square Feet", "ft²", UnitType.Area, UnitSystem.English, new LinearUnitConversion(Unit.SquareMeter, 0.09290304));
        /// <summary>
        /// Square Inch
        /// </summary>
        public static Unit SquareInch = new Unit("Square Inch", "in²", UnitType.Area, UnitSystem.English, new LinearUnitConversion(Unit.SquareMeter, 0.00064516));
        /// <summary>
        /// Acre
        /// </summary>
        public static Unit Acre = new Unit("Acre", "acre", UnitType.Area, UnitSystem.English, new LinearUnitConversion(Unit.SquareMeter, 4046.856));
        /// <summary>
        /// Hectare
        /// </summary>
        public static Unit Hectare = new Unit("Hectare", "hectare", UnitType.Area, UnitSystem.English, new LinearUnitConversion(Unit.SquareMeter, 10000));
        /// <summary>
        /// All Area Units
        /// </summary>
        public static UnitCollection AreaUnits = new UnitCollection(new Unit[] { SquareMeter, SquareFeet, SquareInch, Acre, Hectare });
        #endregion

        #region Volume Units
        /// <summary>
        /// Cubic Meter
        /// </summary>
        public static Unit CubicMeter = new Unit("Cubic Meter", "m³", UnitType.Volume, UnitSystem.SI, null);
        /// <summary>
        /// Cubic Meter
        /// </summary>
        public static Unit CubicCentimeter = new Unit("Cubic Centimeter", "cm³", UnitType.Volume, UnitSystem.SI, new LinearUnitConversion(CubicMeter, 0.000001));
        /// <summary>
        /// Cubic Meter
        /// </summary>
        public static Unit CubicInch = new Unit("Cubic Inch", "in³", UnitType.Volume, UnitSystem.SI, new LinearUnitConversion(CubicMeter, 0.000016387));
        /// <summary>
        /// Litre
        /// </summary>
        public static Unit Litre = new Unit("Litre", "L", UnitType.Volume, UnitSystem.SI, new LinearUnitConversion(CubicMeter, 0.001));
        /// <summary>
        /// Cubic Feet
        /// </summary>
        public static Unit CubicFeet = new Unit("Cubic Feet", "ft³", UnitType.Volume, UnitSystem.English, new LinearUnitConversion(CubicMeter, 0.028328));
        /// <summary>
        /// 100 cubic feet
        /// </summary>
        public static Unit Ccf = new Unit("Ccf ", "Ccf", UnitType.Volume, UnitSystem.English, new LinearUnitConversion(CubicMeter, 2.8328));
        /// <summary>
        /// 1,000 cubic feet
        /// </summary>
        public static Unit Mcf = new Unit("Mcf", "Mcf", UnitType.Volume, UnitSystem.English, new LinearUnitConversion(CubicMeter, 28.328));
        /// <summary>
        /// Gallons (U.S.)
        /// </summary>        
        public static Unit usg = new Unit("Gallons (U.S.)", "usg", UnitType.Volume, UnitSystem.English, new LinearUnitConversion(CubicMeter, 0.003785413));
        /// <summary>
        /// Imperial Gallon
        /// </summary>        
        public static Unit ImperialGallon = new Unit("Gallons (UK)", "gal", UnitType.Volume, UnitSystem.English, new LinearUnitConversion(CubicMeter, 0.00454609188));
        /// <summary>
        /// All Volume Units
        /// </summary>
        public static UnitCollection VolumeUnits = new UnitCollection(new Unit[] { CubicMeter, CubicCentimeter, CubicInch, Litre, CubicFeet, usg, ImperialGallon });
        #endregion

        #region Mass Units
        /// <summary>
        /// Kilogram
        /// </summary>
        public static Unit kg = new Unit("Kilogram", "kg", UnitType.Mass, UnitSystem.SI, null);
        /// <summary>
        /// Pound
        /// </summary>
        public static Unit lb = new Unit("Pound", "lb", UnitType.Mass, UnitSystem.English, new LinearUnitConversion(Unit.kg, 0.453592370));
        /// <summary>
        /// All Area Units
        /// </summary>
        public static UnitCollection MassUnits = new UnitCollection(new Unit[] { kg, lb });
        #endregion

        #region Force Units
        /// <summary>
        /// Newton
        /// </summary>
        public static Unit N = new Unit("Newton", "N", UnitType.Force, UnitSystem.SI, null);
        /// <summary>
        /// Pound-Force
        /// </summary>
        public static Unit lbf = new Unit("Pound-Force", "lbf", UnitType.Force, UnitSystem.English, new LinearUnitConversion(Unit.N, 4.448221615260));
        /// <summary>
        /// Pound-Force
        /// </summary>
        public static Unit kip = new Unit("Kip", "kip", UnitType.Force, UnitSystem.English, new LinearUnitConversion(Unit.N, 4448.221615260));
        /// <summary>
        /// Pound-Force
        /// </summary>
        public static Unit kgf = new Unit("Kilogram-Force", "kgf", UnitType.Force, UnitSystem.SI, new LinearUnitConversion(Unit.N, 9.80665));
        /// <summary>
        /// All Area Units
        /// </summary>
        public static UnitCollection ForceUnits = new UnitCollection(new Unit[] { N, lbf, kip, kgf });
        #endregion

        #region Temperature
        /// <summary>
        /// Celsius
        /// </summary>
        public static Unit Celsius = new Unit("Celsius", "°C", UnitType.Temperature, UnitSystem.SI, null);
        /// <summary>
        /// Kelvin
        /// </summary>
        public static Unit Kelvin = new Unit("Kelvin", "°K", UnitType.Temperature, UnitSystem.SI, new LinearUnitConversion(Unit.Celsius, 1, -273.15));
        /// <summary>
        /// Fahrenheit
        /// </summary>
        public static Unit Fahrenheit = new Unit("Fahrenheit", "°F", UnitType.Temperature, UnitSystem.English, new LinearUnitConversion(Unit.Celsius, 1.0 / 1.8, -32.0 / 1.8));
        /// <summary>
        /// All Temperature Units
        /// </summary>
        public static UnitCollection TemperatureUnits = new UnitCollection(new Unit[] { Celsius, Kelvin, Fahrenheit });
        #endregion

        #region Pressure Units
        /// <summary>
        /// Pascal
        /// </summary>
        public static Unit Pa = new Unit("Pascal", "Pa", UnitType.Pressure, UnitSystem.SI, null);
        /// <summary>
        /// Kilopascal
        /// </summary>
        public static Unit KPa = new Unit("Kilopascal", "KPa", UnitType.Pressure, UnitSystem.SI, new LinearUnitConversion(Unit.Pa, 1000));
        /// <summary>
        /// Megapascal
        /// </summary>
        public static Unit MPa = new Unit("Megapascal", "MPa", UnitType.Pressure, UnitSystem.SI, new LinearUnitConversion(Unit.Pa, 1000 * 1000));
        /// <summary>
        /// Gigapascal
        /// </summary>
        public static Unit GPa = new Unit("Gigapascal", "GPa", UnitType.Pressure, UnitSystem.SI, new LinearUnitConversion(Unit.Pa, 1000 * 1000 * 1000));
        /// <summary>
        /// Pounds Per Sq Inch
        /// </summary>
        public static Unit psi = new Unit("Pounds/Square Inch", "psi", UnitType.Pressure, UnitSystem.English, new LinearUnitConversion(Unit.Pa, 6894.757));
        /// <summary>
        /// Kilo Pounds Per Sq Inch
        /// </summary>
        public static Unit ksi = new Unit("Kilopounds/Square Inch", "ksi", UnitType.Pressure, UnitSystem.English, new LinearUnitConversion(Unit.Pa, 6894.757 * 1000));
        /// <summary>
        /// All units of pressure
        /// </summary>
        public static UnitCollection PressureUnits = new UnitCollection(new Unit[] { Pa, KPa, MPa, GPa, psi, ksi });
        #endregion

        #region Energy Units
        /// <summary>
        /// Joule
        /// </summary>
        public static Unit J = new Unit("Joule", "J", UnitType.Energy, UnitSystem.SI, null);
        /// <summary>
        /// Kilojoules
        /// </summary>
        public static Unit kJ = new Unit("Kilojoules ", "kJ", UnitType.Energy, UnitSystem.SI, new LinearUnitConversion(J, 1000));
        /// <summary>
        /// Megajoules
        /// </summary>
        public static Unit MJ = new Unit("Megajoules ", "MJ", UnitType.Energy, UnitSystem.SI, new LinearUnitConversion(J, 1000000));
        /// <summary>
        /// Gigajoules
        /// </summary>
        public static Unit GJ = new Unit("Gigajoules ", "GJ", UnitType.Energy, UnitSystem.SI, new LinearUnitConversion(J, 1000000000));
        /// <summary>
        /// Btu 
        /// </summary>
        public static Unit Btu = new Unit("Btu", "Btu", UnitType.Energy, UnitSystem.English, new LinearUnitConversion(J, 1055.05585262));
        /// <summary>
        /// MBtu 
        /// </summary>
        public static Unit MBtu = new Unit("MBtu", "MBtu", UnitType.Energy, UnitSystem.English, new LinearUnitConversion(J, 1055055.85262));
        /// <summary>
        /// Therm  (natural Gass)
        /// </summary>
        public static Unit Therm = new Unit("Therm", "therm", UnitType.Energy, UnitSystem.English, new LinearUnitConversion(J, 105500000));
        /// <summary>
        /// kWh 
        /// </summary>
        public static Unit kWh = new Unit("Kilowatt Hours", "kWh", UnitType.Energy, UnitSystem.SI, new LinearUnitConversion(J, 3600000));
        /// <summary>
        /// kWh 
        /// </summary>
        public static Unit ekWh = new Unit("Equivalent Kilowatt Hours ", "ekWh", UnitType.Energy, UnitSystem.SI, new LinearUnitConversion(J, 3600000));
        /// <summary>
        /// Cubic Meter Natural Gas 
        /// </summary>
        public static Unit CubicMeterNaturalGas = new Unit("Cubic Meter Natural Gas", "m³ (Natural Gas)", UnitType.Energy, UnitSystem.SI, new LinearUnitConversion(J, 37150000));
        /// <summary>
        /// Litre Light Fuel Oil 
        /// </summary>
        public static Unit LitreLightFuelOil = new Unit("Litre #2 Light Fuel Oil", "L (#2 Light Fuel Oil)", UnitType.Energy, UnitSystem.SI, new LinearUnitConversion(J, 38990000));
        /// <summary>
        /// Litre Bunker C Fuel Oil
        /// </summary>
        public static Unit LitreBunkerCFuelOil = new Unit("Litre #6 Bunker C Fuel Oil", "L (#6 Bunker C Fuel Oil)", UnitType.Energy, UnitSystem.SI, new LinearUnitConversion(J, 40490000));
        /// <summary>
        /// Litre Propane
        /// </summary>
        public static Unit LitrePropane = new Unit("Litre Propane", "L (Propane)", UnitType.Energy, UnitSystem.SI, new LinearUnitConversion(J, 26600000));
        /// <summary>
        /// Kilogram Coal
        /// </summary>
        public static Unit KilogramCoal = new Unit("Kilogram Coal", "Kg (Coal)", UnitType.Energy, UnitSystem.SI, new LinearUnitConversion(J, 34880000));
        /// <summary>
        /// All Energy Units
        /// </summary>
        public static UnitCollection EnergyUnits = new UnitCollection(new Unit[] { J, kJ, MJ, GJ, Btu, MBtu, Therm, kWh, ekWh, CubicMeterNaturalGas, LitreLightFuelOil, LitreBunkerCFuelOil, LitrePropane, KilogramCoal });
        #endregion

        #region Time Units
        /// <summary>
        /// Second
        /// </summary>
        public static Unit Second = new Unit("Second", "s", UnitType.Time, UnitSystem.SI, null);
        /// <summary>
        /// Minute
        /// </summary>
        public static Unit Minute = new Unit("Minute", "min", UnitType.Time, UnitSystem.SI, new LinearUnitConversion(Unit.Second, 60));
        /// <summary>
        /// Hour
        /// </summary>
        public static Unit Hour = new Unit("Hour", "h", UnitType.Time, UnitSystem.SI, new LinearUnitConversion(Unit.Second, 3600));
        /// <summary>
        /// Day
        /// </summary>
        public static Unit Day = new Unit("Day", "d", UnitType.Time, UnitSystem.SI, new LinearUnitConversion(Unit.Second, 86400));
        /// <summary>
        /// Week
        /// </summary>
        public static Unit Week = new Unit("Week", "wk", UnitType.Time, UnitSystem.SI, new LinearUnitConversion(Unit.Second, 604800));
        /// <summary>
        /// Year (Gregorian)
        /// </summary>
        public static Unit Year = new Unit("Year (Gregorian)", "y", UnitType.Time, UnitSystem.SI, new LinearUnitConversion(Unit.Second, 31556952));
        /// <summary>
        /// All Area Units
        /// </summary>
        public static UnitCollection TimeUnits = new UnitCollection(new Unit[] { Second, Minute, Hour, Day, Week, Year });
        #endregion

        #region Power Units
        /// <summary>
        /// Watt
        /// </summary>
        public static Unit W = new Unit("Watt", "W", UnitType.Power, UnitSystem.SI, null);
        /// <summary>
        /// Kilowatt
        /// </summary>
        public static Unit kW = new Unit("Kilowatt", "kW", UnitType.Power, UnitSystem.SI, new LinearUnitConversion(W, 1000));
        /// <summary>
        /// Mechanical Horsepower
        /// </summary>
        public static Unit hp = new Unit("Horsepower", "hp", UnitType.Power, UnitSystem.English, new LinearUnitConversion(W, 745.7));
        /// <summary>
        /// All Power Units
        /// </summary>
        public static UnitCollection PowerUnits = new UnitCollection(new Unit[] { W, kW, hp });
        #endregion

        #region Lighting Units
        /// <summary>
        /// The lux (symbol: lx) is the SI unit of illuminance. It is used in photometry as a measure of the intensity of light, with wavelengths weighted according to the luminosity function, a standardized model of human brightness perception. (1 lx = 1 lm/m2 = 1 cd·m2·m–4)
        /// </summary>
        public static Unit Lux = new Unit("Lux", "lx", UnitType.Illuminance, UnitSystem.SI, null);
        /// <summary>
        /// A foot-candle (sometimes designated footcandle; abbreviated fc, lm/ft², or sometimes ft-c) is a non-SI unit of illuminance or light intensity widely used in photography, film, television, and the lighting industry.
        /// </summary>
        public static Unit Footcandle = new Unit("Footcandle", "fc", UnitType.Illuminance, UnitSystem.English, new LinearUnitConversion(Lux, 10.76));
        /// <summary>
        /// All Illuminance Units
        /// </summary>
        public static UnitCollection IlluminanceUnits = new UnitCollection(new Unit[] { Lux, Footcandle });
        #endregion

        #region BOSS Specific Units
        /// <summary>
        /// BOSS Electrical Consumption Units
        /// </summary>
        public static UnitCollection ElectricalConsumptionUnits = new UnitCollection(new Unit[] { Unit.kWh });

        /// <summary>
        /// BOSS Electrical Demand Units
        /// </summary>
        public static UnitCollection ElectricalDemandUnits = new UnitCollection(new Unit[] { Unit.kW });

        /// <summary>
        /// BOSS Natrual Gas Consumption Units
        /// </summary>
        public static UnitCollection NGConsumptionUnits = new UnitCollection(new Unit[] { Unit.CubicMeter, Unit.Therm });

        /// <summary>
        /// BOSS Water Consumption Units
        /// </summary>
        public static UnitCollection WaterConsumptionUnits = new UnitCollection(new Unit[] { Unit.CubicMeter, Unit.usg });

        #region Secondary Measurement Units
        /// <summary>
        /// Suites
        /// </summary>
        public static Unit Suites = new Unit("Suites", "suites", UnitType.SecondaryMeasurement, UnitSystem.None, null);
        /// <summary>
        /// Ice Sheets
        /// </summary>
        public static Unit IceSheets = new Unit("Ice Sheets", "ice sheets", UnitType.SecondaryMeasurement, UnitSystem.None, null);
        /// <summary>
        /// Occupants
        /// </summary>
        public static Unit Occupants = new Unit("Occupants", "occupants", UnitType.SecondaryMeasurement, UnitSystem.None, null);
        /// <summary>
        /// Street Light Standards
        /// </summary>
        public static Unit StreetLightStandards = new Unit("Street Light Standards", "street light standards", UnitType.SecondaryMeasurement, UnitSystem.None, null);
        /// <summary>
        /// Students
        /// </summary>
        public static Unit Students = new Unit("Students", "students", UnitType.SecondaryMeasurement, UnitSystem.None, null);
        /// <summary>
        /// All Secondary Floor Area Units
        /// </summary>
        public static UnitCollection SecondaryMeasurementUnits = new UnitCollection(new Unit[] { Suites, IceSheets, Occupants, StreetLightStandards, Students });
        #endregion
        #endregion

        /// <summary>
        /// All units
        /// </summary>
        public static UnitCollection AllUnits = new UnitCollection(LengthUnits, AreaUnits, VolumeUnits, MassUnits, ForceUnits, TemperatureUnits, PressureUnits, EnergyUnits, TimeUnits, PowerUnits, IlluminanceUnits, SecondaryMeasurementUnits);

        #endregion

        #region Properties
        /// <summary>
        /// The unit name
        /// </summary>
        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        /// <summary>
        /// The unit symbol
        /// </summary>
        public string Symbol
        {
            get
            {
                return this.mSymbol;
            }
        }

        /// <summary>
        /// The system of measure
        /// </summary>
        public UnitSystem System
        {
            get
            {
                return this.mSystem;
            }
        }

        /// <summary>
        /// The conversion to a standard unit
        /// </summary>
        public UnitConversion StandardUnitConversion
        {
            get
            {
                return this.mStdUnitConversion;
            }

        }

        /// <summary>
        /// The unit type
        /// </summary>
        public UnitType Type
        {
            get
            {
                return this.mType;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>        
        /// <param name="name">The unit name</param>
        /// <param name="symbol">The unit symbol</param>
        /// <param name="type">The unit type</param>
        /// <param name="system">The unit system</param>
        /// <param name="stdUnitConversion">The conversion used to obtain standard units of the unit type</param>
        private Unit(string name, string symbol, UnitType type, UnitSystem system, UnitConversion stdUnitConversion)
        {
            this.mType = type;
            this.mName = name;
            this.mSymbol = symbol;
            this.mSystem = system;
            this.mStdUnitConversion = stdUnitConversion;
        }

        /// <summary>
        /// Return the name of the unit
        /// </summary>
        /// <returns>The name of the unit</returns>
        public override string ToString()
        {
            return this.mName;
        }

        /// <summary>
        /// Attempts to create a unit based on a name
        /// </summary>
        /// <param name="value">The name of the unit</param>
        /// <param name="unit">The parsed unit</param>
        /// <returns>True if the unit was parsed, false otherwise</returns>
        static public bool TryParse(string value, out Unit unit)
        {
            foreach (Unit u in Unit.AllUnits)
            {
                if (string.Compare(value, u.Name, true) == 0 || string.Compare(value, u.Symbol, true) == 0)
                {
                    unit = u;
                    return true;
                }
            }
            unit = null;
            return false;
        }

        /// <summary>
        /// Parses unit name
        /// </summary>
        /// <param name="value">The name of the unit</param>
        /// <returns>The unit specified by the name</returns>
        static public Unit Parse(string value)
        {
            Unit unit = null;
            if (!Unit.TryParse(value, out unit))
                throw new ArgumentException(string.Format("Could not parse the unit '{0}'", value));
            else
                return unit;
        }
        #endregion
    }

}
