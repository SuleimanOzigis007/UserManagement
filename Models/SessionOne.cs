namespace UserMangement.Models
{
    public class SessionOne
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<SessionClassOne> SessionClasses { get; set; }
    }

    public class SessionClassOne
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public int SessionId { get; set; }
    }

    public class SessionOneReq
    {
        public string Name { get; set; }
    }

    public class SessionOneDept
    {
        public string Name { get; set; }
        public int SessionId { get; set; }
    }

    public class SessionOneRequest
    {
        public string Name { get; set; }
        public List<SessionOneClassRequest> sessionOneClassRequests { get;}

    }

    public class SessionOneClassRequest
    {
        public string Name { get; set; }
    }
}
         
