#region Usings.

// System.
using System;

#endregion

namespace vkapi.methods
{
    // Exception will be called when method error occured.
    public class MethodException : Exception
    {
        public MethodException(string message) : base(message) { }
    }
}