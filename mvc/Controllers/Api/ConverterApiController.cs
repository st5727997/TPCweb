//using Microsoft.AspNetCore.Mvc;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using mvc.Data;
//using mvc.Models;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace mvc.Controllers.Api
//{
//    [Route("api/converter/[action]")]
//    [ApiController]
//    public class ConverterApiController : ControllerBase
//    {
//        private readonly ApplicationDbContext _db;
//        public ConverterApiController(ApplicationDbContext context)
//        {
//            _db = context;
//        }

//        public List<ConverterModel> GetConverters()
//        {
//        return _db.ConverterDatas

//        }

//        // GET: api/<ConverterApiController>
//        [HttpGet]

//        public ActionResult<List<ConverterModel>> GetConverterModel()
//        {

//        }


//        // GET api/<ConverterApiController>/5
//        [HttpGet("{id}")]
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST api/<ConverterApiController>
//        [HttpPost]
//        public void Post([FromBody] string value)
//        {
//        }

//        // PUT api/<ConverterApiController>/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }

//        // DELETE api/<ConverterApiController>/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}
