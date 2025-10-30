namespace HomeExercise.Tasks.ObjectComparison;

public class TsarRegistry
{
    public static Person GetCurrentTsar()
    {
        return new Person(
            "Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));
    }
    
    public static Person GetCurrentTsarWithCyclicDependency()
    {
        var tsar = new Person("Ivan IV The Terrible", 54, 170, 70, null);
        var parent = new Person("Vasili III of Russia", 28, 170, 60, tsar); 
        tsar.Parent = parent;

        return tsar;
    }
    
    public static Person GetCurrentTsarWithAncestryChain(int generations)
    {
        Person current = null!;
        
        for (int i = generations; i > 0; i--)
        {
            current = new Person($"Ancestor {i}", 40 + i * 5, 170 + i, 65 + i, current);
        }
        
        var tsar = new Person("Ivan IV The Terrible", 54, 170, 70, current);
    
        return tsar;
    }
}