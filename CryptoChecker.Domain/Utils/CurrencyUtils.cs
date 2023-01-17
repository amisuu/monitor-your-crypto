using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Domain.Utils
{
    public class CurrencyUtils
    {
        public static string GetCurrencyLabel(Currency currency)
        {
            switch (currency)
            {
                case Currency.Pln:
                    return "PLN";
                case Currency.Eur:
                    return "EUR";
                case Currency.Usd:
                    return "USD";
            }

            return "UNDEFINED";
        }

        public static string FormatWithPlusSign(decimal value, Currency currency) =>
            (value > 0 ? "+" : "") + Format(value, currency);

        public static string Format(decimal value, Currency currency)
        {
            var valueStr = DecimalUtils.FormatTwoDecimalPlaces(Math.Abs(value));
            var output = "";
            switch (currency)
            {
                case Currency.Pln:
                    output = $"{valueStr},- zł";
                    break;
                case Currency.Eur:
                    output = $"€{valueStr}";
                    break;
                case Currency.Usd:
                    output = $"${valueStr}";
                    break;
                default:
                    output = "UNDEFINED_CURRENCY";
                    break;
            }

            if (value < 0)
            {
                output = "-" + output;
            }

            return output;
        }
    }
}
