using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharMove : MonoBehaviour
{

    [Header("캐릭터 애니메이션"), SerializeField] CharAnim _charAnim;
    
    [Header("이동 속도"), SerializeField] float _moveSpeed = 2f;
    CharacterController _charController;

    List<HexNode> _paths = new List<HexNode>();
    
    int _range = 1;
    Vector3 _targetPos = Vector3.zero;

    [Header("공격 속도"), SerializeField] float _attSpeed = 1f;
    [Header("타일 컨트롤러"), SerializeField] CharTile _charTile;

    [Header("적 / 아군 구분"), SerializeField] eTILESTATE _state;

    [Header("카메라 이동"), SerializeField] MoveBar _moveHpBar;
    eTILESTATE _targetState;

    Tile _attackTargetTile = null;

    public GameObject _targetGo = null;

    [Header("스턴"), SerializeField] bool _isStun = false;
    
    [Header("에어본")]
    [SerializeField] bool _isAirbone = false;
    float _airboneSpeed = 3f;

    [Header("속박"), SerializeField] bool _isBind = false;

    CharInfo _charInfo;

    public bool _isAttak = false;
    bool _isManaFull = false;

    HexNode _hexNode = null;
    public HexNode _HexNode => _hexNode;
    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        
        if (_charTile == null)
        {
            _charTile = GetComponent<CharTile>();
        }
        _charAnim.SetCharMove(this);
    }
    public void SetState(eTILESTATE state)
    {
        _state = state;
        _targetState = (state == eTILESTATE.Player) ? eTILESTATE.Enemy : eTILESTATE.Player;
    }
    public void SetMoveHpBar(MoveBar moveHpBar)
    {
        _moveHpBar = moveHpBar;
    }
    public void SetCharInfo(CharInfo charInfo)
    {
        _charInfo = charInfo;
    }
    IEnumerator CRT_MoveCharacter(Vector3 targetPos, float moveSpeed)
    {
        //_targetPos = targetPos;
        _isAttak = false;
        _charTile.SetTargetTileInfo(_state, gameObject);
        _charTile.SetOnTileInfo(eTILESTATE.Empty, null);
        //_charTile.SetTargetTileState(_state);
        //_charTile.SetTargetTileObj(gameObject);
        while (Vector3.Distance(transform.position, _targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, moveSpeed * Time.deltaTime);
            SetRotation(_targetPos);
            _moveHpBar.MoveBarObj(transform.position);
            //transform.rotation = Quaternion.LookRotation(_targetPos - transform.position);
            yield return null;
        }
        ArriveTarget();
    }
    IEnumerator CRT_IdleRoation()
    {
        while(true)
        {
            SetRotation(_targetPos);
            yield return null;
        }
        
    }
    
    public void SetRotation(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f; // y축 회전은 무시 (수평 회전만 적용)

        // 방향 벡터를 바라보는 회전값 계산
        if (dir != Vector3.zero) // 방향 벡터가 유효한 경우에만 회전 적용
        {
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = quat;
        }
    }
    void ArriveTarget()
    {
        //_node.SetTileState(eTILESTATE.Empty);
        //_node = _paths[0];
        if (_state == eTILESTATE.Player)
        {
            Debug.Log("e");
        }

        
        _charTile.ChangeTile();
        //_onTile = _targetTile;
        _charAnim.SetIsWalking(false);
        SetPath();
        Debug.Log(_state + "ArriveTarget");
        if (_state == eTILESTATE.Enemy)
        {
            Debug.Log("e");
        }
        
        
    }
    
    public void MoveCharacter()
    {
        if(!_isStun)
        { 
            if (_paths.Count > 0)
            {
                _targetPos.y = 1f;
                _charAnim.SetIsWalking(true);
                StartCoroutine(CRT_MoveCharacter(_targetPos, _moveSpeed));
            }
        }
    }
    public void JumpCharacter(Vector3 targetPos, float jumpSpeed)
    {

        _targetPos = targetPos;
        _targetPos.y = 1f;
        StartCoroutine(CRT_MoveCharacter(_targetPos, jumpSpeed));
        
    }
    
    public void SetPath()
    {
        HexGrid grid = HexBoardMaker.Instance._hexGrid;
        Pathfinding pathFinding = new Pathfinding(grid);
        if (_targetGo == null || !_targetGo)
        {
            _isAttak = false;
            eTILESTATE targetState = (_state == eTILESTATE.Player) ? eTILESTATE.Enemy : eTILESTATE.Player;
            _targetGo = pathFinding.FindNearestEnemyGo(_charTile._Node, targetState);

        }
        if (!_isAttak)
        {



            _hexNode = _charTile._Node;
            int xPos = _hexNode._XPos;
            int yPos = _hexNode._YPos;
            if (_state == eTILESTATE.Player)
            {
                Debug.Log("s");
            }
            _paths = pathFinding.FindPath(xPos, yPos, _range, _state, _targetGo);
            if (_paths != null)
            {
                if (_paths.Count > 0)
                {
                    /*
                    if (_state == eTILESTATE.Player)
                    {
                        Debug.Log("s");
                    }
                    if (_state == eTILESTATE.Enemy)
                    {
                        Debug.Log("e");
                    }
                    */
                    _attackTargetTile = HexBoardMaker.Instance.GetTileByNode(_paths.Last());
                }

                if (_paths.Count > _range)
                {

                    for (int i = 0; i < _range; i++)
                    {
                        _paths.RemoveAt(_paths.Count - 1);
                    }

                    _targetPos = _paths[0]._TilePos;

                    Tile target = HexBoardMaker.Instance.GetTileByNode(_paths[0]);
                    _charTile.SetTargetTile(target);
                }
                else
                {
                    _paths.Clear();
                }
            }
            if (_paths.Count > 0)
            {
                MoveCharacter();
            }
            else
            {
                StartAttack();
            }
        }
        



    }
    public void ChangeTarget(GameObject target)
    {
        _targetGo = target;
    }
    public void SetRange(int range)
    {
        _range = range;
    }
    void StartAttack()//도착시 공격 시작
    {
        //StartCoroutine(CRT_StartAttack());
        _isAttak = true;
        GameObject targetObj = _targetGo;
        _charAnim.StartAttack(_attSpeed, targetObj);
    }
    public void FinishAttack()
    {
        //Debug.Log("FinishAttack");
        SetPath();
        if(_paths != null)
        {
            if(_isManaFull)
            {
                _charInfo.ExcuteSkill();
                _isManaFull = false;
            }
            else
            {
                if (_paths.Count > 0)
                {
                    MoveCharacter();
                }
                else
                {
                    StartAttack();
                }
            }
            
        }
        
    }
    public void SetManaState()
    {
        _isManaFull = true;
    }
    
    public void SetStun(float dur)
    {
        StartCoroutine(CRT_Stun(dur));
    }
    public void SetAirbone(float dur)
    {
        StartCoroutine(CRT_Airbone(dur));
    }
    IEnumerator CRT_Airbone(float dur)
    {
        StartCoroutine(CRT_Stun(dur));
        float timer = 0;
        Vector3 airBoneDir = new Vector3(0f, 1f, 0f);
        Vector3 startPos = transform.position;
        while(timer < dur * 0.5f)
        {
            timer += Time.deltaTime;
            transform.position += airBoneDir * _airboneSpeed * Time.deltaTime;
            yield return null;
        }
        while (timer < dur)
        {
            timer += Time.deltaTime;
            transform.position -= airBoneDir * _airboneSpeed * Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;
    }
    IEnumerator CRT_Stun(float dur)
    {
        _isStun = true;
        _charAnim.StopAttack();
        yield return new WaitForSeconds(dur);
        _isStun = false;
        if (_paths != null)
        {
            if (_paths.Count > 0)
            {
                MoveCharacter();
            }
            else
            {
                StartAttack();
            }
        }
    }
    public void SetBind(float dur)
    {

    }
}
