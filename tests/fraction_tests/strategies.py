from hypothesis import strategies as _st

from tests.binding import (BigInteger as _BigInteger,
                           Fractions as _Fractions)

zero_big_integers = _st.just(_BigInteger(0))
big_integers = numerators = _st.integers().map(_BigInteger)
negative_big_integers = _st.integers(max_value=-1).map(_BigInteger)
positive_big_integers = _st.integers(min_value=1).map(_BigInteger)
denominators = non_zero_big_integers = (negative_big_integers
                                        | positive_big_integers)
zero_fractions = _st.builds(_Fractions.Fraction, zero_big_integers)
fractions = _st.builds(_Fractions.Fraction, numerators, denominators)
non_zero_fractions = _st.builds(_Fractions.Fraction, non_zero_big_integers,
                                denominators)
rationals = big_integers | fractions
non_zero_rationals = non_zero_big_integers | non_zero_fractions
zero_rationals = zero_big_integers | zero_fractions
