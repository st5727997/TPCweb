namespace Models.IndexModels
{
    public class IndexModel
    {


        public double PESACTotalKW { get; set; }
        public double TotalGeneratedPowerRatio { get; set; }
        public double TotalPerformanceRatio { get; set; }
        public double TodaysKWH { get; set; }
        public double GeneratedHRS { get; set; }
        public double PESACTotalKWH { get; set; }
        public double SolarIntensity { get; set; }
        public double AirTemperature { get; set; }
        public double Shadetemperature { get; set; }
        public double ArrayTemperature { get; set; }
        public double WindSpeed { get; set; }
        public double CumulatedSolarRadiation { get; set; }
        


        public double SolarIntensity1 { get; set; }
        public double SolarIntensity2 { get; set; }
        public double TotalSystemEffciency { get; set; }
        public double ArrayTemperature1 { get; set; }
        public double ArrayTemperature2 { get; set; }
        public double ArrayTemperature3 { get; set; }
        public double ArrayTemperature4 { get; set; }
        public double ArrayTemperature5 { get; set; }
        public double ArrayTemperature6 { get; set; }
        public double ArrayTemperature7 { get; set; }
        public double CO2Reduction { get; internal set; }
        
    }
}
