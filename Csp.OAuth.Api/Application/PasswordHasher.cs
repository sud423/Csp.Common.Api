using System;
using System.Security.Cryptography;

namespace Csp.OAuth.Api.Application
{
    public class PasswordHasher
    {
        /// <summary>
        /// 盐的大小
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// 哈希大小
        /// </summary>
        private const int HashSize = 20;

        private const string Key = "$CSPHASH$V1$";


        /// <summary>
        /// 密码加密为哈希
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="iterations">迭代次数</param>
        /// <returns>the hash</returns>
        public static string Hash(string password, int iterations)
        {
            //创建 salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
            //创建 hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);
            //合并 salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
            //转换为 base64
            var base64Hash = Convert.ToBase64String(hashBytes);
            //格式化带有额外信息的哈希
            return $"{Key}{iterations}${base64Hash}";
        }
        /// <summary>
        /// 使用10000次迭代创建哈希密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>哈希</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }
        /// <summary>
        /// 检查是否支持哈希
        /// </summary>
        /// <param name="hashString">哈希</param>
        /// <returns>支持吗？</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains(Key);
        }
        /// <summary>
        /// 针对哈希验证密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="hashedPassword">哈希</param>
        /// <returns>could be verified?</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            //check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("待检验密码不是哈希类型");
            }
            //提取Base64字符串
            var splittedHashString = hashedPassword.Replace(Key, "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];
            //获取哈希字节
            var hashBytes = Convert.FromBase64String(base64Hash);
            //盐
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            //用给定的盐创建哈希
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            //得到结果
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
