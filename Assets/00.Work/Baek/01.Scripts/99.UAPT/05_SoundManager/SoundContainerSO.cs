using UnityEngine;

[CreateAssetMenu(menuName = "SO/Baek/SoundSetting")]
public class SoundContainerSO : ScriptableObject
{
    [Header("Background Sound Setting")]
    [SerializeField] private AudioClip _title;
    [SerializeField] private AudioClip _menu;
    [SerializeField] private AudioClip _shop;
    [SerializeField] private AudioClip _jungle;
    [SerializeField] private AudioClip _mountain;
    [SerializeField] private AudioClip _desert;
    [SerializeField] private AudioClip _endingcredits;
    [SerializeField] private AudioClip _playerfaild;

    [Header("UI Sound Setting")]
    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _hover;
    [SerializeField] private AudioClip _typing;

    [Header("Player Sound Setting")]
    [SerializeField] private AudioClip _attack;
    [SerializeField] private AudioClip _playerhit;
    [SerializeField] private AudioClip _playerdie;
    [SerializeField] private AudioClip _orb;

    [Header(" Setting")]
    [SerializeField] private AudioClip _none;
    [SerializeField] private AudioClip _burn;
    [SerializeField] private AudioClip _slow;
    [SerializeField] private AudioClip _frostbite;
    [SerializeField] private AudioClip _paralysis;
    [SerializeField] private AudioClip _addiction;
    [SerializeField] private AudioClip _heal;
    [SerializeField] private AudioClip _defense;

    [Header("Enemy Sound Setting")]
    [SerializeField] private AudioClip _enemyattack;
    [SerializeField] private AudioClip _enemyhit;
    [SerializeField] private AudioClip _enemydie;
}
