using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MQReceiver
{
    public class PersonExt : Person
    {
        public int Id { get; set; }
        public string Xmlstring { get; set; }
        public PersonExt()
        {
        
        }
        public PersonExt(Person person, string xmltext)
        {
            this.Surname = person.Surname;
            this.Name = person.Name;
            this.Patronymic = person.Patronymic;
            this.Birthdate = person.Birthdate;
            this.Type = person.Type;
            this.Sex = person.Sex;
            this.Xmlstring = xmltext;
        }
    }
    public class Person
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthdate { get; set; }
        public int Type { get; set; }
        public bool Sex { get; set; }
    }
}
