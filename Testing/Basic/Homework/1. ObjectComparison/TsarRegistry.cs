using Basic.Homework._1._ObjectComparison;

namespace HomeExercise.Tasks.ObjectComparison;

public class TsarRegistry
{
    public static Person GetCurrentTsar()
    {
        return new Person(
            "Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null, new City(1, "Peterburg")),
            new City(1, "Peterburg"));
    }
    
    public static Person GetCurrentTsarWithCyclicDependency()
    {
        var tsar = new Person("Ivan IV The Terrible", 54, 170, 70, null, new City(1, "Peterburg"));
        var parent = new Person("Vasili III of Russia", 28, 170, 60, tsar, new City(1, "Peterburg")); 
        tsar.Parent = parent;

        return tsar;
    }
    
    public static Person GetCurrentTsarWithAncestryChain(int generations)
    {
        Person current = null!;
        
        for (int i = generations; i > 0; i--)
        {
            current = new Person($"Ancestor {i}", 40 + i * 5, 170 + i, 65 + i, current, new City(1, "Peterburg"));
        }
        
        var tsar = new Person("Ivan IV The Terrible", 54, 170, 70, current, new City(1, "Peterburg"));
    
        return tsar;
    }
    
    public static Person GetTsarWithWrongAncestor(int generations, int wrongAncestorLevel)
    {
        Person current = null!;
        
        for (int i = generations; i > 0; i--)
        {
            current = new Person($"Ancestor {i}", 40 + i * 5, 170 + i, 65 + i, current, new City(1, "Peterburg"));
            
            if (i == wrongAncestorLevel)
            {
                current.Name = "Imposter";
            }
        }
        
        var tsar = new Person("Ivan IV The Terrible", 54, 170, 70, current, new City(1, "Peterburg"));
    
        return tsar;
    }
}