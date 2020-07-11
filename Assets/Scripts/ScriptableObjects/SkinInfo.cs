using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkin", menuName = "Skin", order = 55)]
public class SkinInfo : ScriptableObject
{
    [SerializeField] private ViewTypePair[] pairs;
    protected Dictionary<ViewTypeId, ViewTypeId> dictionary;

    void OnEnable()
    {
        if(pairs == null)
        {
            dictionary = new Dictionary<ViewTypeId, ViewTypeId>(0);
            return;
        }

        dictionary = new Dictionary<ViewTypeId, ViewTypeId>(pairs.Length);
        foreach (var (key, value) in pairs)
        {
            dictionary.Add(key, value);
        }
    }

    public void AddSkin(GameEntity entity, GameContext gameContext)
    {
        var children = entity.GetAllChildrenGameEntities(gameContext);
        foreach (var child in children)
        {
            child.AddSkin(this);
            if (child.hasViewType && dictionary.TryGetValue(child.viewType.id, out var newViewType)) child.ReplaceViewType(newViewType);
        }
    }
}