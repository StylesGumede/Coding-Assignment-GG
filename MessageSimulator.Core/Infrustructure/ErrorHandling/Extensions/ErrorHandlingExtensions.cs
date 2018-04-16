using System;

namespace MessageSimulator.Core.Infrustructure.ErrorHandling.Extensions
{
    public static class ErrorHandlingExtensions
    {
        public static void ThrowOnFalse<TExceptionType>(this bool value, string message)
            where TExceptionType: Exception
        {
            if(value)
                return;

            throw (TExceptionType) Activator.CreateInstance(typeof(TExceptionType), message);
        }

        public static void ThrowOnNull<TExceptionType, TNullableType, TThrowingType>(this TNullableType value, 
            string message = null)
            where TExceptionType : Exception
            where TNullableType : class
            where TThrowingType : class 
        {
            if (value != null)
                return;

            if(string.IsNullOrWhiteSpace(message))
                throw (TExceptionType) Activator.CreateInstance(typeof(TExceptionType), $"{typeof(TNullableType).Name} can not be null.\n" +
                                                                                        $"Thrown at '{typeof(TThrowingType).FullName}'");
            else
                throw (TExceptionType) Activator.CreateInstance(typeof(TExceptionType),message);
        }

        public static void ThrowOnNull<TExceptionType, TNullableType>(this TNullableType value, 
            Type throwingType,
            string message = null)
            where TExceptionType : Exception
            where TNullableType : class
        {
            if (value != null)
                return;

            if(string.IsNullOrWhiteSpace(message))
                throw (TExceptionType) Activator.CreateInstance(typeof(TExceptionType), $"{typeof(TNullableType).Name} can not be null.\n" +
                                                                                        $"Thrown at '{throwingType.FullName}'");
            else
                throw (TExceptionType) Activator.CreateInstance(typeof(TExceptionType),message);
        }

        public static void ThrowOnNullEmptyOrWhitespace<TExceptionType, TThrowingType>(this string value, 
            string parameterName)
            where TExceptionType : Exception
            where TThrowingType : class
        {
            if (string.IsNullOrWhiteSpace(value))
                throw (TExceptionType) Activator.CreateInstance(typeof(TExceptionType), $"{parameterName} can not be null, empty or contain whitespaces.\n" +
                                                                                        $"Thrown at '{typeof(TThrowingType).FullName}'");
        }

        public static void ThrowOnNullEmptyOrWhitespace<TExceptionType>(this string value,
            string parameterName, Type throwingType)
            where TExceptionType : Exception
        {
            if (string.IsNullOrWhiteSpace(value))
                throw (TExceptionType)Activator.CreateInstance(typeof(TExceptionType), $"{parameterName} can not be null, empty or contain whitespaces.\n" +
                                                                                        $"Thrown at '{throwingType.FullName}'");
        }

        public static void ThrowOnNullEmptyOrWhitespace<TExceptionType>(this string value,
            string parameterName, Type throwType, string message)
            where TExceptionType : Exception
        {
            if (string.IsNullOrWhiteSpace(value))
                throw (TExceptionType)Activator.CreateInstance(typeof(TExceptionType), $"{message}\n" +
                                                                                        $"Thrown at '{throwType.FullName}'");
        }

        public static void ThrowOnNullEmptyOrWhitespace<TExceptionType>(this string value,
            string parameterName, string message)
            where TExceptionType : Exception
        {
            if (string.IsNullOrWhiteSpace(value))
                throw (TExceptionType)Activator.CreateInstance(typeof(TExceptionType), $"{message}");
        }
    }
}
