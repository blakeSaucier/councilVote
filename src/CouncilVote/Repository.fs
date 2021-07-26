module CouncilVote.Repository

open System
open System.Runtime.Caching
open Domain.Models

let private memoryCache = new MemoryCache("Council Votes")

let getMeasureById (id: Guid) =
    let measure = memoryCache.Get(id.ToString())
    match measure with
    | null -> None
    | _ -> Some (measure :?> Measure)
    
let insertMeasure (measure: Measure) =
    memoryCache.Add(measure.Id.ToString(), measure, DateTimeOffset.MaxValue) |> ignore
    
let updateMeasure (measure: Measure) =
    memoryCache.Set(measure.Id.ToString(), measure, DateTimeOffset.MaxValue)
    
let deleteMeasure (id: Guid) =
    memoryCache.Remove(id.ToString()) |> ignore
    
let seedData () =
    let exampleMeasure =
        { Id = Guid.Parse "d447119f-7dba-4b41-864f-4f29947f3b8a"
          Subject = "James's Important Strata Vote"
          Description = "Seeking approval of next year's budget"
          Status = Active
          Votes = []
          Configuration = [ MinimumNumberOfVotes 5 ] }
    
    insertMeasure exampleMeasure