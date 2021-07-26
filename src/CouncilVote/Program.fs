module CouncilVote.Program

open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer
open CouncilVote.Api.Router
open Repository
open Giraffe

let webApp =
    choose [
        route "/health" >=> json {| Status = "ok" |}
        councilVoteRouter ] 

type Startup() =
    member _.Configure (app : IApplicationBuilder)
                       (env : IHostEnvironment)
                       (loggerFactory : ILoggerFactory) =
        app.UseGiraffe webApp
        app.UseStaticFiles() |> ignore
        app.UseSpaStaticFiles()
        app.UseSpa (fun spa ->
            spa.Options.SourcePath <- "ClientApp"
            if env.IsDevelopment() then
                spa.UseReactDevelopmentServer(npmScript = "start"))
        seedData ()

    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore
        services.AddSpaStaticFiles (fun config ->
            config.RootPath <- "ClientApp/build")
        services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .UseStartup<Startup>()
                    |> ignore)
        .Build()
        .Run()
    0