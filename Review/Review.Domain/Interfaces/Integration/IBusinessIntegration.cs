namespace Review.Domain.Interfaces.Integration;

public interface IBusinessIntegration
{
    Task<bool> ExistsById(int businessId, string accessToken);
}
