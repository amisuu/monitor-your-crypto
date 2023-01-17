namespace CryptoChecker.Domain.Utils
{
    public static class EnumUtils
    {
        /// <summary>
        /// Returns all possible values of the given enumerable
        /// </summary>
        public static List<TEnum> GetEnumList<TEnum>() where TEnum : Enum
            => ((TEnum[])Enum.GetValues(typeof(TEnum))).ToList();
    }
}
