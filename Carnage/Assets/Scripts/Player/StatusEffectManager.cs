using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    public List<StatusEffect> effects = new List<StatusEffect>();

    public void ApplyEffect(StatusEffect effect, float duration) {
        StatusEffect toRemove = null;
        foreach (StatusEffect currentEffect in effects)
        {
            if (currentEffect.GetType() == effect.GetType()) {
                currentEffect.Deactivate();
                toRemove = currentEffect;
                break;
            }
        }
        effects.Remove(toRemove);
        effect.Activate(duration);
        effects.Add(effect);
    }
}
