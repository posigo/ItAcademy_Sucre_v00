namespace Sucre_Utility
{
    public static class WM
    {
        /// <summary>
        /// Hash code generator using MD5 algorithm
        /// </summary>
        /// <param name="input">Input string for which to generate hash code</param>
        /// <param name="salt">Salt, to complicate the hash code, empty by default</param>
        /// <param name="low">Convert alphabetic characters to lower case, default is false</param>
        /// <returns>Hash code obtained using the MD algorithm</returns>
        public static string GenerateMD5Hash(string input, string salt="", bool low=false)
        {            
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputstr = $"{input}{salt??""}";
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes($"{input}{salt??""}");
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                var result = Convert.ToHexString(hashBytes);
                if (low)
                    result = result.ToLower();

                return result;
            }

            //return ""; 
        }

        public static string GetStringName(params string[] pm)
        {
            List<string> listText = new List<string>();
            foreach (string p in pm)
            {
                if (p != null && p.Trim() != "")
                    listText.Add(p);
            }
            return String.Join(" ", listText.ToArray());
            
        }
    }
}
