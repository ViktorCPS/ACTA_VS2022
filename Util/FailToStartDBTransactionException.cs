using System;
using System.Runtime.Serialization;

namespace Util
{
    /// <summary>
    /// This exception is raised from DAO layer when DB connection BeginTransaction() method fires an exception for
    /// e.g. connection is closed reason.
    /// </summary>
    [Serializable]
    public class FailToStartDBTransactionException : ApplicationException
    {
        public FailToStartDBTransactionException()
            : base()
        {
        }

        public FailToStartDBTransactionException(string message)
            : base(message)
        {
        }

        public FailToStartDBTransactionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FailToStartDBTransactionException(SerializationInfo info, StreamingContext sx)
            : base(info, sx)
        {
        }
    }
}
