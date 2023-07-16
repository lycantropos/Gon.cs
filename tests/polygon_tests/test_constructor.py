import typing as t

from hypothesis import given

from tests.binding import (Contour,
                           Polygon)
from . import strategies


@given(strategies.borders_with_holes)
def test_basic(
        border_with_holes: t.Tuple[Contour, t.Sequence[Contour]]
) -> None:
    border, holes = border_with_holes

    result = Polygon(border, holes)

    assert isinstance(result, Polygon)
    assert result.border == border
    assert result.holes == holes


@given(strategies.contours)
def test_no_holes(contour: Contour) -> None:
    result = Polygon(contour)

    assert isinstance(result, Polygon)
    assert result.border == contour
    assert result.holes == []
