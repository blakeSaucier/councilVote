namespace CouncilVote.Common

module List =
  let mustPass (lst: bool list) =
    lst |> List.forall id
  let somes lst =
    lst |> List.choose id