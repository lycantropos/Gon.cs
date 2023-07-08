from ground.base import get_context

from tests.binding import (Contour,
                           Point,
                           Segment)

context = get_context().replace(contour_cls=Contour,
                                segment_cls=Segment,
                                point_cls=Point)
