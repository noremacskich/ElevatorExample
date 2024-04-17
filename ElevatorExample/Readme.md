
# Assumptions
Due to simplicity sakes, the following cases are not covered

- Elevator will not stop between requests.  For example if the elevator is moving between level 1 and 5 and could 
make stop at 2, it will not.
- Elevator is synchronous, meaning it will immediately execute the current command, and not wait for more commands
- 