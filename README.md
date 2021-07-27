# Council Vote

This is a simple app to help strata councils vote on measures. The state of the repo represents the progress I was able to make given the time constraints.

I chose to build this demo application using F# as I believe it is a strong language for modeling a domain and its rules.

When building this app I starting with a focus on the domain, thought about how best to represent it in code, and then build the use cases and workflows out from there.

The following rules are implemented:
- Minimum number of votes required
- Minimum percentage of yes votes
- Users with specific names must vote for it pass. (Domain level only, ran out of time to do this in the UI)

## Building and running 
- Install .Net 5 SDK
- `cd` into `src/CouncilVote/`
- `dotnet run`

## Tech Stack
- .Net 5.0
- F# with Giraffe as the backend
- No database - the data is persisted in memory. I made this decision simply to make development faster.
- React front end. This is the default 'create-reate-app' project which the dotnet cli spits out.

## Architecture
A note on how this is structured: 

Most C# solutions include multiple projects (web api, infrastructure, domain, application) so as to control dependency flow and provide organization. 

F# strictly enforces the order of files in a project.
You'll notice that the topmost files are `Domain` and `Common` and the bottom files are the entry point and web api specific code. 
Because of this, I didn't split the solution into separate projects but I wanted to call this out as an intentional decision.            

## Assumptions
- A Measure can be created with one or more configurations on how the Measure can 'Complete'
- These rules must ALL PASS for a Measure to be considered completed, and then the PASSED or FAILED can be evaluated
- A Measure is evaluated as PASSED if it receives a majority vote (> 50%) or if a minimum percentage of YES votes was defined and that threshold was reached.

## Testing
I added some quick tests around the domain and the use cases. Of course, it's better to have more tests but I only have limited time.
- `cd` in `test/CouncilVote.Test/`
- `dotnet run`

