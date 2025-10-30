using FluentAssertions;
using HomeExercise.Tasks.ObjectComparison;
using NUnit.Framework;

namespace HomeExercise.Tasks.TsarTests;

public class TsarTests
{
    private static void CheckTsarEquality(Person actualTsar, Person expectedTsar)
    {
        actualTsar.Should().BeEquivalentTo(expectedTsar, o => o
            .ExcludingMembersNamed(nameof(Person.Id))
            .IgnoringCyclicReferences()
            .AllowingInfiniteRecursion());
    }
    
    [Test]
    public void CheckTsarEquality_WithValidTsar_ShouldNotThrow()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));
        
        var act = () => CheckTsarEquality(actualTsar, expectedTsar);
        
        act.Should().NotThrow();
    }

    [Test]
    public void CheckTsarEquality_WithCyclicDependency_ShouldNotThrow()
    {
        var actualTsar = TsarRegistry.GetCurrentTsarWithCyclicDependency();
        
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70, null);
        var expectedParent = new Person("Vasili III of Russia", 28, 170, 60, expectedTsar);
        expectedTsar.Parent = expectedParent;

        var act = () => CheckTsarEquality(actualTsar, expectedTsar);
        
        act.Should().NotThrow();
    }

    [Test]
    public void CheckTsarEquality_WithParentNullInOneObject_ShouldThrow()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70, null);
        
        var act = () => CheckTsarEquality(actualTsar, expectedTsar);
        
        act.Should().Throw<AssertionException>();
    }
    
    [Test]
    public void CheckTsarEquality_WithDifferentChainLength_ShouldThrow()
    {
        var actualTsar = TsarRegistry.GetCurrentTsarWithAncestryChain(5);
        var expectedTsar = TsarRegistry.GetCurrentTsarWithAncestryChain(3);

        var act = () => CheckTsarEquality(actualTsar, expectedTsar);

        act.Should().Throw<AssertionException>();
    }
    
    
    [TestCase(5, 3, TestName = "WrongAncestorInMiddleOfChain")]
    [TestCase(5, 1, TestName = "WrongAncestorAtStartOfChain")]
    [TestCase(5, 5, TestName = "WrongAncestorAtEndOfChain")]
    [TestCase(100, 50, TestName = "WrongAncestorLongChain")]
    public void CheckTsarEquality_WithWrongAncestorInChain_ShouldThrow(int generations, int wrongAncestorLevel)
    {
        var actualTsar = TsarRegistry.GetCurrentTsarWithAncestryChain(generations);
        var expectedTsar = TsarRegistry.GetTsarWithWrongAncestor(generations, wrongAncestorLevel);

        var act = () => CheckTsarEquality(actualTsar, expectedTsar);

        act.Should().Throw<AssertionException>();
    }
}