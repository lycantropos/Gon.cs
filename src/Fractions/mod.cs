using System;
using System.Numerics;

namespace Fractions
{
    public class Fraction : IComparable<Fraction>, IEquatable<Fraction>
    {
        public BigInteger numerator;
        public BigInteger denominator;

        public Fraction(BigInteger numerator, BigInteger denominator)
            : this(numerator, denominator, true) { }

        public static Fraction operator +(Fraction self) => self;

        public static Fraction operator -(Fraction self) =>
            new Fraction(-self.numerator, self.denominator, false);

        public static Fraction operator +(Fraction self, Fraction other) =>
            new Fraction(
                self.numerator * other.denominator + other.numerator * self.denominator,
                self.denominator * other.denominator
            );

        public static Fraction operator -(Fraction self, Fraction other) =>
            new Fraction(
                self.numerator * other.denominator - other.numerator * self.denominator,
                self.denominator * other.denominator
            );

        public static bool operator ==(Fraction self, Fraction other)
        {
            if (ReferenceEquals(self, null))
                return false;
            return self.Equals(other);
        }

        public static bool operator !=(Fraction self, Fraction other) => !(self == other);

        public static bool operator <=(Fraction self, Fraction other) => self.CompareTo(other) <= 0;

        public static bool operator >=(Fraction self, Fraction other) => self.CompareTo(other) >= 0;

        public static bool operator <(Fraction self, Fraction other) => self.CompareTo(other) < 0;

        public static bool operator >(Fraction self, Fraction other) => self.CompareTo(other) > 0;

        public bool Equals(Fraction other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return numerator.Equals(other.numerator) && denominator.Equals(other.denominator);
        }

        public override bool Equals(object other) => Equals(other as Fraction);

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

        public int CompareTo(Fraction other)
        {
            return ReferenceEquals(other, null)
                ? 1
                : (numerator * other.denominator).CompareTo(denominator * other.numerator);
        }

        private static BigInteger HashModulus = 2147483647;
        private static int HashInf = 314159;

        private Fraction(BigInteger numerator, BigInteger denominator, bool normalize)
        {
            if (normalize)
            {
                if (denominator < BigInteger.Zero)
                {
                    numerator = -numerator;
                    denominator = -denominator;
                }
                var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
                numerator /= gcd;
                denominator /= gcd;
            }
            this.numerator = numerator;
            this.denominator = denominator;
        }
    }
}
