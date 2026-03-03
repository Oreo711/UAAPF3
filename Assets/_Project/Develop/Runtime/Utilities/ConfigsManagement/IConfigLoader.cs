using System;
using System.Collections;
using System.Collections.Generic;


namespace _Project.Develop.Runtime.Utilities.ConfigsManagement
{
    public interface IConfigLoader
    {
        IEnumerator LoadAsync(Action<Dictionary<Type, object>> onConfigsLoaded);
    }
}
