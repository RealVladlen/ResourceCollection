using Building;

public class GameEventManager
{
    /// <summary>
    /// Ивент для определения будет перемещаться камера или нет
    /// </summary>
    public delegate void CameraMoverState(bool state);
    public static event CameraMoverState OnCameraMoverState;
    public static void CameraMoverStateMethod(bool state) => OnCameraMoverState?.Invoke(state);
    
    /// <summary>
    /// Ивент для определения будет перемещаться игрок или нет
    /// </summary>
    public delegate void PlayerMoverState(bool state);
    public static event PlayerMoverState OnPlayerMoverState;
    public static void PlayerMoverStateMethod(bool state) => OnPlayerMoverState?.Invoke(state);
    
    /// <summary>
    /// Ивент для обновления счетчика ресурсов
    /// </summary>
    public delegate void UpdateResourcesCount(EResourceType type, int count);
    public static event UpdateResourcesCount OnUpdateResourcesCount;
    public static void UpdateResourcesCountMethod(EResourceType type, int count) => OnUpdateResourcesCount?.Invoke(type, count);
    
}