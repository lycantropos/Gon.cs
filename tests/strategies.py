from hypothesis import strategies as _st

scalars = _st.integers() | _st.fractions() | _st.floats(allow_infinity=False,
                                                        allow_nan=False)
