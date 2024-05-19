using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Businesses.API.IntegrationTests.API.MockModels;

public static class JwtTokenProvider
{
    public static string Issuer { get; } = "https://localhost:7019";
    public static string Audience { get; } = "https://localhost:3000";

    public static SecurityKey SecurityKey { get; } =
        new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("82d6a294c62a497eb9646191a4fe0450")
        );
    public static SigningCredentials SigningCredentials { get; } =
        new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    internal static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
}
