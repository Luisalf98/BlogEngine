using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogEngine.Entities
{
  public class Post
  {
    public enum State
    {
      Draft,
      Pending,
      Approved,
      Rejected
    }

    public enum Action
    {
      Submit,
      Approve,
      Reject
    }

    public long Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }
    public User Author { get; set; }
    [Required, ForeignKey("User")]
    public long AuthorId { get; set; }
    [Required]
    public State CurrentState { get; set; }

    public bool TakeAction(Action action)
    {
      var previousState = CurrentState;
      CurrentState = (CurrentState, action) switch
      {
        (State.Draft, Action.Submit) => State.Pending,
        (State.Rejected, Action.Submit) => State.Pending,
        (State.Pending, Action.Approve) => State.Approved,
        (State.Pending, Action.Reject) => State.Rejected,
        _ => CurrentState
      };

      return CurrentState != previousState;
    }
  }
}
