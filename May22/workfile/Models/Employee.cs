namespace Delegates.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public float Salary { get; set; }

        public Employee(int id, int age, string name, float salary)
        {
            Id = id;
            Age = age;
            Name = name;
            Salary = salary;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Age: {Age}, Salary: {Salary}";
        }
    }
}
