using ElevatorExample.Mocking;

namespace ElevatorExample;

internal sealed class Elevator
{
    private IConsoleWriter Console;
    
    private int TopFloor = 5;
    private int BottomFloor = 1;
    private int CurrentFloor = 1;
    private List<FloorRequest> _floorQueue = new();

    public Elevator()
    {
        Console = new ConsoleWriter();
    }
    
    public Elevator(IConsoleWriter consoleWriter)
    {
        Console = consoleWriter;
    }

    public void FloorRequestElevator(int floor, ElevatorDirection direction)
    {
        if (floor > TopFloor || floor < BottomFloor)
        {
            Console.WriteLine($"That floor does not exist.  The floor range is from {BottomFloor} to {TopFloor}.");
            return;
        }
        
        _floorQueue.Add(new FloorRequest(floor, direction));

    }
    
    private void OpenDoor()
    {
        Console.WriteLine("Opening Door");
        CloseDoor();
    }

    private void CloseDoor()
    {
        Console.WriteLine("Closing Door");
    }

    public void StartElevator()
    {
        if (_floorQueue.Count == 0)
        {
            Console.WriteLine("You need to add floors first!");
        }

        foreach (var floor in _floorQueue)
        {
            MoveToFloor(floor.number);
        }
        
        _floorQueue.Clear();
    }
    
    private void MoveToFloor(int floor)
    {
        Console.WriteLine($"Moving To Floor {floor}");

        while (floor != CurrentFloor)
        {

            if (floor < CurrentFloor)
            {
                CurrentFloor--;
            }
            else if (floor > CurrentFloor)
            {
                CurrentFloor++;
            }

            if(floor != CurrentFloor)
                Console.WriteLine($"Passing Floor {CurrentFloor}");
        }
       
        Console.WriteLine($"Arrived At Floor {CurrentFloor}");
        
        OpenDoor();
    }
}