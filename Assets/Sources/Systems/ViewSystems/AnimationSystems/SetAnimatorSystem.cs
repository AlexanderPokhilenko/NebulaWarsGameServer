using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class SetAnimatorSystem : ReactiveSystem<GameEntity>
{
    public SetAnimatorSystem(Contexts contexts) : base(contexts.game)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AnimatorController);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasView && entity.hasAnimatorController;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (GameEntity e in entities)
        {
            Animator animator;
            if (e.hasAnimator)
            {
                animator = e.animator.value;
            }
            else
            {
                animator = e.view.gameObject.AddComponent<Animator>();
                e.AddAnimator(animator);
            }
            animator.runtimeAnimatorController = e.animatorController.value;
        }
    }
}