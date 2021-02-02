using System;
using System.Security.Cryptography;
using System.Text;


namespace ZemogaTechnicalTest.Tools
{
    public static class Encryptor
    {
        public static string Encrypt(string value, string key)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            byte[] keyArray;
            byte[] cryptoArray = UTF8Encoding.UTF8.GetBytes(value);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider()
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(cryptoArray, 0, cryptoArray.Length);
            tdes.Clear();

            return StringToHex(Convert.ToBase64String(resultArray, 0, resultArray.Length));
        }

        public static string Decrypt(string value, string key)
        {
            byte[] decryptArray;
            try { decryptArray = Convert.FromBase64String(HexToString(value)); }
            catch (Exception) { return string.Empty; }
            if (decryptArray.Length == 0) return string.Empty;

            byte[] keyArray;

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider()
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = tdes.CreateDecryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(decryptArray, 0, decryptArray.Length);
            tdes.Clear();

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        private static string StringToHex(string text)
        {
            char[] values = text.ToCharArray();
            string hexOutput = string.Empty;
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                hexOutput += String.Format("{0:X}", value) + "!";
            }
            return hexOutput.Substring(0, hexOutput.Length - 1);
        }

        private static string HexToString(string hexText)
        {
            if (string.IsNullOrWhiteSpace(hexText)) return string.Empty;
            string[] hexSplit = hexText.Split('!');
            string strinoutput = string.Empty;
            Int64 value = 0;
            foreach (String hex in hexSplit)
            {
                try { value = Convert.ToInt64(hex, 16); }
                catch (Exception) { return string.Empty; }
                char charValue = (char)value;
                strinoutput += charValue;
            }
            return strinoutput;
        }
    }
}
