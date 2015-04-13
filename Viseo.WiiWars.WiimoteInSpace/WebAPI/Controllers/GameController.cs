using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Viseo.WiiWars.WiimoteInSpace.WebApi.Dal;

namespace Viseo.WiiWars.WiimoteInSpace.WebApi.Controllers
{
    public class GameController : ApiController
    {
        //private IGameRepository _gameRepository;

        private List<SaberController> _saberControllers;

        public GameController(List<SaberController> controllers)
        {
            //_gameRepository = GameRepository.Instance;
            _saberControllers = controllers;
        }

        public List<SaberController> SaberControllers
        {
            get
            {
                return _saberControllers;
            }

            set
            {
                _saberControllers = value;
            }
        }

        [ActionName("LaunchGame")]
        [HttpGet]
        public bool launchGame(int id)
        {
            //var game = _gameRepository.Get(id);
            //if (game != null && game.IsGameStarted == false)
            //{
            //    game.IsGameStarted = true;
            //    return true;
            //}
            return false;
        }


    }
}
