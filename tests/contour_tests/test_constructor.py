import typing as t

from hypothesis import given

from tests.binding import (Contour,
                           Point)
from . import strategies


@given(strategies.contours_vertices)
def test_basic(vertices: t.Sequence[Point]) -> None:
    result = Contour(vertices)

    assert isinstance(result, Contour)
    assert result.vertices == vertices
