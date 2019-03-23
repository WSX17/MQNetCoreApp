using System;
using System.Xml.Linq;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MQReceiver
{
    public class Handler
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private static ConcurrentDictionary<int, byte> cacheDict = new ConcurrentDictionary<int, byte>();
        public static void ProceedRequestAsync(object message)
        {
            Person pers = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>((string)message);
            Task.Delay(new Random().Next(1000, 5001));
            if (pers.Type % 2 == 1)
            {
                string xmlstring = XmlString(pers);
                AddToDBAsync(pers, xmlstring);
                logger.Info("GUID {1}  {0} added to db with Type {2}", pers.Name, Guid.NewGuid(), pers.Type);
            }
            else
            {
                if (cacheDict.TryAdd(pers.Type,default(byte)))
                {
                    string xmlstring = XmlString(pers);
                    AddToDBAsync(pers, xmlstring);
                    logger.Info("GUID {2} {0} added to db and cache with Type {1}", pers.Name, pers.Type,Guid.NewGuid());
                }
                else logger.Info("GUID {1} Type {0} already cached", pers.Type, Guid.NewGuid());
            }
        }
        public static string XmlString(Person pers)
        {
            XDocument xml = new XDocument(new XElement("person", new XElement("surname", pers.Surname), new XElement("name", pers.Name), new XElement("patronymic", pers.Patronymic),
                    new XElement("birthdate", pers.Birthdate), new XElement("type", pers.Type), new XElement("sex", pers.Sex)));
            return xml.ToString();
        }
        public static async void AddToDBAsync(Person pe, string xml)
        {
            using (PersonContext db = new PersonContext())
            {
                db.Add(new PersonExt(pe,xml));
                await db.SaveChangesAsync();
            }
        }
        public static async void GetCache()
        {
            using (PersonContext db = new PersonContext())
            {
                var res = (await (from st in db.PersonsDb where (st.Type % 2) == 0 select st.Type).ToListAsync()).ToDictionary(x=>x,y=>default(byte));
                cacheDict = new ConcurrentDictionary<int, byte>(res);
                logger.Info("cache read from database");
            }
        }
    }
}
