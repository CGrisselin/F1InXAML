using System;

namespace F1InXAML
{
    public class Driver
    {
        public Driver(string id, string code, string url, string permanentNumber, string givenName, string familyName, string dateOfBirth, string nationality)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            Id = id;
            Code = code;
            Url = url;
            PermanentNumber = permanentNumber;
            GivenName = givenName;
            FamilyName = familyName;
            DateOfBirth = dateOfBirth;
            Nationality = nationality;
        }

        public string Id { get; }

        public string Code { get; }

        public string Url { get; }

        public string PermanentNumber { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string DateOfBirth { get; }

        public string Nationality { get; }       
    }
}