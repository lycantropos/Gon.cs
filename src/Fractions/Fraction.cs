using System;
using System.Numerics;

namespace Fractions
{
    public readonly struct Fraction
        : IComparable<BigInteger>,
            IComparable<Fraction>,
            IComparable<int>,
            IEquatable<BigInteger>,
            IEquatable<Fraction>,
            IEquatable<int>
#if NET7_0_OR_GREATER
            ,
            IAdditionOperators<Fraction, BigInteger, Fraction>,
            IAdditionOperators<Fraction, Fraction, Fraction>,
            IDivisionOperators<Fraction, BigInteger, Fraction>,
            IDivisionOperators<Fraction, Fraction, Fraction>,
            IMultiplyOperators<Fraction, BigInteger, Fraction>,
            IMultiplyOperators<Fraction, Fraction, Fraction>,
            ISubtractionOperators<Fraction, BigInteger, Fraction>,
            ISubtractionOperators<Fraction, Fraction, Fraction>,
            IUnaryNegationOperators<Fraction, Fraction>,
            IUnaryPlusOperators<Fraction, Fraction>
#endif
    {
        public readonly BigInteger numerator;
        public readonly BigInteger denominator;

        public bool IsZero => numerator.IsZero;

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
            var (numerator, otherDenominator) = NormalizeComponentsModuli(
                self.numerator,
                other.denominator
            );
            var (otherNumerator, denominator) = NormalizeComponentsModuli(
                other.numerator,
                self.denominator
            );
            return new Fraction(numerator * otherNumerator, denominator * otherDenominator, false);
        }

        public static Fraction operator *(Fraction self, BigInteger other)
        {
            var (otherNormalized, denominator) = NormalizeComponentsModuli(other, self.denominator);
            return new Fraction(self.numerator * otherNormalized, denominator, false);
        }

        public static Fraction operator /(Fraction self, Fraction other)
        {
            var (numerator, otherNumerator) = NormalizeComponentsModuli(
                self.numerator,
                other.numerator
            );
            var (denominator, otherDenominator) = NormalizeComponentsModuli(
                self.denominator,
                other.denominator
            );
            var (resultNumerator, resultDenominator) = NormalizeComponentsSign(
                numerator * otherDenominator,
                denominator * otherNumerator
            );
            return new Fraction(resultNumerator, resultDenominator, false);
        }

        public static Fraction operator /(Fraction self, BigInteger other)
        {
            var (numerator, otherNormalized) = NormalizeComponentsModuli(self.numerator, other);
            var (resultNumerator, resultDenominator) = NormalizeComponentsSign(
                numerator,
                self.denominator * otherNormalized
            );
            return new Fraction(resultNumerator, resultDenominator, false);
        }

        public static Fraction operator -(Fraction self, Fraction other) =>
            new Fraction(
                self.numerator * other.denominator - other.numerator * self.denominator,
                self.denominator * other.denominator
            );

        public static Fraction operator -(Fraction self, BigInteger other) =>
            new Fraction(self.numerator - other * self.denominator, self.denominator);

        public static bool operator ==(Fraction self, BigInteger other) => self.Equals(other);

        public static bool operator ==(Fraction self, Fraction other) => self.Equals(other);

        public static bool operator ==(Fraction self, int other) => self.Equals(other);

        public static bool operator !=(Fraction self, BigInteger other) => !self.Equals(other);

        public static bool operator !=(Fraction self, Fraction other) => !self.Equals(other);

        public static bool operator !=(Fraction self, int other) => !self.Equals(other);

        public static bool operator <=(Fraction self, BigInteger other) =>
            self.CompareTo(other) <= 0;

        public static bool operator <=(Fraction self, Fraction other) => self.CompareTo(other) <= 0;

        public static bool operator <=(Fraction self, int other) => self.CompareTo(other) <= 0;

        public static bool operator >=(Fraction self, BigInteger other) =>
            self.CompareTo(other) >= 0;

        public static bool operator >=(Fraction self, Fraction other) => self.CompareTo(other) >= 0;

        public static bool operator >=(Fraction self, int other) => self.CompareTo(other) >= 0;

        public static bool operator <(Fraction self, BigInteger other) => self.CompareTo(other) < 0;

        public static bool operator <(Fraction self, Fraction other) => self.CompareTo(other) < 0;

        public static bool operator <(Fraction self, int other) => self.CompareTo(other) < 0;

        public static bool operator >(Fraction self, BigInteger other) => self.CompareTo(other) > 0;

        public static bool operator >(Fraction self, Fraction other) => self.CompareTo(other) > 0;

        public static bool operator >(Fraction self, int other) => self.CompareTo(other) > 0;

        public bool Equals(BigInteger other) => denominator.IsOne && numerator.Equals(other);

        public bool Equals(Fraction other) =>
            numerator.Equals(other.numerator) && denominator.Equals(other.denominator);

        public bool Equals(int other) => denominator.IsOne && numerator.Equals(other);

        public static Fraction Abs(Fraction self) =>
            new Fraction(BigInteger.Abs(self.numerator), self.denominator, false);

        public override bool Equals(object other) =>
            other is Fraction otherFraction && Equals(otherFraction);

        public override int GetHashCode()
        {
            var denominatorModularInverse = BigInteger.ModPow(
                denominator,
                HashModulus - 2,
                HashModulus
            );
            int result =
                denominatorModularInverse == BigInteger.Zero
                    ? HashInf
                    : (int)(BigInteger.Abs(numerator) * denominatorModularInverse % HashModulus);
            return numerator < BigInteger.Zero ? -result : result;
        }

        public override string ToString() => $"Fraction({numerator}, {denominator})";

        public int CompareTo(BigInteger other) => numerator.CompareTo(denominator * other);

        public int CompareTo(Fraction other) =>
            (numerator * other.denominator).CompareTo(denominator * other.numerator);

        public int CompareTo(int other) => numerator.CompareTo(denominator * other);

        private static readonly BigInteger HashModulus = 2147483647;
        private static readonly int HashInf = 314159;

        private Fraction(BigInteger numerator, BigInteger denominator, bool normalize)
        {
            if (denominator.IsZero)
            {
                throw new DivideByZeroException("Denominator should not be zero.");
            }
            if (normalize)
            {
                (numerator, denominator) = NormalizeComponentsSign(numerator, denominator);
                (numerator, denominator) = NormalizeComponentsModuli(numerator, denominator);
            }
            this.numerator = numerator;
            this.denominator = denominator;
        }

        private static (BigInteger, BigInteger) NormalizeComponentsModuli(
            BigInteger numerator,
            BigInteger denominator
        )
        {
            var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
            return (numerator / gcd, denominator / gcd);
        }

        private static (BigInteger, BigInteger) NormalizeComponentsSign(
            BigInteger numerator,
            BigInteger denominator
        ) => denominator < BigInteger.Zero ? (-numerator, -denominator) : (numerator, denominator);
    }
}
