using System.Security.Cryptography.X509Certificates;

namespace UserMangement.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { set; get; }
        public DateTime CreatedAt { set; get; }
        public List<SessionClass> sessionClasses { get; set; }

    }

    public class SessionClass
    {
        public int Id { get; set; }
        public string Name {  set; get; }
        public int SessionId { get; set; }
        //public Session Session{ get; set; }
    }


    public class SessionReq
    {
        public string Name { get; set; }
    }
    public class DeptRequest
    {
        public string Name { get; set; }
        public int SessionId { set; get; }
    }
    public class SessionRequest
    {
        public string Name { get; set; }
        public List<SessionClassRequest> sessionClass { set; get;}
    }
    public class SessionClassRequest
    {
        public string Name { get; set; }
    }
}

