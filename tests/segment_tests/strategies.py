from operator import attrgetter

from hypothesis_geometry import planar

from tests import strategies as _strategies
from tests.utils import context

scalars = _strategies.scalars
segments = planar.segments(scalars,
                           context=context)
segments_endpoints = segments.map(attrgetter('start', 'end'))
