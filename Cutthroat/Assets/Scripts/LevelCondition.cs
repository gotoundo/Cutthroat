using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Result { Win, Lose, Story }
public enum TriggerFrequency { Timed, Continuous }
public enum Qualifier { LessThan, GreaterThan, None }
public enum Metric { Gold, PopularityPercent, None }

public class LevelCondition
{
    public Result result;
    public TriggerFrequency trigger;
    public Qualifier qualifier;
    public Metric metric;
    public float amount;
    public int deadline;
    public StoryEventData triggeredStory;
    public bool hasBeenTriggered;

    public LevelCondition(Result result, TriggerFrequency trigger, Qualifier qualifier, Metric metric, float amount, int deadline = int.MaxValue)
    {
        this.result = result;
        this.trigger = trigger;
        this.qualifier = qualifier;
        this.metric = metric;
        this.amount = amount;
        this.deadline = deadline;
        hasBeenTriggered = false;
    }

    public LevelCondition(Result result, int deadline) : this(result, TriggerFrequency.Timed, Qualifier.None, Metric.None, 0, deadline)
    { }

    public int ifPassedResult()
    {
        if (result != Result.Win && result != Result.Lose)
            return 0;
        return result == Result.Win ? 1 : -1;
    }
}