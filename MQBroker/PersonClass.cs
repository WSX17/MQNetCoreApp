using System;
using System.Linq;

namespace MQBroker
{
    public class Person
    {
        static Random random = new Random();

        public string Surname { get; set; }
        public string Name;
        public string Patronymic;
        public DateTime Birthdate;
        public int Type;
        public bool Sex;

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public Person()
        {
            Surname = RandomString(random.Next(1, 25));
            Name = RandomString(random.Next(1, 20));
            Patronymic = RandomString(random.Next(1, 25));
            Birthdate = new DateTime(random.Next(1, DateTime.Today.Year + 1), random.Next(1, 13), random.Next(1, 28)).AddDays(random.Next(28));
            Type = random.Next(1, 501);
            Sex = (random.Next(0, 2) == 1);
        }
    }
}
