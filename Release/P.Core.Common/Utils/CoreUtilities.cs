using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace P.Core.Common.Utils
{
    /// <summary>
    /// Static class containing Additional general utilities.
    /// </summary>
    public class CoreUtilities
    {       
        #region Pattern checking
        
        /// <summary>
        /// Test for Integers both Positive and Negative
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsInteger(string strNumber)
        {
            Regex NotIntPattern = new Regex("[^0-9-]");
            Regex IntPattern = new Regex("^-[0-9]+$|^[0-9]+$");
            return !NotIntPattern.IsMatch(strNumber) && IntPattern.IsMatch(strNumber);
        }
        
        /// <summary>
        /// Test whether the string is valid number or not 
        /// (including real and negative numbers)
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(string strNumber)
        {
            Regex NotNumberPattern = new Regex("[^0-9.-]");
            Regex TwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex TwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            string ValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            string ValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex NumberPattern = new Regex("(" + ValidRealPattern + ")|(" + ValidIntegerPattern + ")");
            return !NotNumberPattern.IsMatch(strNumber) &&
                   !TwoDotPattern.IsMatch(strNumber) &&
                   !TwoMinusPattern.IsMatch(strNumber) &&
                   NumberPattern.IsMatch(strNumber);
        }
        
        /// <summary>
        /// Test whether string is currency
        /// </summary>
        /// <param name="strToCheck"></param>
        /// <returns></returns>
        public static bool IsCurrency(string strCurrency)
        {
            var IsCurrency = false;
            double test;
            
            IsCurrency = double.TryParse(strCurrency, NumberStyles.Currency, CultureInfo.CurrentCulture, out test);
            return IsCurrency;
        }
        
        /// <summary>
        /// Checks a string for alpha characters only
        /// </summary>
        /// <param name="strToCheck"></param>
        /// <returns></returns>
        public static bool IsAlpha(string strToCheck)
        {
            Regex AlphaPattern = new Regex("[^a-zA-Z]");
            return !AlphaPattern.IsMatch(strToCheck);
        }
        
        /// <summary>
        /// Checks a string for alpha-numeric characters
        /// </summary>
        /// <param name="strToCheck"></param>
        /// <returns></returns>
        public static bool IsAlphaNumeric(string strToCheck)
        {
            Regex AlphaNumericPattern = new Regex("[^a-zA-Z0-9]");
            return !AlphaNumericPattern.IsMatch(strToCheck);
        }

        /// <summary>
        /// Checks a string for alpha-numeric characters plus the following special characters !#@\-_:;""'.,?&
        /// </summary>
        /// <param name="strToCheck"></param>
        /// <returns></returns>
        public static bool IsSpecialAlphaNumeric(string input)
        {
            Regex pattern = new Regex(@"[^a-zA-Z0-9!#@\-_:;""'.,?&]");
            return !pattern.IsMatch(input);
        }
        
        /// <summary>
        /// Checks a string for a valid email address format simple
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            bool IsValid = false;
            string regex = "@";

            IsValid = Regex.IsMatch(emailAddress, regex);
        
            return IsValid;
        }
        
        /// <summary>
        /// Checks a string for a valid email address format using complex regular expression
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static bool IsValidEmailAddressComplex(string emailAddress)
        {
            bool IsValid = false;
            string regex = @"^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*""\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*"")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$";

            IsValid = Regex.IsMatch(emailAddress, regex);
        
            return IsValid;
        }
        
        /// <summary>
        /// Returns only the numeric characters from a string of characters
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string ParseDigits(string Value)
        {
            int i = 0;
            string val = "";
            string ret = "";
            for (i = 0; i < Value.Length; i++)
            {
                val = Value.Substring(i, 1);
                
                if (char.IsDigit(Value, i))
                {
                    ret += val;
                }
            }
            return ret;
        }

        #endregion
        
        #region Object manipulation with reflection
       
        #endregion
        
        #region SSN   
        /// <summary>
        /// Format ssn string 000-00-0000
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public static string FormatSSN(string ssn)
        {
            if (ssn == null)
                ssn = string.Empty;
            
            Regex regStr;
            regStr = new Regex("(\\d{3})(\\d{2})(\\d{4})");
            if (regStr.IsMatch(ssn))
            {
                return regStr.Replace(ssn, "$1-$2-$3");
            }
            else
            {
                return ssn;
            }
        }
        
        /// <summary>
        /// Is the given SSN valid?
        /// </summary>
        /// <param name="ssn">value to be checked</param>
        /// <returns>true/false</returns>
        public static bool IsValidSSN(string ssn)
        {
            if (ssn == null)
            {
                return false;
            }
            string tempSSN = ssn.Replace("-", "").Trim(); //Remove spaces and dashes
            //Check for valid length
            if (tempSSN.Length != 9)
            {
                return false;
            }
            
            //Check for all numeric values
            try
            {
                Convert.ToInt32(tempSSN);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public static string FormatPhone(string phone)
        {
            if (phone == null)
                phone = string.Empty;
            
            Regex regStr = new Regex(@"(\d{3})(\d{3})(\d{4})");
            
            if (regStr.IsMatch(phone))
            {
                return regStr.Replace(phone, "($1) $2-$3");
            }
            else
            {
                return phone;
            }
        }
        
        #endregion
        
        #region String Manipulation     
        /// <summary>
        /// Repeats an expression for a specified occurance
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="occurance"></param>
        /// <returns></returns>
        public static string Replicate(string expression, int occurance)
        {
            //Create a stringBuilder
            StringBuilder sb = new StringBuilder();
            
            //Insert the expression into the StringBuilder
            sb.Insert(0, expression, occurance);
            
            //Convert it to a string and return it back
            return sb.ToString();
        }
        
        /// <summary>
        /// Adjust a string to a specific fixed width size..pads with spaces
        /// </summary>
        public static string AdjustStringSize(string toAdjust, int size)
        {
            return toAdjust.PadRight(size).Substring(0, size).Trim();
        }
        
        /// <summary>
        /// Formats text to HTML.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string FormatTextToHTML(string text)
        {
            try
            {
                string tempString = text.Replace(System.Convert.ToString(System.Convert.ToChar(10)), "<br />");
                return tempString;
            }
            catch (System.Exception)
            {
                return text;
            }
        }
        
        #endregion
        
        #region Max, Min, Between, InList      
        /// <summary>
        /// Returns the max value from a list
        /// </summary>
        /// <param name="compare"></param>
        /// <param name="items"></param>
        /// <remarks>
        /// Works with any type that implements the generic IComparable
        /// Null is treated as the minimum possible value
        /// </remarks>
        public static T Max<T>(T compare, params T[] items) where T : IComparable<T>
        {
            T result = compare;
            foreach (T item in items)
            {
                // Treat NULL as the minimum possible value
                if (item != null)
                {
                    if (result == null)
                    {
                        result = item;
                    }
                    else
                    {
                        result = (result.CompareTo(item) > 0) ? result : item;
                    }
                }
            }
            return result;
        }
        
        /// <summary>
        /// Returns the min value from a list
        /// </summary>
        /// <param name="compare"></param>
        /// <param name="items"></param>
        /// <remarks>
        /// Works with any type that implements the generic IComparable
        /// If any item is NULL, then NULL will be returned
        /// </remarks>
        public static T Min<T>(T compare, params T[] items) where T : IComparable<T>
        {
            T result = compare;
            
            if (result != null)
            {
                foreach (T item in items)
                {
                    // Treat NULL as the minimum possible value
                    if (item == null)
                        return item;
                
                    result = (result.CompareTo(item) < 0) ? result : item;
                }
            }
        
            return result;
        }
        
        /// <summary>
        /// Checks to see if a value is between (inclusive) of two other values
        /// </summary>
        /// <param name="toTest">value to test</param>
        /// <param name="minValue">minimum value to be test greater than or equal to</param>
        /// <param name="maxValue">maximum value to be test less than or equal to</param>
        /// <returns>true/false if value to test in between min and max</returns>
        /// <exception cref="System.ArgumentNullException" />
        /// <remarks>Works with any type that implements the generic IComparable</remarks>
        public static bool Between<T>(T toTest, T minValue, T maxValue) where T : IComparable<T>
        {
            if (minValue == null)
            {
                throw new ArgumentNullException("minValue", "Minimum value cannot be NULL");
            }
            
            if (maxValue == null)
            {
                throw new ArgumentNullException("maxValue", "Maximum value cannot be NULL");
            }
            
            // If our value is null, return false
            if (toTest == null)
            {
                return false;
            }
            else
            {
                return (toTest.CompareTo(minValue) >= 0) && (toTest.CompareTo(maxValue) <= 0);
            }
        }
        
        /// <summary>
        /// Looks for an item in a list of items
        /// </summary>
        /// <param name="toFind">item to search for</param>
        /// <param name="toSearch">array of items to search</param>
        /// <returns>true/false if found</returns>
        /// <remarks>Works with any type that implements the generic IComparable</remarks>
        public static bool InList<T>(T toFind, params T[] toSearch) where T : IComparable<T>
        {
            // If the value we're searching for is null, we just want to
            // look for a null within the list
            if (toFind == null)
            {
                foreach (T item in toSearch)
                {
                    if (item == null)
                    {
                        return true;
                    }
                }
            }
            // Value is not null
            else
            {
                foreach (T item in toSearch)
                {
                    if ((item != null) && toFind.CompareTo(item) == 0)
                    {
                        return true;
                    }
                }
            }
            
            // No match was found
            return false;
        }
        
        #endregion
        
        #region Password Generation        
        /// <summary>
        /// Generates a random password 8 characters long using lower alpha, upper alpha and numbers.
        /// </summary>
        /// <remarks>
        /// Same as <c>Utils.GeneratePassword(8, true, true, true, false)</c>
        /// </remarks>
        public static string GeneratePassword()
        {
            return GeneratePassword(8, true, true, true, false);
        }
        
        /// <summary>
        /// Generates a random password of specified length optionally using lower alpha, upper alpha, numbers and the following special characters .
        /// </summary>
        //(! " # $ % & ' ( ) * + , - . /)
        public static string GeneratePassword(short length, bool includeLowerCase, bool includeUpperCase, bool includeNumbers, bool includeOthers)
        {
            string pswd = "";
            int count = 0;
            Random random = new Random();
                
            if (includeLowerCase)
                count++;
            if (includeUpperCase)
                count++;
            if (includeNumbers)
                count++;
            if (includeOthers)
                count++;
            
            // make first character a letter
            pswd += (char)random.Next('a', 'z' + 1);
            
            for (int i = 0; i < length - 1; i++)
            {
                int which = random.Next(0, count + 1);
                
                switch (which)
                {
                    case 0:
                        pswd += (char)random.Next('a', 'z' + 1);
                        break;
                    case 1:
                        pswd += (char)random.Next('a', 'z' + 1);
                        break;
                    case 2:
                        pswd += (char)random.Next('A', 'Z' + 1);
                        break;
                    case 3:
                        pswd += (char)random.Next('0', '9' + 1);
                        break;
                    case 4:
                        pswd += (char)random.Next('!', '/' + 1);
                        break;
                    default:
                        pswd += (char)random.Next('0', '9' + 1);
                        break;
                }
            }
        
            return pswd;
        }
        
        #endregion
        
        #region Rounding       
        /// <summary>
        /// Returns a decimal rounded down
        /// </summary>
        /// <example>
        ///		Utils.RoundDown(10.245, 2) = 10.24
        ///		Utils.RoundDown(12345, -2) = 12300
        /// </example>
        /// <returns></returns>
        public static decimal RoundDown(decimal number, int position)
        {
            number = number * (decimal)Math.Pow(10, position);
            number = Convert.ToDecimal(Math.Floor(Convert.ToDouble(number)));
            number = number / (decimal)Math.Pow(10, position);
            return number;
        }
        
        /// <summary>
        /// Returns a decimal rounded up
        /// </summary>
        /// <example>
        ///		Utils.RoundDown(10.245, 2) = 10.25
        ///		Utils.RoundDown(12345, -2) = 12400
        /// </example>
        /// <returns></returns>
        public static decimal RoundUp(decimal number, int position)
        {
            number = number * (decimal)Math.Pow(10, position);
            number = Convert.ToDecimal(Math.Ceiling(Convert.ToDouble(number)));
            number = number / (decimal)Math.Pow(10, position);
            return number;
        }
        
        #endregion
        
        #region ConvertTo
        
        /// <summary>
        /// ConvertToDecimal
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Decimal ConvertToDecimal(string input)
        {
            decimal temp = 0;
            return (Decimal.TryParse(input, out temp)) ? temp : Decimal.Zero;
        }
        
        /// <summary>
        /// ConvertToNullableDecimal
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Decimal? ConvertToNullableDecimal(string input)
        {
            decimal temp = 0;
            return (Decimal.TryParse(input, out temp)) ? (Decimal?)temp : null;
        }
        
        /// <summary>
        /// ConvertToInt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int32 ConvertToInt(string input)
        {
            int temp = 0;
            return (Int32.TryParse(input, out temp)) ? temp : 0;
        }
        
        /// <summary>
        /// ConvertToNullableInt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int32? ConvertToNullableInt(string input)
        {
            int temp = 0;
            return (Int32.TryParse(input, out temp)) ? (Int32?)temp : null;
        }
        
        /// <summary>
        /// ConvertToShort
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int16 ConvertToShort(string input)
        {
            short temp = 0;
            return (Int16.TryParse(input, out temp)) ? temp : (short)0;
        }
        
        /// <summary>
        /// ConvertToNullableShort
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int16? ConvertToNullableShort(string input)
        {
            short temp = 0;
            return (Int16.TryParse(input, out temp)) ? (Int16?)temp : null;
        }
        
        /// <summary>
        /// ConvertToByte
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Byte ConvertToByte(string input)
        {
            byte temp;
            return (Byte.TryParse(input, out temp)) ? temp : Convert.ToByte("0");
        }
        
        #endregion
            
        #region String Compression
        
        #endregion
            
        #region RemovePunct Overload Methods
        
        //,-()+#.:;%'@&`*~${}/     -- XML does not like all these characters
        /// <summary>
        /// Remove punctuations 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string RemovePuncts(string value)
        {
            return RemovePuncts(value, "");
        }
        
        //,-()+#.:;%'@&`*~${}/     -- XML does not like all these characters
        /// <summary>
        /// Removes punctuation marks from string. This overload includes
        /// an omit list that will preserve any punctuation marks that 
        /// are included in the omitList
        /// </summary>
        public static string RemovePuncts(string strValue, string omitList)
        {
            strValue = strValue.Replace(",", omitList.IndexOf(",") > -1 ? "," : "");
            strValue = strValue.Replace("-", omitList.IndexOf("-") > -1 ? "-" : "");
            strValue = strValue.Replace("(", omitList.IndexOf("(") > -1 ? "(" : "");
            strValue = strValue.Replace(")", omitList.IndexOf(")") > -1 ? ")" : "");
            strValue = strValue.Replace("+", omitList.IndexOf("+") > -1 ? "+" : "");
            strValue = strValue.Replace("#", omitList.IndexOf("#") > -1 ? "#" : "");
            strValue = strValue.Replace(".", omitList.IndexOf(".") > -1 ? "." : "");
            strValue = strValue.Replace(":", omitList.IndexOf(":") > -1 ? ":" : "");
            strValue = strValue.Replace(";", omitList.IndexOf(";") > -1 ? ";" : "");
            strValue = strValue.Replace("%", omitList.IndexOf("%") > -1 ? "%" : "");
            strValue = strValue.Replace("'", omitList.IndexOf("'") > -1 ? "'" : "");
            strValue = strValue.Replace("@", omitList.IndexOf("@") > -1 ? "@" : "");
            strValue = strValue.Replace("&", omitList.IndexOf("&") > -1 ? "&" : "");
            strValue = strValue.Replace("`", omitList.IndexOf("`") > -1 ? "`" : "");
            strValue = strValue.Replace("*", omitList.IndexOf("*") > -1 ? "*" : "");
            strValue = strValue.Replace("~", omitList.IndexOf("~") > -1 ? "~" : "");
            strValue = strValue.Replace("$", omitList.IndexOf("$") > -1 ? "$" : "");
            strValue = strValue.Replace("}", omitList.IndexOf("}") > -1 ? "}" : "");
            strValue = strValue.Replace("{", omitList.IndexOf("{") > -1 ? "{" : "");
            strValue = strValue.Replace("/", omitList.IndexOf("/") > -1 ? "/" : "");
            strValue = strValue.Replace(@"\", omitList.IndexOf(@"\") > -1 ? @"\" : "");
            strValue = strValue.Replace("_", omitList.IndexOf("_") > -1 ? "_" : "");
            strValue = strValue.Replace("!", omitList.IndexOf("!") > -1 ? "!" : "");
            strValue = strValue.Replace("[", omitList.IndexOf("[") > -1 ? "[" : "");
            strValue = strValue.Replace("]", omitList.IndexOf("]") > -1 ? "]" : "");
            strValue = strValue.Replace("=", omitList.IndexOf("=") > -1 ? "=" : "");
            strValue = strValue.Replace("\"", omitList.IndexOf("\"") > -1 ? "\"" : "");
            strValue = strValue.Replace("“", omitList.IndexOf("“") > -1 ? "“" : "");
            strValue = strValue.Replace("”", omitList.IndexOf("”") > -1 ? "”" : "");

            return strValue;
        }
                
        #endregion
                
        #region Other stuff

        /// <summary>
        /// Validates an ABA Rounting Number
        /// </summary>
        /// <param name="Routing9Digits"></param>
        /// <returns></returns>
        public static bool ValidateABARoutingNumber(string Routing9Digits)
        {
            // This funtion has not been tested yet and is not currently in use
            // Run through each digit and calculate the total.
            if (Routing9Digits.Trim().Length != 9 ||
                Routing9Digits.IndexOf(' ') >= 0)
            {
                // If it is ot 9 digits or contains a space it is invalid
                return false;
            }
            
            // Apply the ABA algorithm to ensure that it is valid
            // Algorithm is ((d1 + d4 + d7) * 3) + 
            //             ((d2 + d5 + d8) * 7) + 
            //             d3 + d6 + checkid = multiple of 10
                
            int n = 0;
            for (int i = 0; i < Routing9Digits.Length; i += 3)
            {
                n += Convert.ToInt32(Routing9Digits.Substring(i, 1)) * 3 +
                     Convert.ToInt32(Routing9Digits.Substring(i + 1, 1)) * 7 +
                     Convert.ToInt32(Routing9Digits.Substring(i + 2, 1));
            }
            
            // If the resulting sum is an even multiple of ten (but not zero),
            // the aba routing number is good.
            
            if (n != 0 && n % 10 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
            
        public static string SQLStringEncode(string str)
        {
            if (str != null && str != string.Empty)
            {
                str = str.Replace("'", "''");
            }
            return str;
        }
        #endregion

        /// <summary>
        /// Removes server script brackets from a markup string (uses simple Replace to do the replacements)
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static string StripServerScriptBracket(string htmlString)
        {
            return htmlString.Replace("<%", "").Replace("%>", "");
        }
            
        public static Dictionary<string, string> ParseNameValueString(string input, char delim1 = ';', char delim2 = '=', bool keepItemsWithoutValues = false)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(input))
            {
                string[] items = input.Split(delim1);
                foreach (string item in items)
                {
                    string[] elements = item.Split(delim2);
                    
                    if (elements.Length == 2)
                    {
                        results.Add(elements[0], elements[1]);
                    }
                    else if (keepItemsWithoutValues && !string.IsNullOrWhiteSpace(elements[0]))
                    {
                        results.Add(elements[0], string.Empty);
                    }
                }
            }
            return results;
        }
        
        public static string BuildNameValueString(Dictionary<string, string> input)
        {
            return BuildNameValueString(input, ';', '=');
        }
                
        public static string BuildNameValueString(Dictionary<string, string> input, char delim1, char delim2)
        {
            StringBuilder result = new StringBuilder();
            
            foreach (string item in input.Keys)
            {
                result.AppendFormat("{0}{1}{2}{3}", item, delim2, input[item], delim1);
            }
        
            return result.ToString();
        }
    }
}