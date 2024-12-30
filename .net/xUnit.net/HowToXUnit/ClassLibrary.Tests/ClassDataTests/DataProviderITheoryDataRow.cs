﻿namespace ClassLibrary.Tests.ClassDataTests;

using System.Collections;

public class DataProviderITheoryDataRow : IEnumerable<ITheoryDataRow>
{
    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<ITheoryDataRow> GetEnumerator()
    {
        yield return new TheoryDataRow(["foo", 5, new Data("bar")]);
        yield return new TheoryDataRow(["foobar", 5, new Data("baz")]);
    }

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}