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
    
    public void ElevatorRequest(int floor)
    {
        // This is where a more dynamic solution would be warrented.
        // This will always return up since the elevator starts at 1
        var elevatorDirection = floor >= CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
        _floorQueue.Add(new FloorRequest(floor, elevatorDirection));
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

        var initialDirection = _floorQueue.First().direction;

        var initialElevatorDirection = _floorQueue.Where(x => x.direction == initialDirection).ToList();

        var maxValue = initialElevatorDirection.Max(x => x.number);
        var minValue = initialElevatorDirection.Min(x => x.number);
        
        
        foreach (var floor in initialElevatorDirection)
        {
            MoveToFloor(floor.number);
            _floorQueue.Remove(floor);
            
            // Can I state that this is always max value?
            if (floor.number == maxValue)
            {
                var secondElevatorDirection = _floorQueue.Where(x => x.direction != initialDirection).ToList();
                 
                foreach (var oppositeFloor in secondElevatorDirection)
                {
                    MoveToFloor(oppositeFloor.number);
                    _floorQueue.Remove(floor);
                    
                    // Can I state that this is always min value?
                    if (floor.number == minValue)
                    {
                        break;
                    }
                }
            }
        }
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