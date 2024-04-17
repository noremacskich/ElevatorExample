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

    /// <summary>
    /// The user is on a floor requesting elevator access.  On the first and last floors, it won't matter what direction
    /// they choose, as the floor selection itself will be handled in the Elevator request
    /// </summary>
    /// <param name="floor">A value between 1 and 5</param>
    /// <param name="direction">The Direction they wish to go</param>
    public void FloorRequest(int floor, ElevatorDirection direction)
    {
        if (floor > TopFloor || floor < BottomFloor)
        {
            Console.WriteLine($"That floor does not exist.  The floor range is from {BottomFloor} to {TopFloor}.");
            return;
        }
        
        _floorQueue.Add(new FloorRequest(floor, direction));

    }
    
    /// <summary>
    /// This is basically identical to the above function, only difference is that you need to be aware of what floor
    /// you are on when the request is made, to determine the direction of the request.  (Yet to be implemented)
    /// </summary>
    /// <param name="floor">The floor the user is requesting</param>
    public void ElevatorRequest(int floor)
    {
        if (floor > TopFloor || floor < BottomFloor)
        {
            Console.WriteLine($"That floor does not exist.  The floor range is from {BottomFloor} to {TopFloor}.");
            return;
        }
        
        // This logic would work if the elevator was actively running, but since I'm loading in requests up front, this 
        // needs to be moved into the Start Elevator function
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

    /// <summary>
    /// I'm considering getting a background task in place to run the elevator and allow live inputs to be out of scope
    /// for this excercise
    /// </summary>
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
            
            // TODO: Can I state that this is always max value? - Gut Says no with addition of Elevator Request
            if (floor.number == maxValue)
            {
                var secondElevatorDirection = _floorQueue.Where(x => x.direction != initialDirection).ToList();
                 
                foreach (var oppositeFloor in secondElevatorDirection)
                {
                    MoveToFloor(oppositeFloor.number);
                    _floorQueue.Remove(floor);
                    
                    // TODO: Can I state that this is always min value?  - Gut Says no with addition of Elevator Request
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