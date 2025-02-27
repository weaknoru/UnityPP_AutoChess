using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCHARTYPE
{
    Kassadin,
    Khazix,
    RekSai,
    ChoGath,
    KaiSa,

    Nidalee,
    Warwick,
    Ahri,
    Rengar,
    Gnar,

    Pyke,
    Lulu,
    Blitzcrank,
    Varus

}
public enum eSYNERGY
{
    None,
    Void,
    Mage,
    Assassin,
    Brawler,
    Scout,
    Wild,
    Shapeshifter,
    Yordle,
    Robot
}
public class CStat
{
    public float _maxHp { get; private set; }
    public float _att { get; private set; }
    public float _attSpeed { get; private set; }
    public float _maxMp { get; private set; }
    public float _startMp { get; private set; }
    public float _def { get; private set; }
    public float _mr { get; private set; }
    public int _range { get; private set; }
    
    public CStat(float maxHp, float att, float attSpeed, float maxMp, float startMp, float def, float mr, int range)
    {
        _maxHp = maxHp;
        _att = att;
        _attSpeed = attSpeed;
        _maxMp = maxMp;
        _startMp = startMp;
        _def = def;
        _mr = mr;
        _range = range;
    }
}
public class CChar
{
    public string _name { get; private set; }
    public eCHARTYPE _type { get; private set; }
    public int _cost { get; private set; }
    public string _prefName { get; private set; }

    public CStat _stat { get; private set; }
    Dictionary<int, CStat> _statMap = new Dictionary<int, CStat>();
    public Dictionary<int, CStat> _StatMap => _statMap;
    public List<eSYNERGY> _synergies { get; private set; }
    
    public int _level { get; private set; }
    /*
    public void UpgradeLevel(int level)
    {
        _level = level;
        _stat = _statMap[level];
    }
    */
    public void SetStatMap(int level, CStat stat)
    {
        _statMap.Add(level, stat);
    }
    public CStat GetStat(int level)
    {
        return _statMap[level];
    }
    public CChar(string name, eCHARTYPE type, int cost, string prefName, CStat stat, List<eSYNERGY> synergies)
    {
        _name = name;
        _type = type;
        _cost = cost;
        _prefName = prefName;
        _stat = stat;
        _synergies = synergies;
    }
}
public class CharInfo : MonoBehaviour
{
    [Header("적 / 아군 구분"), SerializeField] eTILESTATE _state;
    public eTILESTATE _State => _state;
    

    [Header("타입"), SerializeField] eCHARTYPE _charType;
    public eCHARTYPE _CharType => _charType;

    [Header("레벨"), SerializeField] int _level;

    [Header("최대 체력"), SerializeField] float _maxHp;
    [Header("공격력"), SerializeField] float _att;
    [Header("공격 속도"), SerializeField] float _attSpeed;
    [Header("최대 MP / 시작 MP"), SerializeField] 
    float _maxMp;
    float _startMp;
    [Header("방어력"), SerializeField] float _def;
    [Header("마법 저항력"), SerializeField] float _mr;

    [Header("마나 회복량"), SerializeField] float _mpRecovery = 10f;
    public int _range { get; private set; }
   

    [Header("타일 좌표"), SerializeField] Vector2 _tilePos;

    [SerializeField] CharMove _charMove;
    [SerializeField] CharHealth _charHealth;
    [SerializeField] MovableUnit _movable;
    [SerializeField] BasicAttack _basicAttack;
    [SerializeField] CharTile _charTile;
    Tile _onTile = null;
    CStat _stat;

    Tile _startTile = null;
    private void Awake()
    {
        _basicAttack.SetCharInfo(this);
        _charMove.SetCharInfo(this);
    }
    public void UpdateOnTile(Tile tile)
    {
        _onTile = tile;
        _onTile.PutCharacter(gameObject);
        _onTile.SetCharObj(gameObject);
        _onTile.SetTileState(eTILESTATE.Player);

        //_tilePos = new Vector2(_onTile._hexNode._XPos, _onTile._hexNode._YPos);
    }
    public void SetStat(CStat stat)
    {
        _stat = stat;

        _maxHp = _stat._maxHp;
        _att = _stat._att;
        _attSpeed = _stat._attSpeed;
        _maxMp = _stat._maxMp;
        _startMp = _stat._startMp;
        _def = _stat._def;
        _mr = _stat._mr;
        _range = _stat._range;

        _charMove.SetRange(_range);
        _basicAttack.SetAttDamage(_att);
        _basicAttack.SetManaRecover(_maxMp, _startMp, _mpRecovery);
    }
    public void SetMovable(bool isPlayer, Tile tile)
    {
        _movable.SetIsPlayer(isPlayer);
        if(tile != null)
        {
            _movable.SetTile(tile);
        }
    }
    public void SetHealth(bool isPlayer)
    {
        _charHealth.SetIsPlayer(isPlayer);
        CharManager.Instance.SpawnBar(_movable, _charHealth, _basicAttack);
        _charHealth.SetHealthStat(_maxHp, _def, _mr);


        //CharManager.Instance.SetSkill(_basicAttack, _charType);
        //_basicAttack.SetActiveSKill()
    }
    public void SetState(eTILESTATE state)
    {
        _state = state;
        _basicAttack.SetTileState(_state);

        _charMove.SetState(_state);
        CharManager.Instance.AddCharMove(_charMove);
    }
    public void UpdateLevel(int level)
    {
        _level = level;
        _basicAttack.SetLevel(_level);
    }
    public void GetHeal(float amount)
    {
        _charHealth.GetHeal(amount);
    }
    public void SetManaState()
    {
        _charMove.SetManaState();
    }
    public void ExcuteSkill()
    {
        _basicAttack.ExecuteSkill();
    }
    public void FinishSkill()
    {
        _charMove.FinishAttack();
    }
    public void JumpCharacter()
    {

    }
    public void JumpToFarthestChar(float range, float jumpSpeed)
    {

        HexGrid grid = HexBoardMaker.Instance._hexGrid;
        Pathfinding pathFinding = new Pathfinding(grid);

        //GameObject targetGo = pathFinding.FindFarthestEnemyGo(_charMove._HexNode, _state);
        eTILESTATE state = (_state == eTILESTATE.Player) ? eTILESTATE.Enemy : eTILESTATE.Player;
        HexNode attTargetNode = pathFinding.FindFarthestNode(_charMove._HexNode, state); //가장 먼 캐릭



        _charMove.ChangeTarget(attTargetNode._charGo);

        bool targetIsRight = ((_charMove._HexNode._XPos - attTargetNode._XPos) < 0);
        HexNode moveTargetNode = grid.GetAwayNode(attTargetNode, targetIsRight, _range);
        Tile target = HexBoardMaker.Instance.GetTileByNode(moveTargetNode);
        _charTile.SetTargetTile(target);
        _charMove.JumpCharacter(moveTargetNode._TilePos, jumpSpeed);
        
    }
}
