using asafaweb.console.Enums;


namespace asafaweb.console.Logic
{
    public class StatusLogic
    {
        public static AsafaResult GetStatus(string status)
        {
            switch (status)
            {
                case "Pass":
                    return AsafaResult.Pass;
                case "Fail":
                    return AsafaResult.Fail;
                case "Warning":
                    return AsafaResult.Warning;
                default:
                    return AsafaResult.NotTested;
            }
        }
    }
}
