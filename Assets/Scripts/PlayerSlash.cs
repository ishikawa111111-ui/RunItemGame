using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSlash : MonoBehaviour
{
    //対象スラッシュオブジェクト
    public GameObject slashObject;

    InputAction slashAction;

    [Header("スラッシュ時間")]
    public float slashTime = 0.5f;

    Coroutine slashStart;

    void Start()
    {
        //PlayerInputコンポーネントの現Mapを取得
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;

        //現Mapの「Crouch」アクションを取得（Westボタン）
        slashAction = actionMap.FindAction("Crouch");
        slashObject.SetActive(false);   //開始時は必ず非表示
    }

    void Update()
    {
        if (slashAction.WasPerformedThisFrame())
        {
            Slash();
        }
    }

    void Slash()
    {
        if (slashStart == null) //スラッシュ中でなければ
        {
            slashObject.SetActive(true);    //スラッシュオブジェクト表示
            slashStart = StartCoroutine(SlashStart());  //コルーチン発動
        }
    }

    IEnumerator SlashStart()
    {
        yield return new WaitForSeconds(slashTime); //待つ
        slashObject.SetActive(false);   //スラッシュを消す
        slashStart = null;  //スラッシュ中を解放
    }
}
