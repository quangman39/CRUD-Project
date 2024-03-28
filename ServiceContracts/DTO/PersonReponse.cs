using Enities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type of most method of PersonService 
    /// </summary>
    public class PersonReponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double? Age { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool? ReceiveNewsLetters { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is PersonReponse))
            {
                return false;
            }

            PersonReponse person_to_compare = (PersonReponse)obj;


            return person_to_compare.PersonID == this.PersonID &&
                   person_to_compare.PersonName == this.PersonName &&
                   person_to_compare.Gender == this.Gender &&
                   person_to_compare.Country == this.Country &&
                   person_to_compare.CountryId == this.CountryId &&
                   person_to_compare.Address == this.Address &&
                   person_to_compare.ReceiveNewsLetters == this.ReceiveNewsLetters &&
                   person_to_compare.DateOfBirth == this.DateOfBirth;


        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonId = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions?)Enum.Parse(typeof(GenderOptions), Gender, true),
                CountryId = CountryId,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }

    }

    public static class PersonExtensions
    {
        /// <summary>
        /// An extention method to covert an object of Person class into PersonPonse
        /// </summary>
        /// <param name="person">obj to covert</param>
        /// <returns>Return the converted PersonRespone </returns>
        public static PersonReponse ToPersonReoponse(this Person person)
        {
            return new PersonReponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Country = person.Country?.CountryName,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365) : null

            };
        }
    }


}
