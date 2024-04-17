
# Operations
- Elevator Door will always open if the elevator is at the floor requested
- Elevator will state any floors it's passing
- Elevator will state what floor it's moving to, and when it has arrived at the floor
- Elevator will not stop for mid-move pickups


# Assumptions
Due to simplicity sakes, the following cases are not covered

- Elevator will not stop between requests.  For example if the elevator is moving between level 1 and 5 and could 
make stop at 2, it will not.
- Elevator is synchronous, meaning it will immediately execute the current command, and not wait for more commands
- 