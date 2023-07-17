from hypothesis import given

from tests.binding import (Location,
                           Polygon)
from . import strategies


@given(strategies.polygons)
def test_vertices(polygon: Polygon) -> None:
    assert all(polygon.locate(vertex) is Location.BOUNDARY
               for vertex in polygon.border.vertices)
    assert all(polygon.locate(vertex) is Location.BOUNDARY
               for hole in polygon.holes
               for vertex in hole.vertices)
