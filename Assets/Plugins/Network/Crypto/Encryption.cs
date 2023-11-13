using System;
using System.IO;
using System.Security.Cryptography;
namespace Winking.Crypto
{
    /// <summary>
    /// ���ܽ��ܵ�ʵ�ð�����
    /// </summary>
    public sealed class Encryption 
    {
        private static byte[] encryptstring = new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
							   0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76};
        /// <summary>
        /// ���ܶ��������飬���ؼ��ܺ�Ķ���������
        /// </summary>
        /// <param name="clearData"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] clearData, byte[] key, byte[] iv)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = key;
            alg.IV = iv;
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateEncryptor(), CryptoStreamMode.Write);
            // Write the data and make it do the encryption 
            cs.Write(clearData, 0, clearData.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }
        /// <summary>
        /// ʹ��һ����������ַ��������ؼ��ܺ���ַ���
        /// </summary>
        public static string Encrypt(string clearText, string password)
        {
            // First we need to turn the input string into a byte array. 
            byte[] clearBytes =
                System.Text.Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, encryptstring);
            byte[] encryptedData = Encrypt(clearBytes,
                pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }
        /// <summary>
        /// ʹ��һ��������ܶ���������
        /// </summary>
        public static byte[] Encrypt(byte[] clearData, string password)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,encryptstring);
            return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
        }
        /// <summary>
        /// ʹ��һ����������ļ�
        /// </summary>
        public static void Encrypt(string fileIn, string fileOut, string password)
        {
            //Dont overwrite existing?
            Encrypt(File.OpenRead(fileIn), File.Open(fileOut, FileMode.OpenOrCreate, FileAccess.Write), password, true);
        }
        /// <summary>
        /// ʹ��һ����������ļ�
        /// </summary>
        public static void Encrypt(Stream fsIn, Stream fsOut, string password, bool closeStreams)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,encryptstring);
            Rijndael alg = Rijndael.Create();
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);
            CryptoStream cs = new CryptoStream(fsOut,
                alg.CreateEncryptor(), CryptoStreamMode.Write);
            int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int bytesRead;
            do
            {
                // read a chunk of data from the input file 
                bytesRead = fsIn.Read(buffer, 0, bufferLen);
                // encrypt it 
                cs.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);
            // close everything 
            // this will also close the unrelying fsOut stream
            if (closeStreams)
            {
                cs.Close();
                fsIn.Close();
            }
            else
                cs.FlushFinalBlock();
        }
        /// <summary>
        /// ���ܶ���������
        /// </summary>
        public static byte[] Decrypt(byte[] cipherData, byte[] key, byte[] iv)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = key;
            alg.IV = iv;
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);
            // Write the data and make it do the decryption 
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }
        /// <summary>
        /// ʹ����������ַ���
        /// </summary>
        public static string Decrypt(string cipherText, string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,encryptstring);
            byte[] decryptedData = Decrypt(cipherBytes,
                pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }
        /// <summary>
        /// ʹ��������ܶ���������
        /// </summary>
        public static byte[] Decrypt(byte[] cipherData, string password)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,encryptstring);
            return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
        }
        /// <summary>
        /// ʹ����������ļ�
        /// </summary>
        public static void Decrypt(string fileIn, string fileOut, string password)
        {
            Decrypt(File.OpenRead(fileIn), File.Open(fileOut, FileMode.OpenOrCreate, FileAccess.Write), password, true);
        }
        /// <summary>
        /// ʹ����������ļ�
        /// </summary>
        public static void Decrypt(Stream fsIn, Stream fsOut, string password, bool closeStreams)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,encryptstring);
            Rijndael alg = Rijndael.Create();
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);
            CryptoStream cs = new CryptoStream(fsOut,
                alg.CreateDecryptor(), CryptoStreamMode.Write);
            int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int bytesRead;
            do
            {
                bytesRead = fsIn.Read(buffer, 0, bufferLen);
                cs.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);
            if (closeStreams)
            {
                cs.Close(); 
                fsIn.Close();
            }
            else
            {
                cs.FlushFinalBlock();
            }
        }
    }
}
