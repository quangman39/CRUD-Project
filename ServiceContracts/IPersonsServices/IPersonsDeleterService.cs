namespace ServiceContracts.IPersonsServices
{
    /// <summary>
    /// Reperensent buniness logic for manipulating Person entity
    /// </summary>
    public interface IPersonsDeleterService
    {

        /// <summary>
        /// Delete matching person is found by the given person Id 
        /// </summary>
        /// <param name="personId"> personId to find person object </param>
        /// <returns> true if successfully remove obj and fail if it fails </returns>
        Task<bool> DeletePerosn(Guid? personId);
    }
}
