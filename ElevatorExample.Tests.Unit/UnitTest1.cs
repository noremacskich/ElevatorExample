using ElevatorExample.Mocking;
using NSubstitute;

namespace ElevatorExample.Tests.Unit;

public class Tests
{
    private IConsoleWriter _mockConsole;
    private Elevator _elevator;
    
    [SetUp]
    public void Setup()
    {
        _mockConsole = Substitute.For<IConsoleWriter>();
        _elevator = new Elevator(_mockConsole);
    }

    [Test]
    public void IfFloorRequestIsToLow_TellUserFloorRange()
    {
        _elevator.FloorRequestElevator(-1);

        _mockConsole.Received(1).WriteLine("That floor does not exist.  The floor range is from 1 to 5.");
        Assert.That(_mockConsole.ReceivedCalls().Count(), Is.EqualTo(1));
    }
    
    [Test]
    public void IfFloorRequestIsToHigh_TellUserFloorRange()
    {
        _elevator.FloorRequestElevator(10);

        _mockConsole.Received(1).WriteLine("That floor does not exist.  The floor range is from 1 to 5.");
        Assert.That(_mockConsole.ReceivedCalls().Count(), Is.EqualTo(1));
    }
    
    [Test]
    public void IfElevatorIsOnSameFloor_OpensThenClosesTheDoor()
    {
        _elevator.FloorRequestElevator(1);

        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
        
    }

    [Test]
    public void IfFloorIsImmediatelyAboveAndBelowElevator_ShowMovingAndArrivedMessages()
    {
        _elevator.FloorRequestElevator(2);
        
        
        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Moving To Floor 2");
            _mockConsole.WriteLine("Arrived At Floor 2");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
        
        _mockConsole.ClearReceivedCalls();

        _elevator.FloorRequestElevator(1);
        
        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Moving To Floor 1");
            _mockConsole.WriteLine("Arrived At Floor 1");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
    }

}