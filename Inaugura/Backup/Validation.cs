using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Inaugura
{
	static public class Validation
    {
        #region Variables
        static private Regex mRegexEmail = new Regex((@"(?<User>^[a-zA-Z][\w\.-]*[a-zA-Z0-9])@(?<Domain>[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$)"));
        /// <summary>
        /// A regex for validating 10 (or 11) digit phone numbers
        /// </summary>
        private static Regex mRegPhone10Digit = new Regex(@"^(?<Code>\d{1})?(?:-|\s?)\(?(?<NPA>\d{3})\)?(?:-|\s?)(?<NXX>\d{3})(?:-|\s?)(?<Exchange>\d{4})$");
        static private Regex mRegexPhone7Digit = new Regex(@"(?<NXX>^\d{3})(?:-|\s?)(?<Exchange>\d{4}$)");
        static private Regex mRegexPostalCode = new Regex(@"(?:[a-zA-Z]\d[a-zA-Z](?:-|\s?)\d[a-zA-Z]\d)");
        static private Regex mRegexZipCode = new Regex(@"(?:\d{5}(?:-\d{4})?)");
        #endregion

        #region Properties
        /// <summary>
        /// A regex for email
        /// </summary>
        static public Regex RegexEmail
        {
            get
            {
                return Validation.mRegexEmail;
            }
        }

        /// <summary>
        /// The regex for a 10 digit phone number
        /// </summary>
        static public Regex RegexPhone10Digit
        {
            get
            {
                return Validation.mRegPhone10Digit;
            }
        }

        /// <summary>
        /// The regex for a 7 digit phone number
        /// </summary>
        static public Regex RegexPhone7Digit
        {
            get
            {
                return Validation.mRegexPhone7Digit;
            }
        }

        /// <summary>
        /// The regex for a postal code
        /// </summary>
        static public Regex RegexPostalCode
        {
            get
            {
                return Validation.mRegexPostalCode;
            }
        }

        /// <summary>
        /// The regex for a zip code
        /// </summary>
        static public Regex RegexZipCode
        {
            get
            {
                return Validation.mRegexZipCode;
            }
        }
        #endregion

        /// <summary>
		/// Validates the format of an Email
		/// </summary>
		/// <param name="email">The email address</param>
		/// <returns>True if the email is valid, false otherwise</returns>
		public static bool ValidateEmail(string email)
		{
            return Validation.RegexEmail.IsMatch(email);
		}

		/// <summary>
		/// Validates a password
		/// </summary>
		/// <param name="password">The password</param>
		/// <param name="minLength">The minimum allowable length of the password</param>
		/// <param name="maxLength">The maximum allowable length of the password</param>
		/// <param name="mustIncludeUpper">True if the password must contain at least one upper case letter, false otherwise</param>
		/// <param name="mustIncludeLower">True if the password must contain at least one lower case letter, false otherwise</param>
		/// <param name="mustIncludeDigit">True if the password must contain at least one digit, false otherwise</param>
		/// <returns>True if the password is valid, false otherwise</returns>
		public static bool ValidatePassword(string password, int minLength, int maxLength, bool mustIncludeUpper, bool mustIncludeLower, bool mustIncludeDigit)
		{
			if(password == null)
				throw new ArgumentNullException("password");

			if(password.Length < minLength)
				return false;
			if(password.Length > maxLength)
				return false;


			string regexString = string.Format("^{2}{3}{4}(?!.*\\s).{{{0},{1}}}$",minLength,maxLength, mustIncludeDigit ? @"(?=.*\d)" : string.Empty, mustIncludeUpper ? @"(?=.*[A-Z])" : string.Empty, mustIncludeLower ? @"(?=.*[a-z])" : string.Empty);

			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(regexString);
			return regex.IsMatch(password);			
		}      

        /// <summary>
        /// Validates a numeric string
        /// </summary>
        /// <param name="numeric">The numeric string</param>
        /// <returns>True if the string is numeric, false otherwise</returns>
        public static bool ValidateNumeric(string numeric)
        {
            foreach (char c in numeric)
            {
                if (!char.IsNumber(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Validates a numeric string
        /// </summary>
        /// <param name="numeric">The numeric string</param>
        /// <param name="minLength">The minimum length</param>
        /// <param name="maxLength">The maximum length</param>
        /// <returns>True if the string is numeric and within the size constraints, otherwise false</returns>
        public static bool ValidateNumeric(string numeric, int minLength, int maxLength)
        {
            if (numeric == null)
                throw new ArgumentNullException("numeric");

            if (numeric.Length < minLength || numeric.Length > maxLength)
                return false;
            return ValidateNumeric(numeric);
        }

        /// <summary>
        /// Validates a numeric string of a specific length
        /// </summary>
        /// <param name="numeric">The numeric string</param>
        /// <param name="length">The length of the string</param>
        /// <returns>True if the string is numeric and the specified length, otherwise false</returns>
        public static bool ValidateNumeric(string numeric, int length)
        {
            return ValidateNumeric(numeric, length, length);
        }

		/// <summary>
		/// Validates a numeric pincode
		/// </summary>
		/// <param name="pinCode">The pincode</param>
		/// <param name="length">The length of the pincode</param>
		/// <returns>True if the pincode is valid, false otherwise</returns>
		public static bool ValidatePinCode(string pinCode, int length)
		{
			return ValidateNumeric(pinCode, length, length);
		}
	}
}
