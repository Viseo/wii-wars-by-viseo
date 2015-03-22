using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;

namespace Viseo.WiiWars.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        IDictionary<int, Models.Saber> AllSabers = new Dictionary<int, Models.Saber>();
        const int SABER_ONE = 1;

        // GET: api/SaberStatus. For example : http://localhost:15707/api/Values/
        [HttpGet]
        public IEnumerable<string> GetSaberStatus()
        {
            Models.Saber _mySaber1 = new Models.Saber(SABER_ONE, Models.Saber.enumSaberColor.Blue);
            AllSabers.Add(SABER_ONE, _mySaber1);
            return new string[] {
                SABER_ONE.ToString(),
                _mySaber1.id.ToString(),
                _mySaber1.isSaberOn.ToString(),
                Models.Saber.enumSaberColor.Blue.ToString() };
        }

        // GET api/values/5. For instance : http://localhost:15707/api/Values/saber/1
        [HttpGet("saber/{saberId}")]
        public string Get(int saberId)
        {
            Models.Saber _mySaber1 = new Models.Saber(SABER_ONE, Models.Saber.enumSaberColor.Blue);
            AllSabers.Add(SABER_ONE, _mySaber1);

            string test = "Is it On ? " + AllSabers[saberId].isSaberOn.ToString();

            return test;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            Models.Saber _mySaber1 = new Models.Saber(id, Models.Saber.enumSaberColor.Blue);
            AllSabers.Add(id, _mySaber1);

            string test = "Is it On ? " + AllSabers[id].isSaberOn.ToString();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
