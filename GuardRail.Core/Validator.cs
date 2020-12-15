﻿using System.Text.RegularExpressions;

namespace GuardRail.Core
{
    /// <summary>
    /// Validation extensions.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Validates that the string is formatted as a valid phone number.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>bool</returns>
        public static bool IsValidPhoneFormat(this string value) =>
            Regex.IsMatch(
                value,
                @"^(?<CountryCode>\+?\d{1,2}|1)?[-. (]*(?<AreaCode>[2-9](?!11)\d{2})[-. )]*(?<ExchangeCode>\d{3})[-. ]*(?<LineNumber>\d{4})(?: *x(\d+))?$");

        /// <summary>
        /// Validates that the string is formatted as a valid email.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>bool</returns>
        public static bool IsValidEmailFormat(this string value)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(value);
                return mailAddress.Address == value;
            }
            catch
            {
                return false;
            }
        }
    }
}