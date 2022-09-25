using UnityEngine;

public abstract class ParametersScreenBase : UIScreen
{
    [SerializeField] private InoutParametersViewConfig _config;
    public InoutParametersViewConfig Config => _config;
}
