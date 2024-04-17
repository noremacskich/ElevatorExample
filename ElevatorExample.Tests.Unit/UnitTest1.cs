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
        _elevator.FloorRequest(-1, ElevatorDirection.Down);

        _mockConsole.Received(1).WriteLine("That floor does not exist.  The floor range is from 1 to 5.");
        Assert.That(_mockConsole.ReceivedCalls().Count(), Is.EqualTo(1));
    }
    
    [Test]
    public void IfFloorRequestIsToHigh_TellUserFloorRange()
    {
        _elevator.FloorRequest(10, ElevatorDirection.Up);

        _mockConsole.Received(1).WriteLine("That floor does not exist.  The floor range is from 1 to 5.");
        Assert.That(_mockConsole.ReceivedCalls().Count(), Is.EqualTo(1));
    }
    
    [Test]
    public void IfElevatorRequestIsToLow_TellUserFloorRange()
    {
        _elevator.ElevatorRequest(-1);

        _mockConsole.Received(1).WriteLine("That floor does not exist.  The floor range is from 1 to 5.");
        Assert.That(_mockConsole.ReceivedCalls().Count(), Is.EqualTo(1));
    }
    
    [Test]
    public void IfElevatorRequestIsToHigh_TellUserFloorRange()
    {
        _elevator.ElevatorRequest(10);

        _mockConsole.Received(1).WriteLine("That floor does not exist.  The floor range is from 1 to 5.");
        Assert.That(_mockConsole.ReceivedCalls().Count(), Is.EqualTo(1));
    }

    
    [Test]
    public void DoorOpensAndCloses_IfElevatorIsOnSameFloor()
    {
        _elevator.FloorRequest(1, ElevatorDirection.Up);
        _elevator.StartElevator();

        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
        
    }

    [Test]
    public void IfFloorIsImmediatelyAboveAndBelowElevator_ShowMovingAndArrivedMessages()
    {
        _elevator.FloorRequest(2, ElevatorDirection.Up);
        _elevator.StartElevator();
        
        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Moving To Floor 2");
            _mockConsole.WriteLine("Arrived At Floor 2");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
        
        _mockConsole.ClearReceivedCalls();

        _elevator.FloorRequest(1, ElevatorDirection.Down);
        _elevator.StartElevator();
        
        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Moving To Floor 1");
            _mockConsole.WriteLine("Arrived At Floor 1");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
    }
    
    [Test]
    public void ShowPassingFloorMessages_WhenFloorDifferenceIsMoreThenOne()
    {
        _elevator.FloorRequest(4, ElevatorDirection.Up);
        _elevator.StartElevator();
        
        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Moving To Floor 4");
            _mockConsole.WriteLine("Passing Floor 2");
            _mockConsole.WriteLine("Passing Floor 3");
            _mockConsole.WriteLine("Arrived At Floor 4");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
        
        _mockConsole.ClearReceivedCalls();

        _elevator.FloorRequest(1, ElevatorDirection.Down);
        _elevator.StartElevator();
        
        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Moving To Floor 1");
            _mockConsole.WriteLine("Passing Floor 3");
            _mockConsole.WriteLine("Passing Floor 2");
            _mockConsole.WriteLine("Arrived At Floor 1");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
    }
    
    [Test]
    public void WhileMovingBetweenFloors_ItWillStatAllFloorsItIsPassing()
    {
        _elevator.FloorRequest(2, ElevatorDirection.Up);
        _elevator.FloorRequest(5, ElevatorDirection.Up);
        _elevator.FloorRequest(3, ElevatorDirection.Down);
        _elevator.StartElevator();
        
        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Moving To Floor 2");
            _mockConsole.WriteLine("Arrived At Floor 2");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
            _mockConsole.WriteLine("Moving To Floor 5");
            _mockConsole.WriteLine("Passing Floor 3");
            _mockConsole.WriteLine("Passing Floor 4");
            _mockConsole.WriteLine("Arrived At Floor 5");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
            _mockConsole.WriteLine("Moving To Floor 3");
            _mockConsole.WriteLine("Passing Floor 4");
            _mockConsole.WriteLine("Arrived At Floor 3");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
    }
    
    [Test]
    public void ElevatorWillKeepInitialUpDirectionConsistent_AndMakePitStopsForSaidDirection()
    {
        _elevator.FloorRequest(2, ElevatorDirection.Up);
        _elevator.FloorRequest(3, ElevatorDirection.Down);
        _elevator.FloorRequest(5, ElevatorDirection.Up);
        _elevator.FloorRequest(1, ElevatorDirection.Down);

        _elevator.StartElevator();
        
        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Moving To Floor 2");
            _mockConsole.WriteLine("Arrived At Floor 2");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
            _mockConsole.WriteLine("Moving To Floor 5");
            _mockConsole.WriteLine("Passing Floor 3");
            _mockConsole.WriteLine("Passing Floor 4");
            _mockConsole.WriteLine("Arrived At Floor 5");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
            _mockConsole.WriteLine("Moving To Floor 3");
            _mockConsole.WriteLine("Passing Floor 4");
            _mockConsole.WriteLine("Arrived At Floor 3");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
            _mockConsole.WriteLine("Moving To Floor 1");
            _mockConsole.WriteLine("Passing Floor 2");
            _mockConsole.WriteLine("Arrived At Floor 1");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
    }
    
    [Test]
    public void OnceElevatorReachesTop_ItWillReversDirection_ThenWillReverseAgain()
    {
        _elevator.FloorRequest(5, ElevatorDirection.Up);
        _elevator.FloorRequest(3, ElevatorDirection.Down);
        _elevator.FloorRequest(4, ElevatorDirection.Up);
        _elevator.FloorRequest(1, ElevatorDirection.Down);
        _elevator.FloorRequest(2, ElevatorDirection.Up);

        _elevator.StartElevator();
        
        Received.InOrder(() =>
        {
            _mockConsole.WriteLine("Moving To Floor 5");
            _mockConsole.WriteLine("Passing Floor 2");
            _mockConsole.WriteLine("Passing Floor 3");            
            _mockConsole.WriteLine("Passing Floor 4");
            _mockConsole.WriteLine("Arrived At Floor 5");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
            _mockConsole.WriteLine("Moving To Floor 3");
            _mockConsole.WriteLine("Passing Floor 4");
            _mockConsole.WriteLine("Arrived At Floor 3");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
            _mockConsole.WriteLine("Moving To Floor 1");
            _mockConsole.WriteLine("Passing Floor 2");
            _mockConsole.WriteLine("Arrived At Floor 1");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
            _mockConsole.WriteLine("Moving To Floor 4");
            _mockConsole.WriteLine("Passing Floor 2");
            _mockConsole.WriteLine("Passing Floor 3");
            _mockConsole.WriteLine("Arrived At Floor 4");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
            _mockConsole.WriteLine("Moving To Floor 2");
            _mockConsole.WriteLine("Passing Floor 3");
            _mockConsole.WriteLine("Arrived At Floor 2");
            _mockConsole.WriteLine("Opening Door");
            _mockConsole.WriteLine("Closing Door");
        });
    }


}
