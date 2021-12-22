using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.Utilities.Exceptions
{
    public class VisingPackException : Exception
    {
        public VisingPackException()
        {

        }
        public VisingPackException(string message) : base(message)
        {

        }
        public VisingPackException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
