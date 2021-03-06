﻿#nowarn "67"
namespace Funcy.Test

open Funcy
open Persimmon
open UseTestNameByReflection

module MaybeTest =
    let ``Maybe.Some creates Some<T> instance`` = test {
        let some = Maybe.Some(1)
        do! assertEquals typeof<Some<int>> <| some.GetType()
    }
    let ``Maybe.None creates None<T> instance`` = test {
        let none = Maybe<string>.None()
        do! assertEquals typeof<None<string>> <| none.GetType()
    }
    let ``Some<T>.Value should return its value`` = test {
        let some = Maybe.Some(2.5)
        do! assertEquals 2.5 some.Value
    }
    let ``Some<T>.IsSome should return true`` = test {
        let some = Maybe.Some("hoge")
        do! assertPred some.IsSome
    }
    let ``Some<T>.IsNone should return false`` = test {
        let some = Maybe.Some(3.14m)
        do! assertPred <| not some.IsNone
    }
    let ``When Some<T> as Maybe<T> then IsSome should return true`` = test {
        let some = Maybe.Some(-1.0f) :> Maybe<float32>
        do! assertPred some.IsSome
    }
    let ``When Some<T> as Maybe<T> then IsNone should return false`` = test {
        let some = Maybe.Some(obj()) :> Maybe<obj>
        do! assertPred <| not some.IsNone
    }
    let ``When Some<T> as Maybe<T> then ToSome should return ISome<T> instance`` = test {
        let some = Maybe.Some(-1) :> Maybe<int>
        do! assertPred (some.ToSome() :? Some<int>)
    }
    let ``When Some<T> as Maybe<T> then ToNone should raise InvalidCastException`` = test {
        let some = Maybe.Some("egg") :> Maybe<string>
        let! e = trap { some.ToNone() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``None<T>.IsSome should return false`` = test {
        let none = Maybe<int>.None()
        do! assertPred <| not none.IsSome
    }
    let ``None<T>.IsNone should return true`` = test {
        let none = Maybe<int>.None()
        do! assertPred none.IsNone
    }
    let ``When None<T> as Maybe<T> then IsSome should return false`` = test {
        let none = Maybe<float>.None() :> Maybe<float>
        do! assertPred <| not none.IsSome
    }
    let ``When None<T> as Maybe<T> then IsNone should return true`` = test {
        let none = Maybe<decimal>.None() :> Maybe<decimal>
        do! assertPred none.IsNone
    }
    let ``When None<T> as Maybe<T> then ToSome should raise InvalidCastException`` = test {
        let none = Maybe<int list>.None() :> Maybe<int list>
        let! e = trap { none.ToSome() |> ignore }
        do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
    }
    let ``When None<T> as Maybe<T> then ToNone should return INone<T> instance`` = test {
        let none = Maybe<bool>.None() :> Maybe<bool>
        do! assertPred (none.ToNone() :? None<bool>)
    }
    let ``Some<T> should have equality1`` = test {
        let some = Maybe.Some(5)
        let other = Maybe.Some(5)
        do! assertEquals some other
        do! assertPred (some = other)
        do! assertEquals <|| (some.GetHashCode(), other.GetHashCode())
    }
    let ``Some<T> should have equality2`` = test {
        let some = Maybe.Some("hoge")
        let other = Maybe.Some("fuga")
        do! assertNotEquals some other
    }
    let ``Some<T> should have equality3`` = test {
        let some = Maybe.Some(3.14)
        let other = Maybe.Some(3.14f)
        do! (not >> assertPred) <| some.Equals(other)
    }
    let ``None<T> should have equality1`` = test {
        let none : None<float> = Maybe.None()
        let other : None<float> = Maybe.None()
        do! assertEquals none other
        do! assertEquals <|| (none.GetHashCode(), other.GetHashCode())
    }
    let ``None<T> should have equality2`` = test {
        let none : None<float32> = Maybe.None()
        let other : None<decimal> = Maybe.None()
        do! (not >> assertPred) <| none.Equals(other)
    }
    let ``Maybe<T> should have equality`` = test {
        let some1 = Maybe.Some(System.DateTime(2015, 7, 10)) :> Maybe<System.DateTime>
        let some2 = Maybe.Some(System.DateTime(2015, 7, 10)) :> Maybe<System.DateTime>
        let some3 = Maybe.Some(System.DateTime(2015, 7, 11)) :> Maybe<System.DateTime>
        let none = Maybe.None() :> Maybe<System.DateTime>
        do! assertEquals some1 some2
        do! assertEquals <|| (some1.GetHashCode(), some2.GetHashCode())
        do! assertNotEquals some1 some3
        do! assertNotEquals some1 none
        do! assertNotEquals some3 none
    }
    let ``Maybe<T> should have comparability`` = test {
        let some1 = Maybe.Some(1) :> Maybe<int>
        let some2 = Maybe.Some(2) :> Maybe<int>
        let none = Maybe.None() :> Maybe<int>
        do! assertPred (some1 < some2)
        do! assertPred (some2 > some1)
        do! assertPred (none < some1)
        do! assertPred (none < some2)
    }
