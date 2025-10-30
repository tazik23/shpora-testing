using Basic.Homework._1._ObjectComparison;

namespace HomeExercise.Tasks.ObjectComparison;


public class Person
{
    public static int IdCounter = 0;
    public int Age, Height, Weight;
    public string Name;
    public Person Parent;
    public int Id;
    public City City;

    public Person(string name, int age, int height, int weight, Person parent, City city)
    {
        Id = IdCounter++;
        Name = name;
        Age = age;
        Height = height;
        Weight = weight;
        Parent = parent;
        City = city;
    }
}