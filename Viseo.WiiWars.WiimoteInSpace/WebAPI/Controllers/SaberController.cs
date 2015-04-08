using System;
using System.Collections.Generic;
using System.Web.Http;
using Viseo.WiiWars.WiimoteInSpace.Models;
using Viseo.WiiWars.WiimoteInSpace.WebApi.Dal;

namespace Viseo.WiiWars.WiimoteInSpace.WebApi.Controllers
{
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

        [ActionName("ChangeColorGreen")]
        [HttpGet]
        public bool ChangeColorGreen(int id)
        {
            return ChangeColor(id, Models.Saber.SaberColor.Green);
        }
        [ActionName("ChangeColorRed")]
        [HttpGet]
        public bool ChangeColorRed(int id)
        {
            return ChangeColor(id, Models.Saber.SaberColor.Red);
        }
        [ActionName("ChangeColorBlue")]
        [HttpGet]
        public bool ChangeColorBlue(int id)
        {
            return ChangeColor(id, Models.Saber.SaberColor.Blue);
        }

        private bool ChangeColor(int id, Saber.SaberColor color)
        {
            var saber = _saberRepository.Get(id);
            if (saber != null)
            {
                saber.Color = color;
                return true;
            }
            return false;
        }
    }
}
