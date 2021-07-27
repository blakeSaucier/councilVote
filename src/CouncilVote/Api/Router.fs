module CouncilVote.Api.Router

open Giraffe
open CouncilVote.Api.CreateMeasure
open CouncilVote.Api.GetMeasure
open CouncilVote.Api.Vote

let councilVoteRouter: HttpHandler =
    choose [
        GET >=> routef "/api/measure/%O" getMeasureHandler
        POST >=> choose [
            route "/api/measure" >=> createMeasureHandler
            route "/api/vote" >=> createVoteHandler 
        ]
    ]

