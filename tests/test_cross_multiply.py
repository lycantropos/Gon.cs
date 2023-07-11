from fractions import Fraction

from hypothesis import given

from tests.binding import (Point,
                           cross_multiply)
from . import strategies


@given(strategies.points, strategies.points, strategies.points,
       strategies.points)
def test_basic(first_start: Point,
               first_end: Point,
               second_start: Point,
               second_end: Point) -> None:
    result = cross_multiply(first_start, first_end, second_start, second_end)

    assert isinstance(result, Fraction)


@given(strategies.points, strategies.points)
def test_same_endpoints(first_start: Point, first_end: Point) -> None:
    assert not cross_multiply(first_start, first_end, first_start,
                              first_end)


@given(strategies.points, strategies.points, strategies.points,
       strategies.points)
def test_segments_permutation(first_start: Point,
                              first_end: Point,
                              second_start: Point,
                              second_end: Point) -> None:
    result = cross_multiply(first_start, first_end, second_start, second_end)

    assert result == -cross_multiply(second_start, second_end, first_start,
                                     first_end)
