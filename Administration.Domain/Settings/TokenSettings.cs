using System;

namespace Administration.Domain.Settings
{
    /// <summary>
    /// Token settings
    /// </summary>
    public class TokenSettings
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public TokenSettings()
        {
            EncryptionKey = "fTjWnZr4u7x!A%D*G-JaNdRgUkXp2s5v";
            Expiration = DateTime.UtcNow.AddHours(6);
        }

        /// <summary>
        /// Gets the encryption key
        /// </summary>
        public string EncryptionKey { get; }

        /// <summary>
        /// Gets the date when the session will expire
        /// </summary>
        public DateTime Expiration { get; }
    }
}