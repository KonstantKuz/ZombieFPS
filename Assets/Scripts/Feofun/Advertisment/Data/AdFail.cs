namespace Feofun.Advertisment.Data
{
    public class AdFail
    {
        public string Message { get; }
        
        public AdFailStatus Status { get; }
        
        public AdFail(string message, AdFailStatus status)
        {
            Message = message;
            Status = status;
        }

        public override string ToString()
        {
            return Message + $". Status:= {Status}";
        }
    }
}