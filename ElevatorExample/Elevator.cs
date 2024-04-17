using ElevatorExample.Mocking;

namespace ElevatorExample;

internal sealed class Elevator
{
    private IConsoleWriter Console;
    
    private int TopFloor = 5;
    private int BottomFloor = 1;
    private int CurrentFloor = 1;

    public Elevator()
    {
        Console = new ConsoleWriter();
    }
    
    public Elevator(IConsoleWriter consoleWriter)
    {
        Console = consoleWriter;
    }

    public void FloorRequestElevator(int floor)
    {
        if (floor > TopFloor || floor < BottomFloor)
        {
            Console.WriteLine($"That floor does not exist.  The floor range is from {BottomFloor} to {TopFloor}.");
            return;
        }
        if (floor != CurrentFloor)
        {
            MoveToFloor(floor);
        }

        OpenDoor();
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
    
    private void MoveToFloor(int floor)
    {
        Console.WriteLine($"Moving To Floor {floor}");

        CurrentFloor = floor;
       
        Console.WriteLine($"Arrived At Floor {CurrentFloor}");
    }
}