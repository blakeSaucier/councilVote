module CouncilVote.Api.CreateMeasure

open System
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks
open Giraffe
open FSharp.Json
open CouncilVote.Domain.Models
open CouncilVote.Repository

type CreateMeasureDto =
  { subject: string
    description: string
    minimumVotesSelected: bool
    minimumVotes: int
    minimumYesSelected: bool
    minimumYesPercent: int }

let validateMinimumVotes (createMeasure: CreateMeasureDto) =
    match createMeasure.minimumVotesSelected, createMeasure.minimumVotes with
    | true, votes when votes <= 0 -> Error "Cannot have negative votes"
    | _ -> Ok createMeasure
    
let validateMinimumVotePercent (createMeasure: CreateMeasureDto) =    
    match createMeasure.minimumYesSelected, createMeasure.minimumYesPercent with
    | true, percent when percent <= 0 -> Error "Cannot have negative vote percent"
    | _ -> Ok createMeasure
    
let validateConfigurations (createMeasure: CreateMeasureDto) =
    match createMeasure.minimumVotesSelected, createMeasure.minimumYesSelected with
    | false, false -> Error "A configuration must be selected"
    | _ -> Ok createMeasure
    
let validate (createMeasure: CreateMeasureDto) =
    validateConfigurations createMeasure
    |> Result.bind validateMinimumVotes
    |> Result.bind validateMinimumVotePercent
    
let getConfigurations createMeasure =
    let getMinVotesConfig create =
        if create.minimumVotesSelected then [ MinimumNumberOfVotes create.minimumVotes ] else []
    let getMinVotePercent create =
        if create.minimumYesSelected then [ MinimumYesPercentage ((float create.minimumYesPercent) / 100.0) ] else []
    (getMinVotesConfig createMeasure) @ (getMinVotePercent createMeasure)

let createMeasure : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! body = ctx.ReadBodyFromRequestAsync()
            let parsed = Json.deserialize<CreateMeasureDto> body
            match validate parsed with
            | Ok newMeasure ->
                let measure =
                    { Id = Guid.NewGuid()
                      Subject = newMeasure.subject
                      Description = newMeasure.description
                      Votes = []
                      Status = Active
                      Configuration = getConfigurations newMeasure }
                insertMeasure measure // persist to data store
                return! json measure next ctx
            | Error e ->
                return! RequestErrors.badRequest (json {| Error = e |}) next ctx
        }