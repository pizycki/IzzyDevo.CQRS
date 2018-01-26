using System;

namespace IzzyDevo.CQRS.ExternalServices.Santa.Contracts
{
    public class Wish
    {
        private Wish(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static Wish Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("Cannot create empty wish.");

            return new Wish(name);
        }
    }
}