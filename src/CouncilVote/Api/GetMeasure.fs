module CouncilVote.Api.GetMeasure

open System
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks
open Giraffe
open CouncilVote.Repository

let getMeasureHandler (id: Guid) =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            return! match getMeasureById id with
                    | Some m -> json m next ctx
                    | None -> RequestErrors.NOT_FOUND "Measure not found" next ctx
        }