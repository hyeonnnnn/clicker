using UnityEngine;
using System;

public readonly struct Currency
{
    public readonly double Value;

    public Currency(double value)
    {
        if (value < 0)
        {
            throw new Exception("마이너스 값");
        }

        Value = value;
    }


    // 연산자 오버라이딩
    public static Currency operator +(Currency currency1, Currency currency2)
    {
        return new Currency(currency1.Value + currency2.Value);
    }

    public static Currency operator -(Currency currency1, Currency currency2)
    {
        return new Currency(currency1.Value - currency2.Value);
    }

    public static bool operator >(Currency currency1, Currency currency2)
    {
        return currency1.Value > currency2.Value;
    }

    public static bool operator <(Currency currency1, Currency currency2)
    {
        return currency1.Value < currency2.Value;
    }

    public static bool operator >=(Currency currency1, Currency currency2)
    {
        return currency1.Value >= currency2.Value;
    }

    public static bool operator <=(Currency currency1, Currency currency2)
    {
        return currency1.Value <= currency2.Value;
    }

    public static implicit operator Currency(double value)
    {
        return new Currency(value);
    }

    public static explicit operator double(Currency currency)
    {
        return currency.Value;
    }

    // 정해진 표기법 사용
    public override string ToString()
    {
        return Value.ToFormattedString();
    }
}
