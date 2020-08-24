// using System.Collections.Generic;
// using UnityEngine;
//
// [CreateAssetMenu(fileName = "NewSkin", menuName = "Skin", order = 55)]
// public class SkinInfo : ScriptableObject
// {
// #pragma warning disable 649 // Присваивается в Unity Editor.
//     [SerializeField] private ViewTypePair[] pairs;
// #pragma warning restore 649
//     protected Dictionary<ViewTypeId, ViewTypeId> dictionary;
//
//     void OnEnable()
//     {
//         if(pairs == null)
//         {
//             dictionary = new Dictionary<ViewTypeId, ViewTypeId>(0);
//             return;
//         }
//
//         dictionary = new Dictionary<ViewTypeId, ViewTypeId>(pairs.Length);
//         foreach (var (key, value) in pairs)
//         {
//             dictionary.Add(key, value);
//         }
//     }
//
//     public ViewTypeId Apply(ViewTypeId oldId) => dictionary.TryGetValue(oldId, out var newId) ? newId : oldId;
//
//     public void AddSkin(ServerGameEntity entity, ServerGameContext gameContext)
//     {
//         var children = entity.GetAllChildrenGameEntities(gameContext);
//         foreach (var child in children)
//         {
//             child.AddSkin(this);
//             if (child.hasViewType && dictionary.TryGetValue(child.viewType.id, out var newViewType)) child.ReplaceViewType(newViewType);
//         }
//     }
// }