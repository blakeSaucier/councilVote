module CouncilVote.Test.DomainTests

open CouncilVote.Domain.Models
open CouncilVote.Domain.UseCases
open Expecto

let testMeasure =
  { Id = 24
    Subject = "Test Measure"
    Description = "A Description"
    Votes = [ ]
    Status = Active
    Configuration = [ ] }
  
let measureConfigTests = testList "Measure Configuration tests" [
  test "Measure should complete if Min Yes % and Min Num of votes is met" {
    let measure =
      { testMeasure with Configuration = [ MinimumYesPercentage 0.75; MinimumNumberOfVotes 4 ] }
    
    let res = 
      addVote { VoterName = "James"; Vote = Yes } measure
      |> addVote { VoterName = "Blake"; Vote = Yes }
      |> addVote { VoterName = "Alan"; Vote = No }
      |> addVote { VoterName = "Hank"; Vote = Yes }
      
    Expect.equal res.Status Passed "Should have completed and passed"
  }
  
  test "Measure should not complete if min number of votes is not met" {
    let measure =
      { testMeasure with Configuration = [ MinimumNumberOfVotes 10 ] }
    
    let res = 
      addVote { VoterName = "James"; Vote = No } measure
      |> addVote { VoterName = "Blake"; Vote = Yes }
      |> addVote { VoterName = "Alan"; Vote = No }
      |> addVote { VoterName = "Hank"; Vote = Yes }
      
    Expect.equal res.Status Active "Should not complete"
  }
  
  test "Measure should complete if required voter votes FOR" {
    let measure =
      { testMeasure with Configuration = [ MinimumNumberOfVotes 4; VotersWithNameInFavor ["Hank"] ] }
    
    let res = 
      addVote { VoterName = "James"; Vote = No } measure
      |> addVote { VoterName = "Blake"; Vote = Yes }
      |> addVote { VoterName = "Alan"; Vote = Yes }
      |> addVote { VoterName = "Hank"; Vote = Yes }
      
    Expect.equal res.Status Passed "Should not complete"
  }  
]

[<Tests>]
let domainTests =
  testList "Domain Tests" [
    measureConfigTests
  ]
