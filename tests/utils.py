from ground.base import get_context

from tests.binding import (BigInteger,
                           Contour,
                           Fractions,
                           Point,
                           Segment)

context = get_context().replace(contour_cls=Contour,
                                segment_cls=Segment,
                                point_cls=Point)


def equivalence(left: bool, right: bool) -> bool:
    return left is right


def implication(antecedent: bool, consequent: bool) -> bool:
    return not antecedent or consequent


def is_fraction_valid(value: Fractions.Fraction) -> bool:
    return (value.denominator > 0
            and BigInteger.GreatestCommonDivisor(value.numerator,
                                                 value.denominator).IsOne)
