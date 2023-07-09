using System;
using System.Numerics;

namespace Fractions
{
    public class Fraction : IComparable<Fraction>, IEquatable<Fraction>
    {
        public BigInteger numerator;
        public BigInteger denominator;

        public bool IsZero
        {
            get { return numerator.IsZero; }
        }

        public Fraction()
            : this(BigInteger.Zero, BigInteger.One, false) { }

        public Fraction(BigInteger value)
            : this(value, BigInteger.One, false) { }

        public Fraction(BigInteger numerator, BigInteger denominator)
            : this(numerator, denominator, true) { }

        public Fraction(Fraction fraction)
            : this(fraction.numerator, fraction.denominator, false) { }

        public static Fraction operator +(Fraction self) => self;

        public static Fraction operator -(Fraction self) =>
            new Fraction(-self.numerator, self.denominator, false);

        public static Fraction operator +(Fraction self, Fraction other) =>
            new Fraction(
                self.numerator * other.denominator + other.numerator * self.denominator,
                self.denominator * other.denominator
            );

        public static Fraction operator +(Fraction self, BigInteger other) =>
            new Fraction(self.numerator + other * self.denominator, self.denominator);

        public static Fraction operator *(Fraction self, Fraction other)
        {
            var (numerator, other_denominator) = normalizeComponentsModuli(
                self.numerator,
                other.denominator
            );
            var (other_numerator, denominator) = normalizeComponentsModuli(
                other.numerator,
                self.denominator
            );
            return new Fraction(
                numerator * other_numerator,
                denominator * other_denominator,
                false
            );
        }

        public static Fraction operator *(Fraction self, BigInteger other)
        {
            var (other_normalized, denominator) = normalizeComponentsModuli(
                other,
                self.denominator
            );
            return new Fraction(self.numerator * other_normalized, denominator, false);
        }

        public static Fraction operator /(Fraction self, Fraction other)
        {
            var (numerator, other_numerator) = normalizeComponentsModuli(
                self.numerator,
                other.numerator
            );
            var (denominator, other_denominator) = normalizeComponentsModuli(
                self.denominator,
                other.denominator
            );
            var (result_numerator, result_denominator) = normalizeComponentsSign(
                numerator * other_denominator,
                denominator * other_numerator
            );
            return new Fraction(result_numerator, result_denominator, false);
        }

        public static Fraction operator /(Fraction self, BigInteger other)
        {
            var (numerator, other_normalized) = normalizeComponentsModuli(self.numerator, other);
            var (result_numerator, result_denominator) = normalizeComponentsSign(
                numerator,
                self.denominator * other_normalized
            );
            return new Fraction(result_numerator, result_denominator, false);
        }

        public static Fraction operator -(Fraction self, Fraction other) =>
            new Fraction(
                self.numerator * other.denominator - other.numerator * self.denominator,
                self.denominator * other.denominator
            );

        public static Fraction operator -(Fraction self, BigInteger other) =>
            new Fraction(self.numerator - other * self.denominator, self.denominator);

        public static bool operator ==(Fraction? self, Fraction? other) =>
            ReferenceEquals(self, null) ? ReferenceEquals(other, null) : self.Equals(other);

        public static bool operator !=(Fraction? self, Fraction? other) =>
            ReferenceEquals(self, null) ? !ReferenceEquals(other, null) : !self.Equals(other);

        public static bool operator <=(Fraction self, Fraction other) => self.CompareTo(other) <= 0;

        public static bool operator >=(Fraction self, Fraction other) => self.CompareTo(other) >= 0;

        public static bool operator <(Fraction self, Fraction other) => self.CompareTo(other) < 0;

        public static bool operator >(Fraction self, Fraction other) => self.CompareTo(other) > 0;

        public bool Equals(Fraction? other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return numerator.Equals(other.numerator) && denominator.Equals(other.denominator);
        }

        public static Fraction Abs(Fraction self) =>
            new Fraction(BigInteger.Abs(self.numerator), self.denominator, false);

        public override bool Equals(object? other) => Equals(other as Fraction);

        public override int GetHashCode()
        {
            var denominatorModularInverse = BigInteger.ModPow(
                denominator,
                HashModulus - 2,
                HashModulus
            );
            int result;
            if (denominatorModularInverse == BigInteger.Zero)
            {
                result = HashInf;
            }
            else
            {
                result = (int)(BigInteger.Abs(numerator) * denominatorModularInverse % HashModulus);
            }
            return numerator < BigInteger.Zero ? -result : result;
        }

        public int CompareTo(Fraction? other)
        {
            return ReferenceEquals(other, null)
                ? 1
                : (numerator * other.denominator).CompareTo(denominator * other.numerator);
        }

        private static BigInteger HashModulus = 2147483647;
        private static int HashInf = 314159;

        private Fraction(BigInteger numerator, BigInteger denominator, bool normalize)
        {
            if (denominator.IsZero)
                throw new DivideByZeroException("Denominator should not be zero.");
            if (normalize)
            {
                (numerator, denominator) = normalizeComponentsSign(numerator, denominator);
                (numerator, denominator) = normalizeComponentsModuli(numerator, denominator);
            }
            this.numerator = numerator;
            this.denominator = denominator;
        }

        private static (BigInteger, BigInteger) normalizeComponentsModuli(
            BigInteger numerator,
            BigInteger denominator
        )
        {
            var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
            return (numerator / gcd, denominator / gcd);
        }

        private static (BigInteger, BigInteger) normalizeComponentsSign(
            BigInteger numerator,
            BigInteger denominator
        )
        {
            return denominator < BigInteger.Zero
                ? (-numerator, -denominator)
                : (numerator, denominator);
        }
    }
}
