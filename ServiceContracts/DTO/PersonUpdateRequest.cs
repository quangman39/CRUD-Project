using Enities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Reprensent a dto class that contains the person object details to update 
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required]
        public Guid PersonId { get; set; }
        [Required]
        public string? PersonName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool? ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Convert the current object of PersonUpdateRequest into a new object of person class
        /// </summary>
        /// <returns>Person obj</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonID = PersonId,
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

