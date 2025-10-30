using Basic.Homework._1._ObjectComparison;
using FluentAssertions;
using HomeExercise.Tasks.ObjectComparison;
using NUnit.Framework;

namespace HomeExercise.Tasks.TsarTests;

public class TsarTests
{
    [Test]
    public void CheckTsarEquality_WithValidTsar_ShouldNotThrow()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null, new City(1, "Peterburg")),
            new City(1, "Peterburg"));
        
        var act = () => TsarEqualityComparer.CheckTsarEquality(actualTsar, expectedTsar);
        
        act.Should().NotThrow();
    }

    [Test]
    public void CheckTsarEquality_WithCyclicDependency_ShouldNotThrow()
    {
        var actualTsar = TsarRegistry.GetCurrentTsarWithCyclicDependency();
        
        var expectedTsar =
            new Person("Ivan IV The Terrible", 54, 170, 70, null, new City(1, "Peterburg"));
        var expectedParent =
            new Person("Vasili III of Russia", 28, 170, 60, expectedTsar, new City(1, "Peterburg"));
        expectedTsar.Parent = expectedParent;

        var act = () => TsarEqualityComparer.CheckTsarEquality(actualTsar, expectedTsar);
        
        act.Should().NotThrow();
    }

    [Test]
    public void CheckTsarEquality_WithParentNullInOneObject_ShouldThrow()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar =
            new Person("Ivan IV The Terrible", 54, 170, 70, null, new City(1, "Peterburg"));
        
        var act = () => TsarEqualityComparer.CheckTsarEquality(actualTsar, expectedTsar);
        
        act.Should().Throw<AssertionException>();
    }
    
    [Test]
    public void CheckTsarEquality_WithDifferentChainLength_ShouldThrow()
    {
        var actualTsar = TsarRegistry.GetCurrentTsarWithAncestryChain(5);
        var expectedTsar = TsarRegistry.GetCurrentTsarWithAncestryChain(3);

        var act = () => TsarEqualityComparer.CheckTsarEquality(actualTsar, expectedTsar);

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

        var act = () => TsarEqualityComparer.CheckTsarEquality(actualTsar, expectedTsar);

        act.Should().Throw<AssertionException>();
    }
    
    [Test]
    public void CheckTsarEquality_WithDifferentCityIds_ShouldThrow()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null, new City(2, "Peterburg")),
            new City(1, "Peterburg"));
        
        var act = () => TsarEqualityComparer.CheckTsarEquality(actualTsar, expectedTsar);
        
        act.Should().Throw<AssertionException>();
    }
    
    [Test]
    public void CheckTsarEquality_WithDifferentIds_ShouldThrow()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        
        var expectedTsar = TsarRegistry.GetCurrentTsar();
        expectedTsar.Id = 2;
        expectedTsar.City = new City(2, "Peterburg");
        
        var act = () => TsarEqualityComparer.CheckTsarEquality(actualTsar, expectedTsar);
        
        act.Should().Throw<AssertionException>();
    }
    
}

/*
 * Что хорошо в решении:
 * 1.) Тест легче читать: 3 строчки vs найти метод, залезть в него и посмотреть: а что он там делает
 * 2.) Явно указываем, какие поля мы исключаем из проверки
 * 3.) При добавлении новых полей в Person нужно провести минимальный рефакторинг(исключить из проверки
 *     или вообще ничего не трогать)
 * 4.) Падаем с адекватной информацией об ошибке, в отличие от просто непонятного False
 * 5.) Проверяем всю династию(не только родителя)
 * 6.) Прошлое решение падало при циклической зависимости(StackOverflow), мое решение корректно обрабатывает этот случай
 */