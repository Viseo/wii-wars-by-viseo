using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;

namespace Viseo.WiiWars.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class SaberScoreController : Controller
    {
        IDictionary<int, Models.Saber> AllSabers = new Dictionary<int, Models.Saber>();
        const int SABER_ONE = 1;

        // GET: api/SaberStatus. For example : http://localhost:15707/api/SaberScore/
        [HttpGet]
        public IEnumerable<string> GetSaberStatus()
        {
            Models.Saber _mySaber1 = new Models.Saber(SABER_ONE, Models.Saber.enumSaberColor.Blue);
            AllSabers.Add(SABER_ONE, _mySaber1);
            return new string[] {
                "Expected ID=" + SABER_ONE.ToString(),
                "Actual ID=" + _mySaber1.id.ToString(),
                "Is Saber On? =" + _mySaber1.isSaberOn.ToString(),
                "Saber Color=" + Models.Saber.enumSaberColor.Blue.ToString(),
                "Saber Score=" + AllSabers[_mySaber1.id].score
            };
        }

        // POST api/SaberScore
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/SaberScore/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            Models.Saber _mySaber1 = new Models.Saber(id, Models.Saber.enumSaberColor.Blue);
            AllSabers.Add(id, _mySaber1);

            string test = "Is it On ? " + AllSabers[id].isSaberOn.ToString();
        }

        // DELETE api/SaberScore/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
