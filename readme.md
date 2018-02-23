## Actor Instance Lifecycle

**Starting**  
Being initialised into actor system

**Receiving Messages**  
Receiving and processing messages

**Stopping**  
Actor cleaning up after itself

**Restarting**  
About to go back into starting state

**Terminated**  
Dead, cannot be restarted


## Lifecycle Hook Methods

`PreStart()`
  * called before actor instance receives first message
  * custom initialisation code, getting actor ready to start receiving messages
  * eg opening/creating files, system handles etc
  
`PostStop()`
  * called after the actor has been stopped and is not receiving messages any more
  * custom cleanup code
  * eg release system resources/handles such as file system

`PreRestart()`
  * called before actor begins restarting
  * allows code to do something with current message/exception
  * eg save current message for reprocessing later when actor restarts

`PostRestart()`
  * called after PreRestart() and before PreStart()
  * allows code to do something with exception
  * eg additional custom diagnostic/logging
