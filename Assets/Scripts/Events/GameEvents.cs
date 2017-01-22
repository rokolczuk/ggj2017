using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartedEvent
{

}

public class LifeLostEvent
{
    public readonly int LivesLeft;

    public LifeLostEvent(int Lives)
    {
        this.LivesLeft = Lives;
    }
}


public class GameOverEvent
{
}

public class GameRestartEvent
{
	
}

public class CameraShakeEvent
{
}
