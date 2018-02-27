using System;

public enum AnimationType {
    None,
    Open,
    GhostActivateOnKeyHold,
    GhostMoveOnKeySmash,
    GhostActivateOnKeySmash,
    OpenLinkedOnHold,
    ParentAnimation
}

public enum KeyType {
    X,
    Y,
    A,
    B,
    R1,
    R2,
    L1,
    L2,
    Start,
    none
}

public enum PlayerDamage {
    None,
    DamageOnTouch,
    DamageAfterFail
}

public enum ActivateChildWhen {
    AnimationDone,
    ButtonPressed
}

public enum ItemId {
    Testitem,
}

public enum CombineWith {
    Clean,
    Item,

}

public enum CharacterType {
    Unassigned,
    Ghost,
    Human
}

public enum NumButtonType {
    Number,
    Reset,
    Confirm,
}

public enum Direction {
    Up,
    Down,
    Left,
    Right
}

public static class ButtonNames {

    /// <summary>
    /// Get the button name of the pressed key depends on Key Type
    /// </summary>
    /// <param name="KeyType">KeyType Enum</param>
    /// <returns>Returns the Button name as string</returns>
    public static string GetButtonName(KeyType keyType) {
        switch(keyType) {
            case KeyType.A:
                return GhostJoystickButtonA;
            case KeyType.B:
                return GhostjoystickButtonB;
            case KeyType.X:
                return GhostJoystickButtonX;
            case KeyType.Y:
                return GhostJoystickButtonY;
            case KeyType.R1:
                return null;
            case KeyType.R2:
                return null;
            case KeyType.L1:
                return null;
            case KeyType.L2:
                return null;
            case KeyType.Start:
                return GhostMenu;
            default:
                return null;
        }
    }

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

    public static string GhostJoystickButtonA {
        get {return _ghostJoystickButtonA;}
    }

    public static string HumanJoystickButtonA {
        get {return _humanJoystickButtonA;}
    }

    public static string GhostjoystickButtonB {
        get {return _ghostjoystickButtonB;}
    }

    public static string HumanjoystickButtonB {
        get {return _humanjoystickButtonB;}
    }

    public static string GhostJoystickButtonX {
        get {return _ghostJoystickButtonX;}
    }

    public static string HumanJoystickButtonX {
        get {return _humanJoystickButtonX;}
    }

    public static string GhostJoystickButtonY {
        get {return _ghostJoystickButtonY;}
    }

    public static string HumanJoystickButtonY {
        get {return _humanJoystickButtonY;}
    }

    public static string GhostVerticalPad {
        get {return _ghostVerticalPad;}
    }

    public static string HumanVerticalPad {
        get {return _humanVerticalPad;}
    }

    public static string GhostHorizontalPad {
        get {return _ghostHorizontalPad;}
    }

    public static string HumanHorizontalPad {
        get {return _humanHorizontalPad;}
    }

    public static string GhostHighlighting {
        get {return _ghostHighighting;}
    }

    public static string HumanHighlighting {
        get {return _humanHighlighting;}
    }

    public static string GhostMenu {
        get {return _ghostMenu;}
    }

    public static string HumanMenu {
        get {return _humanMenu;}
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
    private static string _ghostJoystickButtonA = "Ghost Joystick Button A";
    private static string _humanJoystickButtonA = "Human Joystick Button A";
    private static string _ghostjoystickButtonB = "Ghost Joystick Button B";
    private static string _humanjoystickButtonB = "Human Joystick Button B";
    private static string _ghostJoystickButtonX = "Ghost Joystick Button X";
    private static string _humanJoystickButtonX = "Human Joystick Button X";
    private static string _ghostJoystickButtonY = "Ghost Joystick Button Y";
    private static string _humanJoystickButtonY = "Human Joystick Button Y";
    private static string _ghostVerticalPad = "Ghost Vertical Pad";
    private static string _humanVerticalPad = "Human Vertical Pad";
    private static string _ghostHorizontalPad = "Ghost Horizontal Pad";
    private static string _humanHorizontalPad = "Human Horizontal Pad";
    private static string _ghostHighighting = "Ghost Highlighting";
    private static string _humanHighlighting = "Human Highlighting";
    private static string _ghostMenu = "Ghost Menu";
    private static string _humanMenu = "Human Menu";
}

public static class DataPath {
    private static string _itemDatabase = "data/itemDatabase.yml";
    private static string _saveGame = "data/savegame.yml";

    public static string ItemDatabase {
        get {return _itemDatabase;}
    }

    public static string SaveGame {
        get {return _saveGame;}
    }


}
