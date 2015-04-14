using System;
using System.Collections.Generic;
using System.Linq;

namespace Viseo.WiiWars.Models.Dal
{
    public class SaberRepository : ISaberRepository
    {
        private Dictionary<int, Saber> _db = new Dictionary<int, Saber>();

        private static SaberRepository _instance;

        private SaberRepository() { }

        public static SaberRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SaberRepository();
                }
                return _instance;
            }
        }

        public IEnumerable<Saber> GetAll()
        {
            return _db.Values;
        }

        public Saber Get(int id)
        {
            Saber saber;
            if (_db.TryGetValue(id, out saber))
                return saber;
            return null;
        }

        public Saber Add(Saber item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            int nextId = (_db.Count == 0) ? 1 : _db.Keys.Max() + 1;
            item.Id = nextId;
            _db.Add(nextId, item);

            return item;
        }

        public void Remove(int id)
        {
            _db.Remove(id);
        }

        public bool Update(Saber item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            var saber = Get(item.Id);
            if (saber == null)
                return false;

            item.CopyProperties(saber);
            return true;
        }
    }
}
