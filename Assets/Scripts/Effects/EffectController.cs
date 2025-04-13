using UnityEngine;

public class EffectController : MonoBehaviour
{
    private Animator effect_animator_;

    void Start()
    {
        effect_animator_ = GetComponent<Animator>();
        Destroy(gameObject, effect_animator_.GetCurrentAnimatorStateInfo(0).length);
    }

    void Update()
    {

    }
}
