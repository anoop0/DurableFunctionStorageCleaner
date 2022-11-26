## DurableFunctionStorageCleaner
Cleans blob, queue and table used by durable function in one click. It will resets Durabel Function app runs and storage used.

### Use Case
Durable Function restarts/resume on server restart. It manages cycle by own.
While development we had to stop server in middle and run again. This triggers incomplete/not failed functions reasuming again. To avoid it only option is to 
manually delete function storage two containers, five Queues and two tables from
storage emulator.

### Solution
#### DurableFunctionStorageCleaner deletes the containers, queues and tables in one click.
This is also helpful when we need to reset function app on server. The purge option of 
Durable Functioin can't reset the hung operations or running functions.

### Executing program 
#### Download executable program to run
> [Downlaod DurableFunctionStorageCleaner.exe](https://github.com/anoop0/DurableFunctionStorageCleaner/releases/download/1.0/DurableFunctionStorageCleaner.exe)

### Option 1. 
```
> DurableFunctionStorageCleaner <Storage-Account-connection-string> <storage-pattern-prefix>
```
In local development <storage-pattern-prefix>  is usually testhubname. If your storage prefix is testhubname. You don't need to provide argument for <storage-pattern-prefix>

### Option 2. 
Just run and run window will give you options to provide value for parameter
```
> DurableFunctionStorageCleaner
```
You can also create batch files to feed input parameters
```
@echo off

set /p cont=Enter Y to continue:

if %cont% equ y (
	ECHO Continue cleaning up storage: Blob Container, Queues and Tables

	DurableFunctionStorageCleaner <Storage-Account-connection-string> <storage-pattern-prefix>

)

PAUSE
```

<span style="color:red">Be aware while providing connection string and storage names. It will delete them in one go </span>.

## Example Run

When running executable directly
![Running executable](https://github.com/anoop0/DurableFunctionStorageCleaner/releases/download/1.0/Running.executable.png "Running executable")

When running via batch file
![Running executable](https://github.com/anoop0/DurableFunctionStorageCleaner/releases/download/1.0/Running.From.Batch.file.png "Running from batch file")
