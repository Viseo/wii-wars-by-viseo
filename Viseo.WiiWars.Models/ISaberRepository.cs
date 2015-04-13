using System.Collections.Generic;
// using Viseo.WiiWars.WiimoteInSpace.Models;

namespace Viseo.WiiWars.Models.Dal
{
    public interface ISaberRepository
    {
        IEnumerable<Saber> GetAll();
        Saber Get(int id);
        Saber Add(Saber item);
        void Remove(int id);
        bool Update(Saber item);
    }
}