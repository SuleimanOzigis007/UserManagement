using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using UserMangement.Data;
using UserMangement.Models;

namespace UserMangement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionOneController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public SessionOneController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("get-all-session")]
        public ActionResult<IEnumerable<Session>> GetSessions()
        {
            return Ok(_dataContext.sessionOnes.Include(x => x.SessionClasses).ToList());
        }

        [HttpPost]
        [Route("new-department-new-session")]
        public IActionResult NewSessionAndDepartment([FromBody] SessionOneRequest sessionOneClassRequest)
        {
           try
            {
                var sc = new SessionOne
                {
                    Name = sessionOneClassRequest.Name,
                    CreatedBy = "User",
                    CreatedOn = DateTime.Now,
                };

                sc.SessionClasses = new List<SessionClassOne>() { };
                sessionOneClassRequest.sessionOneClassRequests.ForEach(x =>
                {
                    var dd = new SessionClassOne
                    {
                        SessionId = sc.Id,
                        Name = x.Name,
                    };
                    sc.SessionClasses.Add(dd);
                });
                _dataContext.sessionOnes.Add(sc);
                _dataContext.SaveChanges();
                return Ok("SessionClass Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("new-deaprtment")]
        public IActionResult NewDepartment([FromBody] SessionOneDept sessionOneDept)
        {
            try
            {
                var SessionsExists = _dataContext.sessionClassesOne.Find(sessionOneDept.SessionId);
                if (SessionsExists != null)
                {
                    var cs = new SessionClassOne
                    {
                        SessionId = SessionsExists.Id,
                        Name = sessionOneDept.Name,
                    };
                    _dataContext.sessionClassesOne.Add(cs);
                    _dataContext.SaveChanges();

                }
                return Ok("SessionClass Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex); 
            }
        }



        [HttpPost]
        [Route("new-session")]
        public IActionResult NewSession([FromBody] SessionOneReq sessionOneReq)
        {
            try
            {
                var c = new SessionOne
                {
                    Name = sessionOneReq.Name,
                    CreatedBy = "User",
                    CreatedOn = DateTime.Now,
                };
                _dataContext.sessionOnes.Add(c);
                _dataContext.SaveChanges();
                return Ok("Session added Succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
