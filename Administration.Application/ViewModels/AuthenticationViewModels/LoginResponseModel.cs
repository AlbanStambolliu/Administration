using System;
using System.IdentityModel.Tokens.Jwt;

namespace Administration.Application.ViewModels.AuthenticationViewModels
{
    /// <summary>
    /// Login response model
    /// </summary>
    public class LoginResponseModel
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="token">Security token</param>
        /// <param name="expiration">The date the <see cref="Token"/> expires</param>
        public LoginResponseModel(JwtSecurityToken token = default, DateTime expiration = default)
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token);
            Expiration = expiration;
        }

        /// <summary>
        /// Gets the security token
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Gets the date the <see cref="Token"/> expires
        /// </summary>
        public DateTime Expiration { get; }
    }
}