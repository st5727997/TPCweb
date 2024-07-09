namespace mvc.Models
{
    public class SQLInsertModel
    {
        public string CumulatedSolarRadiation { get; set; } //Wh/m2

        public string TransformationEfficiency { get; set; } //%

        public string PR { get; set; } //%

        public string Daily { get; set; }
    }
}
