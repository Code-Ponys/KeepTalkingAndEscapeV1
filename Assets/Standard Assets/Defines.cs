using System;

public enum AnimationType {
    Open,
    MoveOnKeyHold,
    MoveOnKeySmash,
    ActivateOnKeySmash
}

public enum KeyType {
    X,
    Y,
    A,
    B,
    R1,
    R2,
    L1,
    L2
}

public static class ButtonNames {
    public static string MoveGhostX {
        get {return _moveGhostX;}
    }

    public static string MoveGhostY {
        get {return _moveGhostY;}
    }

    public static string MoveHumanX {
        get {return _moveHumanX;}
    }

    public static string MoveHumanY {
        get {return _moveHumanY;}
    }

    public static string CameraGhostX {
        get {return _cameraGhostX;}
    }

    public static string CameraGhostY {
        get {return _cameraGhostY;}
    }

    public static string CameraHumanX {
        get {return _cameraHumanX;}
    }

    public static string CameraHumanY {
        get {return _cameraHumanY;}
    }

    public static string GhostInteract {
        get {return _ghostInteract;}
    }

    public static string HumanInteract {
        get {return _humanInteract;}
    }

    public static string GhostInspect {
        get {return _ghostInspect;}
    }

    public static string HumanInspect {
        get {return _humanInspect;}
    }

    public static string GhostInventory {
        get {return _ghostInventory;}
    }

    public static string HumanInventory {
        get {return _humanInventory;}
    }

    public static string Menu {
        get {return _menu;}
    }

    private static string _moveGhostX = "Move Ghost Left Joystick X-Axis";
    private static string _moveGhostY = "Move Ghost Left Joystick Y-Axis";
    private static string _moveHumanX = "Move Human Left Joystick X-Axis";
    private static string _moveHumanY = "Move Human Left Joystick Y-Axis";
    private static string _cameraGhostX = "Camera Ghost Right Joystick X-Axis";
    private static string _cameraGhostY = "Camera Ghost Right Joystick Y-Axis";
    private static string _cameraHumanX = "Camera Human Right Joystick X-Axis";
    private static string _cameraHumanY = "Camera Human Right Joystick Y-Axis";
    private static string _ghostInteract = "Ghost Interact";
    private static string _humanInteract = "Human Interact";
    private static string _ghostInspect = "Ghost Inspect";
    private static string _humanInspect = "Human Inspect";
    private static string _ghostInventory = "Ghost Inventory";
    private static string _humanInventory = "Human Inventory";
    private static string _menu = "Menu";
}
