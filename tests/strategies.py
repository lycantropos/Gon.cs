import sys as _sys

from hypothesis import strategies as _st
from hypothesis_geometry import planar as _planar

from tests.utils import context as _context

MAX_VALUE = 10 ** 50
MIN_VALUE = -MAX_VALUE
scalars = (_st.integers(MIN_VALUE, MAX_VALUE)
           | _st.fractions(MIN_VALUE, MAX_VALUE,
                           max_denominator=MAX_VALUE)
           | _st.floats(MIN_VALUE, MAX_VALUE,
                        allow_infinity=False,
                        allow_nan=False))
points = _planar.points(scalars,
                        context=_context)
indices = _st.integers(0, _sys.maxsize)
