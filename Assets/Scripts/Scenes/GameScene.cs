using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{



    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Managers.UI.ShowSceneUI<UI_Inven>();
        //Managers.Resource.Instantiate("UnityChan");
    }

    public override void Clear()
    {

    }
}