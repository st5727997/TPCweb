namespace mvc.Models
{
    public class WeatherTempModel
    {
        public Dictionary<string, double?> WeatherTempKeyValues { get; set; }
        public List<List<string?>> WeatherDataKeys { get; set; }
        public List<List<string?>> LPDataKeys { get; set; }
        public List<List<string?>> PVBoxDataKeys { get; set; }

        public int PVBoxUnitCount { get; set; }
        public int PVBoxCountPerUnit { get; set; }
        public int WeatherUnitCount { get; set; }
    }
}
