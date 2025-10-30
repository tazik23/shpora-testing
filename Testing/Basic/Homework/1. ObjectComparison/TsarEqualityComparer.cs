using FluentAssertions;
using HomeExercise.Tasks.ObjectComparison;

namespace Basic.Homework._1._ObjectComparison;

public static class TsarEqualityComparer
{
    public static void CheckTsarEquality(Person actualTsar, Person expectedTsar)
    {
        actualTsar.Should().BeEquivalentTo(expectedTsar, o => o
            .Excluding(m => m.Name.Equals(nameof(Person.Id)) && m.DeclaringType == typeof(Person))
            .IgnoringCyclicReferences()
            .AllowingInfiniteRecursion());
    }
}