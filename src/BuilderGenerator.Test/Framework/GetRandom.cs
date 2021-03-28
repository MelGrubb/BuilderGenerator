using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuilderGenerator.Test.Framework
{
    /// <summary>Creates and returns a variety of random value types.</summary>
    public static class GetRandom
    {
        /// <summary>Default Prefix for random string values.</summary>
        public const string DefaultPrefix = "¿";

        public static int Id = 1;

        private static string _prefix;

        /// <summary>Returns a sequential (non-random) integer to be used as an object identifier.</summary>
        /// <returns>The next integer in the series.</returns>
        /// <remarks>The Int64 ids are drawn from the same pool of numbers as the Int32 ids.</remarks>
        public static int Int32Id
        {
            get => Id++;
            set => Id = value;
        }

        /// <summary>Gets or sets the prefix that gets prepended onto all random strings.</summary>
        /// <value>The prefix string.</value>
        /// <remarks>
        ///     You can set this to null if you don't want a prefix at all, but using a prefix such as the
        ///     default ("¿") can make it easier to query for test data that accidentally makes it into a
        ///     database.  Proper use of a mocking framework should be the first line of defense, though
        /// </remarks>
        public static string Prefix
        {
            get => _prefix ?? DefaultPrefix;
            set => _prefix = value;
        }

        /// <summary>
        ///     Gets or sets the random.
        /// </summary>
        /// <value>The random.</value>
        public static Random Random { get; set; } = new();

        /// <summary>
        ///     Returns a random <see cref="bool" /> value.
        /// </summary>
        /// <returns>
        ///     A random <see cref="bool" /> value.
        /// </returns>
        public static bool Bool()
        {
            var value = Random.Next();

            return value % 2 == 0;
        }

        /// <summary>
        ///     Returns a random <see cref="byte" /> value.
        /// </summary>
        /// <returns>
        ///     A random <see cref="byte" /> value.
        /// </returns>
        public static byte Byte() => Byte(0, byte.MaxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="byte" /> value less than the specified maximum.
        /// </summary>
        /// <returns>
        ///     A <see cref="byte" /> value greater than or equal to zero, and less than <paramref name="maxValue" />.
        /// </returns>
        public static byte Byte(byte maxValue) => Byte(0, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="byte" /> value within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        ///     A <see cref="byte" /> value greater than or equal to <paramref name="minValue" />, and less than
        ///     <paramref
        ///         name="maxValue" />
        ///     .
        /// </returns>
        public static byte Byte(byte minValue, byte maxValue) => Byte(minValue, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="byte" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>
        ///     A <see cref="byte" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     , and not equal to <paramref name="exclude" />.
        /// </returns>
        public static byte Byte(byte minValue, byte maxValue, byte? exclude)
        {
            byte result;

            do
            {
                result = (byte)Random.Next(minValue, maxValue);
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        /// <summary>Returns an array of the specified length, filled with random byte values.</summary>
        /// <param name="length">The length of the array to be created.</param>
        /// <returns>an array of the specified length, filled with random byte values.</returns>
        public static byte[] ByteArray(int length)
        {
            var result = new byte[length];
            Random.NextBytes(result);

            return result;
        }

        /// <summary>Returns a random, printable character.</summary>
        /// <returns>A random, printable character.</returns>
        /// <remarks>This overload will limit the results to a subset of the range of printable characters, '!' through '~'.</remarks>
        public static char Char() => Char('!', '~');

        /// <summary>Returns a random, printable character.</summary>
        /// <param name="maxValue">The maximum desired character.</param>
        /// <returns>A random, printable character from '!' through the specified maximum.</returns>
        public static char Char(char maxValue) => Char('!', maxValue, null);

        /// <summary>Returns a random, printable character within the specified range.</summary>
        /// <param name="minValue">The minimum desired value.</param>
        /// <param name="maxValue">The maximum desired value.</param>
        /// <returns>A random, printable character within the specified range.</returns>
        public static char Char(char minValue, char maxValue) => Char(minValue, maxValue, null);

        /// <summary>Returns a random, printable character within the specified range, excluding the specified value.</summary>
        /// <param name="minValue">The minimum desired value.</param>
        /// <param name="maxValue">The maximum desired value.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>
        ///     A <see cref="byte" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     , and not equal to <paramref name="exclude" />.
        /// </returns>
        public static char Char(char minValue, char maxValue, char? exclude)
        {
            char result;

            do
            {
                result = Convert.ToChar(Int32(minValue, maxValue));
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        /// <summary>
        ///     Returns a random <see cref="System.DateTime" /> value.
        /// </summary>
        /// <returns>
        ///     A random <see cref="System.DateTime" /> value.
        /// </returns>
        public static DateTime DateTime() => DateTime(System.DateTime.MinValue, System.DateTime.MaxValue);

        /// <summary>
        ///     Returns a random <see cref="System.DateTime" /> value less than the specified maximum value.
        /// </summary>
        /// <param name="maxValue">The maximum desired value.</param>
        /// <returns>
        ///     A <see cref="System.DateTime" /> value less than <paramref name="maxValue" />.
        /// </returns>
        public static DateTime DateTime(DateTime maxValue) => DateTime(System.DateTime.MinValue, maxValue);

        /// <summary>
        ///     Returns a random <see cref="System.DateTime" /> value within the specified range.
        /// </summary>
        /// <param name="minValue">The minimum desired value.</param>
        /// <param name="maxValue">The maximum desired value.</param>
        /// <returns>
        ///     A <see cref="System.DateTime" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     .
        /// </returns>
        public static DateTime DateTime(DateTime minValue, DateTime maxValue) => DateTime(minValue, maxValue, null);

        /// <summary>
        ///     Returns a random <see cref="System.DateTime" /> value within the specified range.
        /// </summary>
        /// <param name="minValue">The minimum desired value.</param>
        /// <param name="maxValue">The maximum desired value.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>
        ///     A <see cref="System.DateTime" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     , and not equal to <paramref name="exclude" />.
        /// </returns>
        public static DateTime DateTime(DateTime minValue, DateTime maxValue, DateTime? exclude)
        {
            DateTime result;

            do
            {
                result = new DateTime(Int64(minValue.Ticks, maxValue.Ticks));
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        /// <summary>
        ///     Returns a random date from <paramref name="daysInThePast" /> days ago to <paramref name="daysInTheFuture" /> from now.
        /// </summary>
        /// <param name="daysInThePast">The maximum number of days in the past.</param>
        /// <param name="daysInTheFuture">The maximum number of days in the future.</param>
        /// <returns>
        ///     A random date from <paramref name="daysInThePast" /> days ago to <paramref name="daysInTheFuture" /> from now.
        /// </returns>
        public static DateTime DateTime(int daysInThePast, int daysInTheFuture) => System.DateTime.Now.AddDays(Int32(-daysInThePast, daysInTheFuture));

        /// <summary>
        ///     Returns a nonnegative random <see cref="decimal" /> value.
        /// </summary>
        /// <returns>
        ///     A <see cref="decimal" /> value greater than or equal to zero.
        /// </returns>
        public static decimal Decimal() => Decimal(0, decimal.MaxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="decimal" /> value less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random value returned.</param>
        /// <returns>
        ///     A <see cref="decimal" /> value greater than or equal to zero, and less than <paramref name="maxValue" />.
        /// </returns>
        public static decimal Decimal(decimal maxValue) => Decimal(0, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="decimal" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        ///     A <see cref="decimal" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     .
        /// </returns>
        public static decimal Decimal(decimal minValue, decimal maxValue) => Decimal(minValue, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="decimal" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>
        ///     A <see cref="decimal" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     , and not equal to <paramref name="exclude" />.
        /// </returns>
        public static decimal Decimal(decimal minValue, decimal maxValue, decimal? exclude)
        {
            decimal result;

            do
            {
                result = minValue + ((maxValue - minValue) * Convert.ToDecimal(Double()));
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        /// <summary>
        ///     Returns a nonnegative random <see cref="double" /> value.
        /// </summary>
        /// <returns>
        ///     A <see cref="double" /> value greater than or equal to zero.
        /// </returns>
        public static double Double() => Random.NextDouble();

        /// <summary>
        ///     Returns a nonnegative random <see cref="double" /> value less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random value returned.</param>
        /// <returns>
        ///     A <see cref="double" /> value greater than or equal to zero, and less than <paramref name="maxValue" />.
        /// </returns>
        public static double Double(double maxValue) => Double(0, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="double" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        ///     A <see cref="double" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     .
        /// </returns>
        public static double Double(double minValue, double maxValue) => Double(minValue, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="double" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>
        ///     A <see cref="double" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     , and not equal to <paramref name="exclude" />.
        /// </returns>
        public static double Double(double minValue, double maxValue, double? exclude)
        {
            double result;

            do
            {
                result = minValue + ((maxValue - minValue) * Double());
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        /// <summary>Returns a random element from the provided list.</summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list of items.</param>
        /// <returns>A random element from the provided list.</returns>
        public static T Element<T>(IEnumerable<T> list)
        {
            var enumerable = list as List<T> ?? list.ToList();
            var result = enumerable.ElementAt(Int32(enumerable.Count()));

            return result;
        }

        /// <summary>Returns a random element from the provided list, excluding the specified element.</summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list of items.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>A random element from the provided list, excluding the specified element.</returns>
        public static T Element<T>(IEnumerable<T> list, T exclude)
        {
            T result;
            var enumerable = list as List<T> ?? list.ToList();

            do
            {
                result = Element(enumerable);
            } while (Equals(result, exclude));

            return result;
        }

        /// <summary>Returns a random element from the specified enumeration.</summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <returns>A random element from the specified enumeration.</returns>
        public static TEnum EnumValue<TEnum>()
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumeration.");
            }

            return Element((IEnumerable<TEnum>)Enum.GetValues(enumType));
        }

        /// <summary>Returns a random element from the specified enumeration, excluding the specified element.</summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>A random element from the specified enumeration, excluding the specified element.</returns>
        public static TEnum EnumValue<TEnum>(TEnum exclude)
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumeration.");
            }

            return Element((IEnumerable<TEnum>)Enum.GetValues(enumType), exclude);
        }

        /// <summary>Returns a randomly-chosen first name.</summary>
        /// <param name="male">A nullable Boolean value indicating whether the name should be male or female.</param>
        /// <returns>If <paramref name="male" /> is true, a male name; If false, a female name; If null, a randomly-chosen male or female name.</returns>
        public static string FirstName(bool? male = null)
        {
            if (!male.HasValue)
            {
                male = Bool();
            }

            var result = Element(male.Value ? Names.Male : Names.Female);

            return result;
        }

        /// <summary>
        ///     Returns a nonnegative random <see cref="short" /> value.
        /// </summary>
        /// <returns>
        ///     A <see cref="short" /> value greater than or equal to zero.
        /// </returns>
        public static short Int16() => Int16(0, short.MaxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="short" /> value less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random value returned.</param>
        /// <returns>
        ///     A <see cref="short" /> value greater than or equal to zero, and less than <paramref name="maxValue" />.
        /// </returns>
        public static short Int16(short maxValue) => Int16(0, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="short" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        ///     A <see cref="short" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     .
        /// </returns>
        public static short Int16(short minValue, short maxValue) => Int16(minValue, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="short" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>
        ///     A <see cref="short" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     , and not equal to <paramref name="exclude" />.
        /// </returns>
        public static short Int16(short minValue, short maxValue, short? exclude)
        {
            short result;

            do
            {
                result = (short)Random.Next(minValue, maxValue);
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        /// <summary>
        ///     Returns a nonnegative random <see cref="int" /> value.
        /// </summary>
        /// <returns>
        ///     A <see cref="int" /> value greater than or equal to zero.
        /// </returns>
        public static int Int32() => Random.Next();

        /// <summary>
        ///     Returns a nonnegative random <see cref="int" /> value less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random value returned.</param>
        /// <returns>
        ///     A <see cref="int" /> value greater than or equal to zero, and less than <paramref name="maxValue" />.
        /// </returns>
        public static int Int32(int maxValue) => Random.Next(maxValue);

        /// <summary>
        ///     Returns a nonnegative random <see cref="int" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        ///     A <see cref="int" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     .
        /// </returns>
        public static int Int32(int minValue, int maxValue) => Random.Next(minValue, maxValue);

        /// <summary>
        ///     Returns a nonnegative random <see cref="int" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>
        ///     A <see cref="int" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     , and not equal to <paramref name="exclude" />.
        /// </returns>
        public static int Int32(int minValue, int maxValue, int? exclude)
        {
            int result;

            do
            {
                result = Random.Next(minValue, maxValue);
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        /// <summary>
        ///     Returns a nonnegative random <see cref="long" /> value.
        /// </summary>
        /// <returns>
        ///     A <see cref="long" /> value greater than or equal to zero.
        /// </returns>
        public static long Int64() => Int64(0, long.MaxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="long" /> value less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random value returned.</param>
        /// <returns>
        ///     A <see cref="long" /> value greater than or equal to zero, and less than <paramref name="maxValue" />.
        /// </returns>
        public static long Int64(long maxValue) => Int64(0, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="long" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        ///     A <see cref="long" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     .
        /// </returns>
        public static long Int64(long minValue, long maxValue) => Int64(minValue, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="long" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>
        ///     A <see cref="long" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     , and not equal to <paramref name="exclude" />.
        /// </returns>
        public static long Int64(long minValue, long maxValue, long? exclude)
        {
            long result;

            do
            {
                result = minValue + Convert.ToInt64(Convert.ToDouble(maxValue - minValue) * Double());
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        /// <summary>Returns a randomly-chosen last name.</summary>
        /// <returns>A string containing a last name chosen at random from a list of typical names.</returns>
        public static string LastName()
        {
            var result = Element(Names.Last);

            return result;
        }

        /// <summary>Returns a randomly-chosen first and last name.</summary>
        /// <returns>A string containing a first and last name chosen at random from a list of typical names.</returns>
        public static string Name() => FirstName() + " " + LastName();

        /// <summary>Returns an instance of the specified class.</summary>
        /// <typeparam name="T">The type of the class to return.</typeparam>
        /// <returns>A new instance of the specified class.</returns>
        /// <remarks>
        ///     This method is meant to provide for some consistency in how unit tests are written. It doesn't do anything
        ///     that "new T()" wouldn't have provided, but makes the code "read" more consistently.
        /// </remarks>
        public static T Object<T>() where T : class, new() => new();

        /// <summary>
        ///     Returns a nonnegative random <see cref="float" /> value.
        /// </summary>
        /// <returns>
        ///     A <see cref="float" /> value greater than or equal to zero.
        /// </returns>
        public static float Single() => Single(0, float.MaxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="float" /> value less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random value returned.</param>
        /// <returns>
        ///     A <see cref="float" /> value greater than or equal to zero, and less than <paramref name="maxValue" />.
        /// </returns>
        public static float Single(float maxValue) => Single(0, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="float" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        ///     A <see cref="float" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     .
        /// </returns>
        public static float Single(float minValue, float maxValue) => Single(minValue, maxValue, null);

        /// <summary>
        ///     Returns a nonnegative random <see cref="float" /> value within the specified range, excluding the specified value.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random value returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random value returned. maxValue must be greater than or equal to minValue.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>
        ///     A <see cref="float" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref
        ///         name="maxValue" />
        ///     , and not equal to <paramref name="exclude" />.
        /// </returns>
        public static float Single(float minValue, float maxValue, float? exclude)
        {
            float result;

            do
            {
                result = Convert.ToSingle(Double(minValue, maxValue));
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        /// <summary>Returns a random string of less than 50 characters.</summary>
        /// <returns>A random string of less than 50 characters.</returns>
        public static string String() => String(0, 50, null);

        /// <summary>Returns a random string with a length less than the specified maximum.</summary>
        /// <param name="maxLength">The exclusive upper bound for the length of the string to be returned.</param>
        /// <returns>A random string with a length less than the specified maximum.</returns>
        public static string String(int maxLength) => String(0, maxLength, null);

        /// <summary>Returns a random string with a length in the specified range.</summary>
        /// <param name="minLength">The inclusive lower bound for the length of the string to be returned.</param>
        /// <param name="maxLength">The exclusive upper bound for the length of the string to be returned.</param>
        /// <returns>A random string with a length in the specified range.</returns>
        public static string String(int minLength, int maxLength) => String(minLength, maxLength, null);

        /// <summary>Returns a random string with a length in the specified range, excluding the specified value.</summary>
        /// <param name="minLength">The inclusive lower bound for the length of the string to be returned.</param>
        /// <param name="maxLength">The exclusive upper bound for the length of the string to be returned.</param>
        /// <param name="exclude">A value that should not be returned, used when you want to get a value other than one you already have.</param>
        /// <returns>A random string with a length in the specified range, excluding the specified value.</returns>
        public static string String(int minLength, int maxLength, string exclude)
        {
            string result;

            do
            {
                var length = Int32(minLength, maxLength);
                var prefixLength = Prefix == null ? 0 : Prefix.Length;
                var sb = new StringBuilder(Prefix ?? string.Empty, length);

                for (var index = 0; index < length - prefixLength; index++)
                {
                    sb.Append(Convert.ToChar(Int32(65, 90)));
                }

                result = sb.ToString(0, length);
            } while (!string.IsNullOrEmpty(exclude) && result == exclude);

            return result;
        }

        public static class Names
        {
            public static readonly string[] Female =
            {
                "Aaliyah", "Abby", "Abigail", "Addison", "Adriana", "Adrianna", "Alana", "Alexa", "Alexandra", "Alexandria",
                "Alexia", "Alexis", "Alicia", "Allison", "Alondra", "Alyssa", "Amanda", "Amber", "Amelia", "Amy",
                "Ana", "Andrea", "Angel", "Angela", "Angelica", "Angelina", "Anna", "Ariana", "Arianna", "Ashley",
                "Ashlyn", "Aubrey", "Audrey", "Autumn", "Ava", "Avery", "Bailey", "Bianca", "Breanna", "Briana",
                "Brianna", "Brittany", "Brooke", "Brooklyn", "Caitlin", "Caitlyn", "Camila", "Caroline", "Cassandra", "Cassidy",
                "Catherine", "Charlotte", "Chelsea", "Cheyenne", "Chloe", "Christina", "Claire", "Courtney", "Crystal", "Daisy",
                "Daniela", "Danielle", "Delaney", "Destiny", "Diana", "Elizabeth", "Ella", "Ellie", "Emily", "Emma",
                "Erica", "Erin", "Eva", "Evelyn", "Faith", "Gabriela", "Gabriella", "Gabrielle", "Genesis", "Gianna",
                "Giselle", "Grace", "Gracie", "Hailey", "Haley", "Hannah", "Hope", "Isabel", "Isabella", "Isabelle",
                "Jacqueline", "Jada", "Jade", "Jasmin", "Jasmine", "Jayla", "Jazmin", "Jenna", "Jennifer", "Jessica",
                "Jillian", "Jocelyn", "Jordan", "Jordyn", "Julia", "Juliana", "Kaitlyn", "Karen", "Karina", "Kate",
                "Katelyn", "Katherine", "Kathryn", "Katie", "Kayla", "Kaylee", "Kelly", "Kelsey", "Kendall", "Kennedy",
                "Kiara", "Kimberly", "Kylee", "Kylie", "Laura", "Lauren", "Layla", "Leah", "Leslie", "Liliana",
                "Lillian", "Lilly", "Lily", "Lindsey", "Lucy", "Lydia", "Mackenzie", "Madeline", "Madelyn", "Madison",
                "Makayla", "Makenzie", "Margaret", "Maria", "Mariah", "Marissa", "Mary", "Maya", "Mckenzie", "Megan",
                "Melanie", "Melissa", "Mia", "Michelle", "Mikayla", "Miranda", "Molly", "Morgan", "Mya", "Naomi",
                "Natalia", "Natalie", "Nevaeh", "Nicole", "Olivia", "Paige", "Payton", "Peyton", "Rachel", "Reagan",
                "Rebecca", "Riley", "Ruby", "Rylee", "Sabrina", "Sadie", "Samantha", "Sara", "Sarah", "Savannah",
                "Serenity", "Shelby", "Sierra", "Skylar", "Sofia", "Sophia", "Sophie", "Stephanie", "Summer", "Sydney",
                "Taylor", "Tiffany", "Trinity", "Valeria", "Valerie", "Vanessa", "Veronica", "Victoria", "Zoe", "Zoey",
            };

            public static readonly string[] Last =
            {
                "Abbott", "Acevedo", "Acosta", "Adams", "Adkins", "Aguilar", "Aguirre", "Albert", "Alexander", "Alford",
                "Allen", "Allison", "Alston", "Alvarado", "Alvarez", "Anderson", "Andrews", "Anthony", "Armstrong", "Arnold",
                "Ashley", "Atkins", "Atkinson", "Austin", "Avery", "Avila", "Ayala", "Ayers", "Bailey", "Baird",
                "Baker", "Baldwin", "Ball", "Ballard", "Banks", "Barber", "Barker", "Barlow", "Barnes", "Barnett",
                "Barr", "Barrera", "Barrett", "Barron", "Barry", "Bartlett", "Barton", "Bass", "Bates", "Battle",
                "Bauer", "Baxter", "Beach", "Bean", "Beard", "Beasley", "Beck", "Becker", "Bell", "Bender",
                "Benjamin", "Bennett", "Benson", "Bentley", "Benton", "Berg", "Berger", "Bernard", "Berry", "Best",
                "Bird", "Bishop", "Black", "Blackburn", "Blackwell", "Blair", "Blake", "Blanchard", "Blankenship", "Blevins",
                "Bolton", "Bond", "Bonner", "Booker", "Boone", "Booth", "Bowen", "Bowers", "Bowman", "Boyd",
                "Boyer", "Boyle", "Bradford", "Bradley", "Bradshaw", "Brady", "Branch", "Bray", "Brennan", "Brewer",
                "Bridges", "Briggs", "Bright", "Britt", "Brock", "Brooks", "Brown", "Browning", "Bruce", "Bryan",
                "Bryant", "Buchanan", "Buck", "Buckley", "Buckner", "Bullock", "Burch", "Burgess", "Burke", "Burks",
                "Burnett", "Burns", "Burris", "Burt", "Burton", "Bush", "Butler", "Byers", "Byrd", "Cabrera",
                "Cain", "Calderon", "Caldwell", "Calhoun", "Callahan", "Camacho", "Cameron", "Campbell", "Campos", "Cannon",
                "Cantrell", "Cantu", "Cardenas", "Carey", "Carlson", "Carney", "Carpenter", "Carr", "Carrillo", "Carroll",
                "Carson", "Carter", "Carver", "Case", "Casey", "Cash", "Castaneda", "Castillo", "Castro", "Cervantes",
                "Chambers", "Chan", "Chandler", "Chaney", "Chang", "Chapman", "Charles", "Chase", "Chavez", "Chen",
                "Cherry", "Christensen", "Christian", "Church", "Clark", "Clarke", "Clay", "Clayton", "Clements", "Clemons",
                "Cleveland", "Cline", "Cobb", "Cochran", "Coffey", "Cohen", "Cole", "Coleman", "Collier", "Collins",
                "Colon", "Combs", "Compton", "Conley", "Conner", "Conrad", "Contreras", "Conway", "Cook", "Cooke",
                "Cooley", "Cooper", "Copeland", "Cortez", "Cote", "Cotton", "Cox", "Craft", "Craig", "Crane",
                "Crawford", "Crosby", "Cross", "Cruz", "Cummings", "Cunningham", "Curry", "Curtis", "Dale", "Dalton",
                "Daniel", "Daniels", "Daugherty", "Davenport", "David", "Davidson", "Davis", "Dawson", "Day", "Dean",
                "Decker", "Dejesus", "Delacruz", "Delaney", "Deleon", "Delgado", "Dennis", "Diaz", "Dickerson", "Dickson",
                "Dillard", "Dillon", "Dixon", "Dodson", "Dominguez", "Donaldson", "Donovan", "Dorsey", "Dotson", "Douglas",
                "Downs", "Doyle", "Drake", "Dudley", "Duffy", "Duke", "Duncan", "Dunlap", "Dunn", "Duran",
                "Durham", "Dyer", "Eaton", "Edwards", "Elliott", "Ellis", "Ellison", "Emerson", "England", "English",
                "Erickson", "Espinoza", "Estes", "Estrada", "Evans", "Everett", "Ewing", "Farley", "Farmer", "Farrell",
                "Faulkner", "Ferguson", "Fernandez", "Ferrell", "Fields", "Figueroa", "Finch", "Finley", "Fischer", "Fisher",
                "Fitzgerald", "Fitzpatrick", "Fleming", "Fletcher", "Flores", "Flowers", "Floyd", "Flynn", "Foley", "Forbes",
                "Ford", "Foreman", "Foster", "Fowler", "Fox", "Francis", "Franco", "Frank", "Franklin", "Franks",
                "Frazier", "Frederick", "Freeman", "French", "Frost", "Fry", "Frye", "Fuentes", "Fuller", "Fulton",
                "Gaines", "Gallagher", "Gallegos", "Galloway", "Gamble", "Garcia", "Gardner", "Garner", "Garrett", "Garrison",
                "Garza", "Gates", "Gay", "Gentry", "George", "Gibbs", "Gibson", "Gilbert", "Giles", "Gill",
                "Gillespie", "Gilliam", "Gilmore", "Glass", "Glenn", "Glover", "Goff", "Golden", "Gomez", "Gonzales",
                "Gonzalez", "Good", "Goodman", "Goodwin", "Gordon", "Gould", "Graham", "Grant", "Graves", "Gray",
                "Green", "Greene", "Greer", "Gregory", "Griffin", "Griffith", "Grimes", "Gross", "Guerra", "Guerrero",
                "Guthrie", "Gutierrez", "Guy", "Guzman", "Hahn", "Hale", "Haley", "Hall", "Hamilton", "Hammond",
                "Hampton", "Hancock", "Haney", "Hansen", "Hanson", "Hardin", "Harding", "Hardy", "Harmon", "Harper",
                "Harrell", "Harrington", "Harris", "Harrison", "Hart", "Hartman", "Harvey", "Hatfield", "Hawkins", "Hayden",
                "Hayes", "Haynes", "Hays", "Head", "Heath", "Hebert", "Henderson", "Hendricks", "Hendrix", "Henry",
                "Hensley", "Henson", "Herman", "Hernandez", "Herrera", "Herring", "Hess", "Hester", "Hewitt", "Hickman",
                "Hicks", "Higgins", "Hill", "Hines", "Hinton", "Hobbs", "Hodge", "Hodges", "Hoffman", "Hogan",
                "Holcomb", "Holden", "Holder", "Holland", "Holloway", "Holman", "Holmes", "Holt", "Hood", "Hooper",
                "Hoover", "Hopkins", "Hopper", "Horn", "Horne", "Horton", "House", "Houston", "Howard", "Howe",
                "Howell", "Hubbard", "Huber", "Hudson", "Huff", "Huffman", "Hughes", "Hull", "Humphrey", "Hunt",
                "Hunter", "Hurley", "Hurst", "Hutchinson", "Hyde", "Ingram", "Irwin", "Jackson", "Jacobs", "Jacobson",
                "James", "Jarvis", "Jefferson", "Jenkins", "Jennings", "Jensen", "Jimenez", "Johns", "Johnson", "Johnston",
                "Jones", "Jordan", "Joseph", "Joyce", "Joyner", "Juarez", "Justice", "Kane", "Kaufman", "Keith",
                "Keller", "Kelley", "Kelly", "Kemp", "Kennedy", "Kent", "Kerr", "Key", "Kidd", "Kim",
                "King", "Kinney", "Kirby", "Kirk", "Kirkland", "Klein", "Kline", "Knapp", "Knight", "Knowles",
                "Knox", "Koch", "Kramer", "Lamb", "Lambert", "Lancaster", "Landry", "Lane", "Lang", "Langley",
                "Lara", "Larsen", "Larson", "Lawrence", "Lawson", "Le", "Leach", "Leblanc", "Lee", "Leon",
                "Leonard", "Lester", "Levine", "Levy", "Lewis", "Lindsay", "Lindsey", "Little", "Livingston", "Lloyd",
                "Logan", "Long", "Lopez", "Lott", "Love", "Lowe", "Lowery", "Lucas", "Luna", "Lynch",
                "Lynn", "Lyons", "MacDonald", "Macias", "Mack", "Madden", "Maddox", "Maldonado", "Malone", "Mann",
                "Manning", "Marks", "Marquez", "Marsh", "Marshall", "Martin", "Martinez", "Mason", "Massey", "Mathews",
                "Mathis", "Matthews", "Maxwell", "May", "Mayer", "Maynard", "Mayo", "Mays", "McBride", "McCall",
                "McCarthy", "McCarty", "McClain", "McClure", "McConnell", "McCormick", "McCoy", "McCray", "McCullough", "McDaniel",
                "McDonald", "McDowell", "McFadden", "McFarland", "McGee", "McGowan", "McGuire", "McIntosh", "McIntyre", "McKay",
                "McKee", "McKenzie", "McKinney", "McKnight", "McLaughlin", "McLean", "McLeod", "McMahon", "McMillan", "McNeil",
                "McPherson", "Meadows", "Medina", "Mejia", "Melendez", "Melton", "Mendez", "Mendoza", "Mercado", "Mercer",
                "Merrill", "Merritt", "Meyer", "Meyers", "Michael", "Middleton", "Miles", "Miller", "Mills", "Miranda",
                "Mitchell", "Molina", "Monroe", "Montgomery", "Montoya", "Moody", "Moon", "Mooney", "Moore", "Morales",
                "Moran", "Moreno", "Morgan", "Morin", "Morris", "Morrison", "Morrow", "Morse", "Morton", "Moses",
                "Mosley", "Moss", "Mueller", "Mullen", "Mullins", "Munoz", "Murphy", "Murray", "Myers", "Nash",
                "Navarro", "Neal", "Nelson", "Newman", "Newton", "Nguyen", "Nichols", "Nicholson", "Nielsen", "Nieves",
                "Nixon", "Noble", "Noel", "Nolan", "Norman", "Norris", "Norton", "Nunez", "O'Brien", "Ochoa",
                "O'Connor", "Odom", "O'Donnell", "Oliver", "Olsen", "Olson", "O'Neal", "O'Neil", "O'Neill", "Orr",
                "Ortega", "Ortiz", "Osborn", "Osborne", "Owen", "Owens", "Pace", "Pacheco", "Padilla", "Page",
                "Palmer", "Park", "Parker", "Parks", "Parrish", "Parsons", "Pate", "Patel", "Patrick", "Patterson",
                "Patton", "Paul", "Payne", "Pearson", "Peck", "Pena", "Pennington", "Perez", "Perkins", "Perry",
                "Peters", "Petersen", "Peterson", "Petty", "Phelps", "Phillips", "Pickett", "Pierce", "Pittman", "Pitts",
                "Pollard", "Poole", "Pope", "Porter", "Potter", "Potts", "Powell", "Powers", "Pratt", "Preston",
                "Price", "Prince", "Pruitt", "Puckett", "Pugh", "Quinn", "Ramirez", "Ramos", "Ramsey", "Randall",
                "Randolph", "Rasmussen", "Ratliff", "Ray", "Raymond", "Reed", "Reese", "Reeves", "Reid", "Reilly",
                "Reyes", "Reynolds", "Rhodes", "Rice", "Rich", "Richard", "Richards", "Richardson", "Richmond", "Riddle",
                "Riggs", "Riley", "Rios", "Rivas", "Rivera", "Rivers", "Roach", "Robbins", "Roberson", "Roberts",
                "Robertson", "Robinson", "Robles", "Rocha", "Rodgers", "Rodriguez", "Rodriquez", "Rogers", "Rojas", "Rollins",
                "Roman", "Romero", "Rosa", "Rosales", "Rosario", "Rose", "Ross", "Roth", "Rowe", "Rowland",
                "Roy", "Ruiz", "Rush", "Russell", "Russo", "Rutledge", "Ryan", "Salas", "Salazar", "Salinas",
                "Sampson", "Sanchez", "Sanders", "Sandoval", "Sanford", "Santana", "Santiago", "Santos", "Sargent", "Saunders",
                "Savage", "Sawyer", "Schmidt", "Schneider", "Schroeder", "Schultz", "Schwartz", "Scott", "Sears", "Sellers",
                "Serrano", "Sexton", "Shaffer", "Shannon", "Sharp", "Sharpe", "Shaw", "Shelton", "Shepard", "Shepherd",
                "Sheppard", "Sherman", "Shields", "Short", "Silva", "Simmons", "Simon", "Simpson", "Sims", "Singleton",
                "Skinner", "Slater", "Sloan", "Small", "Smith", "Snider", "Snow", "Snyder", "Solis", "Solomon",
                "Sosa", "Soto", "Sparks", "Spears", "Spence", "Spencer", "Stafford", "Stanley", "Stanton", "Stark",
                "Steele", "Stein", "Stephens", "Stephenson", "Stevens", "Stevenson", "Stewart", "Stokes", "Stone", "Stout",
                "Strickland", "Strong", "Stuart", "Suarez", "Sullivan", "Summers", "Sutton", "Swanson", "Sweeney", "Sweet",
                "Sykes", "Talley", "Tanner", "Tate", "Taylor", "Terrell", "Terry", "Thomas", "Thompson", "Thornton",
                "Tillman", "Todd", "Torres", "Townsend", "Tran", "Travis", "Trevino", "Trujillo", "Tucker", "Turner",
                "Tyler", "Tyson", "Underwood", "Valdez", "Valencia", "Valentine", "Valenzuela", "Vance", "Vang", "Vargas",
                "Vasquez", "Vaughan", "Vaughn", "Vazquez", "Vega", "Velasquez", "Velazquez", "Velez", "Villarreal", "Vincent",
                "Vinson", "Wade", "Wagner", "Walker", "Wall", "Wallace", "Waller", "Walls", "Walsh", "Walter",
                "Walters", "Walton", "Ward", "Ware", "Warner", "Warren", "Washington", "Waters", "Watkins", "Watson",
                "Watts", "Weaver", "Webb", "Weber", "Webster", "Weeks", "Weiss", "Welch", "Wells", "West",
                "Wheeler", "Whitaker", "White", "Whitehead", "Whitfield", "Whitley", "Whitney", "Wiggins", "Wilcox", "Wilder",
                "Wiley", "Wilkerson", "Wilkins", "Wilkinson", "William", "Williams", "Williamson", "Willis", "Wilson", "Winters",
                "Wise", "Witt", "Wolf", "Wolfe", "Wong", "Wood", "Woodard", "Woods", "Woodward", "Wooten",
                "Workman", "Wright", "Wyatt", "Wynn", "Yang", "Yates", "York", "Young", "Zamora", "Zimmerman",
            };

            public static readonly string[] Male =
            {
                "Aaron", "Abraham", "Adam", "Adrian", "Aidan", "Aiden", "Alan", "Alejandro", "Alex", "Alexander",
                "Alexis", "Andres", "Andrew", "Angel", "Anthony", "Antonio", "Ashton", "Austin", "Ayden", "Benjamin",
                "Blake", "Braden", "Bradley", "Brady", "Brandon", "Brayden", "Brendan", "Brian", "Brody", "Bryan",
                "Bryce", "Bryson", "Caden", "Caleb", "Cameron", "Carlos", "Carson", "Carter", "Cesar", "Charles",
                "Chase", "Christian", "Christopher", "Cody", "Colby", "Cole", "Colin", "Collin", "Colton", "Conner",
                "Connor", "Cooper", "Cristian", "Dakota", "Dalton", "Damian", "Daniel", "David", "Derek", "Devin",
                "Devon", "Diego", "Dominic", "Donovan", "Dylan", "Edgar", "Eduardo", "Edward", "Edwin", "Eli",
                "Elias", "Elijah", "Emmanuel", "Eric", "Erick", "Erik", "Ethan", "Evan", "Fernando", "Francisco",
                "Gabriel", "Gage", "Garrett", "Gavin", "George", "Giovanni", "Grant", "Gregory", "Hayden", "Hector",
                "Henry", "Hunter", "Ian", "Isaac", "Isaiah", "Ivan", "Jack", "Jackson", "Jacob", "Jaden",
                "Jake", "Jalen", "James", "Jared", "Jason", "Javier", "Jayden", "Jeffrey", "Jeremiah", "Jeremy",
                "Jesse", "Jesus", "Joel", "John", "Johnathan", "Jonah", "Jonathan", "Jordan", "Jorge", "Jose",
                "Joseph", "Joshua", "Josiah", "Juan", "Julian", "Justin", "Kaden", "Kaleb", "Kenneth", "Kevin",
                "Kyle", "Landon", "Leonardo", "Levi", "Liam", "Logan", "Lucas", "Luis", "Luke", "Malachi",
                "Manuel", "Marco", "Marcus", "Mario", "Mark", "Martin", "Mason", "Matthew", "Max", "Maxwell",
                "Micah", "Michael", "Miguel", "Nathan", "Nathaniel", "Nicholas", "Nicolas", "Noah", "Nolan", "Oliver",
                "Omar", "Oscar", "Owen", "Parker", "Patrick", "Paul", "Peter", "Peyton", "Preston", "Raymond",
                "Ricardo", "Richard", "Riley", "Robert", "Ryan", "Samuel", "Sean", "Sebastian", "Sergio", "Seth",
                "Shane", "Shawn", "Spencer", "Stephen", "Steven", "Tanner", "Thomas", "Timothy", "Travis", "Trenton",
                "Trevor", "Tristan", "Tyler", "Victor", "Vincent", "Wesley", "William", "Wyatt", "Xavier", "Zachary",
            };
        }
    }
}
