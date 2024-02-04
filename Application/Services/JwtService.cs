
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Domain.Options;
using Domain.Responses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public interface IJwtService
{
    JwtResponse CreateToken(User user);
    JwtSecurityToken ValidateToken(string jwtToken);
}

public class JwtService : IJwtService
{
    private readonly TokenOptions _tokenOptions;
    private readonly SigningCredentials _signingCredentials;

    public JwtService(IOptions<TokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Key!));
        _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
    }

    public JwtResponse CreateToken(User user)
    {
        var identity = CreateClaimsIdentity(user);

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = _tokenOptions.Issuer,
            Audience = _tokenOptions.Audience,
            IssuedAt = _tokenOptions.IssuedAt,
            NotBefore = _tokenOptions.NotBefore,
            Expires = _tokenOptions.AccessTokenExpiration,
            SigningCredentials = _signingCredentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new JwtResponse
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = token.ValidTo
        };
    }

    public JwtSecurityToken ValidateToken(string jwtToken)
    {
        try
        {

            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.FromMinutes(5),
                IssuerSigningKey = _signingCredentials.Key,
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateLifetime = false,
                ValidateAudience = true,
                ValidAudience = _tokenOptions.Audience,
                ValidateIssuer = true,
                ValidIssuer = _tokenOptions.Issuer
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var rawValidatedToken);
            return (JwtSecurityToken)rawValidatedToken;
        }
        catch (Exception)
        {
            throw new SecurityTokenValidationException("Invalid token");
        }
    }

    private ClaimsIdentity CreateClaimsIdentity(User user)
    {
        var userDataClaims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.Username!),
            new Claim(ClaimTypes.Role, user.Role!),
        };

        return new ClaimsIdentity(userDataClaims);

    }
}

