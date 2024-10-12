#if PANCAKE_AI
namespace Pancake.AI
{
    using UnityEngine;

    public abstract class Consideration : ScriptableObject
    {
        public abstract float Evalute(AIContext context);
    }
}
#endif