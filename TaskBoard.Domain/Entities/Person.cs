using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBoard.Domain.Entities
{
    internal class Person
    {
        private Guid _id;
        private string _firstName;
        private string _lastName;

        public Guid Id { get => _id; set => _id = value; }
        public string FirstName { get => _firstName; 
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("First name error, is null or whitespace");
                    //Trzeba zrobic custom ArgumentException
                } else
                {
                    _firstName = value;
                }
            }
        }
        public string LastName { get => _lastName; 
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Last name error, is null or whitespace");
                    //Trzeba zrobic custom ArgumentException
                }
                else
                {
                    _lastName = value;
                }
            }
        }

        public Person()
        {
            _id = Guid.NewGuid();
        }

        public Person(string firstName, string lastName)
        {
            if(string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Please fill out both First and Last names");
            }
            FirstName = firstName;
            LastName = lastName;
        }

        public string GetDisplayName()
        {
            return $"{FirstName} {LastName}";
        }

        //public Person GetPersonById(Guid id)
        //{
        //    tbd
        //}
    }   
}
