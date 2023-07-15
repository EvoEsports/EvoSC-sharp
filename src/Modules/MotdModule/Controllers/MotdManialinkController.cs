using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdManialinkController : ManialinkController
{
    private const string Template = "MotdModule.MotdTemplate";
    private bool _isChecked = false;
    
    public MotdManialinkController()
    {
        
    }

    public async Task ToggleCheckbox()
    {
        _isChecked = !_isChecked;
        //await Close();
        //await ShowAsync(Context.Player, Template, new { isChecked = _isChecked });
    }
    
    public async Task Close()
    {
        await HideAsync(Context.Player, Template);
    }
}
