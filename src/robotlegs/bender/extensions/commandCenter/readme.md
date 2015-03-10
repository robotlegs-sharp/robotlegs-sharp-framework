# CommandCenter

This extension is used by command based extensions such as the Event Command Map.

It is not intended to be used directly.

# James' Notes

## Command Mapper
This is utility which you give the command mapping list.
With this class, you can call 'toCommand' 'withGuards' 'withHooks' 'withExecuteMethod'.
Doing this will create a command mapping, (which is just a basic VO with guards, hooks, executemethod, commandclass. And then adds it to the command mapping list.

## Command Mapping
The data stored from the command mapper util.

## Command Mapping List
Stores all the Command Mapping (data from command mapper)
and adds them to dictionarys for quick retrieval.
It's also has a sortFunction, which will sort before getting all mappings.

It also has 'processors'.
Adding a processor is a function invoked with each new mapping as a parameter.
When a mapping is added, all processors will run and be passed the new mapping.

It also has 'triggers' which will get activated when there is mapping in the list, and deactivated when there are no mappings.
This is used to clean up the global event dispatcher when not being used.

## Command Trigger Map
I may need to look into the EventCommandMap first to explain

## Command Executor
Creates a child injector, (so it can easily overrite mappings without ruining the original)

It then can execute commands from a CommandMapping

### When executing a command

* It will instantiate and inject the command, or just inject into the command (It will also inject payload if required)
* Then it will apply hooks
* It will then call the execute method (with payloads if it takes them)

