module CouncilVote.Api.Router

open Giraffe
open CouncilVote.Api.CreateMeasure
open CouncilVote.Api.GetMeasure

let councilVoteRouter: HttpHandler =
    choose [
        GET >=> routef "/api/measure/%O" getMeasure
        POST >=> choose [
            route "/api/measure" >=> createMeasure
        ]
    ]

