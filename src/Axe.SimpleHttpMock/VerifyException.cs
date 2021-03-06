﻿using System;

#if !NET_CORE
using System.Runtime.Serialization;
#endif

namespace Axe.SimpleHttpMock
{
    /// <summary>
    /// Representing an verification error.
    /// </summary>
#if !NET_CORE
    [Serializable]
#endif
    public class VerifyException : Exception
    {
        /// <summary>
        /// Create a <see cref="VerifyException"/>.
        /// </summary>
        public VerifyException() { }

        /// <summary>
        /// Create a <see cref="VerifyException"/> with custom message.
        /// </summary>
        /// <param name="message">The message describing the exception.</param>
        public VerifyException(string message) : base(message) { }

        /// <summary>
        /// Create a <see cref="VerifyException"/> with custom message and an original exception.
        /// </summary>
        /// <param name="message">The message describing the exception.</param>
        /// <param name="inner">The original exception that caused current exception.</param>
        public VerifyException(string message, Exception inner) : base(message, inner) { }

#if !NET_CORE
        /// <summary>
        /// Create a <see cref="VerifyException"/> from serializing context. 
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Serializing context.</param>
        protected VerifyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
#endif
    }
}