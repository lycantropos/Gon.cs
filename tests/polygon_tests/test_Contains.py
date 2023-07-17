from hypothesis import given

from tests.binding import Polygon
from . import strategies


@given(strategies.polygons)
def test_vertices(polygon: Polygon) -> None:
    assert all(vertex in polygon for vertex in polygon.border.vertices)
    assert all(vertex in polygon
               for hole in polygon.holes
               for vertex in hole.vertices)
