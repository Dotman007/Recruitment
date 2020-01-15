using ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.Models
{
    public class PasswordGeneratorSettings:IPasswordSettings
    {
        const string LowerCaseCharacters = "abcdefghijklmnopqrstuvwxyz";
        const string UpperCaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string NumericCharacters = "1234567890";
        const string SpecialCharacters = @"!#$%&*@\";
        const int MininumPasswordLength = 8;
        const int MaximumPasswordLength = 128;





        public bool IncludeUpperCase { get; set; }
        public bool IncludeLowerCase { get; set; }
        public bool IncludeNumbers { get; set; }
        public bool IncludeSpecial { get; set; }
        public int IncludeLength { get; set; }
        public int PasswordLength { get; set; }
        public string CharacterSet { get; set; }
        public int MaximumAttempts { get; set; }

        public PasswordGeneratorSettings(bool includeUpperCase, bool includeLowerCase, bool includeNumbers, bool includeSpecial, int includeLength)
        {
            IncludeUpperCase = includeUpperCase;
            IncludeLowerCase = includeLowerCase;
            IncludeNumbers = includeNumbers;
            IncludeSpecial = includeSpecial;
            IncludeLength = includeLength;
            StringBuilder _appendText = new StringBuilder();
            if (includeUpperCase)
            {
                _appendText.Append(UpperCaseCharacters);
            }
            if (includeLowerCase)
            {
                _appendText.Append(LowerCaseCharacters);
            }
            if (includeNumbers)
            {
                _appendText.Append(NumericCharacters);
            }
            if (includeSpecial)
            {
                _appendText.Append(SpecialCharacters);
            }
            CharacterSet = _appendText.ToString();
        }

        public bool isValidLength()
        {
            var checkPassword = PasswordLength >= MininumPasswordLength && PasswordLength <= MaximumPasswordLength;
            return checkPassword;
        }

        public int LengthErrorMessage()
        {
            var errorMessage =string.Format("Password length must be between {0} and {1} characters", MininumPasswordLength, MaximumPasswordLength);
            return Convert.ToInt32(errorMessage);
        }
    }
    public  class PasswordGenerator :IPasswordGenerator
    {
        /// <summary>
        /// Generate Password based of the PasswordGeneratorSettings Criteria.
        /// </summary>
        /// <param name="_passwordGeneratorSettings"></param>
        /// <returns></returns>
        public string GeneratePassword(PasswordGeneratorSettings _passwordGeneratorSettings)
        {
            const int MaximumIdenticalCharacters = 2;
            char[] password = new char[_passwordGeneratorSettings.PasswordLength];
            int characterSetLength = _passwordGeneratorSettings.CharacterSet.Length;
            System.Random random = new System.Random();
            for (int characterPosition = 0; characterPosition < _passwordGeneratorSettings.PasswordLength; characterPosition++)
            {
                password[characterPosition] = _passwordGeneratorSettings.CharacterSet[random.Next(characterSetLength - 1)];
                bool moreThanTwoIdenticalInARow = characterPosition > MaximumIdenticalCharacters && password[characterPosition] == password[characterPosition - 1] && password[characterPosition - 1] == password[characterPosition - 2];
                if (moreThanTwoIdenticalInARow)
                {
                    characterPosition--;
                }
            }
            return string.Join(null, password);
        }

        public bool PasswordIsValid(PasswordGeneratorSettings settings, string password)
        {
                    const string RegexLowerCase = @"[a-z]";
                    const string RegexUpperCase = @"[A-Z]";
                    const string RegexNumeric = @"[\d]";
                    const string RegexSpecial = @"([!#$%&*@\\])+";
                    bool lowerCaseIsValid = !settings.IncludeLowerCase || (settings.IncludeLowerCase && Regex.IsMatch(password, RegexLowerCase));
                    bool upperCaseIsValid = !settings.IncludeUpperCase || (settings.IncludeUpperCase && Regex.IsMatch(password, RegexUpperCase));
                    bool numericIsValid = !settings.IncludeNumbers || (settings.IncludeNumbers && Regex.IsMatch(password, RegexNumeric));
                    bool symbolsAreValid = !settings.IncludeSpecial || (settings.IncludeSpecial && Regex.IsMatch(password, RegexSpecial));
                    return lowerCaseIsValid && upperCaseIsValid && numericIsValid && symbolsAreValid;
        }
    }
}