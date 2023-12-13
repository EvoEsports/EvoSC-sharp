namespace EvoSC.Common.Themes.Exceptions;

public class ThemeDoesNotExistException(string name) : ThemeException($"The theme with name '{name}' does not exist.");
