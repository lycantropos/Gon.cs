import typing as t

from hypothesis import given

from tests.binding import (Multipolygon,
                           Polygon)
from . import strategies


@given(strategies.polygons_sequences)
def test_basic(polygons: t.Sequence[Polygon]) -> None:
    result = Multipolygon(polygons)

    assert isinstance(result, Multipolygon)
    assert result.polygons == polygons
