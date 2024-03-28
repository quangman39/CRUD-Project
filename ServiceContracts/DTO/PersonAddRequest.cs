using Enities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    ///     DTO class for adding new Person 
    /// </summary>
    public class PersonAddRequest
    {
        [Required]
        public string? PersonName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public GenderOptions? Gender { get; set; }
        [Required]
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool? ReceiveNewsLetters { get; set; }

        /// <summary>
        /// convert person from PersonAddRequest to Person
        /// </summary>
        /// <returns>Person obj</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryId = CountryId,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
