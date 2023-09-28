using System;

namespace ValidationGenerator.Shared
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotNullAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the error message that will be showed to the user when validation fail.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}

