using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viseo.WiiWars.WiimoteInSpace.Helper;
using Viseo.WiiWars.WiimoteInSpace.Models;

namespace Viseo.WiiWars.WiimoteInSpace.WebApi.Dal
{
    public class GameRepository : IGameRepository
    {
        private Dictionary<int, Game> _db = new Dictionary<int, Game>();

        private static GameRepository _instance;

        private GameRepository() { }

        public static GameRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameRepository();
                }
                return _instance;
            }
        }

        public Game Add(Game item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            int nextId = (_db.Count == 0) ? 1 : _db.Keys.Max() + 1;
            item.Id = nextId;
            _db.Add(nextId, item);

            return item;
        }

        public Game Get(int id)
        {
            Game game;
            if (_db.TryGetValue(id, out game))
                return game;
            return null;
        }

        public IEnumerable<Game> GetAll()
        {
            return _db.Values;
        }

        public void Remove(int id)
        {
            _db.Remove(id);
        }

        public bool Update(Game item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            var game = Get(item.Id);
            if (game == null)
                return false;

            item.CopyProperties(game);
            return true;
        }
    }
}
