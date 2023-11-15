namespace EvoSC.Manialinks.Exceptions.Themes;

public class ThemeDoesNotExistException : ThemeException
{
    public ThemeDoesNotExistException(string name) : base($"The theme with name '{name}' does not exist.")
    {
    }
}
