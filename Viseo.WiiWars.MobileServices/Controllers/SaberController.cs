using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Viseo.WiiWars.MobileServices;
using Viseo.WiiWars.Models.Dal;

namespace Viseo.WiiWars.WiimoteInSpace.WebApi.Controllers
{
    public class SaberController : ApiController
    {
        
        private ISaberRepository _saberRepository;
        public ApiServices Services { get; set; }

        public SaberController()
        {
            _saberRepository = SaberRepository.Instance;
        }


        [HttpGet]
        // GET api/saber
        public IEnumerable<Saber> Get()
        {
            //IHubContext hubContext = Services.GetRealtime<SaberHub>();
            //hubContext.Clients.All.hello("Hello Chat Hub clients from custom controller!");

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
            IHubContext hubContext = Services.GetRealtime<SaberHub>();
            hubContext.Clients.All.TurnOn(id);

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
            IHubContext hubContext = Services.GetRealtime<SaberHub>();
            hubContext.Clients.All.TurnOff(id);

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
            IHubContext hubContext = Services.GetRealtime<SaberHub>();
            hubContext.Clients.All.ChangeColorGreen(id);

            return ChangeColor(id, Saber.SaberColor.Green);
        }
        [ActionName("ChangeColorRed")]
        [HttpGet]
        public bool ChangeColorRed(int id)
        {
            IHubContext hubContext = Services.GetRealtime<SaberHub>();
            hubContext.Clients.All.ChangeColorRed(id);

            return ChangeColor(id, Saber.SaberColor.Red);
        }
        [ActionName("ChangeColorBlue")]
        [HttpGet]
        public bool ChangeColorBlue(int id)
        {
            IHubContext hubContext = Services.GetRealtime<SaberHub>();
            hubContext.Clients.All.ChangeColorBlue(id);

            return ChangeColor(id, Saber.SaberColor.Blue);
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