from itertools import product

from hypothesis import given

from tests.binding import (Orientation,
                           Point,
                           orient)
from tests.utils import (is_even_permutation,
                         permute)
from . import strategies


@given(strategies.points, strategies.points, strategies.points)
def test_basic(vertex: Point,
               first_ray_point: Point,
               second_ray_point: Point) -> None:
    result = orient(vertex, first_ray_point, second_ray_point)

    assert isinstance(result, Orientation)


@given(strategies.points, strategies.points)
def test_same_endpoints(first: Point, second: Point) -> None:
    assert [
               (vertex, first_ray_point, second_ray_point)
               for vertex, first_ray_point, second_ray_point
               in product((first, second),
                          repeat=3)
               if (orient(vertex, first_ray_point, second_ray_point)
                   is not Orientation.COLLINEAR)
           ] == []


@given(strategies.points, strategies.points, strategies.points,
       strategies.indices)
def test_permutations(vertex: Point,
                      first_ray_point: Point,
                      second_ray_point: Point,
                      index: int) -> None:
    points_triplet = vertex, first_ray_point, second_ray_point

    result = orient(vertex, first_ray_point, second_ray_point)

    assert (orient(*permute(points_triplet, index))
            is (result
                if is_even_permutation(index, len(points_triplet))
                else Orientation(-result)))
