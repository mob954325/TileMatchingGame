// 사용하는 Enum 모음

using UnityEngine;

/// <summary>
/// 블록 타입 
/// </summary>
public enum BlockType
{
    Normal,
    Long,
    Bomb
}

public enum BlockColor
{
    Blue,
    Red,    
    Green,
    Yellow
}

/// <summary>
/// 4방향 움직이는 방향 타입
/// </summary>
public enum MoveDirection
{
    Right,
    Left,
    Up,
    Down
}

public enum SoundType
{
    Open,
    Close,
    Pop
}