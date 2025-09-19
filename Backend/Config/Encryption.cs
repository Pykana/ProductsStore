using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace BACKEND_STORE.Shared
{
    public class Encryption
    {
        // Constants for Argon2 parameters
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int DegreeOfParallelism = 8; // Number of threads to use
        private const int Iterations = 4; // Number of iterations
        private const int MemorySize = 1024 * 1024; // 1 GB

        /// <summary>
        /// Hashes the specified password using a cryptographically secure algorithm and returns the result as a
        /// Base64-encoded string.
        /// </summary>
        /// <remarks>The returned hash includes both the salt and the hashed password, allowing for
        /// verification of the password later. This method uses a secure random salt and a cryptographic hash function
        /// to ensure password security.</remarks>
        /// <param name="password">The plaintext password to hash. Cannot be null or empty.</param>
        /// <returns>A Base64-encoded string containing the hashed password and the salt used during hashing.</returns>
        public string HashPassword(string password)
        {
            // Generate a random salt using a secure random number generator
            byte[] salt = new byte[SaltSize];
            // Use RandomNumberGenerator to fill the salt array with cryptographically secure random bytes
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            // Create hash
            byte[] hash = HashPassword(password, salt);
            // Combine salt and hash
            var combinedBytes = new byte[salt.Length + hash.Length];
            // Copy salt into the combined array
            Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
            // Copy the hash into the combined array after the salt
            Array.Copy(hash, 0, combinedBytes, salt.Length, hash.Length);
            // Convert to base64 for storage
            return Convert.ToBase64String(combinedBytes);
        }

        /// <summary>
        /// Computes a secure hash of the specified password using the Argon2id algorithm.
        /// </summary>
        /// <remarks>This method uses the Argon2id algorithm to generate a secure hash of the provided
        /// password.  The hashing process incorporates the specified salt, as well as preconfigured parameters  for
        /// degree of parallelism, iterations, memory size, and hash size. These parameters are  designed to balance
        /// security and performance.</remarks>
        /// <param name="password">The password to be hashed. Cannot be null or empty.</param>
        /// <param name="salt">The cryptographic salt to use for hashing. Must be a non-null byte array.</param>
        /// <returns>A byte array containing the computed hash of the password.</returns>
        private byte[] HashPassword(string password, byte[] salt)
        {
            // Validate input parameters
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemorySize
            };

            // Compute the hash
            return argon2.GetBytes(HashSize);
        }

        /// <summary>
        /// Verifies whether the provided password matches the specified hashed password.
        /// </summary>
        /// <remarks>This method uses a cryptographic comparison to ensure that the verification process
        /// is resistant to timing attacks. The hashed password must be in a specific format that includes the salt and
        /// hash concatenated together.</remarks>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="hashedPassword">The hashed password, encoded as a Base64 string, which includes both the hash and the salt.</param>
        /// <returns><see langword="true"/> if the provided password matches the hashed password; otherwise, <see
        /// langword="false"/>.</returns>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Decode the stored hash
            byte[] combinedBytes = Convert.FromBase64String(hashedPassword);

            // Extract salt and hash
            byte[] salt = new byte[SaltSize];
            byte[] hash = new byte[HashSize];
            Array.Copy(combinedBytes, 0, salt, 0, SaltSize);
            Array.Copy(combinedBytes, SaltSize, hash, 0, HashSize);

            // Compute hash for the input password
            byte[] newHash = HashPassword(password, salt);

            // Compare the hashes
            return CryptographicOperations.FixedTimeEquals(hash, newHash);
        }


    }
}
