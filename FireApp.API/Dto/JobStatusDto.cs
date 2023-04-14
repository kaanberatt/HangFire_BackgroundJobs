namespace FireApp.API.Dto
{
    public class JobStatusDto
    {
        public string JobId { get; set; }
        public string Status { get; set; }
        public string LastExecution { get; set; }
        public string NextExecution { get; set; }
    }
}
