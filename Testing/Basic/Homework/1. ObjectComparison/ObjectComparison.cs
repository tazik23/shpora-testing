using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.ObjectComparison;

public class ObjectComparison
{
    
    
    
    [Test]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        actualTsar.Should().BeEquivalentTo(expectedTsar, o => o
            .ExcludingMembersNamed(nameof(Person.Id))
            .IgnoringCyclicReferences());
    }

    [Test]
    public void CheckCurrentTsarWithCyclicDependency()
    {
        var actualTsar = TsarRegistry.GetCurrentTsarWithCyclicDependency();
        
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70, null);
        var expectedParent = new Person("Vasili III of Russia", 28, 170, 60, expectedTsar);
        expectedTsar.Parent = expectedParent;

        actualTsar.Should().BeEquivalentTo(expectedTsar, o => o
            .ExcludingMembersNamed(nameof(Person.Id))
            .IgnoringCyclicReferences());
    }

    [Test]
    public void CheckCurrentTsar_WhenParentNullInOneObject()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70, null);
        
        var act = () => actualTsar.Should().BeEquivalentTo(expectedTsar, o => o
            .ExcludingMembersNamed(nameof(Person.Id))
            .IgnoringCyclicReferences());
        
        act.Should().Throw<AssertionException>();
    }
    
    [Test]
    public void CheckCurrentTsar_WithDifferentAncestorChains_ShouldFail()
    {
        var actualFather = new Person("Vasili III of Russia", 28, 170, 60, 
            new Person("Ivan III", 65, 175, 80, null));
        
        var actualTsar = new Person("Ivan IV The Terrible", 54, 170, 70, actualFather);

        var expectedFather = new Person("Vasili III of Russia", 28, 170, 60, 
            new Person("Different Ancestor", 65, 175, 80, null)); 
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70, expectedFather);
        
        var act = () => actualTsar.Should().BeEquivalentTo(expectedTsar, o => o
            .ExcludingMembersNamed(nameof(Person.Id))
            .IgnoringCyclicReferences());
        
        act.Should().Throw<AssertionException>();
    }

}