namespace CouncilVote.Domain

open System

module Models =
    
    /// Configurable completion rules for a 'Measure' 
    type MeasureCompletionRule =
    | MinimumNumberOfVotes of int
    | MinimumYesPercentage of float
    | RequiredVotersInFavor of string list

    type MeasureState = Active | Passed | Failed

    type Vote = Yes | No

    type MeasureVote =
      { VoterName: string
        Vote: Vote }

    type Measure = 
      { Id: Guid
        Subject: string
        Description: string
        Votes: MeasureVote list
        Configuration: MeasureCompletionRule list
        Status: MeasureState }

module UseCases =
  
  open Models
  open CouncilVote.Common
  
  /// Calculate the percentage of yes votes For a measure
  let calculateYesPercent measure =
    let yesVotes = 
      measure.Votes 
      |> List.where (fun v -> v.Vote = Yes) 
      |> List.length 
      |> float
    let allVotes = float measure.Votes.Length
    if allVotes = 0.0 then 0.0 else yesVotes / allVotes

  /// Check if a minimum yes percentage has been configured, default to 50% as the threshold
  let getMeasureYesThreshold (measure: Measure) =
    let ``fifty percent in favor`` = 0.5
    let threshold =
      measure.Configuration 
      |> List.map (fun config ->
        match config with
        | MinimumYesPercentage p -> Some p
        | _ -> None)
      |> List.somes
      |> List.tryHead
    threshold |> Option.defaultValue ``fifty percent in favor``

  /// Evaluate a Measure completion rule
  let evaluateMeasureRule measure rule =
    match rule with
    | MinimumNumberOfVotes minVotes -> measure.Votes.Length >= minVotes
    | MinimumYesPercentage minYesPercent ->
      let yesPercent = calculateYesPercent measure
      yesPercent >= minYesPercent
    | RequiredVotersInFavor names ->
      names |> List.forall (fun name ->
        let vote = measure.Votes |> List.tryFind (fun v -> v.VoterName = name)
        match vote with
        | None -> false // Have not cast a vote on this measure
        | Some v -> v.Vote = Yes) // They have voted 'Yes'

  /// Evaluate if a Measure has completed based on its configuration
  let hasMeasureCompleted (measure: Measure) =
    measure.Configuration 
      |> List.map (evaluateMeasureRule measure)
      |> List.mustPass

  /// Evaluate if a measure has passed based on its yes votes
  let hasMeasurePassed (measure: Measure) =
    let yesPercent = calculateYesPercent measure
    let yesThreshold = getMeasureYesThreshold measure
    yesPercent >= yesThreshold

  /// Check if a measure has completed then evaluate the percentage of yes votes
  let evaluateMeasureStatus (measure: Measure) =
    let completed = hasMeasureCompleted measure
    let passed = hasMeasurePassed measure
    match completed, passed with
    | true, true -> { measure with Status = Passed }
    | true, false -> { measure with Status = Failed }
    | _, _ -> measure
    
  let addVote (vote: MeasureVote) (measure: Measure) =
    match measure.Status with
    | Active -> 
      let measure = { measure with Votes = vote :: measure.Votes }
      evaluateMeasureStatus measure
    | Passed -> measure
    | Failed -> measure
    
  let createMeasure (subject: string) (description: string) (config: MeasureCompletionRule list) =
    match config with
    | [  ] -> Error "Measure must contain a completion rule"
    | _ -> Ok { Id = Guid.NewGuid()
                Subject = subject
                Description = description
                Status = Active
                Votes = []
                Configuration = config }
    
    