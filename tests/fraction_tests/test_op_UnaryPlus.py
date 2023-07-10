from hypothesis import given

from tests.binding import Fractions
from . import strategies


@given(strategies.fractions)
def test_basic(fraction: Fractions.Fraction) -> None:
    result = +fraction

    assert isinstance(result, Fractions.Fraction)


@given(strategies.fractions)
def test_identity(fraction: Fractions.Fraction) -> None:
    result = +fraction

    assert result == fraction
