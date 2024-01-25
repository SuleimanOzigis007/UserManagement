using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserMangement.Data;
using UserMangement.Models;

namespace UserMangement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public SessionController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("get-all-session")]

        public ActionResult<IEnumerable<Session>> Get()
        {
            return Ok(_dataContext.Sessions.Include(x => x.sessionClasses).ToList());
        }

        [HttpPost]
        [Route("add-session-department")]
        public IActionResult NewSessioAndDepartments([FromBody] SessionRequest model)
        {
            try
            {
                var c = new Session
                {
                    Name = model.Name,
                    CreatedAt = DateTime.Now,
                    CreatedBy="USER"
                };
                c.sessionClasses = new List<SessionClass>() { };

                model.sessionClass.ForEach(x =>
                {
                    var sc = new SessionClass
                    {
                        SessionId = c.Id,
                        Name = x.Name

                    };

                    c.sessionClasses.Add(sc);
                });  

                _dataContext.Sessions.Add(c);
                _dataContext.SaveChanges();

                return Ok("Session Add Succesfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost]
        [Route("add-departmnet")]
        public IActionResult NewDepartment([FromBody] DeptRequest model)
        {
            try
            {
                var sessionExist = _dataContext.Sessions.Find(model.SessionId);
                if (sessionExist != null)
                {
                    var c = new SessionClass
                    {
                        SessionId = sessionExist.Id,
                        Name = model.Name

                    };

                    _dataContext.SessionClasses.Add(c);
                    _dataContext.SaveChanges();

                    return Ok("Session Add Succesfully");

                }
                else
                {
                    return Ok("no session could found with the request session id");

                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpPost]
        [Route("new-session")]
        public IActionResult NewSession([FromBody] SessionReq model)
        {
            try
            {
                var c = new Session
                {
                    Name = model.Name,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "USER"
                };

                _dataContext.Sessions.Add(c);
                _dataContext.SaveChanges();

                return Ok("Session Add Succesfully");


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
