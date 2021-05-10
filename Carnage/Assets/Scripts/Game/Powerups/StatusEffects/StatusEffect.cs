using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    protected PlayerCarController player;
    private IEnumerator coroutine;

    public StatusEffect(PlayerCarController player) {
        this.player = player;
    }

    public void Activate(float duration) {
        OnActivate();
        coroutine = WaitToDeactivate(duration);
        player.StartCoroutine(coroutine);
    }

    public void Deactivate() {
		player.GetComponent<StatusEffectManager>().effects.Remove(this);
        player.StopCoroutine(coroutine);
        OnDeactivate();
    }

    private IEnumerator WaitToDeactivate(float duration)
    {
        yield return new WaitForSeconds(duration);
        Deactivate();
    }

    protected abstract void OnActivate();
    protected abstract void OnDeactivate();
}
