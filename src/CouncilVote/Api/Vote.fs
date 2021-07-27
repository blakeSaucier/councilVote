module CouncilVote.Api.Vote

open System
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks
open Giraffe
open FSharp.Json
open CouncilVote.Domain
open CouncilVote.Domain.Models
open CouncilVote.Repository

type CreateVoteDto =
    { measureId: Guid
      inFavor: bool
      name: string }
        
let createVoteHandler : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! body = ctx.ReadBodyFromRequestAsync()
            let createVoteDto = Json.deserialize<CreateVoteDto> body
            let measure = getMeasureById createVoteDto.measureId
            return! match measure with
                    | Some m ->
                        let vote =
                            { VoterName = createVoteDto.name
                              Vote = if createVoteDto.inFavor then Yes else No }
                        let res = UseCases.addVote vote m
                        updateMeasure res
                        json res next ctx
                    | None ->
                        RequestErrors.BAD_REQUEST "Invalid MeasureId:" next ctx
        }