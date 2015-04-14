using System.Collections.Generic;

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