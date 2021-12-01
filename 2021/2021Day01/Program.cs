
/// <summary>
/// https://adventofcode.com/2021/day/1
/// count the number of times a depth measurement increases from the previous measurement. (There is no measurement    before the first measurement.) How many measurements are larger than the previous measurement?
/// </summary>

Console.WriteLine("Day 01 solver");

var deeperMoreCount = 0;
var deeperLessCount = 0;
var lastDepth = int.MaxValue;
foreach (string depthString in File.ReadLines(@"realData.txt"))
{
    //Console.WriteLine(depthString);
    if (int.TryParse(depthString, out int newDepth))
    { 
        if (newDepth > lastDepth)           
        {       
            deeperMoreCount++;
        }
        else
        {
            deeperLessCount++;
        }
        lastDepth = newDepth;
    }
    else
    {
        Console.WriteLine($"Error of parsing {depthString}");
        break;
    }
}
Console.WriteLine($"Answer is {deeperMoreCount} ");
Console.ReadLine();
