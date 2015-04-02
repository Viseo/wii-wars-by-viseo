using System.Collections.Generic;
using System.Web.Http;
using Viseo.WiiWars.WiimoteInSpace.Models;
using Viseo.WiiWars.WiimoteInSpace.WebAPI.DAL;

namespace Viseo.WiiWars.WiimoteInSpace.WebAPI.Controllers
{
    //[Route("api/[controller]")]
    public class SaberController : ApiController
    {
        private ISaberRepository _saberRepository;

        public SaberController()
        {
            _saberRepository = SaberRepository.Instance;
        }


        [HttpGet]
        // GET api/saber
        public IEnumerable<Saber> Get()
        {
            return _saberRepository.GetAll();
        }

        [HttpGet]
        // GET api/saber/5 
        public Saber Get(int id)
        {
            return _saberRepository.Get(id);
        }

        [ActionName("TurnOn")]
        [HttpGet]
        public bool TurnOn(int id)
        {
            var saber = _saberRepository.Get(id);
            if (saber != null && saber.IsOn == false)
            {
                saber.IsOn = true;
                return true;
            }
            return false;
        }

        [ActionName("TurnOff")]
        [HttpGet]
        public bool TurnOff(int id)
        {
            var saber = _saberRepository.Get(id);
            if (saber != null && saber.IsOn == true)
            {
                saber.IsOn = false;
                return true;
            }
            return false;
        }
    }
}
