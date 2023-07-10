import typing as t

from hypothesis import given

from tests.binding import (BigInteger,
                           Fractions)
from tests.utils import (equivalence,
                         is_fraction_valid)
from . import strategies


@given(strategies.fractions)
def test_basic(fraction: Fractions.Fraction) -> None:
    result = Fractions.Fraction.Abs(fraction)

    assert isinstance(result, Fractions.Fraction)
    assert is_fraction_valid(result)


@given(strategies.fractions)
def test_idempotence(fraction: Fractions.Fraction) -> None:
    result = Fractions.Fraction.Abs(fraction)

    assert result == Fractions.Fraction.Abs(result)


@given(strategies.fractions)
def test_positive_definiteness(fraction: Fractions.Fraction) -> None:
    result = Fractions.Fraction.Abs(fraction)

    assert equivalence(result.IsZero, fraction.IsZero)


@given(strategies.fractions)
def test_evenness(fraction: Fractions.Fraction) -> None:
    result = Fractions.Fraction.Abs(fraction)

    assert result == Fractions.Fraction.Abs(-fraction)


@given(strategies.fractions, strategies.fractions)
def test_multiplicativity(
        first: Fractions.Fraction,
        second: t.Union[BigInteger, Fractions.Fraction]
) -> None:
    result = Fractions.Fraction.Abs(first * second)

    assert (result
            == Fractions.Fraction.Abs(first) * Fractions.Fraction.Abs(second))


@given(strategies.fractions, strategies.fractions)
def test_triangle_inequality(first: Fractions.Fraction,
                             second: Fractions.Fraction) -> None:
    result = Fractions.Fraction.Abs(first + second)

    assert (result
            <= Fractions.Fraction.Abs(first) + Fractions.Fraction.Abs(second))


@given(strategies.fractions, strategies.fractions)
def test_op_Multiply_operand(first: Fractions.Fraction,
                             second: Fractions.Fraction) -> None:
    assert (Fractions.Fraction.Abs(first * second)
            == Fractions.Fraction.Abs(first) * Fractions.Fraction.Abs(second))


@given(strategies.fractions, strategies.non_zero_fractions)
def test_op_Division_operand(first: Fractions.Fraction,
                             second: Fractions.Fraction) -> None:
    assert (Fractions.Fraction.Abs(first / second)
            == Fractions.Fraction.Abs(first) / Fractions.Fraction.Abs(second))
