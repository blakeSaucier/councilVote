module CouncilVote.Api.Router

open Giraffe
open CouncilVote.Api.Handlers
open CouncilVote.Repository
open System

let councilVoteRouter: HttpHandler =
    choose [
        GET >=> routef "/api/measure/%O" (fun (id: Guid) -> json (getMeasureById id))
        POST >=> choose [
            route "/api/measure" >=> createMeasure
        ]
    ]

