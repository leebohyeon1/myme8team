using System.Collections;
using System.Collections.Generic; // ��ųʸ� Ŭ������ �����ؼ� �߰����� ��� Ŭ���� ���� ����
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EVENT_TYPE
{
    SCENE_LOAD,
    CHECK_ATTACK,
    HEALTH_CHANGE,
    PLAYER_ATTACK,
    SHAKE_CAMERA,
    PLAYER_ACT,
    ENEMY_STATE,
    DEAD,
};
public interface IListener
{
    void OnEvent(EVENT_TYPE Event_type, Component Sender, object Param = null);
}

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    //=========================================================

    public delegate void OnEvent(EVENT_TYPE Event_, Component Sender, object Param = null);
    private Dictionary<EVENT_TYPE, List<IListener>> Listeners =
        new Dictionary<EVENT_TYPE, List<IListener>>();
    //=========================================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    //=========================================================

    //����Ʈ�� ������ ������Ʈ �߰�
    public void AddListener(EVENT_TYPE Event_Type, IListener Listener)
    {
        List<IListener> ListenList = null;

        if(Listeners.TryGetValue(Event_Type, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }


        ListenList = new List<IListener>();
        ListenList.Add(Listener);
        Listeners.Add(Event_Type, ListenList);
    }

    //�̺�Ʈ�� �����ʿ��� �����Ѵ�.
    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        List<IListener> ListenList = null;

        if(!Listeners.TryGetValue(Event_Type, out ListenList)) { return; }

        for(int i = 0; i< ListenList.Count; i++) 
        {
            if (!ListenList[i].Equals(null))
            {
                ListenList[i].OnEvent(Event_Type, Sender,Param);
            }
        }
    }

    //�̺�Ʈ ������ ������ �׸��� ��ųʸ����� �����.
    public void RemoveEvenet(EVENT_TYPE Event_Type)
    {
        Listeners.Remove(Event_Type);
    }

    //��ųʸ��� �ʿ���� �׸���� ����
    public void RemoveRedundancies()
    {
        Dictionary<EVENT_TYPE, List<IListener>> TmpListeners =
            new Dictionary<EVENT_TYPE, List<IListener>>();

        foreach(KeyValuePair<EVENT_TYPE, List<IListener>> Item in Listeners)
        {
            for(int i = Item.Value.Count - 1; i >= 0 ; i--)
            {
                if (Item.Value[i].Equals(null))
                {
                    Item.Value.RemoveAt(i);
                }
            }

            if(Item.Value.Count == 0)
            {
                TmpListeners.Add(Item.Key, Item.Value);
            }

            Listeners = TmpListeners;
        }
    }

    //���� ����Ǹ� ȣ��.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RemoveRedundancies(); //��ųʸ� û��
    }

    private void OnEnable()
    {
        // SceneManager.sceneLoaded �̺�Ʈ�� OnSceneLoaded �޼��带 ����.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // SceneManager.sceneLoaded �̺�Ʈ���� OnSceneLoaded �޼��带 ���� ����.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
