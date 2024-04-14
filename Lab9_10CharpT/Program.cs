using System;
using System.Collections.Generic;

public class InvalidColorException : Exception
{
    public InvalidColorException() { }
    public InvalidColorException(string message) : base(message) { }
    public InvalidColorException(string message, Exception inner) : base(message, inner) { }
}

public class InvalidTriangleCastException : Exception
{
    public InvalidTriangleCastException() { }
    public InvalidTriangleCastException(string message) : base(message) { }
    public InvalidTriangleCastException(string message, Exception inner) : base(message, inner) { }
}

public class ITriangle
{
    protected int a, b;
    protected int c;
    public ITriangle(int Base, int side, int color)
    {
        a = Base;
        b = side;
        c = color;
    }

    public void Print()
    {
        Console.WriteLine($"Base: {a}");
        Console.WriteLine($"side: {b}");
    }
    public float P()
    {
        return a + b * 2;
    }

    public float S()
    {
        double p = P() / 2.0;
        float square = (float)Math.Sqrt(p * (p - a) * (p - a) * (p - b));
        if (float.IsNaN(square))
        {
            throw new InvalidTriangleCastException("Invalid triangle parameters: sides are not compatible for triangle construction.");
        }
        return square;
    }

    public bool EquilateralTriangle()
    {
        return a == b;
    }

    public int Side
    {
        get { return b; }
        set { b = value; }
    }
    public int Base
    {
        get { return a; }
        set { a = value; }
    }

    public int Color
    {
        get { return c; }
    }
}



public class DerivedTriangle : ITriangle
{
    public DerivedTriangle(int Base, int side, int color) : base(Base, side, color)
    {
        try
        {
            object test = (object)this;
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine($"Invalid Cast: {ex.Message}");
        }
    }
}

public delegate void BattleActionHandler(object sender, BattleEventArgs e);

public class Warrior
{
    public string Name { get; }

    public event BattleActionHandler BattleAction;

    public Warrior(string name)
    {
        Name = name;
    }

    public void Attack(Warrior opponent)
    {
        Console.WriteLine($"{Name} attacks {opponent.Name}!");
        OnBattleAction(new BattleEventArgs(BattleActionType.Attack, this, opponent));
    }

    public void Defend(Warrior opponent)
    {
        Console.WriteLine($"{Name} defends against {opponent.Name}'s attack!");
        OnBattleAction(new BattleEventArgs(BattleActionType.Defend, this, opponent));
    }

    protected virtual void OnBattleAction(BattleEventArgs e)
    {
        BattleAction?.Invoke(this, e);
    }
}

public enum BattleActionType
{
    Attack,
    Defend
}

public class BattleEventArgs : EventArgs
{
    public BattleActionType Action { get; }
    public Warrior Initiator { get; }
    public Warrior Target { get; }

    public BattleEventArgs(BattleActionType action, Warrior initiator, Warrior target)
    {
        Action = action;
        Initiator = initiator;
        Target = target;
    }
}



class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Lab#9  or  Lab#10");
        Console.WriteLine("What task do you want?");
        Console.WriteLine("1. Task 1");
        Console.WriteLine("2. Task 2");
        Console.WriteLine("5. Exit");

        int choice;
        bool isValidChoice = false;

        do
        {
            Console.Write("Enter number of task ");
            isValidChoice = int.TryParse(Console.ReadLine(), out choice);

            if (!isValidChoice || choice < 1 || choice > 4)
            {
                Console.WriteLine("This task not exist");
                isValidChoice = false;
            }
        } while (!isValidChoice);
        switch (choice)
        {
            case 1:
                task1();
                break;
            case 2:
                task2();
                break;
            case 3:
                break;
        }
    }

    static void task1()
    {
        Console.WriteLine("Task 1");
        List<ITriangle> triangles = new List<ITriangle>();
        triangles.Add(new ITriangle(3, -5, 6));
        triangles.Add(new ITriangle(4, 4, 4));
        triangles.Add(new ITriangle(-2, 7, 9));
        triangles.Add(new ITriangle(8, 8, 1));
        triangles.Add(new ITriangle(1, 1, -1));
        foreach (var t in triangles)
        {
            try
            {
                Console.WriteLine();
                t.Print();
                Console.WriteLine($"P = {t.P()}");
                Console.WriteLine($"S = {t.S()}");
                Console.WriteLine($"Equilateral triangle? = {t.EquilateralTriangle()}");

                if (t.Color < 0 || t.Color > 255)
                {
                    throw new InvalidColorException("Invalid color value: it must be in the range of 0 to 255.");
                }
            }
            catch (InvalidTriangleCastException ex)
            {
                Console.WriteLine($"Invalid Triangle: {ex.Message}");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Invalid Cast: {ex.Message}");
            }
            catch (InvalidColorException ex)
            {
                Console.WriteLine($"Invalid Color: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    static void task2()
    {
        // Створюємо двох воїнів для тестування
        Warrior warrior1 = new Warrior("Warrior 1");
        Warrior warrior2 = new Warrior("Warrior 2");

        // Підписуємося на події кожного воїна
        warrior1.BattleAction += HandleBattleAction;
        warrior2.BattleAction += HandleBattleAction;

        // Проводимо декілька раундів битви
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"Round {i + 1} begins!");
            // Випадково вибираємо, який воїн атакує, а який обороняється
            if (i % 2 == 0)
            {
                warrior1.Attack(warrior2);
            }
            else
            {
                warrior2.Attack(warrior1);
            }
            Console.WriteLine();
        }
    }

    static void HandleBattleAction(object sender, BattleEventArgs e)
    {
        if (e.Action == BattleActionType.Attack)
        {
            Console.WriteLine($"{e.Initiator.Name}'s attack!");
            e.Target.Defend(e.Initiator);
        }
        else if (e.Action == BattleActionType.Defend)
        {
            Console.WriteLine($"{e.Target.Name}'s defense!");
        }
    }
}
