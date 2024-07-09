namespace mvc.Models
{
    public class AttensionNeededModel
    {



        public DateTime EventTime { get; set; }

        public List<string?>? AlarmName { get; set; }

        public List<string?>? AlarmMessage { get; set; }

        public List<double?>? AlarmValue { get; set; }





    }
}
