using System;
using System.Collections.Generic;

namespace SharedSimulationCode.Systems.Spawn
{
    public class ProjectileViewTypePathStorage
    {
        private readonly Dictionary<ViewTypeId, string> viewTypePrefabPath = new Dictionary<ViewTypeId, string>()
        {
            {ViewTypeId.DefaultShoot, "Prefabs/3dWarships/BlueLaserSmallOBJ"}
        };
        
        public string GetPath(ViewTypeId viewType)
        {
            if (!viewTypePrefabPath.ContainsKey(viewType))
            {
                throw new Exception("Нет такого ключа");
            }
            return viewTypePrefabPath[viewType];
        }
    }
}