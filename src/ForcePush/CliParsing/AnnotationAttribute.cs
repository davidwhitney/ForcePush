using System;

namespace ForcePush.CliParsing
{
    public class AnnotationAttribute : Attribute
    {
        public string Message { get; set; }

        public AnnotationAttribute(string message)
        {
            Message = message;
        }
    }
}