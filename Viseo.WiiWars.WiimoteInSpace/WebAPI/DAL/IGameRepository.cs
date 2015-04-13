using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viseo.WiiWars.WiimoteInSpace.Models;

namespace Viseo.WiiWars.WiimoteInSpace.WebApi.Dal
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAll();
        Game Get(int id);
        Game Add(Game item);
        void Remove(int id);
        bool Update(Game item);
    }
}
