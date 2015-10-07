using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//contains information on starting player

public enum Result { Win, Lose }
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

    public LevelCondition(Result result, TriggerFrequency trigger, Qualifier qualifier, Metric metric, float amount, int deadline = int.MaxValue)
    {
        this.result = result;
        this.trigger = trigger;
        this.qualifier = qualifier;
        this.metric = metric;
        this.amount = amount;
        this.deadline = deadline;
    }

    public LevelCondition(Result result, int deadline) : this(result,TriggerFrequency.Timed, Qualifier.None,Metric.None,0,deadline)
    { }

    public int ifPassedResult()
    {
        return result == Result.Win ? 1 : -1;
    }
}

public class LevelDefinition
{
    public List<LevelCondition> Conditions;

    public LevelDefinition()
    {
        Conditions = new List<LevelCondition>();
        Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .5f));
        Conditions.Add(new LevelCondition(Result.Lose, 30));
    }

    //returns -1 for loss, 0 for not over, and 1 for won
    public int CheckGameOver()
    {
        foreach (LevelCondition condition in Conditions)
        {
            if(condition.trigger == TriggerFrequency.Continuous || Timepiece.CurrentDay > condition.deadline)
            {
                if (condition.qualifier == Qualifier.None)
                    return condition.ifPassedResult();

                float comparedValue = condition.metric == Metric.Gold ? GameManager.singleton.player.Gold : ProgressPanel.popularityPercent(GameManager.singleton.player);

                if (condition.qualifier == Qualifier.GreaterThan)
                    if (comparedValue > condition.amount)
                        return condition.ifPassedResult();

                if (condition.qualifier == Qualifier.LessThan)
                    if (comparedValue < condition.amount)
                        return condition.ifPassedResult();
            }
        }

        return 0;
    }

    public bool HasWon()
    {
        return CheckGameOver() == 1;
    }

    public bool HasLost()
    {
        return CheckGameOver() == -1;
    }
}

public class StoreDefinition
{



}

