from fractions import Fraction

from hypothesis import given

from tests.binding import Box
from tests.hints import Scalar
from . import strategies


@given(strategies.scalars, strategies.scalars, strategies.scalars,
       strategies.scalars)
def test_basic(
        min_x: Scalar, max_x: Scalar, min_y: Scalar, max_y: Scalar
) -> None:
    result = Box(min_x, max_x, min_y, max_y)

    assert isinstance(result, Box)
    assert isinstance(result.min_x, Fraction)
    assert isinstance(result.max_x, Fraction)
    assert isinstance(result.min_y, Fraction)
    assert isinstance(result.max_y, Fraction)
    assert result.min_x == min_x
    assert result.max_x == max_x
    assert result.min_y == min_y
    assert result.max_y == max_y
